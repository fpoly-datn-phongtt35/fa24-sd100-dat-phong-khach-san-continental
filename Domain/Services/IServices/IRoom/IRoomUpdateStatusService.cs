using Domain.DTO.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices.IRoom
{
    public interface IRoomUpdateStatusService
    {
        Task<int?> UpdateRoomStatus(RoomUpdateStatusRequest request);
    }
}
