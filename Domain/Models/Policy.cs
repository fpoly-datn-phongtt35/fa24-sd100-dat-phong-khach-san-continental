using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Policy
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PolicyTypeId { get; set; }
        public Guid StaffId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public PolicyType PolicyType { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
