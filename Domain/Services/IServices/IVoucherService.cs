using Domain.DTO.Paging;
using Domain.DTO.Voucher;
using Domain.Models;
using System.Data;

namespace Domain.Services.IServices
{
	public interface IVoucherService
	{
		Task<int> AddVoucher(VoucherCreateRequest request);
		Task<int> UpdateVoucher(VoucherUpdateRequest request);
		Task<int> DeleteVoucher(VoucherDeleteRequest request);
		Task<ResponseData<Voucher>> GetAllVoucher(VoucherGetRequest Voucher);
		Task<Voucher> GetVoucherById(Guid id);
	}
}
