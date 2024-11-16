using System.ComponentModel.DataAnnotations;

namespace ViewClient.Models.DTO.Register
{
    public class RegisterInputRequest
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,10}$", ErrorMessage = "Tên người dùng phải từ 3-10 kí tự, không bao gồm kí tự đặc biệt!")]
        public string UserName { get; set; }
        [Required]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Mật khẩu gồm chữ viết hoa, chữ viết thường, chữ số và chỉ từ 8-15 kí tự!")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Vui lòng nhập đúng định dạng: example@domain.com")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
