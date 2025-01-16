using Domain.Enums;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailCreateRequestForCustomer
    {
        public Guid RoomId { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? ExtraService { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? Expenses { get; set; }
        public decimal? Deposit { get; set; }
        public string? Note { get; set; }
        public RoomBookingStatus? Status { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? NewId { get; set; }
    }
}
