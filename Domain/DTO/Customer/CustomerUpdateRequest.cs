using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Customer
{
    public class CustomerUpdateRequest
    {
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,12}$", ErrorMessage = "Tên người dùng phải từ 3-12 kí tự, không bao gồm kí tự đặc biệt!")]
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Vui lòng nhập đúng định dạng: example@domain.com")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"^(\+?\d{1,2}[- ]?)?\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")]
        public string PhoneNumber { get; set; } = string.Empty;
        public GenderType? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
