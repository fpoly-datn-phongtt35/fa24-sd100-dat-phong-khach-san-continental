using Domain.DTO.Image;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace View.Controllers
{
    public class ImagesController : Controller
    {
        HttpClient _client;

        public ImagesController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7130/");
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? name = null, EntityStatus? status = null)
        {
            // api url
            string requestUrl = "https://localhost:7130/api/Images/GetListImages";

            var request = new ImagesGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Name = name,
                Status = status
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var images = JsonConvert.DeserializeObject<ResponseData<Images>>(responseString);
                ViewBag.StatusList = Enum.GetValues(typeof(EntityStatus));
                return View(images);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        //public async Task<IActionResult> Details(Guid id)
        //{
        //    string requestUrl = $"https://localhost:7130/api/Images/GetImagesById?id={id}";

        //    // Tạo nội dung json cho request
        //    var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
        //    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        //    try
        //    {
        //        var response = await _client.PostAsync(requestUrl, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return View("Error");
        //        }

        //        var responseString = await response.Content.ReadAsStringAsync();
        //        var services = JsonConvert.DeserializeObject<Images>(responseString);



        //        return View(services);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        public async Task<IActionResult> Create()
        {
            return View(new ImagesCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImagesCreateRequest request,IFormFile img)
        {
            
                if (img != null && img.Length > 0)
                {
                    var fileName = Path.GetFileName(img.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    request.Image = fileName;
                }
                request.Status = EntityStatus.Active;
                request.CreatedTime = DateTimeOffset.Now;

                var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                request.CreatedBy = userId;
                var response = await _client.PostAsJsonAsync("api/Images/CreateImages", request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            
            return View(request);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            string requestUrl = $"api/Images/GetImagesById?id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            try
            {
                var response = await _client.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var Imagess = JsonConvert.DeserializeObject<Images>(responseString);



                return View(Imagess);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Images request, IFormFile img)
        {
            if (img != null && img.Length > 0)
            {
                var fileName = Path.GetFileName(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                request.Image = fileName;
            }
            else
            {
                var existingResponse = await _client.GetAsync($"api/Images/GetImagesById?id={request.Id}");
                if (existingResponse.IsSuccessStatusCode)
                {
                    var responseString = await existingResponse.Content.ReadAsStringAsync();
                    var existing = JsonConvert.DeserializeObject<Images>(responseString);

                    if (existing != null)
                    {
                        request.Image = existing.Image;
                    }
                }
            }
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.ModifiedBy = userId;
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _client.PutAsJsonAsync("api/Images/UpdateImages", request);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/Images/DeleteImages";
            var request = new ImagesDeleteRequest
            {
                Id = id,
                DeletedBy = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error", new Exception("Unable to delete the Images."));
        }
    }
}
