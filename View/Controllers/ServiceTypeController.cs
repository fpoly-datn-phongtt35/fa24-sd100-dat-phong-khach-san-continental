using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
    public class ServiceTypeController : Controller
    {
        HttpClient _client;

        public ServiceTypeController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string name = null)
        {
            string requestUrl = "api/ServiceType/GetListServiceType";

            var request = new ServiceTypeGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Name = name
            };

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                var serviceTypes = JsonConvert.DeserializeObject<ResponseData<ServiceType>>(responseString);

                return View(serviceTypes);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: ServiceTypeController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/ServiceType/GetServiceTypeById?id={id}";

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
                var services = JsonConvert.DeserializeObject<ServiceType>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: ServiceTypeController/Create
        public async Task<IActionResult> Create()
        {
            return View(new ServiceTypeCreateRequest());
        }

        // POST: ServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                request.CreatedBy = Guid.NewGuid();
                var response = await _client.PostAsJsonAsync("api/ServiceType/CreateServiceType", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }

        // GET: ServiceController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            string requestUrl = $"api/ServiceType/GetServiceTypeById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            try
            {
                
                var response = await _client.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }


                var responseString = await response.Content.ReadAsStringAsync();
                var services = JsonConvert.DeserializeObject<ServiceType>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Service request)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _client.PutAsJsonAsync("api/ServiceType/UpdateServiceType", request);
            return RedirectToAction("Index");
        }

        // DELETE: ServiceTypeController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/ServiceType/DeleteServiceType";

            var request = new ServiceTypeDeleteRequest
            {
                Id = id,
                DeletedBy = Guid.NewGuid(),  //tạm thời, sau lấy giá trị từ người dùng đang đăng nhập
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }
    }
}
