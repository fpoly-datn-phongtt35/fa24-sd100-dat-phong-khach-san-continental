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
        var exists = await _roomTypeRepository.GetRoomTypeExists(roomTypeAddRequest.Name);
        if (exists != null)
        {
            throw new Exception($"Đã tồn tại loại phòng với tên '{roomTypeAddRequest.Name}'");
        }
        
        var roomType = roomTypeAddRequest.ToRoomType();
        
        //roomType.Deleted = false;
        await _roomTypeRepository.AddRoomType(roomType);
        return roomType.ToRoomTypeResponse();
    }
}