using Domain.DTO.Paging;
using Domain.DTO.PolicyType;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IPolicyTypeService
    {
        Task<int> AddPolicyType(PolicyTypeCreateRequest request);
        Task<int> UpdatePolicyType(PolicyTypeUpdateRequest request);
        Task<int> DeletePolicyType(PolicyTypeDeleteRequest request);
        Task<ResponseData<PolicyType>> GetAllPolicyType(PolicyTypeGetRequest request);
        Task<PolicyType> GetPolicyTypeById(Guid Id);
    }
}
