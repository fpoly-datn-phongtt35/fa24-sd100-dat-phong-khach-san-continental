using System.Text;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace View.Controllers;

public class RoomTypeServiceController : Controller
{
    private readonly HttpClient _httpClient;

    public RoomTypeServiceController(HttpClient httpClient)
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

    public async Task LoadRoomTypes()
    {
        // Call API to GET list RoomTypes
        string roomTypeRequestUrl = "RoomType/GetAllRoomTypes";
        var roomTypesTask = SendHttpRequest<List<RoomTypeResponse>>(roomTypeRequestUrl, HttpMethod.Post);
        
        ViewBag.RoomTypes = await roomTypesTask;
    }

    private async Task LoadServices()
    {
        // Call API to GET list services
        string serviceRequestUrl = "Service/GetListService";
        var serviceResponse = await _httpClient.PostAsync(serviceRequestUrl, 
            new StringContent("{}", Encoding.UTF8, "application/json"));
        var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();
        var service = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

        ViewBag.ServiceList = service?.data;
    }
    
    public async Task<IActionResult> Index(string? search)
    {
        await LoadRoomTypes();
        await LoadServices();
        string requestUrl = $"RoomTypeService/GetAllRoomTypeServices?search={search}";
        
        var roomTypeServices = await SendHttpRequest<List<RoomTypeServiceResponse>>(requestUrl, HttpMethod.Post);
        if(roomTypeServices != null)
            return View (roomTypeServices);

        return View("Error");
    }

    public async Task<IActionResult> Details(Guid roomTypeServiceId)
    {
        await LoadRoomTypes();
        await LoadServices();
        string requestUrl = $"RoomTypeService/GetRoomTypeServiceById?roomTypeServiceId={roomTypeServiceId}";
        
        var roomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, HttpMethod.Post);
        if (roomTypeService != null)
            return View(roomTypeService);
        
        return View("Error");
    }

    public async Task<IActionResult> Create()
    {
        await LoadRoomTypes();
        await LoadServices();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoomTypeServiceAddRequest roomTypeServiceAddRequest)
    {
        const string requestUrl = "RoomTypeService/AddRoomTypeService";
        var createdRoomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl,
            HttpMethod.Post, roomTypeServiceAddRequest);
        
        if(createdRoomTypeService != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> Edit(Guid roomTypeServiceId)
    {
        await LoadRoomTypes();
        await LoadServices();
        string requestUrl = $"RoomTypeService/GetRoomTypeServiceById?roomTypeServiceId={roomTypeServiceId}";
        
        var roomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, HttpMethod.Post);
        if(roomTypeService != null)
            return View(roomTypeService);
        
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoomTypeServiceUpdateRequest roomTypeServiceUpdateRequest)
    {
        string requestUrl = $"RoomTypeService/UpdateRoomTypeService?roomTypeServiceId={roomTypeServiceUpdateRequest.Id}";
        
        var updatedRoomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, 
            HttpMethod.Put, roomTypeServiceUpdateRequest);
        if(updatedRoomTypeService != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> Delete(Guid roomTypeServiceId)
    {
        await LoadRoomTypes();
        await LoadServices();
        string requestUrl = $"RoomTypeService/GetRoomTypeServiceById?roomTypeServiceId={roomTypeServiceId}";
        
        var roomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, HttpMethod.Post);
        if(roomTypeService != null)
            return View(roomTypeService);
        
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(RoomTypeServiceDeleteRequest roomTypeServiceDeleteRequest)
    {
        string requestUrl = $"RoomTypeService/DeleteRoomTypeService?roomTypeServiceId={roomTypeServiceDeleteRequest.Id}";
        
        var deletedRoomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, 
            HttpMethod.Put, roomTypeServiceDeleteRequest);
        if(deletedRoomTypeService != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }
}