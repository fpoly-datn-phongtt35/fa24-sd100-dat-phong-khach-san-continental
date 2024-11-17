using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Client
{
    public class ClientAuthenicationViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public EntityStatus Status { get; set; }
    }
}
