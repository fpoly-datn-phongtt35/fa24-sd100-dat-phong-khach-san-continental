using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingCreateRequest
{
    public BookingType BookingType { get; set; } = BookingType.Offline;
    public Guid CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public RoomBookingStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalRoomPrice { get; set; }
    public decimal? TotalServicePrice { get; set; }
    public decimal? TotalExtraPrice { get; set; }
    public decimal? TotalExpenses { get; set; }
    public decimal? TotalPriceReality { get; set; }
    public BookingBy? BookingBy { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomBookingCreateRequest into a
    /// new object of RoomBooking type
    /// </summary>
    /// <returns>RoomBooking object</returns>
    public Models.RoomBooking ToRoomBooking()
    {
        return new Models.RoomBooking()
        {
            BookingType = BookingType,
            CustomerId = CustomerId,
            StaffId = StaffId,
            Status = Status,
            TotalPrice = TotalPrice,
            TotalRoomPrice = TotalRoomPrice,
            TotalServicePrice = TotalServicePrice,
            TotalExpenses = TotalExpenses,
            TotalPriceReality = TotalPriceReality,
            BookingBy = BookingBy,
            CreatedBy = CreatedBy
        };
    }
}