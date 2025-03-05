using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Image
{
    public class ImagesUpdateRequest
    {
        public Guid Id { get; set; }
        public Guid? ObjId { get; set; }
        public string? Image { get; set; }
        public EntityStatus Status { get; set; }
        public bool? Deleted { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
