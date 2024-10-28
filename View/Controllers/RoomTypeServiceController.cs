using System.Security.Claims;
using System.Text;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using WEB.CMS.Customize;

namespace View.Controllers;
[CustomAuthorize]
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
        var roomTypeGetRequest = new RoomTypeGetRequest()
        {
            PageIndex = 1,
            PageSize = int.MaxValue,
            SearchString = null,
            Status = null
        };
        string roomTypeRequestUrl = "RoomType/GetFilteredRoomTypes";
        var roomTypesTask = SendHttpRequest<ResponseData<RoomTypeResponse>>
            (roomTypeRequestUrl, HttpMethod.Post, roomTypeGetRequest);
        
        var roomTypesResponse = await roomTypesTask;
        ViewBag.RoomTypes = roomTypesResponse?.data ?? new List<RoomTypeResponse>();
    }

    private async Task LoadServices()
    {
        string serviceRequestUrl = "Service/GetListService";
        var serviceResponse = await _httpClient.PostAsync(serviceRequestUrl, 
            new StringContent("{}", Encoding.UTF8, "application/json"));
        var serviceResponseString = await serviceResponse.Content.ReadAsStringAsync();
        var service = JsonConvert.DeserializeObject<ResponseData<Service>>(serviceResponseString);

        ViewBag.ServiceList = service?.data;
    }
    
    public async Task<IActionResult> Index(string? searchString = null, Guid? roomTypeId = null,
        EntityStatus? status = null, int pageIndex = 1, int pageSize = 5)
    {
        await LoadRoomTypes();
        await LoadServices();
        var roomTypeServiceRequest = new RoomTypeServiceGetRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            Status = status,
            RoomTypeId = roomTypeId
        };
        
        string requestUrl = $"RoomTypeService/GetFilteredRoomTypeServices";
        
        var roomTypeServices = await SendHttpRequest<ResponseData<RoomTypeServiceResponse>>
            (requestUrl, HttpMethod.Post, roomTypeServiceRequest);
        if(roomTypeServices != null)
            return View (roomTypeServices);

        return View("Error");
    }

    public async Task<IActionResult> Trash(string? searchString = null, Guid? roomTypeId = null,
        EntityStatus? status = null, int pageIndex = 1, int pageSize = 5)
    {
        await LoadRoomTypes();
        await LoadServices();
        
        var roomTypeServiceRequest = new RoomTypeServiceGetRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            Status = status,
            RoomTypeId = roomTypeId
        };
        string requestUrl = $"RoomTypeService/GetFilteredDeletedRoomTypeServices";

        var deletedRoomTypeServices = await SendHttpRequest<ResponseData<RoomTypeServiceResponse>>
            (requestUrl, HttpMethod.Post, roomTypeServiceRequest);
        if (deletedRoomTypeServices != null)
            return View(deletedRoomTypeServices);

        return View("Error");
    }

    public async Task<IActionResult> Recover(Guid roomTypeServiceId)
    {
        var roomTypeServiceUpdateRequest = new RoomTypeServiceUpdateRequest()
        {
            Id = roomTypeServiceId,
            ModifiedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
        };
        string requestUrl = "RoomTypeService/RecoverDeletedRoomTypeService";
        
        var recoverRoomTypeService = await SendHttpRequest<RoomTypeServiceResponse>
            (requestUrl, HttpMethod.Put, roomTypeServiceUpdateRequest);
        if (recoverRoomTypeService != null)
            return RedirectToAction("Trash");
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
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        roomTypeServiceAddRequest.CreatedBy = userId;
        
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
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        roomTypeServiceUpdateRequest.ModifiedBy = userId;
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
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        roomTypeServiceDeleteRequest.DeletedBy = userId;
        string requestUrl = $"RoomTypeService/DeleteRoomTypeService?roomTypeServiceId={roomTypeServiceDeleteRequest.Id}";
        
        var deletedRoomTypeService = await SendHttpRequest<RoomTypeServiceResponse>(requestUrl, 
            HttpMethod.Put, roomTypeServiceDeleteRequest);
        if(deletedRoomTypeService != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> RoomTypeServicesPdf()
    {
        var roomTypeServiceGetRequest = new RoomTypeServiceGetRequest()
        {
            PageIndex = 1,
            PageSize = int.MaxValue,
            SearchString = null,
            Status = null,
            RoomTypeId = null
        };
        
        string requestUrl = "RoomTypeService/GetFilteredRoomTypeServices";
        var roomTypeServices = await SendHttpRequest<ResponseData<RoomTypeServiceResponse>>
            (requestUrl, HttpMethod.Post, roomTypeServiceGetRequest);
        
        if(roomTypeServices == null)
            return View("Error");
        
        return new ViewAsPdf("RoomTypeServicesPdf", roomTypeServices, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins() {Top = 20, Right = 20, Bottom = 20, Left = 20},
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }
}