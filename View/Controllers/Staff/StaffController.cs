using Domain.DTO.Staff;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WEB.CMS.Customize;

namespace View.Controllers.Staff
{
    [CustomAuthorize]
    public class StaffController : Controller
    {
        private readonly HttpClient _httpClient;

        public StaffController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetListData(StaffGetRequest request)
        {
            return ViewComponent("ListData", request);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Staff/GetStaffById?request={id}";

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
                var PostType = JsonConvert.DeserializeObject<Domain.Models.Staff>(responseString);

                return View(PostType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> AddOrUpdateStaffForm()
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrCreate(StaffUpdateRequest request)
        {
            if (request.Id == Guid.Empty)
            {
                var obj = new StaffCreateRequest() 
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7130/api/Staff/CreateStaff", request);
                if (response.IsSuccessStatusCode) 
                {
                    return Ok(new
                    {
                        status = 200
                    });
                }
            }
            else 
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7130/api/Staff/UpdateStaff", request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        status = 200
                    });
                }
            }
            return Ok(new 
            {
                status = 500
            });
        }
    }
}
