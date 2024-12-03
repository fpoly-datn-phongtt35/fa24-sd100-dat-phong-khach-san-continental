using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.Voucher;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly IPaymentHistoryService _paymentHistoryService;

        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService)
        {
            _paymentHistoryService = paymentHistoryService;
        }

        [HttpPost(nameof(AddPaymentHistory))]
        public async Task<int> AddPaymentHistory(PaymentHistoryCreateRequest request)
        {
            try
            {
                return await _paymentHistoryService.AddPaymentHistory(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetPaymentHistoryById")]
        public async Task<PaymentHistory> GetPaymentHistoryById(Guid Id)
        {
            try
            {
                return await _paymentHistoryService.GetPaymentHistoryById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListPaymentHistory")]
        public async Task<ResponseData<PaymentHistory>> GetListPaymentHistory(PaymentHistoryGetRequest request)
        {
            try
            {
                return await _paymentHistoryService.GetListPaymentHistory(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
