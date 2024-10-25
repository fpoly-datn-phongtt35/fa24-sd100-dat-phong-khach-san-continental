using System.Runtime.InteropServices.JavaScript;
using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingUpdateRequest
{
    public Guid Id { get; set; }
    public BookingType BookingType { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomBookingUpdateRequest into a
    /// new object of RoomBooking type
    /// </summary>
    /// <returns>RoomBooking object</returns>
    public Models.RoomBooking ToRoomBooking()
    {
        return new Models.RoomBooking()
        {
            Id = Id,
            BookingType = BookingType,
            CustomerId = CustomerId,
            StaffId = StaffId,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }
}