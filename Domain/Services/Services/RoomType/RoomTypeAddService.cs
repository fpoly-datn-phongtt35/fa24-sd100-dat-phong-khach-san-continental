using Domain.DTO.RoomType;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeAddService : IRoomTypeAddService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeAddService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<RoomTypeResponse> AddRoomType(RoomTypeAddRequest roomTypeAddRequest)
    {
        if (roomTypeAddRequest == null)
        {
            throw new ArgumentNullException(nameof(roomTypeAddRequest));
        }
        // convert roomTypeAddRequest into RoomType type
        var roomType = roomTypeAddRequest.ToRoomType();
        
        roomType.Id = Guid.NewGuid();
        roomType.Deleted = false;
        roomType.DeletedTime = default(DateTimeOffset);
        
        await _roomTypeRepository.AddRoomType(roomType);

        return roomType.ToRoomTypeResponse();
    }
}