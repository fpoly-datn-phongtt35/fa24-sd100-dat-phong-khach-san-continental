using Domain.Enums;

namespace Domain.DTO.RoomTypeService;

public class RoomTypeServiceUpdateRequest
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public Guid ServiceId { get; set; }
    public int Amount { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomTypeServiceUpdateRequest into a
    /// new object of RoomTypeService type
    /// </summary>
    /// <returns>RoomTypeService object</returns>
    public Models.RoomTypeService ToRoomTypeService()
    {
        return new Models.RoomTypeService()
        {
            Id = Id,
            RoomTypeId = RoomTypeId,
            ServiceId = ServiceId,
            Amount = Amount,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }
}