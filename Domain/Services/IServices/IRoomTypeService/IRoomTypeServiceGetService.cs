using Domain.DTO.RoomTypeService;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceGetService
{
    Task<List<RoomTypeServiceResponse>> GetAllRoomTypeServices(string? search);
    Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId);
    Task<List<RoomTypeServiceResponse>> GetFilteredDeletedRoomTypeServices(string? searchString, 
        Guid? roomTypeId);
}