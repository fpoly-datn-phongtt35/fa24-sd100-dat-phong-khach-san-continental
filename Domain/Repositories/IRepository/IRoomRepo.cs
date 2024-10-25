using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IRoomRepo
    {
        Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomResponse);
        Task<Room?> GetRoomById(Guid RoomId);
        Task<Room> AddRoom(Room Room);
        Task<Room?> UpdateRoom(Room Room);
        Task<Room?> DeleteRoom(Room Room);
    }
}
