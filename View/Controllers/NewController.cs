using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
	public class NewController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
