﻿using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.DTO.RoomType;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeGetService : IRoomTypeGetService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeGetService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<ResponseData<RoomTypeResponse>> GetFilteredRoomTypes(RoomTypeGetRequest roomTypeGetRequest)
    {
        return await _roomTypeRepository.GetFilteredRoomTypes(roomTypeGetRequest);
    }

    public async Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId)
    {
        if (roomTypeId == null) return null;

        var roomType = await _roomTypeRepository.GetRoomTypeById(roomTypeId.Value);
        if (roomType == null) return null;

        return roomType.ToRoomTypeResponse();
    }

    public async Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId)
    {
        var roomType = await _roomTypeRepository.GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(roomTypeId);
        if (roomType == null) return null;

        var roomTypeResponse = roomType.ToRoomTypeResponse();

        // Convert list AmenityRooms
        roomTypeResponse.AmenityRooms = roomType.AmenityRooms
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
                    Name = amenityRoom.Amenity.Name,
                }
            }).ToList();

        // Convert list RoomTypeServices
        roomTypeResponse.RoomTypeServices = roomType.RoomsTypeServices
            .Select(roomTypeService => new RoomTypeServiceResponse
            {
                Id = roomTypeService.Id,
                RoomTypeId = roomTypeService.RoomTypeId,
                ServiceId = roomTypeService.ServiceId,
                Amount = roomTypeService.Amount,
                Status = roomTypeService.Status,
                Service = new ServiceResponse
                {
                    Id = roomTypeService.Service.Id,
                    Name = roomTypeService.Service.Name
                }
            }).ToList();
        
        return roomTypeResponse;
    }

    public async Task<ResponseData<RoomTypeResponse>> GetFilteredDeletedRoomTypes(RoomTypeGetRequest roomTypeGetRequest)
    {
        return await _roomTypeRepository.GetFilteredDeletedRoomTypes(roomTypeGetRequest);
    }
}