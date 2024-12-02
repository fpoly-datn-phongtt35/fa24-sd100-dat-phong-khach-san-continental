using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrderDetail;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderDetailController : ControllerBase
    {
        private readonly IServiceOrderDetailService _serviceOrderDetailService;
        public ServiceOrderDetailController(IServiceOrderDetailService serviceOrderDetailService)
        {
            _serviceOrderDetailService = serviceOrderDetailService;
        }

        [HttpPost]
        [Route("AddServiceOrderDetail")]
        public async Task<int> AddServiceOrderDetail(ServiceOrderDetail request)
        {
            try
            {
                return  await _serviceOrderDetailService.UpsertServiceOrderDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListServiceOrderDetail")]
        public async Task<ResponseData<ServiceOrderDetail>> GetListServiceOrderDetail(ServiceOrderDetailGetRequest request)
        {
            try
            {
                return await _serviceOrderDetailService.GetServiceOrderDetails(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetServiceOrderDetailByRoomBookingId")]
        public async Task<ResponseData<ServiceOrderDetail>> GetListServiceOrderDetailByRoomBookingId(Guid id)
        {
            try
            {
                return await _serviceOrderDetailService.GetServiceOrderDetailByRoomBookingId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteServiceOrderDetail")]
        public async Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request)
        {
            try
            {
                return await _serviceOrderDetailService.DeleteServiceOrderDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetServiceOrderDetailById")]
        public async Task<ServiceOrderDetail> GetServiceOrderDetailById(Guid Id)
        {
            try
            {
                return await _serviceOrderDetailService.GetServiceOrderDetailById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
