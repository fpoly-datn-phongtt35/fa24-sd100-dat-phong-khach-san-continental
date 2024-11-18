using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RoomTypeService
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid ServiceId { get; set; }
        public int? Amount { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public RoomType RoomType { get; set; }
        public Service? Service { get; set; }
    }
}
