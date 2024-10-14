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
        Task<List<Room>> GetAllRooms(string? search, Guid? roomTypeId, Guid? floorId, EntityStatus? status);
        Task<Room?> GetRoomById(Guid RoomId);
        Task<Room> AddRoom(Room Room);
        Task<Room?> UpdateRoom(Room Room);
        Task<Room?> DeleteRoom(Room Room);
    }
}
