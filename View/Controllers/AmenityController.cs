using System.Security.Claims;
using System.Text;
using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
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

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchString = null,
            EntityStatus? status = null)
        {
            string requestUrl = $"api/Amenity/GetFilteredAmenities";

            var amenityGetRequest = new AmenityGetRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchString = searchString,
                Status = status
            };

            var amenitiesResponse = await SendHttpRequest<ResponseData<AmenityResponse>>
                (requestUrl, HttpMethod.Post, amenityGetRequest);
            if (amenitiesResponse != null)
                return View(amenitiesResponse);

            return View("Error");
        }

        public async Task<IActionResult> Trash(int pageIndex = 1, int pageSize = 5, string? searchString = null,
            EntityStatus? status = null)
        {
            string requestUrl = $"api/Amenity/GetFilteredDeletedAmenities";
            var amenityGetRequest = new AmenityGetRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchString = searchString,
                Status = status
            };

            var deletedAmenitiesResponse = await SendHttpRequest<ResponseData<AmenityResponse>>
                (requestUrl, HttpMethod.Post, amenityGetRequest);

            if (deletedAmenitiesResponse != null)
                return View(deletedAmenitiesResponse);

            return View("Error");
        }

        public async Task<IActionResult> Recover(Guid amenityId)
        {
            var amenityUpdateRequest = new AmenityUpdateRequest
            {
                Id = amenityId,
                ModifiedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), // Lấy Guid của người dùng hiện tại
                //ModifiedBy = new Guid("b48bd523-956a-4e67-a605-708e812a8eda"),
                ModifiedTime = DateTimeOffset.Now
            };
            string requestUrl = "api/Amenity/RecoverDeletedAmenity";

            var recoverAmenity = await SendHttpRequest<AmenityResponse>
                (requestUrl, HttpMethod.Put, amenityUpdateRequest);

            if (recoverAmenity != null)
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
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            amenityCreateRequest.CreatedBy = userId;
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
            if (amenity != null)
                return View(amenity);
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AmenityUpdateRequest amenityUpdateRequest)
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            amenityUpdateRequest.ModifiedBy = userId;
            string requestUrl = $"/api/Amenity/UpdateAmenity?amenityId={amenityUpdateRequest.Id}";

            var updatedAmenity = await SendHttpRequest<AmenityResponse>
                (requestUrl, HttpMethod.Put, amenityUpdateRequest);

            if (updatedAmenity != null)
                return RedirectToAction("Index");
            return View("Error");
        }

        public async Task<IActionResult> Delete(Guid amenityId)
        {
            string requestUrl = $"/api/Amenity/GetAmenityById?amenityId={amenityId}";

            var amenity = await SendHttpRequest<AmenityResponse>(requestUrl, HttpMethod.Post);
            if (amenity != null)
                return View(amenity);
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AmenityDeleteRequest amenityDeleteRequest)
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            amenityDeleteRequest.DeletedBy = userId;
            string requestUrl = $"/api/Amenity/DeleteAmenity?amenityId={amenityDeleteRequest.Id}";

            var deletedAmenity = await SendHttpRequest<AmenityResponse>
                (requestUrl, HttpMethod.Put, amenityDeleteRequest);
            if (deletedAmenity != null)
                return RedirectToAction("Index");
            return View("Error");
        }

        public async Task<IActionResult> AmenitiesPdf()
        {
            var roomTypeGetRequest = new AmenityGetRequest()
            {
                PageIndex = 1,
                PageSize = int.MaxValue,
                SearchString = null,
                Status = null
            };

            string requestUrl = $"api/Amenity/GetFilteredAmenities";
            var amenities =
                await SendHttpRequest<ResponseData<AmenityResponse>>(requestUrl, HttpMethod.Post, roomTypeGetRequest);

            if (amenities == null)
                return View("Error");

            // Return view as pdf
            return new ViewAsPdf("AmenitiesPdf", amenities, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}