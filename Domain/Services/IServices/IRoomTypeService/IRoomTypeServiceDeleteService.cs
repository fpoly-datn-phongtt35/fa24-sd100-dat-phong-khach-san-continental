using Domain.DTO.RoomTypeService;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceDeleteService
{
    Task<RoomTypeServiceResponse?> DeleteRoomTypeService
        (RoomTypeServiceDeleteRequest roomTypeServiceDeleteRequest);
}