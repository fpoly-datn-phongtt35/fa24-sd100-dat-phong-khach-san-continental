using Domain.Enums;

namespace Domain.DTO.RoomTypeService;

public class RoomTypeServiceAddRequest
{
    public Guid RoomTypeId { get; set; }
    public Guid ServiceId { get; set; }
    public int Amount { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomTypeServiceAddRequest into a
    /// new object of RoomTypeService type
    /// </summary>
    /// <returns>RoomTypeService object</returns>
    public Models.RoomTypeService ToRoomTypeService()
    {
        return new Models.RoomTypeService()
        {
            RoomTypeId = RoomTypeId,
            ServiceId = ServiceId,
            Amount = Amount,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}