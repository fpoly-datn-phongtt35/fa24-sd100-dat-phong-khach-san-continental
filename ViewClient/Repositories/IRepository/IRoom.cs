using Domain.DTO.Paging;
using Domain.DTO.Room;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoom
    {
        Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest);
    }
}
