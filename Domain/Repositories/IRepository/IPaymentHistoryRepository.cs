using Domain.DTO.PaymentHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IPaymentHistoryRepository
    {
        Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request);
    }
}
