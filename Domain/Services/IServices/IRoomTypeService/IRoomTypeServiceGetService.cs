using Domain.DTO.RoomTypeService;
using Domain.Enums;

namespace Domain.Services.IServices.IRoomTypeService;

public interface IRoomTypeServiceGetService
{
    Task<List<RoomTypeServiceResponse>> GetFilteredRoomTypeServices(string? searchString,
        Guid? roomTypeId, EntityStatus? status);
    Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid? roomTypeServiceId);
    Task<List<RoomTypeServiceResponse>> GetFilteredDeletedRoomTypeServices(string? searchString, 
        Guid? roomTypeId);
}