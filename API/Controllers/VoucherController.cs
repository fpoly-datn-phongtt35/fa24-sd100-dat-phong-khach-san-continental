using Domain.DTO.Paging;
using Domain.DTO.Voucher;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VoucherController : ControllerBase
	{

		private readonly IVoucherService _voucherService;
		public VoucherController(IVoucherService voucherService)
		{
			_voucherService = voucherService;
		}

		[HttpPost("CreateVoucher")]
		public async Task<int> CreateVoucher(VoucherCreateRequest request)
		{
			try
			{
				return await _voucherService.AddVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[HttpPost("GetListVoucher")]
		public async Task<ResponseData<Voucher>> GetListVoucher(VoucherGetRequest request)
		{
			try
			{
				return await _voucherService.GetAllVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost("GetVoucherById")]
		public async Task<Voucher> GetVoucherById(Guid Id)
		{
			try
			{
				return await _voucherService.GetVoucherById(Id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPut("UpdateVoucher")]
		public async Task<int> UpdateVoucher(VoucherUpdateRequest request)
		{
			try
			{
				return await _voucherService.UpdateVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost("DeleteVoucher")]
		public async Task<int> DeleteVoucher(VoucherDeleteRequest request)
		{
			try
			{
				return await _voucherService.DeleteVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
