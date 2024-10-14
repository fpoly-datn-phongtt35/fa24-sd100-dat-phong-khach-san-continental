﻿
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
        Task<List<RoomResponse>> GetAllRooms(string? search, Guid? roomTypeId, Guid? floorId, EntityStatus? status);
        Task<RoomResponse?> GetRoomById(Guid? roomId);
    }
}
