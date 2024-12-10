using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ResidenceRegistration
{
    public class ResidenceUpdateRequest
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType? Gender { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; } 
    }
}
