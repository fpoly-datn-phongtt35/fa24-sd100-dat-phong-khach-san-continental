using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.Staff;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("GetListStaff")]
        public async Task<ResponseData<Staff>> GetListStaff(StaffGetRequest request)
        {
            try
            {
                return await _staffService.GetStaffs(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetStaffById")]
        public async Task<StaffUpdateRequest> GetStaffById(Guid request)
        {
            try
            {
                return await _staffService.GetStaffbyId(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("CreateStaff")]
        public async Task<int> CreateStaff(StaffCreateRequest request)
        {
            try
            {
                return await _staffService.AddStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateStaff")]
        public async Task<int> UpdateStaff(StaffUpdateRequest request)
        {
            try
            {
                return await _staffService.UpdateStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteStaff")]
        public async Task<int> DeleteStaff(StaffDeleteRequest request)
        {
            try
            {
                return await _staffService.DeleteStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
