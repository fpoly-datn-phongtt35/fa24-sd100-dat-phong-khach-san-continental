using System.Text;
using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using WEB.CMS.Customize;

namespace View.Controllers;

[CustomAuthorize]
public class UnitController : Controller
{
    private readonly HttpClient httpClient;

    public UnitController(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.BaseAddress = new Uri("https://localhost:7130/");
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

            var response = await httpClient.SendAsync(request);
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
                // Đọc nội dung lỗi từ response
                var errorMessage = await response.Content.ReadAsStringAsync();
                // Ném exception với thông báo lỗi
                throw new Exception(errorMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    
    public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchString = null,
        EntityStatus? status = null)
    {
        string requestUrl = $"api/Unit/GetFilteredUnits";

        var unitGetRequest = new UnitGetRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchString = searchString,
            Status = status
        };
        
        var unitResponse = await SendHttpRequest<ResponseData<UnitResponse>>
            (requestUrl, HttpMethod.Get, unitGetRequest);
        if (unitResponse != null)
            return View(unitResponse);
        return View("Error");
    }

    public async Task<IActionResult> Details(Guid unitId)
    {
        string requestUrl = $"api/Unit/GetUnitById?id={unitId}";
        var unit = await SendHttpRequest<UnitResponse>(requestUrl, HttpMethod.Get);
        if (unit != null) return View(unit);
        return View("Error");
    }

    public async Task<IActionResult> Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(UnitCreateRequest unitCreateRequest)
    {
        if (!ModelState.IsValid)
            return View(unitCreateRequest);

        string requestUrl = $"api/Unit/CreateUnit";

        try
        {
            var createUnit = await SendHttpRequest<UnitCreateRequest>(requestUrl, HttpMethod.Post, unitCreateRequest);
            if (createUnit != null)
                return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Thêm lỗi vào ModelState để hiển thị ra view
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(unitCreateRequest);
    }

    public async Task<IActionResult> Edit(Guid unitId)
    {
        string requestUrl = $"/api/Unit/GetUnitById?id={unitId}";
        var unit = await SendHttpRequest<UnitResponse>(requestUrl, HttpMethod.Get);
        if(unit != null) return View(unit);
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UnitUpdateRequest unitUpdateRequest)
    {
        string requestUrl = $"/api/Unit/UpdateUnit?id={unitUpdateRequest.Id}";
        
        var updatedUnit = await SendHttpRequest<UnitResponse>(requestUrl, HttpMethod.Put, unitUpdateRequest);
        if(updatedUnit != null) return RedirectToAction("Index");
        return View("Error");
    }

    public async Task<IActionResult> Delete(Guid unitId)
    {
        string requestUrl = $"/api/Unit/UpdateUnit?id={unitId}";
        var unit = await SendHttpRequest<UnitResponse>(requestUrl, HttpMethod.Post);
        if (unit != null) return View(unit);
        return View("Error");
    }

    public async Task<IActionResult> Delete(UnitDeleteRequest unitDeleteRequest)
    {
        string requestUrl = $"/api/Unit/DeleteUnit?id={unitDeleteRequest.Id}";
        var deletedUnit = await SendHttpRequest<UnitResponse>(requestUrl, HttpMethod.Put, unitDeleteRequest);
        if(deletedUnit != null) return RedirectToAction("Index");
        return View("Error");
    }
}
