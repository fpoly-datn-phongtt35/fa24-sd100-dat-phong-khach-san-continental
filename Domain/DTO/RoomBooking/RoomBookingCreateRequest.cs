using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingCreateRequest
{
    public BookingType BookingType { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
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
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}