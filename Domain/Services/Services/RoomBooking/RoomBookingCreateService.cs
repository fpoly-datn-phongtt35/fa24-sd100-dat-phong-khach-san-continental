using Domain.DTO.RoomBooking;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomBooking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.RoomBooking
{
    public class RoomBookingCreateService : IRoomBookingCreateService
    {
        private readonly IRoomBookingRepository _roomBookingRepository;
        private readonly IConfiguration _configuration;
        public RoomBookingCreateService(IConfiguration configuration, IRoomBookingRepository roomBookingRepository)
        {
            _configuration = configuration;
            _roomBookingRepository = roomBookingRepository;
        }
        public async Task<int> CreateRoomBooking(RoomBookingCreateRequest request)
        {
            try
            {
                return await _roomBookingRepository.CreateRoomBooking(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
