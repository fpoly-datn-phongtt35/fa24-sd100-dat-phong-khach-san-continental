using Domain.DTO.Staff;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace View.ViewComponents.Staff
{
    public class ListDataViewComponent : ViewComponent
    {
        private readonly IStaffService _staffService;
        public ListDataViewComponent(IStaffService staffService)
        {
            _staffService = staffService;
        }
        public async Task<IViewComponentResult> InvokeAsync(StaffGetRequest request)
        {
            var Data = await _staffService.GetStaffs(request);
            return View("~/Views/Shared/Components/Staff/ListData.cshtml",Data);
        }
    }
}
