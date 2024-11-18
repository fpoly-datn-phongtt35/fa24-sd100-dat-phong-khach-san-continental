using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FeedBack
    {
        [Key]
        public Guid Id  { get; set; }
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomBookingId { get; set; }
        public string? Comments { get; set; }
        public int? Rating { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Staff? Staff { get; set; }
        public Customer? Customer { get; set; }
        public RoomBooking? RoomBooking { get; set; }
    }
}
