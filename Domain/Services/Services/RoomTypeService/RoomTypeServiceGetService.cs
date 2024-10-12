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

    public async Task<List<RoomTypeServiceResponse>> GetFilteredRoomTypeServices
        (string? searchString, Guid? roomTypeId, EntityStatus? status)
    {
        var roomTypeServices = await _roomTypeServiceRepository
            .GetFilteredRoomTypeServices(searchString, roomTypeId, status);

        var roomTypeServicesResponse = roomTypeServices
            .Select(roomTypeService => roomTypeService.ToRoomTypeServiceResponse())
            .ToList();

        return roomTypeServicesResponse;
    }

    public async Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId)
    {
        if (roomTypeServiceId == null) return null;

        var roomTypeService = await _roomTypeServiceRepository
            .GetRoomTypeServiceById(roomTypeServiceId.Value);

        return roomTypeService?.ToRoomTypeServiceResponse();
    }

    public async Task<List<RoomTypeServiceResponse>> GetFilteredDeletedRoomTypeServices(string? searchString, 
        Guid? roomTypeId)
    {
        var deletedRoomTypeServices = await _roomTypeServiceRepository
            .GetFilteredDeletedRoomTypeServices(searchString, roomTypeId);

        var roomTypeServicesResponse = deletedRoomTypeServices
            .Select(rts => rts.ToRoomTypeServiceResponse())
            .ToList();
        return roomTypeServicesResponse;
    }
}