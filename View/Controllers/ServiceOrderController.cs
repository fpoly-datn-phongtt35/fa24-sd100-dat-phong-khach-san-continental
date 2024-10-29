using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrder;
using Domain.DTO.ServiceOrderDetail;
using Domain.DTO.Voucher;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class ServiceOrderController : Controller
    {
        HttpClient _client;

        public ServiceOrderController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }
        // GET: ServiceOrderController
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, Guid? roomBookingId = null)
        {
            string requestUrl = "https://localhost:7130/api/ServiceOrder/GetListServiceOrders";

            var request = new ServiceOrderGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoomBookingId = roomBookingId
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                var serviceOrders = JsonConvert.DeserializeObject<ResponseData<ServiceOrder>>(responseString);

                if (serviceOrders == null || serviceOrders.data == null)
                {
                    serviceOrders = new ResponseData<ServiceOrder>
                    {
                        data = new List<ServiceOrder>(),
                        totalPage = 0,
                        totalRecord = 0,
                        CurrentPage = 1,
                        PageSize = 10
                    };
                }

                return View(serviceOrders);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: ServiceOrderController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/ServiceOrder/GetServiceOrderById?id={id}";

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
                var services = JsonConvert.DeserializeObject<ServiceOrder>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: ServiceOrderController/Create
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách ServiceOrder
            string rbRequestUrl = "api/RoomBooking/GetFilteredRoomBookings";
            var rbResponse = await _client.PostAsync(rbRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var rbResponseString = await rbResponse.Content.ReadAsStringAsync();
            var rbs = JsonConvert.DeserializeObject<ResponseData<RoomBooking>>(rbResponseString);

            ViewBag.RoomBookings = rbs?.data;
            return View(new ServiceOrderCreateRequest());
        }

        // POST: ServiceOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceOrderCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                var response = await _client.PostAsJsonAsync("api/ServiceOrder/CreateServiceOrder", request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var createdServiceOrder = JsonConvert.DeserializeObject<Guid>(responseString);


                    return RedirectToAction("Create", "ServiceOrderDetail", new { serviceOrderId = createdServiceOrder });
                }
            }

            return View(request);
        }

        // GET: ServiceOrderController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            string requestUrl = $"api/ServiceOrder/GetServiceOrderById?id={id}";

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

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
                var services = JsonConvert.DeserializeObject<ServiceOrder>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: ServiceOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceOrder request)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            request.ModifiedTime = DateTimeOffset.Now;
            await _client.PutAsJsonAsync("api/ServiceOrder/UpdateServiceOrder", request);
            return RedirectToAction("Index");
        }

        // DELETE: ServiceOrderController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/ServiceOrder/DeleteServiceOrder";

            var request = new ServiceOrderDeleteRequest
            {
                Id = id,
                DeletedBy = Guid.NewGuid(),  
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error", new Exception("Unable to delete the service."));
        }

        // GET: ServiceOrderController/Services/{id}
        public async Task<IActionResult> Services(Guid id)
        {
            string requestUrl = $"api/ServiceOrderDetail/GetServiceOrderDetailByServiceOrderId?id={id}";

            // Lấy danh sách Service
            string serviceRequestUrl = "api/Service/GetListService";
            var serviceResponse = await _client.PostAsync(serviceRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();
            var services = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

            ViewBag.Services = services?.data;

            var request = new ServiceOrderDetailGetRequest
            {
                ServiceOrderId = id
            };

            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var respone = await _client.PostAsync(requestUrl, content);

                var responseString = await respone.Content.ReadAsStringAsync();

                var serviceOrderDetails = JsonConvert.DeserializeObject<ResponseData<ServiceOrderDetail>>(responseString);  

                return View(serviceOrderDetails);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
