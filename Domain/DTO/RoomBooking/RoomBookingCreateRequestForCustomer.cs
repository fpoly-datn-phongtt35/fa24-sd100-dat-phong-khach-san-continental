using Domain.Enums;

namespace Domain.DTO.RoomBooking
{
    public class RoomBookingCreateRequestForCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid? StaffId { get; set; }
        public EntityStatus? Status { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
