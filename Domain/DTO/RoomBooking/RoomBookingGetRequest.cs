using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingGetRequest : PagingRequest
{
    public string? SearchString { get; set; }
    public BookingType? BookingType { get; set; }
    public RoomBookingStatus? Status { get; set; }
    public Guid? StaffId { get; set; }
}