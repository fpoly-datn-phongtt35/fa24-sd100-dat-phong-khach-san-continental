using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace View.Controllers
{
    public class RoomController : Controller
    {
        private readonly HttpClient _httpClient;
        public RoomController(HttpClient httpClient)
        {
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

        public async Task<IActionResult> Index(string? name= null, Guid? roomTypeId=null, Guid? floorId = null, RoomStatus? status=null, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo PagingRequest
            var roomRequest = new RoomRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoomTypeId=roomTypeId,
                FloorId=floorId,
                Name = name,
                Status = status
            };

            // Tạo URL yêu cầu cho danh sách phòng
            string roomsRequestUrl = $"/api/Room/GetAllRooms";

            try
            {
                // Gửi yêu cầu để lấy danh sách phòng
                var roomsResponse = await SendHttpRequest<ResponseData<RoomResponse>>(roomsRequestUrl, HttpMethod.Post, roomRequest);

                if (roomsResponse == null)
                {
                    return View("Error", new Exception("Không thể lấy danh sách phòng."));
                }
                        // Gửi yêu cầu lấy danh sách tầng
                        string floorsRequestUrl = "/api/Floor/GetListFloor";
                var floorsRequest = new FloorGetRequest();
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
                return View("Error", ex);
            }
        }



        public async Task<IActionResult> Details(Guid roomId)
        {
            string requestUrl = $"/api/Room/GetRoomById?roomId={roomId}";


            var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);
            if (room != null)
                return View(room);

            return View("Error");
        }

        public async Task<IActionResult> Create()
        {
            // Lấy danh sách tầng
            string floorRequestUrl = "api/Floor/GetListFloor";
            var floorResponse = await _httpClient.PostAsync(floorRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var floorResponseString = await floorResponse.Content.ReadAsStringAsync();
            var floor = JsonConvert.DeserializeObject<ResponseData<Floor>>(floorResponseString);
            ViewBag.Floors = floor?.data;

            // Lấy danh sách loại phòng
            string roomTypeRequestUrl = "api/RoomType/GetFilteredRoomTypes";
            var roomTypeResponse = await _httpClient.PostAsync(roomTypeRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var roomTypeResponseString = await roomTypeResponse.Content.ReadAsStringAsync();
            var roomTypes = JsonConvert.DeserializeObject<ResponseData<RoomType>>(roomTypeResponseString);
            ViewBag.RoomTypes = roomTypes?.data; // Chỉ lấy dữ liệu



            return View(new RoomCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCreateRequest request, List<IFormFile> imgFiles)
        {
            if (ModelState.IsValid)
            {
                request.CreatedTime = DateTimeOffset.Now;

                // Xử lý các tệp hình ảnh
                if (imgFiles != null && imgFiles.Count > 0)
                {
                    foreach (var imgFile in imgFiles)
                    {
                        if (imgFile.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imgFile.FileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await imgFile.CopyToAsync(stream);
                            }
                            request.Images.Add(imgFile.FileName);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không có tệp nào được chọn.");
                }


                // Gửi request đến API
                var response = await _httpClient.PostAsJsonAsync("api/Room/CreateRoom", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể tạo phòng. Vui lòng thử lại.");
                }
            }
            return View(request);
        }



        public async Task<IActionResult> Edit(Guid roomId)
        {
            // Lấy danh sách tầng
            string floorRequestUrl = "api/Floor/GetListFloor";
            var floorResponse = await _httpClient.PostAsync(floorRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var floorResponseString = await floorResponse.Content.ReadAsStringAsync();
            var floor = JsonConvert.DeserializeObject<ResponseData<Floor>>(floorResponseString);
            ViewBag.Floors = floor?.data;

            // Lấy danh sách loại phòng
            string roomTypeRequestUrl = "api/RoomType/GetFilteredRoomTypes";
            var roomTypeResponse = await _httpClient.PostAsync(roomTypeRequestUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
            var roomTypeResponseString = await roomTypeResponse.Content.ReadAsStringAsync();
            var roomTypes = JsonConvert.DeserializeObject<ResponseData<RoomType>>(roomTypeResponseString);
            ViewBag.RoomTypes = roomTypes?.data; // Chỉ lấy dữ liệu

            string requestUrl = $"/api/Room/GetRoomById?roomId={roomId}";

            var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);
            if (room != null)


            return View(room);
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomUpdateRequest roomUpdateRequest, List<IFormFile> imgFiles)
        {
            if (imgFiles != null && imgFiles.Count > 0)
            {
                foreach (var imgFile in imgFiles)
                {
                    if (imgFile.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imgFile.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(stream);
                        }
                        roomUpdateRequest.Images.Add(imgFile.FileName);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không có tệp nào được chọn.");
            }


            string requestUrl = $"api/Room/UpdateRoom?roomId={roomUpdateRequest.Id}";

            var updatedRoom = await SendHttpRequest<RoomResponse>(requestUrl,HttpMethod.Put, roomUpdateRequest);
            if (updatedRoom != null)
                return RedirectToAction("Index");

            return View("Error");
        }

        public async Task<IActionResult> Delete(Guid roomId)
        {
            string requestUrl = $"/api/Room/GetRoomById?roomId={roomId}";

            var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);
            if (room != null)
            return View(room);
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoomDeleteRequest DeleteRequest)
        {
            string requestUrl = $"/api/Room/DeleteRoom?roomId={DeleteRequest.Id}";

            var deleted = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Put, DeleteRequest);
            if (deleted != null)
                return RedirectToAction("Index");
            return View("Error");
        }
    }
}
