using Domain.DTO.RoomType;
using Domain.Services.IServices.IRoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeRollBackService : IRoomTypeRollBackService
{
    public Task<RoomTypeResponse?> RollBackRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest)
    {
        throw new NotImplementedException();
    }
}