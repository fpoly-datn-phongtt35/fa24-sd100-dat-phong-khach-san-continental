using Domain.DTO.RoomType;
using Domain.Repositories.IRepository;

namespace Domain.Services.IServices.RoomType;

public interface IRoomTypeAddService
{
    Task<RoomTypeResponse> AddRoomType(RoomTypeAddRequest roomTypeAddRequest);
}