using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class PhotoController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
