using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class PhotoController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
