using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ServiceOrderDetail
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid RoomBookingDetailId { get; set; } 
        public decimal Price { get; set; }
        public double? Amount { get; set; }
        public int Quantity { get; set; }
        public decimal? ExtraPrice { get; set; }
        public string? Description { get; set; }
        public Guid ServiceId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public virtual Service? Service { get; set; }
        public virtual RoomBookingDetail? RoomBookingDetail { get; set; }
    }
}
