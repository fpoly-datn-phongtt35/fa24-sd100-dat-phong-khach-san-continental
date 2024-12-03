using Domain.DTO.ServiceOrderDetail;
using Domain.Enums;

namespace ViewClient.Models.DTO
{
    public class RoomBookingDetailCreateRequest
    {
        public Guid RoomId { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? Deposit { get; set; }
        public EntityStatus? Status { get; set; }
        public Guid? CreatedBy { get; set; }

        public List<ServiceOrderDetailCreateRequest>? SelectedServices { get; set; }
    }
}
