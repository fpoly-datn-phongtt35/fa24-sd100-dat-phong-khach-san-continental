namespace Domain.DTO.Email
{
    public class AccountRequest
    {
        public string ToEmail { get; set; } // Email người nhận
        public int EmailType { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
