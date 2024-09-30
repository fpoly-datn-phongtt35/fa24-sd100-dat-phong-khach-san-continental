using Domain.DTO.Paging;
using Domain.DTO.VoucherDetail;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherDetailController : ControllerBase
    {
        private readonly IVoucherDetailService _voucherDetailService;

        public VoucherDetailController(IVoucherDetailService voucherDetailService)
        {
            _voucherDetailService = voucherDetailService;
        }

        [HttpPost]
        [Route("AddVoucherDetail")]
        public async Task<int> AddVoucherDetail(VoucherDetailCreateRequest request)
        {
            try
            {
                return await _voucherDetailService.AddVoucherDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListVoucherDetail")]
        public async Task<ResponseData<VoucherDetail>> GetListVoucherDetail(VoucherDetailGetRequest request)
        {
            try
            {
                return await _voucherDetailService.GetVoucherDetails(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteVoucherDetail")]
        public async Task<int> DeleteVoucherDetail(VoucherDetailDeleteRequest request)
        {
            try
            {
                return await _voucherDetailService.DeleteVoucherDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateVoucherDetail")]
        public async Task<int> UpdateVoucherDetail(VoucherDetailUpdateRequest request)
        {
            try
            {
                return await _voucherDetailService.UpdateVoucherDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetVoucherDetailById")]
        public async Task<VoucherDetail> GetVoucherDetailById(Guid Id)
        {
            try
            {
                return await _voucherDetailService.GetVoucherDetailById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
