using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using View.Views.Shared.Helper;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomerController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 10, string userName = null, string email = null, string phoneNumber = null)
        {
            string requestURL = "https://localhost:7130/api/Customer/GetListCustomer";

            var customerRequest = new CustomerGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var jsonRequest = JsonConvert.SerializeObject(customerRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                // gửi request lên api
                var response = await _httpClient.PostAsync(requestURL, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var customers = JsonConvert.DeserializeObject<ResponseData<Customer>>(responseString);

                return View(customers);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Customer/GetCustomerById?Id={id}";

            // Tạo nội dung json cho request
            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseString);

                return View(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Create()
        {
            return View(new CustomerCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                string requestURL = "https://localhost:7130/api/Customer/CreateCustomer";

             
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                request.CreatedBy = _UserLogin;
                var response = await _httpClient.PostAsJsonAsync(requestURL, request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            
            string requestUrl = $"https://localhost:7130/api/Customer/GetCustomerById?Id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            ViewBag.Genders = Enum.GetValues(typeof(GenderType));
            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseString);



                return View(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerUpdateRequest request)
        {
            var _UserLogin = Guid.Empty;

            // Lấy thông tin người dùng đã đăng nhập
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            ViewBag.Genders = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(g => new SelectListItem
                {
                    Value = ((int)g).ToString(),
                    Text = g.ToString() 
                }).ToList();


            if (request.Gender.HasValue)
            {
                var genderString = request.Gender.Value.ToString();
                if (Enum.TryParse<GenderType>(genderString, out var gender))
                {
                    request.Gender = gender;
                }
                else
                {
                    ModelState.AddModelError("Gender", "Giá trị giới tính không hợp lệ.");
                }
            }
            request.ModifiedBy = _UserLogin;
            request.ModifiedTime = DateTimeOffset.Now;

            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/Customer/UpdateCustomer", request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Không thể sửa khách hàng.");
            return View("Error", new Exception("Không thể sửa khách hàng."));
        }
        public async Task<IActionResult> Delete(Guid id)
        {
			string requestUrl = "https://localhost:7130/api/Customer/DeleteCustomer";
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            var request = new CustomerDeleteRequest
			{
				Id = id,
				DeletedBy = _UserLogin,
				DeletedTime = DateTimeOffset.Now
			};

			var jsonRequest = JsonConvert.SerializeObject(request);
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(requestUrl, content);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Unable to delete the customer.");
			return View("Error", new Exception("Unable to delete the customer."));
		}
    }
}
