
using Domain.DTO.Room;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.Room
{
    public class RoomUpdateService : IRoomUpdateService
    {
        private readonly IRoomRepo _roomRepository;

        public RoomUpdateService(IRoomRepo roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse?> UpdateRoomService(RoomUpdateRequest roomUpdateRequest)
        {
            if (roomUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(roomUpdateRequest));
            }

            var existingRoom = await _roomRepository.GetRoomById(roomUpdateRequest.Id);
            if (existingRoom is null)
            {
                throw new Exception("Id  room does not exist");
            }
            existingRoom.RoomSize = roomUpdateRequest.RoomSize;
            existingRoom.FloorId = roomUpdateRequest.FloorId;
            existingRoom.Name = roomUpdateRequest.Name;
            existingRoom.Description = roomUpdateRequest.Description;
            existingRoom.Price = roomUpdateRequest.Price;
            existingRoom.Address = roomUpdateRequest.Address;
            existingRoom.RoomTypeId = roomUpdateRequest.RoomTypeId;
            existingRoom.ModifiedTime = roomUpdateRequest.ModifiedTime;
            existingRoom.ModifiedBy = roomUpdateRequest.ModifiedBy;
            existingRoom.Status = roomUpdateRequest.Status;

            await _roomRepository.UpdateRoom(existingRoom);

            return existingRoom.ToRoomResponse();
        }
    }
}
