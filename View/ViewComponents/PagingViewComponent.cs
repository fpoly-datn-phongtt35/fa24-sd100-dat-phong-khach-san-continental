using Microsoft.AspNetCore.Mvc;
using View.Models.Paging;

namespace View.ViewComponents
{
	public class PagingViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(Paging pageModel)
		{
			return View(pageModel);
		}
	}
}
