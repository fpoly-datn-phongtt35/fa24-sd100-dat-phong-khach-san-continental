
using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
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

        public async Task<RoomAvailableResponse> GetAvailableRooms(RoomAvailableRequest roomRequest)
        {
            return await _roomRepository.GetAvailableRooms(roomRequest);
        }

        public async Task<RoomResponse?> GetRoomById(Guid roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId); // Lấy Room từ repository
                                                                                     //    if (room == null) return null;

            // Chuyển đổi Room thành RoomResponse
            var roomResponse = room.ToRoomResponse(); // Cần sử dụng room, không phải roomType

            // Kiểm tra và chuyển đổi danh sách AmenityRooms
            if (room.RoomType != null && room.RoomType.AmenityRooms != null)
            {

                roomResponse.RoomType.AmenityRooms = room.RoomType.AmenityRooms
                    .Select(amenityRoom => new AmenityRoomResponse
                    {
                        Id = amenityRoom.Id,
                        RoomTypeId = amenityRoom.RoomTypeId,
                        AmenityId = amenityRoom.AmenityId,
                        Amount = amenityRoom.Amount,
                        Status = amenityRoom.Status,
                        Amenity = new AmenityResponse
                        {
                            Id = amenityRoom.Amenity.Id,
                            Name = amenityRoom.Amenity.Name, // Nếu có Description cho Amenity
                        }
                    }).ToList();
            }

            return roomResponse;

        }

        public async Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request)
        {
            return await _roomRepository.SearchRooms(request);
        }
        //public async Task<RoomResponse?> GetRoomTypeWithAmenityRoomById(Guid roomId)
        //{
        //    var room = await _roomRepository.GetRoomTypeWithAmenityRoomById(roomId); // Lấy Room từ repository
        //    if (room == null) return null;

        //    // Chuyển đổi Room thành RoomResponse
        //    var roomResponse = room.ToRoomResponse(); // Cần sử dụng room, không phải roomType

        //    // Kiểm tra và chuyển đổi danh sách AmenityRooms
        //    if (room.RoomType != null && room.RoomType.AmenityRooms != null)
        //    {

        //        roomResponse.RoomType.AmenityRooms = room.RoomType.AmenityRooms
        //            .Select(amenityRoom => new AmenityRoomResponse
        //            {
        //                Id = amenityRoom.Id,
        //                RoomTypeId = amenityRoom.RoomTypeId,
        //                AmenityId = amenityRoom.AmenityId,
        //                Amount = amenityRoom.Amount,
        //                Status = amenityRoom.Status,
        //                Amenity = new AmenityResponse
        //                {
        //                    Id = amenityRoom.Amenity.Id,
        //                    Name = amenityRoom.Amenity.Name, // Nếu có Description cho Amenity
        //                }
        //            }).ToList();
        //    }

        //    return roomResponse;
        //}


    }
}
