using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceType
{
    public class ServiceTypeCreateRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
