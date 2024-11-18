using Domain.DTO.RoomBooking;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomBooking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services.RoomBooking
{
    public class RoomBookingCreateForCustomerService : IRoomBookingCreateForCustomerService
    {
        private readonly IRoomBookingRepository _roomBookingRepository;
        private readonly IConfiguration _configuration;
        public RoomBookingCreateForCustomerService(IConfiguration configuration, IRoomBookingRepository roomBookingRepository)
        {
            _configuration = configuration;
            _roomBookingRepository = roomBookingRepository;
        }

        public async Task<Guid> CreateRoomBookingForCustomer(RoomBookingCreateRequestForCustomer request)
        {
            try
            {
                return await _roomBookingRepository.CreateRoomBookingForCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
