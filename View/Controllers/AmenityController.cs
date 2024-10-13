using System.Text;
using Domain.DTO.Amenity;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;

namespace View.Controllers;

public class AmenityController : Controller
{
    private readonly HttpClient _httpClient;
    
    // Inject HttpClient từ IHttpClientFactory thay vì trực tiếp qua constructor
    public AmenityController(HttpClient httpClient)
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
    
    public async Task<IActionResult> Index(EntityStatus? status, string? searchString)
    {
        string requestUrl = $"api/Amenity/GetFilteredAmenities?status={status}&searchString={searchString}";

        var amenities = await SendHttpRequest<List<AmenityResponse>>(requestUrl, HttpMethod.Post);
        
        if (amenities != null)
            return View(amenities);

        return View("Error");
    }

    public async Task<IActionResult> Trash(string? searchString)
    {
        string requestUrl = $"api/Amenity/GetFilteredDeletedAmenities?searchString={searchString}";
        var deletedAmenities = await SendHttpRequest<List<AmenityResponse>>(requestUrl, HttpMethod.Post);
        
        if(deletedAmenities != null)
            return View(deletedAmenities);

        return View("Error");
    }

    public async Task<IActionResult> Recover(Guid amenityId)
    {
        var amenityUpdateRequest = new AmenityUpdateRequest
        {
            Id = amenityId,
            // ModifiedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value), // Lấy Guid của người dùng hiện tại
            ModifiedBy = new Guid("b48bd523-956a-4e67-a605-708e812a8eda"),
            ModifiedTime = DateTimeOffset.Now
        };
        string requestUrl = "api/Amenity/RecoverDeletedAmenity";

        var recoverAmenity = await SendHttpRequest<AmenityResponse>
            (requestUrl, HttpMethod.Put, amenityUpdateRequest);
        
        if(recoverAmenity != null)
            return RedirectToAction("Trash");
        return View("Error");
    }
    
    public async Task<IActionResult> Details(Guid amenityId)
    {
        string requestUrl = $"/api/Amenity/GetAmenityById?amenityId={amenityId}";

        var amenity = await SendHttpRequest<AmenityResponse>(requestUrl, HttpMethod.Post);
        if (amenity != null)
            return View(amenity);
        
        return View("Error");
    }

    public async Task<IActionResult> Create() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(AmenityCreateRequest amenityCreateRequest)
    {
        string requestUrl = "/api/Amenity/CreateAmenity";

        var createdAmenity = await SendHttpRequest<AmenityResponse>(requestUrl, 
            HttpMethod.Post, amenityCreateRequest);
        if (createdAmenity != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }

    public async Task<IActionResult> Edit(Guid amenityId)
    {
        string requestUrl = $"/api/Amenity/GetAmenityById?amenityId={amenityId}";

        var amenity = await SendHttpRequest<AmenityResponse>(requestUrl, HttpMethod.Post);
        if(amenity != null)
            return View(amenity);
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AmenityUpdateRequest amenityUpdateRequest)
    {
        string requestUrl = $"/api/Amenity/UpdateAmenity?amenityId={amenityUpdateRequest.Id}";

        var updatedAmenity = await SendHttpRequest<AmenityResponse>
            (requestUrl, HttpMethod.Put, amenityUpdateRequest);
        
        if(updatedAmenity != null)
            return RedirectToAction("Index");
        return View("Error");
    }

    public async Task<IActionResult> Delete(Guid amenityId)
    {
        string requestUrl = $"/api/Amenity/GetAmenityById?amenityId={amenityId}";

        var amenity = await SendHttpRequest<AmenityResponse>(requestUrl, HttpMethod.Post);
        if(amenity != null)
            return View(amenity);
        return View("Error");
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(AmenityDeleteRequest amenityDeleteRequest)
    {
        string requestUrl = $"/api/Amenity/DeleteAmenity?amenityId={amenityDeleteRequest.Id}";

        var deletedAmenity = await SendHttpRequest<AmenityResponse>
            (requestUrl, HttpMethod.Put, amenityDeleteRequest);
        if(deletedAmenity != null)
            return RedirectToAction("Index");
        return View("Error");
    }

    public async Task<IActionResult> AmenitiesPdf()
    {
        //Get list of amenities
        string requestUrl = $"api/Amenity/GetFilteredAmenities";
        var amenities = await SendHttpRequest<List<AmenityResponse>>(requestUrl, HttpMethod.Post);

        if(amenities == null)
            return View("Error");
        
        //Return view as pdf
        return new ViewAsPdf("AmenitiesPdf", amenities, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }
}
