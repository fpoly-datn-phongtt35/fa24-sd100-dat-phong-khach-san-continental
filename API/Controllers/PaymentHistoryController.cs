using Domain.DTO.PaymentHistory;
using Domain.Services.IServices;
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
    }
}
