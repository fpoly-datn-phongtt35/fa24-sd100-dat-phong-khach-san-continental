using Domain.DTO.RoomTypeService;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceGetService
{
    Task<List<RoomTypeServiceResponse>> GetAllRoomTypeServices();
    Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId);
}