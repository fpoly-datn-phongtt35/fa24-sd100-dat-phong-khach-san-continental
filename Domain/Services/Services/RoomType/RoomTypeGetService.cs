using Domain.DTO.RoomType;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.RoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeGetService : IRoomTypeGetService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeGetService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }
    
    public async Task<List<RoomTypeResponse>> GetAllRoomTypes()
    {
        var roomTypes = await _roomTypeRepository.GetAllRoomTypes();

        var roomTypesResponse = roomTypes
            .Select(roomType => roomType.ToRoomTypeResponse())
            .ToList();

        return roomTypesResponse;
    }

    public async Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId)
    {
        if (roomTypeId == null) return null;
        
        var roomType = await _roomTypeRepository.GetRoomTypeById(roomTypeId.Value);
        if (roomType == null) return null;
        
        return roomType.ToRoomTypeResponse();
    }   
}