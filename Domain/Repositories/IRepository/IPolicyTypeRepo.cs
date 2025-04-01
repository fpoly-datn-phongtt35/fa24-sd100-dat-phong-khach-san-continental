using Domain.DTO.PolicyType;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IPolicyTypeRepo
    {
        Task<int> AddPolicyType(PolicyTypeCreateRequest request);
        Task<int> UpdatePolicyType(PolicyTypeUpdateRequest request);
        Task<int> DeletePolicyType(PolicyTypeDeleteRequest request);
        Task<DataTable> GetAllPolicyType(PolicyTypeGetRequest PostType);
        Task<DataTable> GetPolicyTypeById(Guid id);
    }
}
