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

        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService,
            IRoomBookingGetService roomBookingGetService, IRoomBookingUpdateService roomBookingUpdateService,
            PayOS payOS)
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
                await UpdatePaymentHistory();
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
                await UpdatePaymentHistory();
                return await _paymentHistoryService.GetListPaymentHistory(request);
            }
            catch (Exception ex)
            {
                throw ex;
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

        private async Task UpdatePaymentHistory()
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
                        await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id,
                            (int)roomBooking.TotalRoomPrice * 20 / 100);
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 5);
                    }
                }
                else if (paymentInfo.status == "CANCELLED")
                {
                    await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                }
            }
        }

        [HttpGet("payment/callback")]
        public async Task<IActionResult> HandlePaymentCallback(string code, string id, string status, long orderCode,
            bool cancel)
        {
            try
            {
                // Kiểm tra các tham số trả về từ PayOS.
                if (orderCode == 0 || string.IsNullOrEmpty(id))
                {
                    return BadRequest(new Response(-1, "Tham số không hợp lệ", null));
                }
                
                // Lấy thông tin PaymentHistory dựa trên orderCode.
                var paymentHistory = await _paymentHistoryService.GetPaymentHistoryByOrderCode(orderCode);
                if (paymentHistory == null)
                {
                    return NotFound(new Response(-1, "Không tìm thấy lịch sử thanh toán", null));
                }

                // Lấy thông tin trạng thái thanh toán từ PayOS.
                var paymentInfo = await _payOS.getPaymentLinkInformation(orderCode);

                // Nếu thanh toán thành công (status == "PAID")
                if (paymentInfo.status == "PAID")
                {
                    // Lấy thông tin RoomBooking
                    var roomBooking = await _roomBookingGetService.GetRoomBookingById(paymentHistory.RoomBookingId);
                    if (roomBooking == null)
                        return NotFound(new Response(-1, "Không tìm thấy thông tin RoomBooking", null));

                    // Cập nhật số tiền trong PaymentHistory dựa trên loại PaymentType
                    if (paymentHistory.Note == PaymentType.Bill)
                    {
                        await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, paymentInfo.amount);
                        // Cập nhật trạng thái RoomBooking
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId,
                            2); // Trạng thái thành công
                    }
                    else if (paymentHistory.Note == PaymentType.Deposit)
                    {
                        int depositAmount = (int)(roomBooking.TotalRoomPrice * 20 / 100);
                        await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, depositAmount);
                        // Cập nhật trạng thái RoomBooking
                        await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId,
                            5); // Trạng thái Deposit
                    }
                }
                else if (paymentInfo.status == "CANCELLED")
                {
                    // Nếu thanh toán bị hủy, xóa lịch sử thanh toán
                    await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                    // Có thể cập nhật trạng thái RoomBooking nếu cần
                    await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId,
                        1); // Trạng thái hủy
                }

                return Ok(new Response(0, "Cập nhật thanh toán thành công", null));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest(new Response(-1, "Đã xảy ra lỗi: " + ex.Message, null));
            }
        }


        //[HttpPut("update-payment-history")]
        //public async Task<IActionResult> UpdatePaymentHistory()
        //{
        //    try
        //    {
        //        var request = new PaymentHistoryGetRequest
        //        {
        //            Amount = 0
        //        };
        //        // Lấy danh sách các bản ghi có Amount = 0
        //        var paymentHistories = await _paymentHistoryService.GetListPaymentHistory(request);

        //        foreach (var paymentHistory in paymentHistories.data)
        //        {
        //            // Gọi API để lấy thông tin trạng thái
        //            var paymentInfo = await _payOS.getPaymentLinkInformation(paymentHistory.OrderCode);
        //            if (paymentInfo.status == "PAID")
        //            {
        //                // Lấy thông tin RoomBooking
        //                var roomBooking = await _roomBookingGetService.GetRoomBookingById(paymentHistory.RoomBookingId);
        //                if (roomBooking == null) continue;

        //                // Cập nhật Amount dựa trên Note
        //                if (paymentHistory.Note == PaymentType.Bill)
        //                {
        //                    await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, paymentInfo.amount);
        //                    await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 2);
        //                }
        //                else if (paymentHistory.Note == PaymentType.Deposit)
        //                {
        //                    await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, (int)roomBooking.TotalRoomPrice * 20 / 100);
        //                    await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 5);
        //                }
        //            }
        //            else if (paymentInfo.status == "CANCELLED")
        //            {
        //                await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
        //            }
        //        }

        //        return Ok(new Response(0, "Payment history updated successfully", null));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new Response(-1, ex.Message, null));
        //    }
        //}
    }
}