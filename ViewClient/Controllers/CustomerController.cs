using Domain.DTO.Customer;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Utilities;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerRepo;
        private readonly HttpClient _httpClient;
        public CustomerController(ICustomer customerRepo, HttpClient httpClient)
        {
            _customerRepo = customerRepo;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
            var result = await _customerRepo.GetCustomerById(userId);
            if (result != null)
            {
                return View(result);
            }
            return View("NotFound");
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Customer/GetCustomerById?Id={id}";

            var content = new StringContent(JsonConvert.SerializeObject(new { Id = id }), Encoding.UTF8, "application/json");

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
                var customerById = JsonConvert.DeserializeObject<CustomerGetByIdRequest>(responseString);

                var customerUpdateRequest = new CustomerUpdateRequest
                {
                    Id = customerById.Id,
                    UserName = customerById.UserName,
                    FirstName = customerById.FirstName,
                    LastName = customerById.LastName,
                    Email = customerById.Email,
                    PhoneNumber = customerById.PhoneNumber,
                    Gender = Enum.TryParse(typeof(GenderType), customerById.Gender.ToString(), out var gender)
                    ? (GenderType)gender
                    : GenderType.Unknown,
                    DateOfBirth = customerById.DateOfBirth,
                    Status = customerById.Status
                };

                return View(customerUpdateRequest);
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
            request.Status = EntityStatus.Active;
            request.ModifiedBy = _UserLogin;
            request.ModifiedTime = DateTimeOffset.Now;

            var result = await _customerRepo.UpdateCustomer(request);
            if (result == -1)
            {
                return Json(new { success = false, message = "Thông tin của bạn đã bị trùng." });
            }
            else if (result == 1)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công." });

            }

            return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật thông tin." });

        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(ClientUpdatePassword request)
        {
            var _UserLogin = Guid.Empty;

            // Lấy thông tin người dùng đã đăng nhập
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            var result = _customerRepo.ClientUpdatePassword(request);
            if (result == null)
            {
                return Json(new { success = false, message = "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại thông tin!" });
            }

            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
        }
    }
}
