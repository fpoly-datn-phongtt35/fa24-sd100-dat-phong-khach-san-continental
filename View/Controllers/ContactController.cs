using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
