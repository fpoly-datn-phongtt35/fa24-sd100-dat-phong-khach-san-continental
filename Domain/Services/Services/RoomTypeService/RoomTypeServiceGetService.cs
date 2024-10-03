using Domain.DTO.RoomTypeService;
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

    public async Task<List<RoomTypeServiceResponse>> GetAllRoomTypeServices()
    {
        var roomTypeServices = await _roomTypeServiceRepository.GetAllRoomTypeServices();

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
}