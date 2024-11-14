using Domain.DTO.Customer;
using Domain.DTO.RoomBooking;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingCreateForCustomerService _roomBookingCreateService;
        public RoomBookingController(IRoomBookingCreateForCustomerService roomBookingCreateService)
        {
            _roomBookingCreateService = roomBookingCreateService;
        }
        [HttpPost("BookingRoom")]
        public async Task<int> BookingRoom(RoomBookingCreateRequestForCustomer request)
        {
            try
            {
                return await _roomBookingCreateService.CreateRoomBookingForCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
