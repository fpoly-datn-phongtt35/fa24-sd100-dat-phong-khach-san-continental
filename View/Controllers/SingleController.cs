using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;

namespace View.Controllers
{
    [CustomAuthorize]
    public class SingleController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
