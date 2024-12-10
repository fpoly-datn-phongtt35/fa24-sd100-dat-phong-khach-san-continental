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

    [HttpPost("send-email-test")]
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
                <h3>Xin chào,</h3>
                <p>Đây là nhắc nhở rằng bạn có một lịch đặt phòng sắp đến hạn.</p>
                <p>Chi tiết đặt phòng:</p>
                <ul>
                    <li>Phòng: {emailRequest.RoomDetails}</li>
                    <li>Thời gian đặt: {emailRequest.BookingTime}</li>
                </ul>
                <p>Xin vui lòng đảm bảo đến đúng giờ. Xin cảm ơn!</p>";
                break;

            case 2: // Xác nhận đặt phòng
                subject = "Xác nhận đặt phòng";
                body = $@"
                    <h3>Xin chào,</h3>
                    <p>Bạn đã đặt phòng thành công.</p>
                    <p>Chi tiết đặt phòng:</p>
                    <ul style='font-family: Arial, sans-serif;'>
                        <li><strong>Phòng:</strong>     {emailRequest.RoomDetails}</li>
                        <li><strong>Thời gian đặt:</strong>     {emailRequest.BookingTime}</li>
                        <li><strong>Tổng tiền phòng:</strong>      {FormatCurrency(emailRequest.TotalPrice)} VND</li>
                        <li><strong>Số tiền đã thanh toán:</strong>     {FormatCurrency(emailRequest.PaidAmount)} VND</li>
                    </ul>
                    <p>Nếu bạn không thực hiện đặt phòng này, hãy bỏ qua email này. Xin cảm ơn!</p>";
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

public class EmailRequest
{
    public string ToEmail { get; set; } // Email người nhận
    public int EmailType { get; set; } // 1: Nhắc nhở, 2: Xác nhận
    public string? RoomDetails { get; set; }
    public string? BookingTime { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? PaidAmount { get; set; }
}