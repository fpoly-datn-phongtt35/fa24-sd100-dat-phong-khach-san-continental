using API.Types;
using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IRoomBookingGetService _roomBookingGetService;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly IRoomBookingUpdateService _roomBookingUpdateService;

        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService, IRoomBookingGetService roomBookingGetService, IRoomBookingUpdateService roomBookingUpdateService, PayOS payOS)
        {
            _paymentHistoryService = paymentHistoryService;
            _roomBookingGetService = roomBookingGetService;
            _roomBookingUpdateService = roomBookingUpdateService;
            _payOS = payOS; 
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


        [HttpPut("update-payment-history")]
        public async Task<IActionResult> UpdatePaymentHistory()
        {
            try
            {
                var request = new PaymentHistoryGetRequest
                {
                    Amount = 0
                };
                // Lấy danh sách các bản ghi có Amount = 0
                var paymentHistories = await _paymentHistoryService.GetListPaymentHistory(request);

                foreach (var paymentHistory in paymentHistories.data)
                {
                    // Gọi API để lấy thông tin trạng thái
                    var paymentInfo = await _payOS.getPaymentLinkInformation(paymentHistory.OrderCode);
                    if (paymentInfo.status == "PAID")
                    {
                        // Lấy thông tin RoomBooking
                        var roomBooking = await _roomBookingGetService.GetRoomBookingById(paymentHistory.RoomBookingId);
                        if (roomBooking == null) continue;

                        // Cập nhật Amount dựa trên Note
                        if (paymentHistory.Note == PaymentType.Bill)
                        {
                            await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, paymentInfo.amount);
                            await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 2);
                        }
                        else if (paymentHistory.Note == PaymentType.Deposit)
                        {
                            await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, (int)roomBooking.TotalRoomPrice * 20 / 100);
                            await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 5);
                        }
                    }
                    else if (paymentInfo.status == "CANCELLED")
                    {
                        await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                    }
                }

                return Ok(new Response(0, "Payment history updated successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response(-1, ex.Message, null));
            }
        }

        [HttpDelete(nameof(DeletePaymentHistory))]
        public async Task<int> DeletePaymentHistory(Guid id)
        {
            try
            {
                return await _paymentHistoryService.DeletePaymentHistory(id);
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
