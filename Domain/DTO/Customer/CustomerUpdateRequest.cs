﻿using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Customer
{
    public class CustomerUpdateRequest
    {
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{4,12}$", ErrorMessage = "Tên người dùng phải từ 6-12 kí tự, không bao gồm kí tự đặc biệt!")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Mật khẩu gồm chữ viết hoa, chữ viết thường, chữ số và chỉ từ 8-15 kí tự!")]
        public string Password { get; set; } = string.Empty;
        [RegularExpression(@"^[\p{L}\s]{3,50}$", ErrorMessage = "Vui lòng nhập tên chỉ chứa chữ!")]
        public string? FirstName { get; set; } = string.Empty;

        [RegularExpression(@"^[\p{L}\s]{3,50}$", ErrorMessage = "Vui lòng nhập tên chỉ chứa chữ!")]
        public string? LastName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Vui lòng nhập đúng định dạng: example@domain.com")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"^(\+?\d{1,2}[- ]?)?\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")]
        public string? PhoneNumber { get; set; } = string.Empty;
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
