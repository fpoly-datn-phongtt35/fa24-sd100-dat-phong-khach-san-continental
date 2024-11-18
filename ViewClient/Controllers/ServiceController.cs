using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ViewClient.Controllers
{
    public class ServiceController : Controller
    {
        private readonly HttpClient _client;

        public ServiceController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> GroupedServices()
        {
            string requestUrl = "https://localhost:7130/api/Service/GetAllServiceNamesGroupedByServiceType";

            try
            {
                var response = await _client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Không thể lấy dữ liệu từ API.";
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                var groupedServices = JsonConvert.DeserializeObject<List<GroupedServiceViewModel>>(responseString);

                if (groupedServices == null || !groupedServices.Any())
                {
                    ViewBag.ErrorMessage = "Không có dữ liệu hợp lệ để hiển thị.";
                    return View("Error");
                }

                return View(groupedServices); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi khi gọi API: " + ex.Message;
                return View("Error");
            }
        }
    }

    public class GroupedServiceViewModel
    {
        [JsonProperty("serviceTypeName")]
        public string ServiceTypeName { get; set; } 

        [JsonProperty("serviceNames")]
        public List<string> ServiceNames { get; set; } 
    }

}
