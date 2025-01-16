using Domain.DTO.RoomBookingDetail;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingDetailController : ControllerBase
    {
        private readonly IRoomBookingDetailServiceForCustomer _roomBookingDetailService;
        public RoomBookingDetailController(IRoomBookingDetailServiceForCustomer roomBookingDetailService)
        {
            _roomBookingDetailService = roomBookingDetailService;
        }
        [HttpPost("BookingRoomDetail")]
        public async Task<ActionResult<Guid>> BookingRoomDetail(RoomBookingDetailCreateRequestForCustomer request)
        {
            try
            {
                Guid newId = await _roomBookingDetailService.CreateRoomBookingDetailForCustomer(request);
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
