using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WEB.CMS.Customize;
using System.Security.Claims;
using Domain.DTO.PolicyType;

namespace View.Controllers
{
    [CustomAuthorize]
    public class PolicyTypeController : Controller
    {
        private readonly HttpClient _httpClient;

        public PolicyTypeController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 10, string? titleOfType = null)
        {
            string requestURL = "https://localhost:7130/api/PolicyType/GetListPolicyType";

            var policyTypeRequest = new PolicyTypeGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TitleOfType = titleOfType
            };

            var jsonRequest = JsonConvert.SerializeObject(policyTypeRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                // gửi request lên api
                var response = await _httpClient.PostAsync(requestURL, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var policyTypes = JsonConvert.DeserializeObject<ResponseData<PolicyType>>(responseString);

                return View(policyTypes);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/PolicyType/GetPolicyTypeById?Id={id}";

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
                var policyType = JsonConvert.DeserializeObject<PolicyType>(responseString);

                return View(policyType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return View(new PolicyTypeCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PolicyTypeCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                string requestURL = "https://localhost:7130/api/PolicyType/CreatePolicyType";
                request.CreatedBy = _UserLogin;
                request.CreatedTime = DateTimeOffset.Now;
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

            string requestUrl = $"https://localhost:7130/api/PolicyType/GetPolicyTypeById?Id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
 
            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var PolicyType = JsonConvert.DeserializeObject<PolicyType>(responseString);



                return View(PolicyType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PolicyType request)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            request.ModifiedBy = _UserLogin;
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/PolicyType/UpdatePolicyType", request);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            string requestUrl = "https://localhost:7130/api/PolicyType/DeletePolicyType";

            var request = new PolicyTypeDeleteRequest
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

            ModelState.AddModelError("", "Unable to delete the PolicyType.");
            return View("Error", new Exception("Unable to delete the PolicyType."));
        }
    }
}
