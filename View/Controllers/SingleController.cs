using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class SingleController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
