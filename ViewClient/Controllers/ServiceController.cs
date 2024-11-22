using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewClient.ViewModels;

namespace ViewClient.Controllers
{
    public class ServiceController : Controller
    {
        HttpClient _client;

        public ServiceController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> GroupedServices()
        {
            string requestUrl = "api/Service/GetAllServiceNamesGroupedByServiceType";

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

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"api/Service/GetServiceById?id={id}";

            // Tạo nội dung json cho request
            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var services = JsonConvert.DeserializeObject<Service>(responseString);

                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
