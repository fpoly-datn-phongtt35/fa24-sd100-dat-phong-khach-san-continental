using Domain.DTO.RoomTypeService;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomTypeService;

namespace Domain.Services.Services.RoomTypeService;

public class RoomTypeServiceUpdateService : IRoomTypeServiceUpdateService
{
    private readonly IRoomTypeServiceRepository _roomTypeServiceRepository;

    public RoomTypeServiceUpdateService(IRoomTypeServiceRepository roomTypeServiceRepository)
    {
        _roomTypeServiceRepository = roomTypeServiceRepository;
    }

    public async Task<RoomTypeServiceResponse?> UpdateRoomTypeService
        (RoomTypeServiceUpdateRequest roomTypeServiceUpdateRequest)
    {
        if (roomTypeServiceUpdateRequest is null)
            throw new ArgumentNullException(nameof(roomTypeServiceUpdateRequest));

        var exisingRoomTypeService = await _roomTypeServiceRepository
            .GetRoomTypeServiceById(roomTypeServiceUpdateRequest.Id);

        if (exisingRoomTypeService is null)
            throw new Exception("Id room type service does not exist");

        if (exisingRoomTypeService.Deleted)
            throw new InvalidOperationException("This room type service already deleted, cannot update it.");

        exisingRoomTypeService.RoomTypeId = roomTypeServiceUpdateRequest.RoomTypeId;
        exisingRoomTypeService.ServiceId = roomTypeServiceUpdateRequest.ServiceId;
        exisingRoomTypeService.Amount = roomTypeServiceUpdateRequest.Amount;
        exisingRoomTypeService.Status = roomTypeServiceUpdateRequest.Status;
        exisingRoomTypeService.ModifiedTime = roomTypeServiceUpdateRequest.ModifiedTime;
        exisingRoomTypeService.ModifiedBy = roomTypeServiceUpdateRequest.ModifiedBy;

        await _roomTypeServiceRepository.UpdateRoomTypeService(exisingRoomTypeService);
        return exisingRoomTypeService.ToRoomTypeServiceResponse();
    }
}