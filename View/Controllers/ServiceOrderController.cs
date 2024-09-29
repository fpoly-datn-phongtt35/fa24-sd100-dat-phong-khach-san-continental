using Domain.DTO.Paging;
using Domain.DTO.ServiceOrder;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace View.Controllers
{
    public class ServiceOrderController : Controller
    {
        HttpClient _client;

        public ServiceOrderController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }
        // GET: ServiceOrderController
        public async Task<IActionResult> Index()
        {
            string requestUrl = "https://localhost:7130/api/ServiceOrder/GetListServiceOrders";

            var request = new ServiceOrderGetRequest
            {
                PageIndex = 1,
                PageSize = 10
                ,RoomBookingDetailId = null
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServiceOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiceOrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
