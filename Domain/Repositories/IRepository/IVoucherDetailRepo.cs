using Domain.DTO.VoucherDetail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IVoucherDetailRepo
    {
        Task<int> AddVoucherDetail(VoucherDetailCreateRequest request);
        Task<int> UpdateVoucherDetail(VoucherDetailUpdateRequest request);
        Task<int> DeleteVoucherDetail(VoucherDetailDeleteRequest request);
        Task<DataTable> GetVoucherDetails(VoucherDetailGetRequest request);
    }
}
