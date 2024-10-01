using Domain.DTO.RoomTypeService;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomTypeService;

namespace Domain.Services.Services.RoomTypeService;

public class RoomTypeServiceDeleteService : IRoomTypeServiceDeleteService
{
    private readonly IRoomTypeServiceRepository _roomTypeServiceRepository;

    public RoomTypeServiceDeleteService(IRoomTypeServiceRepository roomTypeServiceRepository)
    {
        _roomTypeServiceRepository = roomTypeServiceRepository;
    }

    public async Task<RoomTypeServiceResponse?> DeleteRoomTypeService(RoomTypeServiceDeleteRequest roomTypeServiceDeleteRequest)
    {
        if(roomTypeServiceDeleteRequest is null)
            throw new ArgumentNullException(nameof(roomTypeServiceDeleteRequest));

        var existingRoomTypeService = await _roomTypeServiceRepository
            .GetRoomTypeServiceById(roomTypeServiceDeleteRequest.Id);

        if (existingRoomTypeService is null)
            throw new Exception("No room type service found");

        existingRoomTypeService.Status = EntityStatus.Deleted;
        existingRoomTypeService.Deleted = true;
        existingRoomTypeService.DeletedTime = roomTypeServiceDeleteRequest.DeletedTime;
        existingRoomTypeService.DeletedBy = roomTypeServiceDeleteRequest.DeletedBy;
        
        await _roomTypeServiceRepository.DeleteRoomTypeService(existingRoomTypeService);
        return existingRoomTypeService.ToRoomTypeServiceResponse();
    }
}