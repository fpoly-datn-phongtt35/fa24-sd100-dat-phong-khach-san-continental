using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MailHelpers
    {
        /*public async Task<bool> SeedGmail(SeedMailRequest request,string mailName,string appPass)
        {
            User user = new();
            using (var context = new ContinentalDbContext())
            {
                var genericRepository = new GenericRepository<User>(context);
                var getUser = genericRepository.FindAsync(x => x.Email.ToLower() == request.email.ToLower());
                if (getUser != null) 
                {
                    user.Email = getUser.Result.Email;
                }
            }
            
       
            if (user.Email != null)
            {
                var mail = mailName;
                var appPassword = appPass; // Mã xác thực một lần tạo từ tài khoản Gmail của bạn
                var emailMessage = new MimeMessage();
                if (request.type)
                {

                    Random rand = new Random();
                    var message = rand.Next(10000, 99999);


                    emailMessage.From.Add(new MailboxAddress("Mei", mail));
                    emailMessage.To.Add(new MailboxAddress("Recipient Name", request.email));
                    emailMessage.Subject = "Xác nhận thay đổi mật khẩu";
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = $@"
                <p>Xin chào,</p>
                <p>Bạn đã tạo tài khoản thành công. Vui lòng nhấp vào nút bên dưới để kích hoạt tài khoản:</p>
                <a href=""https://example.com/confirm-password?code={message.ToString()}"">
                <button style=""background-color: #4CAF50; color: white; padding: 10px 20px; border: none; font-size: 16px;"">Kích Hoạt</button>
                </a>";

                    emailMessage.Body = bodyBuilder.ToMessageBody();

                }
                else
                {
                    Random rand = new Random();
                    var message = rand.Next(10000, 99999);


                    emailMessage.From.Add(new MailboxAddress("Mei", mail));
                    emailMessage.To.Add(new MailboxAddress("Recipient Name", request.email));
                    emailMessage.Subject = "Xác nhận thay đổi mật khẩu";
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = $@"
                <p>Xin chào,</p>
                <p>Bạn đã yêu cầu thay đổi mật khẩu. Dưới đây là mã xác nhận tài khoản của bạn:</p>
                <a>
                   {message.ToString()}
                </a>
                <p>Mã sẽ có hiệu lực trong vòng 5 phút!</p>
                <p>Vui lòng không chia sẻ cho ai khác!</p>
                ";

                    emailMessage.Body = bodyBuilder.ToMessageBody();

*//*                    user.ConfirmCode = message.ToString();
                    user.SentTime = DateTime.UtcNow.AddMinutes(2);
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();*//*
                }


                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(mail, appPassword);
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
                return true;
            }
            return false;
        }*/
    }
}
