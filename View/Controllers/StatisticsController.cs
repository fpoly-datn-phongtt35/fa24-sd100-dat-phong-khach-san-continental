using System.Net.Http;
using System.Text;
using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace View.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IRoomRepo _roomRepo;
        private readonly HttpClient _httpClient;
        public StatisticsController(HttpClient httpClient, IRoomRepo roomRepo)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
            _roomRepo = roomRepo;
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
        public async Task<IActionResult> Revenue(string revenueFilterType = "Month")
        {
            if (revenueFilterType != "Month" && revenueFilterType != "Year")
            {
                revenueFilterType = "Month"; // Đặt lại filterType nếu giá trị không hợp lệ
            }

            try
            {
                var requestUrl = $"api/Room/GetRevenueAsync?revenueFilterType={revenueFilterType}";
                var topRoomBooking = await SendHttpRequest<List<GetRevenue>>(requestUrl, HttpMethod.Post);

                // Chuyển dữ liệu thành các định dạng phù hợp cho biểu đồ
                var periods = topRoomBooking.Select(x => x.Period).ToList();
                var totalAmounts = topRoomBooking.Select(x => x.TotalAmount).ToList();

                ViewBag.Periods = periods;
                ViewBag.TotalAmounts = totalAmounts;

                return View(topRoomBooking); // Trả về view với dữ liệu doanh thu
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }
        public async Task<IActionResult> Index(string revenueFilterType = "Month", string customerFilterType = "Month", string roomFilterType = "Month")
        {
            try
            {
                if (revenueFilterType != "Month" && revenueFilterType != "Year")
                {
                    revenueFilterType = "Month"; // Đặt lại filterType nếu giá trị không hợp lệ
                }
                if (customerFilterType != "Month" && customerFilterType != "Year")
                {
                    customerFilterType = "Month"; // Đặt lại filterType nếu giá trị không hợp lệ
                }
                if (roomFilterType != "Month" && roomFilterType != "Year")
                {
                    roomFilterType = "Month"; // Đặt lại filterType nếu giá trị không hợp lệ
                }
                // Lấy dữ liệu doanh thu
                var revenueRequestUrl = $"api/Room/GetRevenueAsync?revenueFilterType={revenueFilterType}";
                var revenueData = await SendHttpRequest<List<GetRevenue>>(revenueRequestUrl, HttpMethod.Post);

                var periods = revenueData.Select(x => x.Period).ToList();
                var totalAmounts = revenueData.Select(x => x.TotalAmount).ToList();

                // Lấy danh sách top khách hàng
                var topCustomerRequestUrl = $"api/Room/GetTopCustomerBookings?customerFilterType={customerFilterType}";
                var topCustomerData = await SendHttpRequest<List<TopCustomerBooking>>(topCustomerRequestUrl, HttpMethod.Post);

                // Lấy danh sách top phòng
                var topRoomRequestUrl = $"api/Room/GetTopBookingRoomsAsync?roomFilterType={roomFilterType}";
                var topRoomData = await SendHttpRequest<List<TopRoomBookingViewModel>>(topRoomRequestUrl, HttpMethod.Post);

                // Truyền dữ liệu vào ViewBag
                ViewBag.Periods = periods;
                ViewBag.TotalAmounts = totalAmounts;
                ViewBag.TopCustomers = topCustomerData;
                ViewBag.TopRooms = topRoomData;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }




    }
}
