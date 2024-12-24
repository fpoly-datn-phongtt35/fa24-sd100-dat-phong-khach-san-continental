using Domain.Enums;

namespace Domain.DTO.RoomBooking
{
    public class RoomBookingResponseForCustomer
    {
        public Guid Id { get; set; }
        public BookingType? BookingType { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? StaffId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalRoomPrice { get; set; }
        public decimal? TotalServicePrice { get; set; }
        public decimal? TotalExtraPrice { get; set; }
        public RoomBookingStatus Status { get; set; }
        public string? StaffName { get; set; }
        public string? CustomerName { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
