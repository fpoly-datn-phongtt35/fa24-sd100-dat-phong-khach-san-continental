using Domain.Enums;

namespace Domain.DTO.RoomType;

public class RoomTypeDeleteRequest
{
    public Guid Id { get; set; }
    public EntityStatus Status { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset DeletedTime { get; set; }

    public Models.RoomType ToRoomType()
    {
        return new Models.RoomType()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}