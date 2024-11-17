using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Service
{
    public class ServiceUpdateRequest
    {
        public Guid Id { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int? Unit { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public bool? Deleted { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
