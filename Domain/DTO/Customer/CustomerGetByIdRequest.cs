using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Customer
{
    public class CustomerGetByIdRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public int? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}
