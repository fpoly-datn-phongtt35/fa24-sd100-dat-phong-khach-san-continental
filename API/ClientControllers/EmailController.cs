using Domain.DTO.Email;
using Domain.Services.Services.Email;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : Controller
{
    private readonly SendMailService _sendMailService;

    public EmailController(SendMailService sendMailService)
    {
        _sendMailService = sendMailService;
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
    [HttpPost("send-account")]
    public async Task<IActionResult> SendAccount(AccountRequest? emailRequest)
    {
        if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail))
        {
            return BadRequest("Invalid email request");
        }

        string subject;
        string body;

        switch (emailRequest.EmailType)
        {
            case 1:
                subject = "Cấp tài khoản";
                body = $@"
                <h3>Xin chào,</h3>
                <p>Đây là thư cung cấp tài khoản website đặt phòng khách sạn Continental.</p>
                <p>Chi tiết:</p>
                <ul>
                    <li>Tài khoản: {emailRequest.UserName}</li>
                    <li>Mật khẩu: {emailRequest.Password}</li>
                </ul>
                <p>Bạn có thể thay đổi mật khẩu trong trang web, hãy đăng nhập vào trang web để có trải nghiệm tốt hơn. Xin cảm ơn!</p>";
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
}

