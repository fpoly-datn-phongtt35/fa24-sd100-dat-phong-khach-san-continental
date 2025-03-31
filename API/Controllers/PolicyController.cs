using Domain.DTO.Paging;
using Domain.DTO.Policy;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyRepo;
        public PolicyController(IPolicyService policyRepo)
        {
            _policyRepo = policyRepo;
        }

        [HttpGet("GetAllPolicyTerms")]
        public async Task<IActionResult> GetAllPolicyTerms()
        {
            try
            {
                var policyTerms = await _policyRepo.GetAllPolicyTerms();
                return Ok(policyTerms);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("CreatePolicy")]
        public async Task<int> CreatePolicy(PolicyCreateRequest request)
        {
            try
            {
                return await _policyRepo.AddPolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetListPolicy")]
        public async Task<ResponseData<Policy>> GetListPolicy(PolicyGetRequest request)
        {
            try
            {
                return await _policyRepo.GetAllPolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetPolicyById")]
        public async Task<Policy> GetPolicyById(Guid Id)
        {
            try
            {
                return await _policyRepo.GetPolicyById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdatePolicy")]
        public async Task<int> UpdatePolicy(PolicyUpdateRequest request)
        {
            try
            {
                return await _policyRepo.UpdatePolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeletePolicy")]
        public async Task<int> DeletePolicy(PolicyDeleteRequest request)
        {
            try
            {
                return await _policyRepo.DeletePolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
