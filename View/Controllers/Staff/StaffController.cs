using Domain.DTO.Paging;
using Domain.DTO.Staff;
using Domain.Models;
using Domain.Enums;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace View.Controllers.Staff
{
    //[CustomAuthorize]
    public class StaffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _httpClient = new HttpClient();
            _staffService = staffService;
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ResponseData<Domain.Models.Staff>> GetListStaff() 
        {
            try 
            {
                StaffGetRequest request = new StaffGetRequest();
                var Data = await _staffService.GetStaffs(request);
                return Data;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IActionResult> GetListData(StaffGetRequest request)
        {
            return ViewComponent("ListData", request);
        }

        public async Task<IActionResult> AddOrUpdateStaffForm(Guid Id)
        {
            ViewBag.Statuses = Enum.GetValues(typeof(EntityStatus));
            return ViewComponent("AddOrUpdate", Id);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrCreate(StaffUpdateRequest request)
        {
            if (request.Id == Guid.Empty)
            {
                var obj = new StaffCreateRequest()
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                };
                var response = await _staffService.AddStaff(obj);
                if (response > 0)
                {
                    return Ok(new
                    {
                        msg = "Thêm mới thành công",
                        status = 200
                    });
                }
            }
            else
            {
                var response = await _staffService.UpdateStaff(request);
                if (response > 0)
                {
                    return Ok(new
                    {
                        msg = "Cập nhật thành công",
                        status = 200
                    });
                }
            }
            return Ok(new
            {
                status = 500
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            try
            {
                var deleteRq = new StaffDeleteRequest()
                {
                    Id = id,
                    DeletedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                };
                var rs = await _staffService.DeleteStaff(deleteRq);
                if (rs > 0) 
                {
                    return Ok(new
                    {
                        msg = "Xóa thành công",
                        status = 200
                    });
                }
                return Ok(new
                {
                    msg = "Xóa thất bại",
                    status = 200
                });
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
