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

        public async Task<IActionResult> GroupedServices(int pageIndex = 1, int pageSize = 1)
        {
            string requestUrl = $"api/Service/GetAllServiceNamesGroupedByServiceType?pageIndex={pageIndex}&pageSize={pageSize}";

            try
            {
                var response = await _client.GetAsync(requestUrl);

                var responseString = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

                if (apiResponse == null)
                {
                    return View("Error");
                }

                return View(apiResponse);
            }
            catch (JsonException ex)
            {
                return View(ex.Message, "Error");
            }
            catch (Exception ex)
            {
                return View(ex.Message, "Error");
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
