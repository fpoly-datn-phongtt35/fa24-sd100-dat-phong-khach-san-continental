using Microsoft.AspNetCore.Mvc;
using ViewClient.Models.Paging;

namespace ViewClient.ViewComponents
{
	public class PagingViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(Paging pageModel)
		{
			return View(pageModel);
		}
	}
}
