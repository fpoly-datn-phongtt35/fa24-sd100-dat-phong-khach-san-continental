using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class LiveCamerasController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
