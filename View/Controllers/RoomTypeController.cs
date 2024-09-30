using System.Text;
using Domain.DTO.RoomType;
using Domain.Models;
using Domain.Services.IServices.IRoomType;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace View.Controllers;

public class RoomTypeController : Controller
{
    private readonly HttpClient _httpClient;

    public RoomTypeController(HttpClient httpClient)
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

    public async Task<IActionResult> Index()
    {
        const string requestUrl = "/api/RoomType/GetAllRoomTypes";

        var roomTypes = await SendHttpRequest<List<RoomTypeResponse>>(requestUrl, HttpMethod.Post);
        if (roomTypes != null)
            return View(roomTypes);

        return View("Error");
    }

    public async Task<IActionResult> Details(Guid roomTypeId)
    {
        string requestUrl = $"/api/RoomType/GetRoomTypeById?roomTypeId={roomTypeId}";
        
        var roomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Post);
        if(roomType != null)
            return View(roomType);

        return View("Error");
    }

    public async Task<IActionResult> Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(RoomTypeAddRequest roomTypeAddRequest)
    {
        string requestUrl = $"/api/RoomType/CreateRoomType";

        var createdRoomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Post, roomTypeAddRequest);
        if (createdRoomType != null)
            return RedirectToAction("Index");

        return View("Error");
    }

    public async Task<IActionResult> Edit(Guid roomTypeId)
    {
        string requestUrl = $"/api/RoomType/GetRoomTypeById?roomTypeId={roomTypeId}";
        
        var roomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Post);
        if(roomType != null)
            return View(roomType);

        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoomTypeUpdateRequest roomTypeUpdateRequest)
    {
        string requestUrl = $"/api/RoomType/UpdateRoomType?roomTypeId={roomTypeUpdateRequest.Id}";
        
        var updatedRoomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Put, roomTypeUpdateRequest);
        if(updatedRoomType != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> Delete(Guid roomTypeId)
    {
        string requestUrl = $"/api/RoomType/GetRoomTypeById?roomTypeId={roomTypeId}";
        
        var roomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Post);
        if(roomType != null)
            return View(roomType);
        
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(RoomTypeDeleteRequest roomTypeDeleteRequest)
    {
        string requestUrl = $"/api/RoomType/DeleteRoomType?roomTypeId={roomTypeDeleteRequest.Id}";
        
        var deletedRoomType = await SendHttpRequest<RoomTypeResponse>(requestUrl, HttpMethod.Put, roomTypeDeleteRequest);
        if(deletedRoomType != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }
}