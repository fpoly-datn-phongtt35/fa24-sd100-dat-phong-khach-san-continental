using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace View.Models.Customer
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
    }
}
