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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text;
using View.Models.Room;
using View.Models.Service;
using WEB.CMS.Customize;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace View.Controllers
{
    [CustomAuthorize]
    public class RoomController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        public RoomController(HttpClient httpClient, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
            _environment = environment;
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
            string floorsRequestUrl = "/api/Floor/GetListFloor";

            var floorsRequest = new FloorGetRequest();
            var floorJsonRequest = JsonConvert.SerializeObject(floorsRequest);
            var floorContent = new StringContent(floorJsonRequest, Encoding.UTF8, "application/json");
            var floorResponse = await _httpClient.PostAsync(floorsRequestUrl, floorContent);

            var floorResponseString = await floorResponse.Content.ReadAsStringAsync();
            var floorList = JsonConvert.DeserializeObject<ResponseData<Floor>>(floorResponseString);
            ViewBag.FloorList = floorList.data;
            var roomTypeGetRequest = new RoomTypeGetRequest();
            string roomTypeRequestUrl = "api/RoomType/GetFilteredRoomTypes";
            var roomTypesTask = await SendHttpRequest<ResponseData<RoomTypeResponse>>
                (roomTypeRequestUrl, HttpMethod.Post, roomTypeGetRequest);


            ViewBag.RoomTypes = roomTypesTask?.data ?? new List<RoomTypeResponse>();
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



            return View(new RoomCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCreateViewModel request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedTime = DateTimeOffset.Now;
                var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                request.CreatedBy = userId;
                var imagePaths = new List<string>();

                if (request.Images != null && request.Images.Count > 0)
                {
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "images");
                    foreach (var file in request.Images)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        imagePaths.Add($"/images/{fileName}");
                    }
                }
                var requestData = new
                {
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Address,
                    request.RoomSize,
                    request.FloorId,
                    request.RoomTypeId,
                    request.Status,
                    request.CreatedBy,
                    request.CreatedTime,
                    Images = imagePaths
                };


                
                // Gửi request đến API
                var response = await _httpClient.PostAsJsonAsync("/api/Room/CreateRoom", requestData);

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

            try
            {
                string requestUrl = $"/api/Room/GetRoomById?roomId={roomId}";

                // Gọi phương thức SendHttpRequest
                var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);

                if (room == null)
                {
                    return View("Error");
                }

                var updateModel = new RoomUpdateViewModel()
                {
                    Id = room.Id,
                    Name = room.Name,
                    Description = room.Description,
                    Price = room.Price,
                    Address = room.Address,
                    FloorId = room.FloorId,
                    RoomTypeId = room.RoomTypeId,
                    RoomSize = room.RoomSize,
                    Status = room.Status,
                    ExistingImages = room.Images?.Select(i => i.Image).ToList()
                };

                return View(updateModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RoomUpdateViewModel roomUpdateRequest)
        {
            string requestUrl = $"/api/Room/GetRoomById?roomId={roomUpdateRequest.Id}";
            var room = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Post);

            List<string> newImageUrls = new();
            if (roomUpdateRequest.NewImages != null && roomUpdateRequest.NewImages.Count > 0)
            {
                foreach (var image in roomUpdateRequest.NewImages)
                {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    newImageUrls.Add($"/images/{fileName}");
                }
            }

            // Kết hợp các ảnh còn lại và ảnh mới
            var finalImageList = room.Images
                .Where(i => roomUpdateRequest.DeletedImages == null || !roomUpdateRequest.DeletedImages.Contains(i.Image))
                .Select(i => i.Image)
                .Concat(newImageUrls)
                .ToList();

            // Chuyển danh sách ảnh thành chuỗi
            string imagesString = string.Join(",", finalImageList);

            // Gửi Request cập nhật phòng
            var updateroomRequest = new RoomUpdateRequest
            {
                Id = roomUpdateRequest.Id,
                Name = roomUpdateRequest.Name,
                Description = roomUpdateRequest.Description,
                Address = roomUpdateRequest.Address,
                Price = roomUpdateRequest.Price,
                FloorId = roomUpdateRequest.FloorId,
                RoomSize = roomUpdateRequest.RoomSize,
                Status = roomUpdateRequest.Status,
                RoomTypeId = roomUpdateRequest.RoomTypeId,
                ModifiedTime = DateTimeOffset.Now,
                Images = finalImageList, // Giữ nguyên kiểu List<string> để chuyển đổi bên trong phương thức UpdateRoom
            };

            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            updateroomRequest.ModifiedBy = userId;
            var response1 = await _httpClient.PutAsJsonAsync("api/Room/UpdateRoom", updateroomRequest);

            if (response1.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật phòng.");
            return View(roomUpdateRequest);
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
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            DeleteRequest.DeletedBy = userId;
            var deleted = await SendHttpRequest<RoomResponse>(requestUrl, HttpMethod.Put, DeleteRequest);
            if (deleted != null)
                return RedirectToAction("Index");
            return View("Error");
        }
    }
}
