﻿using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.DTO.Service;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ViewClient.Models;
using ViewClient.Models.DTO;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoom _roomRepo;
        private readonly HttpClient _httpClient;
        public RoomController(IRoom roomRepo, HttpClient httpClient)
        {
            _roomRepo = roomRepo;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
        }
        private async Task<T?> SendHttpRequest<T>(string requestUrl, HttpMethod method, object? body = null)
           where T : class
        {
            try
            {

                HttpRequestMessage request = new HttpRequestMessage(method, requestUrl);
                if (body != null)
                {
                    var json = JsonConvert.SerializeObject(body);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                var response = await _httpClient.SendAsync(request);

                if (response == null)
                {
                    throw new NullReferenceException("Response is null");
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IActionResult> SearchRooms(SearchRoomsRequest request)
        {


            string roomsRequestUrl = $"/api/Room/SearchRooms";

            try
            {
                if (request.CheckIn != null)
                {
                    HttpContext.Session.SetString("CheckIn", request.CheckIn.ToString("yyyy-MM-dd"));
                }
                if (request.CheckOut != null)
                {
                    HttpContext.Session.SetString("CheckOut", request.CheckOut.ToString("yyyy-MM-dd"));
                }
                if (request.MaxiumOccupancy != null)
                {
                    HttpContext.Session.SetInt32("MaxiumOccupancy", request.MaxiumOccupancy);
                }
                if (request.QuantityRoom != null)
                {
                    HttpContext.Session.SetInt32("QuantityRoom", request.QuantityRoom);
                }
                // Gửi yêu cầu để lấy danh sách phòng
                var roomsResponse = await SendHttpRequest<RoomAvailableResponse>(roomsRequestUrl, HttpMethod.Post, request);

                if (roomsResponse == null)
                {
                    return View("Error", new Exception("Không thể lấy danh sách phòng."));
                }
                // Gửi yêu cầu lấy danh sách tầng
                string floorsRequestUrl = "/api/Floor/GetListFloor";
                var floorsRequest = new FloorGetRequest
                {
                    Status = EntityStatus.Active
                };
                var floorJsonRequest = JsonConvert.SerializeObject(floorsRequest);
                var floorContent = new StringContent(floorJsonRequest, Encoding.UTF8, "application/json");
                var floorResponse = await _httpClient.PostAsync(floorsRequestUrl, floorContent);

                var floorResponseString = await floorResponse.Content.ReadAsStringAsync();
                var floorList = JsonConvert.DeserializeObject<ResponseData<Floor>>(floorResponseString);
                ViewBag.FloorList = floorList.data;
                // Lấy danh sách trạng thái
                ViewBag.StatusList = Enum.GetValues(typeof(RoomStatus));
                var roomTypeGetRequest = new RoomTypeGetRequest();
                string roomTypeRequestUrl = "api/RoomType/GetFilteredRoomTypes";
                var roomTypesTask = await SendHttpRequest<ResponseData<RoomTypeResponse>>
                    (roomTypeRequestUrl, HttpMethod.Post, roomTypeGetRequest);

                ViewBag.RoomTypes = roomTypesTask?.data ?? new List<RoomTypeResponse>();

                return View(roomsResponse); // Trả về danh sách phòng
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }
        private async Task<IEnumerable<Service>> IndexService()
        {
            // api url
            string requestUrl = $"/api/Service/GetListService";

            var request = new ServiceGetRequest
            {
                PageIndex = 1,
                PageSize = 100,
                Status = EntityStatus.Active
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var servicesResponse = JsonConvert.DeserializeObject<ResponseData<Service>>(responseString);
                return servicesResponse.data;
            }

            return new List<Service>();
        }
        public async Task<IActionResult> Details(Guid roomId)
        {
            string requestUrl = $"/api/Room/GetRoomById?roomId={roomId}";
            string floorsRequestUrl = "/api/Floor/GetListFloor";

            try
            {
                // Lấy danh sách tầng
                var floorResponse = await _httpClient.PostAsync(floorsRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
                var floorResponseString = await floorResponse.Content.ReadAsStringAsync();
                var floorList = JsonConvert.DeserializeObject<ResponseData<Floor>>(floorResponseString);

                // Lấy danh sách loại phòng
                var roomTypeGetRequest = new RoomTypeGetRequest();
                string roomTypeRequestUrl = "api/RoomType/GetFilteredRoomTypes";
                var roomTypesTask = await SendHttpRequest<ResponseData<RoomTypeResponse>>(roomTypeRequestUrl, HttpMethod.Post, roomTypeGetRequest);

                // Lấy thông tin phòng
                var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);
                if (room != null)
                {
                    // Lấy danh sách dịch vụ
                    var services = await IndexService();
                    var roomType = roomTypesTask?.data?.FirstOrDefault(rt => rt.Id == room.RoomTypeId);
                    var floor = floorList?.data?.FirstOrDefault(f => f.Id == room.FloorId);
                    var viewModel = new RoomDetailViewModel
                    {
                        Room = room,
                        Floor = floor,
                        RoomType = roomType,
                        Services = services
                    };

                    return View(viewModel);
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi ở đây nếu cần
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

    }
}
