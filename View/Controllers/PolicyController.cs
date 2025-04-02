using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WEB.CMS.Customize;
using Domain.DTO.Staff;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.Views.Shared.Helper;
using Domain.DTO.Policy;
using Domain.DTO.PolicyType;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
namespace View.Controllers
{
    [CustomAuthorize]
    public class PolicyController : Controller
    {
        private readonly HttpClient _httpClient;

        public PolicyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
        }

        private async Task<T?> SendHttpRequest<T>(string requestUrl, HttpMethod method, object? body = null)
            where T : class
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, requestUrl);

                // Nếu có body thì serialize nó thành JSON
                if (body != null)
                {
                    var json = JsonConvert.SerializeObject(body);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                // Gửi request
                var response = await _httpClient.SendAsync(request);

                if (response == null)
                {
                    throw new NullReferenceException("Response is null");
                }

                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Deserialize thành đối tượng T
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 5, string title = null, string contentOfPolicy = null)
        {
            string requestURL = "https://localhost:7130/api/Policy/GetListPolicy";

            var PolicyRequest = new PolicyGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Title = title,
                Content = contentOfPolicy
            };

            var jsonRequest = JsonConvert.SerializeObject(PolicyRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(requestURL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var Policies = JsonConvert.DeserializeObject<ResponseData<Policy>>(responseString);

                string policyTypesRequestUrl = "https://localhost:7130/api/PolicyType/GetListPolicyType";
                var policyTypesRequest = new PolicyTypeGetRequest();
                var policyTypeJsonRequest = JsonConvert.SerializeObject(policyTypesRequest);
                var policyTypeContent = new StringContent(policyTypeJsonRequest, Encoding.UTF8, "application/json");
                var policyTypeResponse = await _httpClient.PostAsync(policyTypesRequestUrl, policyTypeContent);

                var policyTypeResponseString = await policyTypeResponse.Content.ReadAsStringAsync();
                var policyTypeList = JsonConvert.DeserializeObject<ResponseData<PolicyType>>(policyTypeResponseString);
                ViewBag.policyTypeList = policyTypeList.data;

                string StaffsRequestUrl = "https://localhost:7130/api/Staff/GetListStaff";
                var StaffsRequest = new StaffGetRequest();
                var StaffJsonRequest = JsonConvert.SerializeObject(StaffsRequest);
                var StaffContent = new StringContent(StaffJsonRequest, Encoding.UTF8, "application/json");
                var StaffResponse = await _httpClient.PostAsync(StaffsRequestUrl, StaffContent);

                var StaffResponseString = await StaffResponse.Content.ReadAsStringAsync();
                var StaffList = JsonConvert.DeserializeObject<ResponseData<Domain.Models.Staff>>(StaffResponseString);
                ViewBag.StaffList = StaffList.data;

                return View(Policies);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Policy/GetPolicyById?Id={id}";

            // Tạo nội dung json cho request
            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            string policyTypesRequestUrl = "https://localhost:7130/api/PolicyType/GetListPolicyType";
            var policyTypesRequest = new PolicyTypeGetRequest();
            var policyTypeJsonRequest = JsonConvert.SerializeObject(policyTypesRequest);
            var policyTypeContent = new StringContent(policyTypeJsonRequest, Encoding.UTF8, "application/json");
            var policyTypeResponse = await _httpClient.PostAsync(policyTypesRequestUrl, policyTypeContent);

            var policyTypeResponseString = await policyTypeResponse.Content.ReadAsStringAsync();
            var policyTypeList = JsonConvert.DeserializeObject<ResponseData<PolicyType>>(policyTypeResponseString);
            ViewBag.policyTypeList = policyTypeList.data;

            ViewBag.PolicyTypes = policyTypeList?.data.Select(pt => new SelectListItem
            {
                Value = pt.Id.ToString(),
                Text = pt.TitleOfType
            }).ToList();

            string StaffsRequestUrl = "https://localhost:7130/api/Staff/GetListStaff";
            var StaffsRequest = new StaffGetRequest();
            var StaffJsonRequest = JsonConvert.SerializeObject(StaffsRequest);
            var StaffContent = new StringContent(StaffJsonRequest, Encoding.UTF8, "application/json");
            var StaffResponse = await _httpClient.PostAsync(StaffsRequestUrl, StaffContent);

            var StaffResponseString = await StaffResponse.Content.ReadAsStringAsync();
            var StaffList = JsonConvert.DeserializeObject<ResponseData<Domain.Models.Staff>>(StaffResponseString);
            ViewBag.StaffList = StaffList.data;

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var Policy = JsonConvert.DeserializeObject<Policy>(responseString);

                return View(Policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Create()
        {
            string policyTypesRequestUrl = "api/PolicyType/GetListPolicyType";
            var policyTypesRequest = new PolicyTypeGetRequest
            {
                PageIndex = 1,
                PageSize = 100
            };
            var policyTypeResponse = await SendHttpRequest<ResponseData<PolicyType>>(policyTypesRequestUrl, HttpMethod.Post, policyTypesRequest);
            ViewBag.PolicyTypes = policyTypeResponse?.data;
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return View(new PolicyCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PolicyCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                string requestURL = "https://localhost:7130/api/Policy/CreatePolicy";
                request.CreatedBy = _UserLogin;
                request.StaffId = _UserLogin;
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

            string requestUrl = $"https://localhost:7130/api/Policy/GetPolicyById?Id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            string policyTypesRequestUrl = "api/PolicyType/GetListPolicyType";
            var policyTypesRequest = new PolicyTypeGetRequest
            {
                PageIndex = 1,
                PageSize = 100
            };
            var policyTypeResponse = await SendHttpRequest<ResponseData<PolicyType>>(policyTypesRequestUrl, HttpMethod.Post, policyTypesRequest);
            ViewBag.PolicyTypes = policyTypeResponse?.data.Select(pt => new SelectListItem
            {
                Value = pt.Id.ToString(),
                Text = pt.TitleOfType
            });
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var policy = JsonConvert.DeserializeObject<Policy>(responseString);



                return View(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Policy request)
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.ModifiedBy = userId;
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/Policy/UpdatePolicy", request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            string requestUrl = "https://localhost:7130/api/Policy/DeletePolicy";

            var request = new PolicyDeleteRequest
            {
                Id = id,
                DeletedBy =_UserLogin,
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Unable to delete the Policy.");
            return View("Error", new Exception("Unable to delete the Policy."));
        }

        public async Task<IActionResult> PolicyPDF()
        {
            var tpolicyGetRequest = new PolicyGetRequest()
            {
                PageIndex = 1,
                PageSize = int.MaxValue
            };
            string trequestUrl = "https://localhost:7130/api/PolicyType/GetListPolicyType";
            var tpolicies = await SendHttpRequest<ResponseData<PolicyType>>(trequestUrl, HttpMethod.Post, tpolicyGetRequest);
            ViewBag.policyTypeList = tpolicies.data;


            var policyGetRequest = new PolicyGetRequest()
            {
                PageIndex = 1,
                PageSize = int.MaxValue
            };
            string requestUrl = "https://localhost:7130/api/Policy/GetListPolicy";
            var policies = await SendHttpRequest<ResponseData<Policy>>(requestUrl, HttpMethod.Post, policyGetRequest);

            if (policies == null)
            {
                return View("Error");
            }
            return new ViewAsPdf("PolicyPDF", policies, ViewData)
            {
                PageMargins = new Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Orientation.Landscape
            };
        }
    }
}
