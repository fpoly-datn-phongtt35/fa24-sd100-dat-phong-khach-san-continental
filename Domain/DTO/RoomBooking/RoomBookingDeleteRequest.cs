using System.Runtime.InteropServices.ComTypes;
using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingDeleteRequest
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public EntityStatus Status { get; set; }
    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public Models.RoomBooking ToRoomBooking()
    {
        return new Models.RoomBooking()
        {
            Id = Id,
            CustomerId = CustomerId,
            StaffId = StaffId,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}