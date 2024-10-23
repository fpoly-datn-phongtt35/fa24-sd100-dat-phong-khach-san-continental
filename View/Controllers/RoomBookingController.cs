using System.Text;
using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB.CMS.Customize;

namespace View.Controllers;
/*[CustomAuthorize]*/
public class RoomBookingController : Controller
{
    private readonly HttpClient _httpClient;

    public RoomBookingController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7130/api/");
    }
    
    private async Task<T?> SendHttpRequest<T>(string requestUrl, HttpMethod method, object? body = null) 
        where T : class
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(method, requestUrl);

            // Nếu có body thì serialize nó thành JSON
            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            // Gửi request
            var response = await _httpClient.SendAsync(request);

            if (response == null)
            {
                throw new NullReferenceException("Response is null");
            }

            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung phản hồi
                var responseString = await response.Content.ReadAsStringAsync();

                // Deserialize thành đối tượng T
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

    public async Task<IActionResult> Index(string? searchString = null, Guid? staffId = null,
        EntityStatus? status = null, BookingType? bookingType = null, int pageIndex = 1, int pageSize = 5)
    {
        var roomBookingGetRequest = new RoomBookingGetRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            Status = status,
            StaffId = staffId,
            BookingType = bookingType
        };
        string requestUrl = "RoomBooking/GetFilteredRoomBookings";
        
        var roomBookings = await SendHttpRequest<ResponseData<RoomBookingResponse>>
            (requestUrl, HttpMethod.Post, roomBookingGetRequest);
        if (roomBookings != null)
            return View(roomBookings);
        
        return View("Error");
    }

    public async Task<IActionResult> Details(Guid roomBookingId)
    {
        string requestUrl = $"RoomBooking/GetRoomBookingById?roomBookingId={roomBookingId}";
        
        var roomBooking = await SendHttpRequest<RoomBooking>(requestUrl, HttpMethod.Post);
        if(roomBooking != null)
            return View(roomBooking);
        
        return View("Error");
    }

    public async Task<IActionResult> Edit(Guid roomBookingId)
    {
        string requestUrl = $"RoomBooking/GetRoomBookingById?roomBookingId={roomBookingId}";
        
        var roomBooking = await SendHttpRequest<RoomBookingResponse>(requestUrl, HttpMethod.Post);
        if(roomBooking != null)
            return View(roomBooking);
        
        return View("Error");
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(RoomBookingUpdateRequest roomBookingUpdateRequest)
    {
        string requestUrl = $"RoomBooking/UpdateRoomBooking?roomBookingId={roomBookingUpdateRequest.Id}";
        
        var roomBooking = await SendHttpRequest<RoomBookingResponse>
            (requestUrl, HttpMethod.Put, roomBookingUpdateRequest);
        if(roomBooking != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }
}