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
                    <h3>Xin chào,</h3>
                    <p>Đây là nhắc nhở rằng bạn có một lịch đặt phòng sắp đến hạn.</p>
                    <p>Chi tiết lịch hẹn:</p>
                    <ul>
                        <li>Phòng: P808</li>
                        <li>Thời gian: 14:00, ngày 5/12/2024</li>
                    </ul>
                    <p>Xin vui lòng đảm bảo đến đúng giờ. Xin cảm ơn!</p>";
                break;

            case 2: // Xác nhận đặt phòng
                subject = "Xác nhận đặt phòng";
                body = $@"
                    <h3>Xin chào,</h3>
                    <p>Bạn đã đặt phòng P808 thành công.</p>
                    <p>Thời gian: 14:00, ngày 5/12/2024</p>
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
}

public class EmailRequest
{
    public string ToEmail { get; set; } // Email người nhận
    public int EmailType { get; set; } // 1: Nhắc nhở, 2: Xác nhận
}