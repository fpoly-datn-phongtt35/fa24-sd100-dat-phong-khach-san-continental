using Domain.DTO.Staff;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace View.ViewComponents.Staff
{
    public class AddOrUpdateViewComponent : ViewComponent
    {
        private readonly IStaffService _staffService;
        public AddOrUpdateViewComponent(IStaffService staffService)
        {
            _staffService = staffService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid Id)
        {
            var Data = new Domain.Models.Staff();
            if (Id != Guid.Empty) 
            {
                Data = await _staffService.GetStaffbyId(Id);
            }
            return View("~/Views/Shared/Components/Staff/AddOrUpdateStaffForm.cshtml", Data);
        }
    }
}
