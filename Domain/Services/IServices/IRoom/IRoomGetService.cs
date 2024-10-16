
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices.IRoom
{
    public interface IRoomGetService
    {
        Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest);
        Task<RoomResponse?> GetRoomById(Guid? roomId);
    }
}
