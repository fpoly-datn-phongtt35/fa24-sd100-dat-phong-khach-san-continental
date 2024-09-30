using Domain.DTO.Paging;
using Domain.DTO.ServiceType;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeRepo;
        public ServiceTypeController(IServiceTypeService serviceTypeRepo)
        {
            _serviceTypeRepo = serviceTypeRepo;
        }

        [HttpPost("CreateServiceType")]
        public async Task<int> CreateServiceType(ServiceTypeCreateRequest request)
        {
            try 
            {
                return await _serviceTypeRepo.AddServiceType(request);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        [HttpPost("GetListServiceType")]
        public async Task<ResponseData<ServiceType>> GetListServiceType(ServiceTypeGetRequest request)
        {
            try
            {
                return await _serviceTypeRepo.GetServiceTypes(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetServiceTypeById")]
        public async Task<ServiceType> GetServiceTypeById(Guid Id)
        {
            try
            {
                return await _serviceTypeRepo.GetServiceTypeById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateServiceType")]
        public async Task<int> UpdateServiceType(ServiceTypeUpdateRequest request)
        {
            try
            {
                return await _serviceTypeRepo.UpdateServiceType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteServiceType")]
        public async Task<int> DeleteServiceType(ServiceTypeDeleteRequest request)
        {
            try
            {
                return await _serviceTypeRepo.DeleteServiceType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
