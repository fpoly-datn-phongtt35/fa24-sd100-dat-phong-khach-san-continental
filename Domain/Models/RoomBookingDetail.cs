using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RoomBookingDetail
    {
        [Key]
        public Guid Id { get; set; }        
        public Guid RoomId { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }= DateTimeOffset.UtcNow.Date.AddHours(14);
        public DateTimeOffset? CheckOutBooking { get; set; } = DateTimeOffset.UtcNow.Date.AddHours(12);
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }        
        public decimal? Price { get; set; }
        public decimal Deposit { get; set; } = 0;
        public decimal ExtraPrice { get; set; }
        public decimal? Expenses { get; set; } = 0;
        public string? Note { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public List<ServiceOrderDetail> ServiceOrderDetails { get; set; }
        public Room Room { get; set; }
        public RoomBooking RoomBooking { get; set; }
        public virtual List<ResidenceRegistration> ResidenceRegistrations { get; set; }
    }
}
