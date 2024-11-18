using Domain.DTO.Room;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoom;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.Room
{
    public class RoomUpdateStatusService : IRoomUpdateStatusService
    {

        private readonly IRoomRepo _roomRepo;
        private readonly IConfiguration _configuration;
        public RoomUpdateStatusService(IConfiguration configuration, IRoomRepo roomRepo)
        {
            _configuration = configuration;
            _roomRepo = roomRepo;
        }

        public async Task<int?> UpdateRoomStatus(RoomUpdateStatusRequest request)
        {
            try
            {
                return await _roomRepo.UpdateRoomStatus(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
