using Domain.DTO.Customer;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewClient.Repositories.IRepository;

namespace ViewClient.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerRepo;
        private readonly HttpClient _httpClient;
        public CustomerController(ICustomer customerRepo, HttpClient httpClient)
        {
            _customerRepo = customerRepo;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.UserData).Value);
            var result = await _customerRepo.GetCustomerById(userId);
            if (result != null)
            {
                return View(result);
            }
            return View("NotFound");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerUpdateRequest request)
        {
            var _UserLogin = Guid.Empty;

            // Lấy thông tin người dùng đã đăng nhập
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            ViewBag.Genderes = Enum.GetValues(typeof(GenderType));
            request.ModifiedBy = _UserLogin;
            request.ModifiedTime = DateTimeOffset.Now;

            var result = await _customerRepo.UpdateCustomer(request);
            if (result == -1)
            {
                ModelState.AddModelError(string.Empty, "Thông tin của khách hàng đã bị trùng.");
            }
            else if (result == 1)
            {
                return RedirectToAction("Index");
            }

            return View("Error", new Exception("Không thể sửa khách hàng."));
        }
        //[HttpPost]
        //public async Task<IActionResult> EditPassword(ClientUpdatePassword request)
        //{
        //    var _UserLogin = Guid.Empty;

        //    // Lấy thông tin người dùng đã đăng nhập
        //    if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
        //    {
        //        _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    }

        //}
    }
}
