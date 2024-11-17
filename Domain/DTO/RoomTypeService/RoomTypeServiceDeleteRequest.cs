using Domain.Enums;

namespace Domain.DTO.RoomTypeService;

public class RoomTypeServiceDeleteRequest
{
    public Guid Id { get; set; }
    public EntityStatus Status { get; set; }
    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomTypeServiceDeleteRequest into a
    /// new object of RoomTypeService type
    /// </summary>
    /// <returns>RoomTypeService object</returns>
    public Models.RoomTypeService ToRoomTypeService()
    {
        return new Models.RoomTypeService()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}