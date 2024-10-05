using Domain.DTO.Role;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IRoleRepo
    {
        Task<int> AddRole(RoleCreateRequest request);
        Task<int> UpdateRole(RoleUpdateRequest request);
        Task<int> DeleteRole(RoleDeleteRequest request);
        Task<DataTable> GetAllRole(RoleGetRequest request);
        Task<DataTable> GetRoleById(Guid id);
    }
}
