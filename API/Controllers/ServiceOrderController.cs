using Domain.DTO.Paging;
using Domain.DTO.ServiceOrder;
using Domain.DTO.ServiceType;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderService;

        public ServiceOrderController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }
        [HttpPost("GetListServiceOrders")]
        public async Task<ResponseData<ServiceOrder>> GetListServiceOrders(ServiceOrderGetRequest request)
        {
            try
            {
                return await _serviceOrderService.GetServiceOrders(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("CreateServiceOrder")]    
        public async Task<int> CreateServiceOrder(ServiceOrderCreateRequest request)
        {
            try
            {
                return await _serviceOrderService.AddServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateServiceOrder")]
        public async Task<int> UpdateServiceOrder(ServiceOrderUpdateRequest request)
        {
            try
            {
                return await _serviceOrderService.UpdateServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteServiceOrder")]
        public async Task<int> DeleteServiceType(ServiceOrderDeleteRequest request)
        {
            try
            {
                return await _serviceOrderService.DeleteServiceOrder(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetServiceOrderById")]
        public async Task<ServiceOrder> GetServiceOrderById(Guid Id)
        {
            try
            {
                return await _serviceOrderService.GetServiceOrderById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
