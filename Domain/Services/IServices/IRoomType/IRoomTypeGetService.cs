using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.Enums;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeGetService
{
    Task<ResponseData<RoomTypeResponse>> GetFilteredRoomTypes(RoomTypeGetRequest roomTypeGetRequest);
    Task<ResponseData<RoomTypeResponse>> GetFilteredDeletedRoomTypes(RoomTypeGetRequest roomTypeGetRequest);
    Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId);
    Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId);
}