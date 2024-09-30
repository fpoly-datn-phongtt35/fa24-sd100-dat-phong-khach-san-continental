
using Domain.DTO.Room;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.Room
{
    public class RoomDeleteService : IRoomDeleteService
    {
        private readonly IRoomRepo _roomRepository;

        public RoomDeleteService(IRoomRepo roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse?> DeleteRoomService(RoomDeleteRequest roomDeleteRequest)
        {
            if (roomDeleteRequest is null)
            {
                throw new ArgumentNullException(nameof(roomDeleteRequest));
            }

            var existingRoom = await _roomRepository.GetRoomById(roomDeleteRequest.Id);

            if (existingRoom is null)
            {
                throw new Exception("No room found");
            }

            existingRoom.Status = (RoomStatus.Deleted);
            existingRoom.Deleted = true;
            existingRoom.DeletedTime = roomDeleteRequest.DeletedTime;
            existingRoom.DeletedBy = roomDeleteRequest.DeletedBy;

            await _roomRepository.DeleteRoom(existingRoom);

            return existingRoom.ToRoomResponse();
        }

    }
    
    
}
