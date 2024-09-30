using Domain.DTO.Paging;
using Domain.DTO.VoucherDetail;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IVoucherDetailService
    {
        Task<int> AddVoucherDetail(VoucherDetailCreateRequest request);
        Task<int> UpdateVoucherDetail(VoucherDetailUpdateRequest request);
        Task<int> DeleteVoucherDetail(VoucherDetailDeleteRequest request);
        Task<VoucherDetail> GetVoucherDetailById(Guid Id);
        Task<ResponseData<VoucherDetail>> GetVoucherDetails(VoucherDetailGetRequest request);
    }
}
