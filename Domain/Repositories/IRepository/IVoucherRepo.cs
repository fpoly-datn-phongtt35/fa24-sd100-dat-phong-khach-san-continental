using Domain.DTO.Voucher;
using System.Data;

namespace Domain.Repositories.IRepository
{
	public interface IVoucherRepo
	{
		Task<int> AddVoucher(VoucherCreateRequest request);
		Task<int> UpdateVoucher(VoucherUpdateRequest request);
		Task<int> DeleteVoucher(VoucherDeleteRequest request);
		Task<DataTable> GetAllVoucher(VoucherGetRequest Voucher);
		Task<DataTable> GetVoucherById(Guid id);
	}
}
