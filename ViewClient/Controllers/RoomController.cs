using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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

        public async Task<IActionResult> Index(string? name = null, Guid? roomTypeId = null, Guid? floorId = null, RoomStatus? status = null, int pageIndex = 1, int pageSize = 12)
        {
            // Tạo PagingRequest
            var roomRequest = new RoomRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoomTypeId = roomTypeId,
                FloorId = floorId,
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
    }
}
