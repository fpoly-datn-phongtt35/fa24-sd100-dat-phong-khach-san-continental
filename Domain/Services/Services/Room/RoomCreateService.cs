using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.Room
{
    public class RoomCreateService : IRoomCreateService
    {
        private readonly IRoomRepo _roomRepository;

        public RoomCreateService(IRoomRepo roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> AddRoomService(RoomCreateRequest roomCreateRequest)
        {
            if (roomCreateRequest == null)
            {
                throw new ArgumentNullException(nameof(roomCreateRequest));
            }
            var room = roomCreateRequest.ToRoom();

            room.Id = Guid.NewGuid();
            room.Deleted = false;
            room.DeletedTime = default;
            room.ModifiedTime = default;

            await _roomRepository.AddRoom(room);

            return room.ToRoomResponse();
        }

    }
}
