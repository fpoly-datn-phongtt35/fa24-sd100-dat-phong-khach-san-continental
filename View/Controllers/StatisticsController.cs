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
        //[HttpGet]
        //public IActionResult GetCustomerDetail(Guid id)
        //{
        //    // Lấy thông tin khách hàng từ cơ sở dữ liệu
        //    var customer = _customerService.GetTopCustomerById(id); // Giả sử bạn có service để lấy thông tin khách hàng
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    // Trả về thông tin dạng JSON
        //    return Json(new
        //    {
        //        id = customer.Id,
        //        firstName = customer.FirstName,
        //        lastName = customer.LastName,
        //        email = customer.Email,
        //        phoneNumber = customer.PhoneNumber,
        //        gender = customer.Gender,
        //        bookingCount = customer.BookingCount,
        //        totalPrice = customer.TotalPrice
        //    });
        //}
        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Customer/GetCustomerById?Id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseString);

                return View(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        public async Task<IActionResult> Index(int? month, int? year, int? selectedMonthCustomer, int? selectedYearCustomer, int? selectedMonthRoom, int? selectedYearRoom, string revenueFilterType = "Month")
        {
            try
            {
                // Kiểm tra giá trị đầu vào của revenueFilterType
                if (revenueFilterType != "Month" && revenueFilterType != "Year")
                {
                    revenueFilterType = "Month"; // Đặt lại giá trị mặc định nếu không hợp lệ
                }

                // Lấy dữ liệu doanh thu
                var revenueRequestUrl = $"api/Room/GetRevenueAsync?revenueFilterType={revenueFilterType}";
                var revenueData = await SendHttpRequest<List<GetRevenue>>(revenueRequestUrl, HttpMethod.Post);

                // Ánh xạ dữ liệu doanh thu
                var periods = revenueData.Select(x => x.Period).ToList();
                    var totalAmounts = revenueData.Select(x => x.TotalAmount).ToList();

                // Lấy dữ liệu top khách hàng
                if (selectedMonthCustomer == null) selectedMonthCustomer = DateTime.Now.Month; // Mặc định là tháng hiện tại
                if (selectedYearCustomer == null) selectedYearCustomer = DateTime.Now.Year; // Mặc định là năm hiện tại

                var topCustomerRequestUrl = $"api/Room/GetTopCustomerBookings?selectedMonthCustomer={selectedMonthCustomer}&selectedYearCustomer={selectedYearCustomer}";
                var topCustomerData = await SendHttpRequest<List<TopCustomerBooking>>(topCustomerRequestUrl, HttpMethod.Post);

                // Lấy dữ liệu top phòng
                if (selectedMonthRoom == null) selectedMonthRoom = DateTime.Now.Month; // Mặc định là tháng hiện tại
                if (selectedYearRoom == null) selectedYearRoom = DateTime.Now.Year; // Mặc định là năm hiện tại

                var topRoomRequestUrl = $"api/Room/GetTopBookingRoomsAsync?selectedMonthRoom={selectedMonthRoom}&selectedYearRoom={selectedYearRoom}";
                var topRoomData = await SendHttpRequest<List<TopRoomBookingViewModel>>(topRoomRequestUrl, HttpMethod.Post);

                // coverage ratio
                if (month == null) month = DateTime.Now.Month; 
                if (year == null) year = DateTime.Now.Year;
                var coverageRatioUrl = $"api/Room/GetCoverageRatio?month={month}&year={year}";

                var coverageRatioResponse = await SendHttpRequest<object>(coverageRatioUrl, HttpMethod.Post);  

                float? coverageRatio = null;

                if (coverageRatioResponse != null)
                {
                    if (float.TryParse(coverageRatioResponse.ToString(), out float parsedValue))
                    {
                        coverageRatio = parsedValue;
                    }
                }

                if (coverageRatio.HasValue)
                {
                    ViewBag.CoverageRatio = coverageRatio.Value;
                }
                else
                {
                    ViewBag.CoverageRatio = "No data available"; 
                }

                // Truyền dữ liệu vào ViewBag
                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Periods = periods;
                ViewBag.TotalAmounts = totalAmounts;
                ViewBag.TopCustomers = topCustomerData;
                ViewBag.TopRooms = topRoomData;
                ViewBag.SelectedMonthCustomer = selectedMonthCustomer;
                ViewBag.SelectedYearCustomer = selectedYearCustomer;
                ViewBag.SelectedMonthRoom = selectedMonthRoom;
                ViewBag.SelectedYearRoom = selectedYearRoom;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }





    }
}
