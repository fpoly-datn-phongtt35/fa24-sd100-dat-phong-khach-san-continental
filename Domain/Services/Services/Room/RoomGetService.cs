
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.Room
{
    public class RoomGetService : IRoomGetService
    {
        private readonly IRoomRepo _roomRepository;

        public RoomGetService(IRoomRepo roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest)
        {
            return await _roomRepository.GetAllRooms(roomRequest);
        }

        public async Task<RoomResponse?> GetRoomById(Guid? roomId)
        {
            if (roomId == null) return null;

            var room = await _roomRepository.GetRoomById(roomId.Value);
            if (room == null) return null;

            return room.ToRoomResponse();

        }
    }
}
