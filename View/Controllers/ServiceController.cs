using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http;
using System.Text;
using View.Models;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class ServiceController : Controller
    {
        HttpClient _client;

        public ServiceController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        // GET: ServiceController
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string serviceName = null, Guid? serviceTypeId = null, decimal? minPrice = null, decimal? maxPrice = null, EntityStatus? status = null)
        {
            // api url
            string requestUrl = "api/Service/GetListService";

            var request = new ServiceGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Name = serviceName,
                ServiceTypeId = serviceTypeId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Status = status
            };

            // comvert request to json
            var jsonRequest = JsonConvert.SerializeObject(request); //chuyển đối tượng thành json

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"); //nội để gửi lên api

            try
            {
                // gửi request lên api
                var response = await _client.PostAsync(requestUrl, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var services = JsonConvert.DeserializeObject<ResponseData<Service>>(responseString);

                #region lấy ra name của serviceType
                string serviceTyperequestUrl = "api/ServiceType/GetListServiceType";

                var serviceTypeRequest = new ServiceTypeGetRequest();

                var serviceTypeJsonRequest = JsonConvert.SerializeObject(serviceTypeRequest);
                var serviceTypeContent = new StringContent(serviceTypeJsonRequest, Encoding.UTF8, "application/json");

                var serviceTypeResponse = await _client.PostAsync(serviceTyperequestUrl, serviceTypeContent);

                var serviceTypeResponseString = await serviceTypeResponse.Content.ReadAsStringAsync();

                var serviceTypes = JsonConvert.DeserializeObject<ResponseData<ServiceType>>(serviceTypeResponseString);

                ViewBag.ServiceTypeList = serviceTypes.data;
                #endregion

                ViewBag.StatusList = Enum.GetValues(typeof(EntityStatus));

                return View(services);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        // GET: ServiceController/Details/5
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


        // GET: ServiceController/Create
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách ServiceType
            string serviceTypeRequestUrl = "api/ServiceType/GetListServiceType";
            var serviceTypeResponse = await _client.PostAsync(serviceTypeRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceTypeResponseString = await serviceTypeResponse.Content.ReadAsStringAsync();
            var serviceType = JsonConvert.DeserializeObject<ResponseData<ServiceType>>(serviceTypeResponseString);


            ViewBag.ServiceTypes = serviceType?.data;
            ViewBag.Units = Enum.GetValues(typeof(UnitType));
            return View(new ServiceCreateRequest());
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
                var response = await _client.PostAsJsonAsync("api/Service/CreateService", request);

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
            // Lấy danh sách ServiceType
            string serviceTypeRequestUrl = "api/ServiceType/GetListServiceType";
            var serviceTypeResponse = await _client.PostAsync(serviceTypeRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var serviceTypeResponseString = await serviceTypeResponse.Content.ReadAsStringAsync();
            var serviceType = JsonConvert.DeserializeObject<ResponseData<ServiceType>>(serviceTypeResponseString);

            ViewBag.ServiceTypes = serviceType?.data;


            //lấy view hiện tại
            string requestUrl = $"api/Service/GetServiceById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Units = Enum.GetValues(typeof(UnitType));
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

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

        // POST: ServiceController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Service request)
        {
            ViewBag.Units = Enum.GetValues(typeof(UnitType));
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _client.PutAsJsonAsync("api/Service/UpdateService", request);
            return RedirectToAction("Index"); 
        }


        // DELETE: ServiceController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/Service/DeleteService";

            var request = new ServiceDeleteRequest
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
            return View("Error", new Exception("Unable to delete the service."));
        }
    }
}
