using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IPaymentHistoryService
    {
        Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request);
        Task<PaymentHistory> GetPaymentHistoryById(Guid id);
        Task<ResponseData<PaymentHistory>> GetListPaymentHistory(PaymentHistoryGetRequest request);
        Task<int> UpdatePaymentHistoryAmount(Guid id, decimal amount);
        Task<int> DeletePaymentHistory(Guid id);
    }
}
