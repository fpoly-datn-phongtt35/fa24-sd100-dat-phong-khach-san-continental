using Domain.DTO.PaymentHistory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IPaymentHistoryRepository
    {
        Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request);
        Task<DataTable> GetPaymentHistoryById(Guid id);
        Task<DataTable> GetPaymentHistoryByOrderCode(long orderCode);
        Task<DataTable> GetListPaymentHistory(PaymentHistoryGetRequest request);
        Task<int> UpdatePaymentHistoryAmount(Guid id, decimal amount);
        Task<int> DeletePaymentHistory(Guid id);
        Task<decimal> GetTotalPaidAmountByRoomBookingId(Guid roomBookingId);
    }
}
