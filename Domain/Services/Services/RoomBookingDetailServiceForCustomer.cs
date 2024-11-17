using Domain.DTO.RoomBookingDetail;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class RoomBookingDetailServiceForCustomer : IRoomBookingDetailServiceForCustomer
    {
        private readonly IRoomBookingDetailRepository _roomBookingDetailRepository;
        private readonly IConfiguration _configuration;
        public RoomBookingDetailServiceForCustomer(IConfiguration configuration, IRoomBookingDetailRepository roomBookingDetailRepository)
        {
            _configuration = configuration;
           _roomBookingDetailRepository = roomBookingDetailRepository;
        }

        public async Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request)
        {
            try
            {
                return await _roomBookingDetailRepository.CreateRoomBookingDetailForCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
