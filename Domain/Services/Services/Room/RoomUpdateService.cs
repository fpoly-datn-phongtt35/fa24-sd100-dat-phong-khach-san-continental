
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

            if (existingRoom.Deleted)
            {
                throw new InvalidOperationException("This  room type already deleted, cannot update it.");
            }

            existingRoom.FloorId = roomUpdateRequest.FloorId;
            existingRoom.RoomTypeId = roomUpdateRequest.RoomTypeId;
            existingRoom.Name = roomUpdateRequest.Name;
            existingRoom.Price = roomUpdateRequest.Price;
            existingRoom.Address = roomUpdateRequest.Address;
            existingRoom.RoomSize = roomUpdateRequest.RoomSize;
            existingRoom.Images = roomUpdateRequest.Images;
            existingRoom.Description = roomUpdateRequest.Description;
            existingRoom.Status = roomUpdateRequest.Status;
            existingRoom.ModifiedTime = roomUpdateRequest.ModifiedTime;
            existingRoom.ModifiedBy = roomUpdateRequest.ModifiedBy;

            await _roomRepository.UpdateRoom(existingRoom);

            return existingRoom.ToRoomResponse();
        }
    }
}
