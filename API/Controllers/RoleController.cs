using Domain.DTO.Paging;
using Domain.DTO.Post;
using Domain.DTO.Role;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("CreateRole")]
        public async Task<int> CreateRole(RoleCreateRequest request)
        {
            try
            {
                return await _roleService.AddRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListRole")]
        public async Task<ResponseData<Role>> GetListRole(RoleGetRequest request)
        {
            try
            {
                return await _roleService.GetAllRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetRoleById")]
        public async Task<Role> GetRoleById(Guid Id)
        {
            try
            {
                return await _roleService.GetRoleById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateRole")]
        public async Task<int> UpdateRole(RoleUpdateRequest request)
        {
            try
            {
                return await _roleService.UpdateRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteRole")]
        public async Task<int> DeleteRole(RoleDeleteRequest request)
        {
            try
            {
                return await _roleService.DeleteRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
