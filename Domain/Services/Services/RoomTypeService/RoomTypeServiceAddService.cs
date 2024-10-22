using Domain.DTO.RoomTypeService;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomTypeService;

namespace Domain.Services.Services.RoomTypeService;

public class RoomTypeServiceAddService : IRoomTypeServiceAddService
{
    private readonly IRoomTypeServiceRepository _roomTypeServiceRepository;

    public RoomTypeServiceAddService(IRoomTypeServiceRepository roomTypeServiceRepository)
    {
        _roomTypeServiceRepository = roomTypeServiceRepository;
    }

    public async Task<RoomTypeServiceResponse> AddRoomTypeService(RoomTypeServiceAddRequest roomTypeServiceAddRequest)
    {
        if(roomTypeServiceAddRequest is null)
            throw new ArgumentNullException(nameof(roomTypeServiceAddRequest));
        
        var roomTypeService = roomTypeServiceAddRequest.ToRoomTypeService();
        
        roomTypeService.Deleted = false;
        roomTypeService.ModifiedTime = default;
        roomTypeService.DeletedTime = default;

        await _roomTypeServiceRepository.CreateRoomTypeService(roomTypeService);
        return roomTypeService.ToRoomTypeServiceResponse();
    }
}