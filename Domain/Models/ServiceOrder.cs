using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ServiceOrder
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? RoomBookingId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public RoomBooking RoomBooking { get; set; }
        public List<ServiceOrderDetail> ServiceOrderDetails { get; set; }
    }
}
