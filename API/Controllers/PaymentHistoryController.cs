using API.ClientControllers;
using API.Types;
using Domain.DTO.Email;
using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services.Email;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

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
        private readonly ICustomerService _customerService;
        private readonly SendMailService _sendMailService;
        private readonly IRoomBookingDetailServiceForCustomer _roomBookingDetailServiceForCustomer;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService,
            IRoomBookingGetService roomBookingGetService, IRoomBookingUpdateService roomBookingUpdateService,
            ICustomerService customerService, SendMailService sendMailService,
            PayOS payOS, IRoomBookingDetailServiceForCustomer roomBookingDetailServiceForCustomer)
        {
            _paymentHistoryService = paymentHistoryService;
            _roomBookingGetService = roomBookingGetService;
            _roomBookingUpdateService = roomBookingUpdateService;
            _customerService = customerService;
            _sendMailService = sendMailService;
            _payOS = payOS;
            _roomBookingDetailServiceForCustomer = roomBookingDetailServiceForCustomer;
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
                throw;
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
            // Tạo request để lấy danh sách các bản ghi cần xử lý
            var request = new PaymentHistoryGetRequest
            {
                Amount = 0
            };

            // Lấy danh sách các bản ghi có Amount = 0
            var paymentHistories = await _paymentHistoryService.GetListPaymentHistory(request);

            // Tạo danh sách các tác vụ xử lý song song
            var tasks = paymentHistories.data.Select(async paymentHistory =>
            {
                try
                {
                    if (paymentHistory.OrderCode == 0)
                    {
                        // Xóa bản ghi nếu OrderCode = 0
                        await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                    }
                    else
                    {
                        // Gọi API để lấy thông tin trạng thái
                        var paymentInfo = await _payOS.getPaymentLinkInformation(paymentHistory.OrderCode);
                        if (paymentInfo.status == "PAID")
                        {
                            // Lấy thông tin RoomBooking
                            var roomBooking =
                                await _roomBookingGetService.GetRoomBookingById(paymentHistory.RoomBookingId);
                            if (roomBooking == null) return;

                            // Cập nhật Amount dựa trên Note
                            if (paymentHistory.Note == PaymentType.Bill)
                            {
                                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id,
                                    paymentInfo.amount);
                            }
                            else if (paymentHistory.Note == PaymentType.Deposit)
                            {
                                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id,
                                    (int)roomBooking.TotalRoomPrice * 20 / 100);
                                await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId,
                                    5);
                            }
                        }
                        else if (paymentInfo.status == "CANCELLED" && paymentHistory.Note == PaymentType.Bill)
                        {
                            // Xóa bản ghi nếu trạng thái là CANCELLED và loại là Bill
                            await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                        }
                        else if (paymentInfo.status == "CANCELLED" && paymentHistory.Note == PaymentType.Deposit)
                        {
                            // Xóa bản ghi nếu trạng thái là CANCELLED và loại là Deposit
                            await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
                            await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });

            // Chờ tất cả các tác vụ xử lý song song hoàn tất
            await Task.WhenAll(tasks);
        }


        [HttpGet("payment/callback-refactor")]
        public async Task<IActionResult> HandlePaymentCallBack(string code, string id, string status, long orderCode,
            bool cancel)
        {
            try
            {
                // Kiểm tra tham số đầu vào
                if (!IsValidCallBackRequest(orderCode, id))
                {
                    return BadRequest(new Response(-1, "Tham số không hợp lệ", null));
                }

                // Lấy thông tin PaymentHistory
                var paymentHistory = await _paymentHistoryService.GetPaymentHistoryByOrderCode(orderCode);
                var roomBookingId = paymentHistory.RoomBookingId;
                var roomBooking = await _roomBookingGetService.GetRoomBookingById(roomBookingId);
                var customerId = roomBooking.CustomerId;
                if (paymentHistory == null)
                {
                    return NotFound(new Response(-1, "Không tìm thấy lịch sử thanh toán", null));
                }

                //var url = $"https://localhost:7173/RoomBooking/BookingHistory?Id={customerId}";
                var url = "https://localhost:7173/Home/index";

                // Lấy thông tin trạng thái thanh toán từ PayOS
                var paymentInfo = await _payOS.getPaymentLinkInformation(orderCode);

                // Xử lý theo trạng thái thanh toán
                if (paymentInfo.status == "PAID")
                {
                    await HandlePaidStatus(paymentHistory, paymentInfo);
                }
                else if (paymentInfo.status == "CANCELLED")
                {
                    await HandleCancelledStatus(paymentHistory);
                }

                return Redirect(url);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return Redirect("https://localhost:7173/Home/index");
            }
        }

        private bool IsValidCallBackRequest(long orderCode, string id)
        {
            return orderCode != 0 && !string.IsNullOrEmpty(id);
        }

        private async Task HandlePaidStatus(PaymentHistory paymentHistory,
            PaymentLinkInformation paymentLinkInformation)
        {
            var roomBookingResponse = await _roomBookingGetService
                .GetRoomBookingById(paymentHistory.RoomBookingId);
            if (roomBookingResponse == null)
                throw new Exception("Không tìm thấy thông tin RoomBooking");

            var roomBooking = roomBookingResponse.ToRoomBooking();

            var customer = await _customerService.GetCustomerById(roomBookingResponse.CustomerId);
            if (customer == null)
                throw new Exception("Không tìm thấy thông tin Customer");

            var roomDetails = await GetRoomDetails(roomBookingResponse.Id);
            var checkinDetails = await GetFormattedCheckinDetails(roomBookingResponse.Id);
            var totalPaidAmount = await UpdatePaymentHistory2(paymentHistory, roomBooking, paymentLinkInformation);

            var emailRequest = new EmailRequest
            {
                ToEmail = customer.Email,
                EmailType = 2,
                RoomDetails = roomDetails,
                BookingTime = checkinDetails,
                TotalPrice = roomBookingResponse.TotalPrice ?? 0,
                PaidAmount = totalPaidAmount,
            };

            await SendEmail(emailRequest);
        }

        private async Task HandleCancelledStatus(PaymentHistory paymentHistory)
        {
            await _paymentHistoryService.DeletePaymentHistory(paymentHistory.Id);
            await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 3);
        }

        private async Task<string> GetRoomDetails(Guid roomBookingId)
        {
            var roomBookingDetails = await _roomBookingDetailServiceForCustomer
                .GetListRoomBookingDetailByRoomBookingId(roomBookingId);
            if (roomBookingDetails == null || !roomBookingDetails.Any())
                throw new Exception("Không tìm thấy thông tin chi tiết phòng");

            return string.Join(", ", roomBookingDetails.Select(d => $"Phòng {d.Name}"));
        }

        private async Task<string> GetFormattedCheckinDetails(Guid roomBookingId)
        {
            var checkinTimes = await _roomBookingGetService
                .GetCheckinRoomBookingByRoomBookingId(roomBookingId);
            return string.Join(", ", checkinTimes.Select(t => t.ToString("HH:mm, 'ngày' dd/MM/yyyy")));
        }

        private async Task<decimal> UpdatePaymentHistory2(PaymentHistory paymentHistory, RoomBooking roomBooking,
            PaymentLinkInformation paymentLinkInformation)
        {
            if (paymentHistory.Note == PaymentType.Bill)
            {
                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id,
                    paymentLinkInformation.amount);
                await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 2);
            }
            else if (paymentHistory.Note == PaymentType.Deposit)
            {
                int depositAmount = (int)(roomBooking.TotalRoomPrice * 20 / 100);
                await _paymentHistoryService.UpdatePaymentHistoryAmount(paymentHistory.Id, depositAmount);
                await _roomBookingUpdateService.UpdateRoomBookingStatus(paymentHistory.RoomBookingId, 5);
            }

            return await _paymentHistoryService.GetTotalPaidAmountByRoomBookingId(roomBooking.Id);
        }


        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail(EmailRequest? emailRequest)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail))
            {
                return BadRequest("Invalid email request");
            }

            string subject;
            string body;

            switch (emailRequest.EmailType)
            {
                case 1: // Nhắc nhở khi có lịch đặt phòng đến hẹn
                    subject = "Nhắc nhở lịch đặt phòng";
                    body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Nhắc nhở lịch đặt phòng</title>
                </head>
                <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                    <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; background-color: #ffffff; border: 1px solid #e0e0e0; margin: 20px auto;'>
                        <!-- Header -->
                        <tr>
                            <td style='background-color: #ffffff; padding: 20px; text-align: center;'>
                                <h2 style='color: #000000; margin: 0; font-size: 24px;'>Nhắc nhở lịch đặt phòng</h2>
                            </td>
                        </tr>
                        <!-- Body -->
                        <tr>
                            <td style='padding: 20px;'>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Xin chào,</p>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Đây là nhắc nhở rằng bạn có một lịch đặt phòng sắp đến hạn. Vui lòng kiểm tra thông tin dưới đây:</p>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin: 20px 0;'>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Phòng:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{emailRequest.RoomDetails}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Thời gian đặt:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{emailRequest.BookingTime}</td>
                                    </tr>
                                </table>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Xin vui lòng đảm bảo đến đúng giờ. Nếu cần hỗ trợ, hãy liên hệ với chúng tôi!</p>
                                <p style='text-align: center; margin: 20px 0;'>
                                    <a href='#' style='background-color: #007bff; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Xem chi tiết</a>
                                </p>
                            </td>
                        </tr>
                        <!-- Footer -->
                        <tr>
                            <td style='background-color: #f8f8f8; padding: 20px; text-align: center; font-size: 14px; color: #666666;'>
                                <p style='margin: 0;'>13 Trịnh Văn Bô, phường Phương Canh, quận Nam Từ Liêm</p>
                                <p style='margin: 5px 0;'>Email: duantotnghiepfptpolytechnic@gmail.com | Hotline: 84 968458834</p>
                                <p style='margin: 5px 0;'>© 2025 Continental. Bảo lưu mọi quyền.</p>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
                    break;

                case 2: // Xác nhận đặt phòng
                    subject = "Xác nhận đặt phòng";
                    body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Xác nhận đặt phòng</title>
                </head>
                <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                    <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; background-color: #ffffff; border: 1px solid #e0e0e0; margin: 20px auto;'>
                        <!-- Header -->
                        <tr>
                            <td style='background-color: #ffffff; padding: 20px; text-align: center;'>
                                <h2 style='color: #000000; margin: 0; font-size: 24px;'>Nhắc nhở lịch đặt phòng</h2>
                            </td>
                        </tr>
                        <!-- Body -->
                        <tr>
                            <td style='padding: 20px;'>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Xin chào,</p>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Cảm ơn bạn đã đặt phòng với chúng tôi. Dưới đây là chi tiết đặt phòng của bạn:</p>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin: 20px 0;'>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Phòng:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{emailRequest.RoomDetails}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Thời gian đặt:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{emailRequest.BookingTime}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Tổng tiền phòng:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{FormatCurrency(emailRequest.TotalPrice)} VND</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'><strong>Số tiền đã thanh toán:</strong></td>
                                        <td style='padding: 10px 0; font-size: 16px; color: #333333;'>{FormatCurrency(emailRequest.PaidAmount)} VND</td>
                                    </tr>
                                </table>
                                <p style='font-size: 16px; color: #333333; line-height: 1.5;'>Nếu bạn không thực hiện đặt phòng này, vui lòng liên hệ chúng tôi ngay lập tức.</p>
                                <p style='text-align: center; margin: 20px 0;'>
                                    <a href='#' style='background-color: #28a745; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Xem chi tiết đặt phòng</a>
                                </p>
                            </td>
                        </tr>
                        <!-- Footer -->
                        <tr>
                            <td style='background-color: #f8f8f8; padding: 20px; text-align: center; font-size: 14px; color: #666666;'>
                                <p style='margin: 0;'>13 Trịnh Văn Bô, phường Phương Canh, quận Nam Từ Liêm</p>
                                <p style='margin: 5px 0;'>Email: duantotnghiepfptpolytechnic@gmail.com | Hotline: 84 968458834</p>
                                <p style='margin: 5px 0;'>© 2025 Continental. Bảo lưu mọi quyền.</p>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
                    break;

                default:
                    return BadRequest("Invalid email type.");
            }

            var mailContent = new MailContent
            {
                To = emailRequest.ToEmail,
                Subject = subject,
                Body = body
            };

            var result = await _sendMailService.SendMail(mailContent);

            if (result == "Success")
            {
                return Ok("Email sent successfully.");
            }

            return StatusCode(500, "Failed to send email.");
        }

        private string FormatCurrency(decimal? amount)
        {
            if (amount.HasValue)
            {
                return string.Format("{0:N0}", amount.Value); // Định dạng với dấu phẩy hàng nghìn
            }

            return "0"; // Nếu không có giá trị, trả về "0"
        }
    }
}