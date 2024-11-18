using Domain.DTO.Customer;
using Domain.DTO.RoomBooking;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services.RoomBooking;
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
        [HttpPost("CreateRoomBookingForCustomer")]
        public async Task<ActionResult<Guid>> CreateRoomBookingForCustomer(RoomBookingCreateRequestForCustomer request)
        {
            try
            {
                Guid newId = await _roomBookingCreateService.CreateRoomBookingForCustomer(request);

                if (newId == Guid.Empty)
                {
                    return BadRequest("Unable to create room booking.");
                }

                return Ok(newId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
