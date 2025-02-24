using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Image
{
    public class ImagesCreateRequest
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public EntityStatus Status { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
