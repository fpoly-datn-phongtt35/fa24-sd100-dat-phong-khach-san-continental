using Domain.DTO.Paging;
using Domain.DTO.Role;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IRoleService
    {
        Task<int> AddRole(RoleCreateRequest request);
        Task<int> UpdateRole(RoleUpdateRequest request);
        Task<int> DeleteRole(RoleDeleteRequest request);
        Task<ResponseData<Role>> GetAllRole(RoleGetRequest Role);
        Task<Role> GetRoleById(Guid Id);
    }
}
