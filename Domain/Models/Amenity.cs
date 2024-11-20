using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Amenity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public EntityStatus Status { get; set; }

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public List<AmenityRoom> AmenityRooms { get; set; }
    }
}
