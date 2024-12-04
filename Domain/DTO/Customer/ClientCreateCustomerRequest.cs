using System.ComponentModel.DataAnnotations;
namespace Domain.DTO.Customer
{
    public class ClientCreateCustomerRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
    }
}
