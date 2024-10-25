using Domain.DTO.RoomTypeService;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceUpdateService
{
    Task<RoomTypeServiceResponse?> UpdateRoomTypeService
        (RoomTypeServiceUpdateRequest roomTypeServiceUpdateRequest);
    Task<RoomTypeServiceResponse?> RecoverDeletedRoomTypeService
        (RoomTypeServiceUpdateRequest roomTypeServiceUpdateRequest);
}