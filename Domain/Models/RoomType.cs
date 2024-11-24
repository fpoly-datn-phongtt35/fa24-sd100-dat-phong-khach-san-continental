using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RoomType
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? MaximumOccupancy { get; set; }
        public int Quantity { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public List<AmenityRoom> AmenityRooms { get; set; }
        public List<Room> Rooms { get; set; }
        public List<RoomTypeService> RoomsTypeServices { get; set; }
    }
}
