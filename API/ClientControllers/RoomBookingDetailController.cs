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
        public async Task<int> BookingRoomDetail(RoomBookingDetailCreateRequestForCustomer request)
        {
            try
            {
                return await _roomBookingDetailService.CreateRoomBookingDetailForCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
