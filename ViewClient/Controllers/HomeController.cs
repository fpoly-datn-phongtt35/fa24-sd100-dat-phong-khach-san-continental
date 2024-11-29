using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using ViewClient.Models;

namespace ViewClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
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
        public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int? maxiumOccupancy, int? quantityRoom)
        {
           var userName = HttpContext.Session.GetString("UserName");
            if (checkIn == null && checkOut == null && maxiumOccupancy == null && quantityRoom == null)
            {
                // If no parameters, return the view with an empty model
                var emptyResponse = new ResponseData<Domain.DTO.Room.RoomResponse>
                {
                    data = new List<Domain.DTO.Room.RoomResponse>(),
                    totalRecord = 0
                };
                return View("Index", emptyResponse);
            }
            var request = new SearchRoomsRequest
            {
                CheckIn = checkIn ?? DateTimeOffset.Now.AddDays(1),
                CheckOut = checkOut ?? DateTimeOffset.Now.AddDays(2),
                MaxiumOccupancy = maxiumOccupancy ?? 1,
                QuantityRoom = quantityRoom ?? 1
            };

            return await SearchRooms(request);
        }
        public async Task<IActionResult> SearchRooms(SearchRoomsRequest request)
        {
            string roomsRequestUrl = $"/api/Room/SearchRooms";

            try
            {
                // Gửi yêu cầu để lấy danh sách phòng
                var roomsResponse = await SendHttpRequest<RoomAvailableResponse>(roomsRequestUrl, HttpMethod.Post, request);

                if (roomsResponse.LstRoom == null || !roomsResponse.LstRoom.Any())
                {
                    return View("NoRoomsFound");
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
                var responseData = new ResponseData<Domain.DTO.Room.RoomResponse>
                {
                    data = roomsResponse.LstRoom,
                    totalRecord = roomsResponse.TotalRoom
                };
 
                return View("Index", responseData);
            }
            catch (Exception ex)
            {
                return View("Index", new ResponseData<Domain.DTO.Room.RoomResponse> { data = new List<Domain.DTO.Room.RoomResponse>() });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
