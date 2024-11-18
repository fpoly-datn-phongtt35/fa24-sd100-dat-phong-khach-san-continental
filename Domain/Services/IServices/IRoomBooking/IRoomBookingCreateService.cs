using Domain.DTO.RoomBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices.IRoomBooking
{
    public interface IRoomBookingCreateService
    {
        Task<int> CreateRoomBooking(RoomBookingCreateRequest request);
    }
}
