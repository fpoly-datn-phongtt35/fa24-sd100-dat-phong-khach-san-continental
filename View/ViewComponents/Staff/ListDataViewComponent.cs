using Domain.DTO.Staff;
using Microsoft.AspNetCore.Mvc;
using View.Controllers.Staff.Service;

namespace View.ViewComponents.Staff
{
    public class ListDataViewComponent : ViewComponent
    {
        private readonly StaffServices _staffService;
        public ListDataViewComponent()
        {
            _staffService = new StaffServices();
        }
        public async Task<IViewComponentResult> InvokeAsync(StaffGetRequest request)
        {
            var lstData = await _staffService.GetListData(request);
            return View("~/Views/Shared/Components/Staff/ListData.cshtml",lstData);
        }
    }
}
