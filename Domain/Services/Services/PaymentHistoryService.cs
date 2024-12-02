using Domain.DTO.PaymentHistory;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepo;
        private readonly IConfiguration _configuration;

        public PaymentHistoryService(IPaymentHistoryRepository paymentHistoryRepo, IConfiguration configuration)
        {
            _paymentHistoryRepo = paymentHistoryRepo;
            _configuration = configuration;
        }

        public async Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request)
        {
            try
            {
                return await _paymentHistoryRepo.AddPaymentHistory(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
