using Domain.DTO.RoomTypeService;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceAddService
{
    Task<RoomTypeServiceResponse> AddRoomTypeService
        (RoomTypeServiceAddRequest roomTypeServiceAddRequest);
}