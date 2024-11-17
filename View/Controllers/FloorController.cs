using Domain.DTO.Building;
using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class FloorController : Controller
    {
        HttpClient _client;

        public FloorController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string Name = null, Guid? BuildingId = null, EntityStatus? status = null,int? numberofroom=null)
        {
            string requestUrl = "api/Floor/GetListFloor";

            var request = new FloorGetRequest { 
                PageIndex= pageIndex,
                PageSize= pageSize,
                Name= Name,
                BuildingId= BuildingId,
                Status= status,
                NumberOfRoom= numberofroom
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var floors = JsonConvert.DeserializeObject<ResponseData<Floor>>(responseString);
                string buildingrequestUrl = "api/Building/GetListBuilding";
                var buildingRequest = new BuildingGetRequest();
                var buildingJsonRequest = JsonConvert.SerializeObject(buildingRequest);
                var buildingContent = new StringContent(buildingJsonRequest, Encoding.UTF8, "application/json");

                var buildingResponse = await _client.PostAsync(buildingrequestUrl, buildingContent);

                var buildingResponseString = await buildingResponse.Content.ReadAsStringAsync();

                var building = JsonConvert.DeserializeObject<ResponseData<Building>>(buildingResponseString);

                ViewBag.BuildingList = building.data;
                ViewBag.StatusList = Enum.GetValues(typeof(EntityStatus));
                return View(floors);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        // GET: ServiceController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"api/Floor/GetFloorById?id={id}";

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
                var services = JsonConvert.DeserializeObject<Floor>(responseString);

                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            string buildingRequestUrl = "api/Building/GetListBuilding";
            var buildingResponse = await _client.PostAsync(buildingRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var buildingResponseString = await buildingResponse.Content.ReadAsStringAsync();
            var building = JsonConvert.DeserializeObject<ResponseData<Building>>(buildingResponseString);


            ViewBag.Buildings = building?.data;
            return View(new FloorCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FloorCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                request.CreatedBy = userId;
                var response = await _client.PostAsJsonAsync("api/Floor/CreateFloor", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            string buildingRequestUrl = "api/Building/GetListBuilding";
            var buildingResponse = await _client.PostAsync(buildingRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var buildingResponseString = await buildingResponse.Content.ReadAsStringAsync();
            var building = JsonConvert.DeserializeObject<ResponseData<ServiceType>>(buildingResponseString);

            ViewBag.Buildings = building?.data;


            string requestUrl = $"api/Floor/GetFloorById?id={id}";

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
                var floors = JsonConvert.DeserializeObject<Floor>(responseString);



                return View(floors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Floor request)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.ModifiedBy = userId;
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _client.PutAsJsonAsync("api/Floor/UpdateFloor", request);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/Floor/DeleteFloor";

            var request = new FloorDeleteRequest
            {
                Id = id,
                DeletedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), 
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
