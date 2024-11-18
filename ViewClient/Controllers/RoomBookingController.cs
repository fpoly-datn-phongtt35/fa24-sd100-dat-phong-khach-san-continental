using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient.Controllers
{
    public class RoomBookingController : Controller
    {
        private readonly IRoombooking _roomBookingRepo;
        private readonly HttpClient _httpClient;
        public RoomBookingController(IRoombooking roomBookingRepo, HttpClient httpClient)
        {
            _roomBookingRepo = roomBookingRepo;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7130/");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index()
        {
            var _UserLogin = Guid.Empty;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (_UserLogin == Guid.Empty) 
                {
                    
                }
            }
            
            return View();
        }
      
    }
}
