using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Athorization
{
    public class AuthorizationViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
    }
}
