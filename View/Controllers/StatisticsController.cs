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
        public async Task<IActionResult> Index(int? selectedMonthCustomer, int? selectedYearCustomer, int? selectedMonthRoom, int? selectedYearRoom, string revenueFilterType = "Month")
        {
            try
            {
                if (revenueFilterType != "Month" && revenueFilterType != "Year")
                {
                    revenueFilterType = "Month";
                }

                string infoRequestUrl = "https://localhost:7130/api/Room/GetHotelInfo";
                var hotelInfo = await SendHttpRequest<HotelInfoDto>(infoRequestUrl, HttpMethod.Post, null);
                ViewBag.HotelInfo = hotelInfo;

                var revenueRequestUrl = $"api/Room/GetRevenueAsync?revenueFilterType={revenueFilterType}";
                var revenueData = await SendHttpRequest<List<GetRevenue>>(revenueRequestUrl, HttpMethod.Post);
                var periods = revenueData.Select(x => x.Period).ToList();
                var totalAmounts = revenueData.Select(x => x.TotalAmount).ToList();

                if (selectedMonthCustomer == null) selectedMonthCustomer = DateTime.Now.Month;
                if (selectedYearCustomer == null) selectedYearCustomer = DateTime.Now.Year;

                var topCustomerRequestUrl = $"api/Room/GetTopCustomerBookings?selectedMonthCustomer={selectedMonthCustomer}&selectedYearCustomer={selectedYearCustomer}";
                var topCustomerData = await SendHttpRequest<List<TopCustomerBooking>>(topCustomerRequestUrl, HttpMethod.Post);

                if (selectedMonthRoom == null) selectedMonthRoom = DateTime.Now.Month;
                if (selectedYearRoom == null) selectedYearRoom = DateTime.Now.Year;

                var topRoomRequestUrl = $"api/Room/GetTopBookingRoomsAsync?selectedMonthRoom={selectedMonthRoom}&selectedYearRoom={selectedYearRoom}";
                var topRoomData = await SendHttpRequest<List<TopRoomBookingViewModel>>(topRoomRequestUrl, HttpMethod.Post);

                // Lấy dữ liệu tỷ lệ phủ tuần
                var coverageWeeklyUrl = "/api/Room/GetWeeklyCoverage";
                var weeklyCoverageResponse = await SendHttpRequest<List<WeeklyCoverageDto>>(coverageWeeklyUrl, HttpMethod.Get);
                List<double> weeklyCoverageRatios = new List<double>();
                List<string> weeklyLabels = new List<string>();

                if (weeklyCoverageResponse != null)
                {
                    weeklyCoverageRatios = weeklyCoverageResponse.Select(x => x.CoverageRatio).ToList();
                    weeklyLabels = weeklyCoverageResponse.Select(x => x.Date.ToString("dd/MM/yyyy")).ToList(); // Chuyển DateTime thành chuỗi ngày
                }

                // Lấy dữ liệu tỷ lệ phủ tháng
                var coverageMonthlyUrl = "/api/Room/GetMonthlyCoverage";
                var monthlyCoverageResponse = await SendHttpRequest<List<MonthlyCoverageDto>>(coverageMonthlyUrl, HttpMethod.Get);
                List<double> monthlyCoverageRatios = new List<double>();
                List<string> monthlyLabels = new List<string>();

                if (monthlyCoverageResponse != null)
                {
                    monthlyCoverageRatios = monthlyCoverageResponse.Select(x => x.CoverageRatio).ToList();
                    monthlyLabels = monthlyCoverageResponse.Select(x => $"{x.MonthNumber}/{x.YearNumber}").ToList(); // Chuyển đổi thành "MM/YYYY"
                }

                // Truyền dữ liệu vào ViewBag
                ViewBag.WeeklyCoverageData = weeklyCoverageRatios;
                ViewBag.WeeklyLabels = weeklyLabels;
                ViewBag.MonthlyCoverageData = monthlyCoverageRatios;
                ViewBag.MonthlyLabels = monthlyLabels;



                // Truyền dữ liệu vào ViewBag
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
