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
using Microsoft.AspNetCore.Mvc.Rendering;
using View.Views.Shared.Helper;
namespace View.Controllers
{
    [CustomAuthorize]
    public class PostController : Controller
    {
        private readonly HttpClient _httpClient;

        public PostController(HttpClient httpClient)
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

            string postTypesRequestUrl = "https://localhost:7130/api/PostType/GetListPostType";
            var postTypesRequest = new PostTypeGetRequest();
            var postTypeJsonRequest = JsonConvert.SerializeObject(postTypesRequest);
            var postTypeContent = new StringContent(postTypeJsonRequest, Encoding.UTF8, "application/json");
            var postTypeResponse = await _httpClient.PostAsync(postTypesRequestUrl, postTypeContent);

            var postTypeResponseString = await postTypeResponse.Content.ReadAsStringAsync();
            var postTypeList = JsonConvert.DeserializeObject<ResponseData<PostType>>(postTypeResponseString);
            ViewBag.postTypeList = postTypeList.data;

            ViewBag.PostTypes = postTypeList?.data.Select(pt => new SelectListItem
            {
                Value = pt.Id.ToString(),
                Text = pt.TitleOfType.HasValue ? PostTypeHelper.DisplayPostType(pt.TitleOfType.Value).ToString() : "Unknown"
            }).ToList();

            string StaffsRequestUrl = "https://localhost:7130/api/Staff/GetListStaff";
            var StaffsRequest = new StaffGetRequest();
            var StaffJsonRequest = JsonConvert.SerializeObject(StaffsRequest);
            var StaffContent = new StringContent(StaffJsonRequest, Encoding.UTF8, "application/json");
            var StaffResponse = await _httpClient.PostAsync(StaffsRequestUrl, StaffContent);

            var StaffResponseString = await StaffResponse.Content.ReadAsStringAsync();
            var StaffList = JsonConvert.DeserializeObject<ResponseData<Domain.Models.Staff>>(StaffResponseString);
            ViewBag.StaffList = StaffList.data;

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
            string postTypesRequestUrl = "api/PostType/GetListPostType";
            var postTypesRequest = new PostTypeGetRequest
            {
                PageIndex = 1,
                PageSize = 100
            };
            var postTypeResponse = await SendHttpRequest<ResponseData<PostType>>(postTypesRequestUrl, HttpMethod.Post, postTypesRequest);
            ViewBag.PostTypes = postTypeResponse?.data;
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
                request.StaffId = _UserLogin;
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

            string postTypesRequestUrl = "api/PostType/GetListPostType";
            var postTypesRequest = new PostTypeGetRequest
            {
                PageIndex = 1,
                PageSize = 100
            };
            var postTypeResponse = await SendHttpRequest<ResponseData<PostType>>(postTypesRequestUrl, HttpMethod.Post, postTypesRequest);
            ViewBag.PostTypes = postTypeResponse?.data.Select(pt => new SelectListItem
            {
                Value = pt.Id.ToString(),
                Text = pt.TitleOfType.HasValue ? PostTypeHelper.DisplayPostType(pt.TitleOfType.Value).ToString() : "Unknown"
            });
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
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.ModifiedBy = userId;
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            request.ModifiedTime = DateTimeOffset.Now;
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7130/api/Post/UpdatePost", request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(request);
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
