using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services.RoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingCreateForCustomerService _roomBookingCreateService;
        private readonly IRoomBookingGetService _getListRoomBookingsService;
        public RoomBookingController(IRoomBookingCreateForCustomerService roomBookingCreateService, IRoomBookingGetService getListRoomBookingsService)
        {
            _roomBookingCreateService = roomBookingCreateService;
            _getListRoomBookingsService = getListRoomBookingsService;
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
        [HttpPost("GetListRoomBookingByCustomerId")]
        public async Task<ResponseData<RoomBookingResponseForCustomer>> GetListRoomBookingByCustomerId
        (RoomBookingGetRequestByCustomer request)
        {
            try
            {
                return await _getListRoomBookingsService.GetListRoomBookingByCustomerId(request);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of room bookings could not be retrieved", e);
            }
        }
    }
}
