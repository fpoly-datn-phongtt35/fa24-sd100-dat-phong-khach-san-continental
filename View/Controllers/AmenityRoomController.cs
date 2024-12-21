using System.Security.Claims;
using System.Text;
using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Rotativa.AspNetCore;
using WEB.CMS.Customize;

namespace View.Controllers;
[CustomAuthorize]
public class AmenityRoomController : Controller
{
    private readonly HttpClient _httpClient;

    public AmenityRoomController(HttpClient httpClient)
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

    private async Task LoadAmenitiesAndRoomTypes()
    {
        // Gọi API để lấy danh sách Amenities
        var amenityGetRequest = new AmenityGetRequest()
        {
            PageIndex = 1,
            PageSize = int.MaxValue,
            SearchString = null,
            Status = EntityStatus.Active
        };
        string amenityRequestUrl = "Amenity/GetFilteredAmenities";
        var amenitiesTask = SendHttpRequest<ResponseData<AmenityResponse>>
            (amenityRequestUrl, HttpMethod.Post, amenityGetRequest);

        // Gọi API để lấy danh sách RoomTypes
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

        // Đợi cả hai lời gọi API hoàn tất
        await Task.WhenAll(amenitiesTask, roomTypesTask);

        // Lấy kết quả của hai danh sách và lưu vào ViewBag
        var amenitiesResponse = await amenitiesTask;
        ViewBag.Amenities = amenitiesResponse?.data ?? new List<AmenityResponse>();
            
        // Lưu danh sách RoomTypes vào ViewBag
        var roomTypesResponse = await roomTypesTask;
        ViewBag.RoomTypes = roomTypesResponse?.data ?? new List<RoomTypeResponse>();
    }

    public async Task<IActionResult> Index(string? searchString = null, Guid? roomTypeId = null, 
        EntityStatus? status = null, int pageIndex = 1, int pageSize = 10)
    {
        await LoadAmenitiesAndRoomTypes();
        string requestUrl = "AmenityRoom/GetFilteredAmenityRooms";
        var amenityRoomGetRequest = new AmenityRoomGetRequest()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            SearchString = searchString,
            Status = status,
            RoomTypeId = roomTypeId
        };

        var amenityRooms = await SendHttpRequest<ResponseData<AmenityRoomResponse>>
            (requestUrl, HttpMethod.Post, amenityRoomGetRequest);
        if (amenityRooms != null)
            return View(amenityRooms);

        return View("Error");
    }

    public async Task<IActionResult> Trash(string? searchString = null, Guid? roomTypeId = null, 
        EntityStatus? status = null, int pageIndex = 1, int pageSize = 5)
    {
        await LoadAmenitiesAndRoomTypes();
        var amenityRoomGetRequest = new AmenityRoomGetRequest()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            SearchString = searchString,
            Status = null,
            RoomTypeId = roomTypeId
        };
        await LoadAmenitiesAndRoomTypes();
        string requestUrl = "AmenityRoom/GetFilteredDeletedAmenityRooms";
        
        var deletedAmenityRooms = await SendHttpRequest<ResponseData<AmenityRoomResponse>>
            (requestUrl, HttpMethod.Post, amenityRoomGetRequest);
        if(deletedAmenityRooms != null)
            return View(deletedAmenityRooms);
        return View("Error");
    }

    public async Task<IActionResult> Recover(Guid amenityRoomId)
    {
        var amenityRoomUpdateRequest = new AmenityRoomUpdateRequest()
        {
            Id = amenityRoomId,
            ModifiedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), // Lấy Guid của người dùng hiện tại
        };
        string requestUrl = "AmenityRoom/RecoverDeletedAmenityRoom";
        
        var recoverAmenityRoom = await SendHttpRequest<AmenityRoomResponse>
            (requestUrl, HttpMethod.Put, amenityRoomUpdateRequest);
        if(recoverAmenityRoom != null)
            return RedirectToAction("Trash");
        
        return View("Error");
    }
    
    public async Task<IActionResult> Details(Guid amenityRoomId)
    {
        await LoadAmenitiesAndRoomTypes();
        string requestUrl = $"AmenityRoom/GetAmenityRoomById?amenityRoomId={amenityRoomId}";
        
        var amenityRoom = await SendHttpRequest<AmenityRoom>(requestUrl, HttpMethod.Post);
        if(amenityRoom != null)
            return View(amenityRoom);

        return View("Error");
    }

    public async Task<IActionResult> Create()
    {
        await LoadAmenitiesAndRoomTypes();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AmenityRoomAddRequest amenityRoomAddRequest)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        amenityRoomAddRequest.CreatedBy = userId;
        const string requestUrl = "AmenityRoom/CreateAmenityRoom";
        
        var createdAmenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, 
            HttpMethod.Post, amenityRoomAddRequest);
        if(createdAmenityRoom != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> Edit(Guid amenityRoomId)
    {
        await LoadAmenitiesAndRoomTypes();
        string requestUrl = $"AmenityRoom/GetAmenityRoomById?amenityRoomId={amenityRoomId}";
        
        var amenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, HttpMethod.Post);
        if(amenityRoom != null)
            return View(amenityRoom);
        
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AmenityRoomUpdateRequest amenityRoomUpdateRequest)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        amenityRoomUpdateRequest.ModifiedBy = userId;
        string requestUrl = $"AmenityRoom/UpdateAmenityRoom?amenityRoomId={amenityRoomUpdateRequest.Id}";
        
        var updatedAmenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, 
            HttpMethod.Put, amenityRoomUpdateRequest);
        if(updatedAmenityRoom != null)
            return RedirectToAction("Index");

        return View("Error");
    }

    public async Task<IActionResult> Delete(Guid amenityRoomId)
    {
        await LoadAmenitiesAndRoomTypes();
        string requestUrl = $"AmenityRoom/GetAmenityRoomById?amenityRoomId={amenityRoomId}";
        
        var amenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, HttpMethod.Post);
        if(amenityRoom != null)
            return View(amenityRoom);
        
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(AmenityRoomDeleteRequest amenityRoomDeleteRequest)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        amenityRoomDeleteRequest.DeletedBy = userId;
        string requestUrl = $"AmenityRoom/DeleteAmenityRoom?amenityRoomId={amenityRoomDeleteRequest.Id}";
        
        var deletedAmenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, 
            HttpMethod.Put, amenityRoomDeleteRequest);
        if(deletedAmenityRoom != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> AmenityRoomsPdf()
    {
        var amenityRoomGetRequest = new AmenityRoomGetRequest()
        {
            PageIndex = 1,
            PageSize = int.MaxValue,
            SearchString = null,
            Status = null
        };

        string requestUrl = "AmenityRoom/GetFilteredAmenityRooms";
        var amenityRooms = await SendHttpRequest<ResponseData<AmenityRoomResponse>>
            (requestUrl, HttpMethod.Post, amenityRoomGetRequest);
        
        if (amenityRooms == null)
            return View("Error");
        
        return new ViewAsPdf("AmenityRoomsPdf", amenityRooms, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins() {Top = 20, Right = 20, Bottom = 20, Left = 20},
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }
} 