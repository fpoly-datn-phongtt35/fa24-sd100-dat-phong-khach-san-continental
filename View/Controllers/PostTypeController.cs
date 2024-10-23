using Domain.DTO.PostType;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class PostTypeController : Controller
    {
        private readonly HttpClient _httpClient;

        public PostTypeController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 10, string titleOfType = null)
        {
            string requestURL = "https://localhost:7130/api/PostType/GetListPostType";

            var PostTypeRequest = new PostTypeGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TitleOfType = titleOfType
            };

            var jsonRequest = JsonConvert.SerializeObject(PostTypeRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                // gửi request lên api
                var response = await _httpClient.PostAsync(requestURL, content);

                // đọc nội dung trả về từ api
                var responseString = await response.Content.ReadAsStringAsync();

                // chuyển đổi lại thành respondata 
                var PostTypes = JsonConvert.DeserializeObject<ResponseData<PostType>>(responseString);

                return View(PostTypes);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/PostType/GetPostTypeById?Id={id}";

            // Tạo nội dung json cho request
            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var PostType = JsonConvert.DeserializeObject<PostType>(responseString);

                return View(PostType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return View(new PostTypeCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostTypeCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                string requestURL = "https://localhost:7130/api/PostType/CreatePostType";
               
                request.CreatedTime = DateTimeOffset.Now;
                var response = await _httpClient.PostAsJsonAsync(requestURL, request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(request);
        }
        public async Task<IActionResult> Edit(Guid id)
        {

            string requestUrl = $"https://localhost:7130/api/PostType/GetPostTypeById?Id={id}";

            var jsonRequest = JsonConvert.SerializeObject(new { Id = id });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var PostType = JsonConvert.DeserializeObject<PostType>(responseString);



                return View(PostType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostType request)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));

            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/PostType/UpdatePostType", request);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string requestUrl = "https://localhost:7130/api/PostType/DeletePostType";

            var request = new PostTypeDeleteRequest
            {
                Id = id,
                DeletedBy = Guid.NewGuid(),
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Unable to delete the PostType.");
            return View("Error", new Exception("Unable to delete the PostType."));
        }
    }
}
