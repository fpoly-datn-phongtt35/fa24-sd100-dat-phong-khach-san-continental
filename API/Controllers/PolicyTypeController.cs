using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.DTO.PolicyType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyTypeController : ControllerBase
    {
        private readonly IPolicyTypeService _policyTypeRepo;
        public PolicyTypeController(IPolicyTypeService policyTypeRepo)
        {
            _policyTypeRepo = policyTypeRepo;
        }

        [HttpPost("CreatePolicyType")]
        public async Task<int> CreatePolicyType(PolicyTypeCreateRequest request)
        {
            try
            {
                return await _policyTypeRepo.AddPolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListPolicyType")]
        public async Task<ResponseData<PolicyType>> GetListPolicyType(PolicyTypeGetRequest request)
        {
            try
            {
                return await _policyTypeRepo.GetAllPolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetPolicyTypeById")]
        public async Task<PolicyType> GetPolicyTypeById(Guid Id)
        {
            try
            {
                return await _policyTypeRepo.GetPolicyTypeById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdatePolicyType")]
        public async Task<int> UpdatePolicyType(PolicyTypeUpdateRequest request)
        {
            try
            {
                return await _policyTypeRepo.UpdatePolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeletePolicyType")]
        public async Task<int> DeletePolicyType(PolicyTypeDeleteRequest request)
        {
            try
            {
                return await _policyTypeRepo.DeletePolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
