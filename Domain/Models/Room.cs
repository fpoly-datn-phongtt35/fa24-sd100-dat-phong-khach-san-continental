using Domain.DTO.RoomBookingDetail;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public Guid? FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Floor Floor { get; set; }
        public RoomType RoomType { get; set; }
        public List<RoomBookingDetail> RoomBookingDetails { get; set; }
    }
}
