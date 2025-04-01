using Domain.DTO.Policy;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IPolicyRepo
    {
        Task<int> AddPolicy(PolicyCreateRequest request);
        Task<int> UpdatePolicy(PolicyUpdateRequest request);
        Task<int> DeletePolicy(PolicyDeleteRequest request);
        Task<DataTable> GetAllPolicy(PolicyGetRequest request);
        Task<DataTable> GetPolicyById(Guid id);
        Task<List<PolicyTermsDto>> GetAllPolicyTerms();
    }
}
