using Domain.DTO.Paging;
using Domain.DTO.Policy;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IPolicyService
    {
        Task<int> AddPolicy(PolicyCreateRequest request);
        Task<int> UpdatePolicy(PolicyUpdateRequest request);
        Task<int> DeletePolicy(PolicyDeleteRequest request);
        Task<ResponseData<Policy>> GetAllPolicy(PolicyGetRequest request);
        Task<Policy> GetPolicyById(Guid Id);
        Task<List<PolicyTermsDto>> GetAllPolicyTerms();
    }
}
