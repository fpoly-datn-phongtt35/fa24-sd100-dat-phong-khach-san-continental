using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
