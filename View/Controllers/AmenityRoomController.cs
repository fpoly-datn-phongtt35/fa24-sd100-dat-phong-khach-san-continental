﻿using System.Text;
using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.RoomType;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace View.Controllers;

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
        string amenityRequestUrl = "Amenity/GetAllAmenities";
        var amenitiesTask = SendHttpRequest<List<AmenityResponse>>(amenityRequestUrl, HttpMethod.Post);

        // Gọi API để lấy danh sách RoomTypes
        string roomTypeRequestUrl = "RoomType/GetAllRoomTypes";
        var roomTypesTask = SendHttpRequest<List<RoomTypeResponse>>(roomTypeRequestUrl, HttpMethod.Post);

        // Đợi cả hai lời gọi API hoàn tất
        await Task.WhenAll(amenitiesTask, roomTypesTask);

        // Lấy kết quả của hai danh sách và lưu vào ViewBag
        ViewBag.Amenities = await amenitiesTask;
        ViewBag.RoomTypes = await roomTypesTask;
    }
    
    public async Task<IActionResult> Index()
    {
        await LoadAmenitiesAndRoomTypes();
        const string requestUrl = "AmenityRoom/GetAllAmenityRooms";
        
        var amenityRooms = await SendHttpRequest<List<AmenityRoomResponse>>(requestUrl, HttpMethod.Post);
        if(amenityRooms != null)
            return View(amenityRooms);
        
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
        string requestUrl = $"AmenityRoom/DeleteRoomType?amenityRoomId={amenityRoomDeleteRequest.Id}";
        
        var deletedAmenityRoom = await SendHttpRequest<AmenityRoomResponse>(requestUrl, 
            HttpMethod.Put, amenityRoomDeleteRequest);
        if(deletedAmenityRoom != null)
            return RedirectToAction("Index");
        
        return View("Error");
    }
} 