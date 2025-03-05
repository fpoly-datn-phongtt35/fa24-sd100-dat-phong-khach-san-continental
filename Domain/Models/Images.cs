using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Images
    {
        [Key]
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public Guid? ObjId { get; set; }
        public EntityStatus Status { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
