using Domain.DTO.Paging;
using Domain.DTO.RoomTypeService;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomTypeService;

namespace Domain.Services.Services.RoomTypeService;

public class RoomTypeServiceGetService : IRoomTypeServiceGetService
{
    private readonly IRoomTypeServiceRepository _roomTypeServiceRepository;

    public RoomTypeServiceGetService(IRoomTypeServiceRepository roomTypeServiceRepository)
    {
        _roomTypeServiceRepository = roomTypeServiceRepository;
    }

    public async Task<ResponseData<RoomTypeServiceResponse>> GetFilteredRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest)
    {
        return await _roomTypeServiceRepository.GetFilteredRoomTypeServices(roomTypeServiceGetRequest);
    }

    public async Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId)
    {
        if (roomTypeServiceId == null) return null;

        var roomTypeService = await _roomTypeServiceRepository
            .GetRoomTypeServiceById(roomTypeServiceId.Value);

        return roomTypeService?.ToRoomTypeServiceResponse();
    }

    public async Task<ResponseData<RoomTypeServiceResponse>> GetFilteredDeletedRoomTypeServices
        (RoomTypeServiceGetRequest roomTypeServiceGetRequest)
    {
        return await _roomTypeServiceRepository.GetFilteredDeletedRoomTypeServices(roomTypeServiceGetRequest);
    }
}