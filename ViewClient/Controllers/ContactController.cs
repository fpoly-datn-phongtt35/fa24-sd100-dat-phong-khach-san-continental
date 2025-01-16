using Microsoft.AspNetCore.Mvc;

namespace ViewClient.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
