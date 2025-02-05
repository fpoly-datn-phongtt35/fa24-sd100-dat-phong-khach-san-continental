﻿using Domain.DTO.Building;
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
    public class BuildingController : Controller
    {
        HttpClient _client;

        public BuildingController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5,string name=null, EntityStatus? status = null)
        {
            // api url
            string requestUrl = "https://localhost:7130/api/Building/GetListBuilding";

            var request = new BuildingGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Name = name,
                Status = status
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var services = JsonConvert.DeserializeObject<ResponseData<Building>>(responseString);
                ViewBag.StatusList = Enum.GetValues(typeof(EntityStatus));
                return View(services);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Building/GetBuildingById?id={id}";

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
                var services = JsonConvert.DeserializeObject<Building>(responseString);



                return View(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            return View(new BuildingCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildingCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;
                var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                request.CreatedBy = userId;
                var response = await _client.PostAsJsonAsync("api/Building/CreateBuilding", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            string requestUrl = $"api/Building/GetBuildingById?id={id}";

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
                var buildings = JsonConvert.DeserializeObject<Building>(responseString);



                return View(buildings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Building request)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.ModifiedBy = userId;
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _client.PutAsJsonAsync("api/Building/UpdateBuilding", request);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/Building/DeleteBuilding";
            var request = new BuildingDeleteRequest
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
            return View("Error", new Exception("Unable to delete the building."));
        }
    }
}
