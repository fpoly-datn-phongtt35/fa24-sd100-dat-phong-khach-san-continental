using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrder;
using Domain.DTO.ServiceOrderDetail;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
    public class ServiceOrderDetailController : Controller
    {
        HttpClient _client;

        public ServiceOrderDetailController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }
        // GET: ServiceOrderDetailController
        public async Task<IActionResult> Index()
        {
            string requestUrl = "api/ServiceOrderDetail/GetListServiceOrderDetail";

            var request = new ServiceOrderDetailGetRequest();

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                var serviceOrderDetails = JsonConvert.DeserializeObject<ResponseData<ServiceOrderDetail>>(responseString);

                #region lấy ra tên service
                string serviceRequestUrl = "api/Service/GetListService";

                var serviceRequest = new ServiceGetRequest();

                var serviceJsonRequest = JsonConvert.SerializeObject(serviceRequest);

                var serviceContent = new StringContent(serviceJsonRequest, Encoding.UTF8, "application/json");

                var serviceResponse = await _client.PostAsync(serviceRequestUrl, serviceContent);

                var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();

                var services = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

                ViewBag.ServiceList = services.data;
                #endregion

                return View(serviceOrderDetails);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: ServiceOrderDetailController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"api/ServiceOrderDetail/GetServiceOrderDetailById?id={id}";

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
                var services = JsonConvert.DeserializeObject<ServiceOrderDetail>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: ServiceOrderDetailController/Create
        public async Task<IActionResult> Create(Guid serviceOrderId)
        {
            // Lấy danh sách ServiceOrder
            string serviceOrderRequestUrl = "api/ServiceOrder/GetListServiceOrders";
            var serviceOrderResponse = await _client.PostAsync(serviceOrderRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceOrderResponseString = await serviceOrderResponse.Content.ReadAsStringAsync();
            var serviceOrders = JsonConvert.DeserializeObject<ResponseData<ServiceOrder>>(serviceOrderResponseString);

            ViewBag.ServiceOrders = serviceOrders?.data;

            // Lấy danh sách Service
            string serviceRequestUrl = "api/Service/GetListService"; 
            var serviceResponse = await _client.PostAsync(serviceRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();
            var services = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

            ViewBag.Services = services?.data;

            var request = new ServiceOrderDetailCreateRequest
            {
                ServiceOrderId = serviceOrderId
            };

            var servicePrices = services?.data.ToDictionary(s => s.Id.ToString(), s => s.Price);
            ViewBag.ServicePrices = servicePrices;

            return View(request);
        }

        // POST: ServiceOrderDetailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceOrderDetailCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                var response = await _client.PostAsJsonAsync("api/ServiceOrderDetail/AddServiceOrderDetail", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","ServiceOrder");
                }
                else
                {
                    // Nếu không thành công, có thể xem phản hồi để debug
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Error: {errorResponse}");
                }
            }
            return View(request); 
        }


        // GET: ServiceOrderDetailController/Edit/5
        public async Task<IActionResult> Edit(Guid id) 
        {
            // Lấy thông tin ServiceOrderDetail hiện tại
            string requestUrl = $"api/ServiceOrderDetail/GetServiceOrderDetailById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            var serviceOrderDetail = JsonConvert.DeserializeObject<ServiceOrderDetail>(responseString);

            // Lấy danh sách ServiceOrder
            string serviceOrderRequestUrl = "https://localhost:7130/api/ServiceOrder/GetListServiceOrders";
            var serviceOrderResponse = await _client.PostAsync(serviceOrderRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceOrderResponseString = await serviceOrderResponse.Content.ReadAsStringAsync();
            var serviceOrders = JsonConvert.DeserializeObject<ResponseData<ServiceOrder>>(serviceOrderResponseString);

            // Lấy danh sách Service
            string serviceRequestUrl = "https://localhost:7130/api/Service/GetListService"; 
            var serviceResponse = await _client.PostAsync(serviceRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();
            var services = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            ViewBag.ServiceOrders = serviceOrders?.data; 
            ViewBag.Services = services?.data; 

            return View(serviceOrderDetail); 
        }


        // POST: ServiceOrderDetailController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceOrderDetail request)
        {
            request.ModifiedTime = DateTimeOffset.Now;
            await _client.PutAsJsonAsync("api/ServiceOrderDetail/UpdateServiceOrderDetail", request);   
            return RedirectToAction("Index");
        }


        // GET: ServiceOrderDetailController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "api/ServiceOrderDetail/DeleteServiceOrderDetail";

            var request = new ServiceOrderDetailDeleteRequest
            {
                Id = id,
                DeletedBy = Guid.NewGuid(),
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            await _client.PostAsync(requestUrl, content);

            return RedirectToAction("Index");
        }
    }
}
