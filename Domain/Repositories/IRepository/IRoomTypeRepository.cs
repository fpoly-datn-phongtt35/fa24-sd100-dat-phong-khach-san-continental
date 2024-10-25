using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeRepository
{
    public Task<ResponseData<RoomTypeResponse>> GetFilteredRoomTypes(RoomTypeGetRequest roomTypeGetRequest);
    public Task<ResponseData<RoomTypeResponse>> GetFilteredDeletedRoomTypes(RoomTypeGetRequest roomTypeGetRequest);
    public Task<RoomType?> GetRoomTypeById(Guid roomTypeId);
    public Task<RoomType?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId);
    public Task<RoomType> AddRoomType(RoomType roomType);
    public Task<RoomType?> UpdateRoomType(RoomType roomType);
    public Task<RoomType?> DeleteRoomType(RoomType roomType);
    public Task<RoomType?> RecoverDeletedRoomType(RoomType roomType);
}