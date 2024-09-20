using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceSV;

        public ServiceController(IServiceService serviceSV)
        {
            _serviceSV = serviceSV;
        }

        [HttpPost("CreateService")]
        public async Task<int> CreateServiceType(ServiceCreateRequest request)
        {
            try
            {
                return await _serviceSV.AddService(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListService")]
        public async Task<ResponseData<Service>> GetListService(ServiceGetRequest request)
        {
            try
            {
                return await _serviceSV.GetServices(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetServiceById")]
        public async Task<Service> GetServiceById(Guid Id)
        {
            try
            {
                return await _serviceSV.GetServiceById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost("GetServiceByTypeId")]
        //public async Task<ResponseData<Service>> GetServiceByTypeId([FromBody] ServiceGetRequest request, [FromQuery] Guid serviceTypeId)
        //{
        //    try
        //    {
        //        return await _serviceSV.GetServiceByTypeId(request, serviceTypeId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        [HttpPut("UpdateService")]
        public async Task<int> UpdateService(ServiceUpdateRequest request)
        {
            try
            {
                return await _serviceSV.UpdateService(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteService")]
        public async Task<int> DeleteService([FromBody] ServiceDeleteRequest request)
        {
            try
            {
                return await _serviceSV.DeleteService(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
