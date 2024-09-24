using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class LiveCamerasController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
