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

    }
}
