using Domain.DTO.Paging;
using Domain.DTO.Post;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WEB.CMS.Customize;
using Domain.DTO.PostType;
using Domain.DTO.Staff;
namespace View.Controllers
{
    [CustomAuthorize]
    public class PostController : Controller
    {
        private readonly HttpClient _httpClient;

        public PostController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 10, string title = null, string contentOfPost = null)
        {
            string requestURL = "https://localhost:7130/api/Post/GetListPost";

            var PostRequest = new PostGetRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Title = title,
                Content = contentOfPost
            };

            var jsonRequest = JsonConvert.SerializeObject(PostRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(requestURL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var Posts = JsonConvert.DeserializeObject<ResponseData<Post>>(responseString);

                string postTypesRequestUrl = "https://localhost:7130/api/PostType/GetListPostType";
                var postTypesRequest = new PostTypeGetRequest();
                var postTypeJsonRequest = JsonConvert.SerializeObject(postTypesRequest);
                var postTypeContent = new StringContent(postTypeJsonRequest, Encoding.UTF8, "application/json");
                var postTypeResponse = await _httpClient.PostAsync(postTypesRequestUrl, postTypeContent);

                var postTypeResponseString = await postTypeResponse.Content.ReadAsStringAsync();
                var postTypeList = JsonConvert.DeserializeObject<ResponseData<PostType>>(postTypeResponseString);
                ViewBag.postTypeList = postTypeList.data;

                string StaffsRequestUrl = "https://localhost:7130/api/Staff/GetListStaff";
                var StaffsRequest = new StaffGetRequest();
                var StaffJsonRequest = JsonConvert.SerializeObject(StaffsRequest);
                var StaffContent = new StringContent(StaffJsonRequest, Encoding.UTF8, "application/json");
                var StaffResponse = await _httpClient.PostAsync(StaffsRequestUrl, StaffContent);

                var StaffResponseString = await StaffResponse.Content.ReadAsStringAsync();
                var StaffList = JsonConvert.DeserializeObject<ResponseData<Domain.Models.Staff>>(StaffResponseString);
                ViewBag.StaffList = StaffList.data;

                return View(Posts);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            string requestUrl = $"https://localhost:7130/api/Post/GetPostById?Id={id}";

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
                var Post = JsonConvert.DeserializeObject<Post>(responseString);

                return View(Post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Create()
        {

            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return View(new PostCreateRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var _UserLogin = Guid.Empty;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                string requestURL = "https://localhost:7130/api/Post/CreatePost";
                request.CreatedBy = _UserLogin;
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

            string requestUrl = $"https://localhost:7130/api/Post/GetPostById?Id={id}";

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
                var Post = JsonConvert.DeserializeObject<Post>(responseString);



                return View(Post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post request)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            request.ModifiedBy = _UserLogin;
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/Post/UpdatePost", request);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            string requestUrl = "https://localhost:7130/api/Post/DeletePost";

            var request = new PostDeleteRequest
            {
                Id = id,
                DeletedBy =_UserLogin,
                DeletedTime = DateTimeOffset.Now
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Unable to delete the Post.");
            return View("Error", new Exception("Unable to delete the Post."));
        }
    }
}
