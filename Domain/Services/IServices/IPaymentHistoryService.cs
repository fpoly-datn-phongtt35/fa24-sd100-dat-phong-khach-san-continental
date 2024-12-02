using Domain.DTO.PaymentHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IPaymentHistoryService
    {
        Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request);
    }
}
