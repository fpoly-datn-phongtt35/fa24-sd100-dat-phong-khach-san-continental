using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.PaymentHistory;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services;
using Domain.Services.Services.RoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using System.Data;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingCreateForCustomerService _roomBookingCreateService;
        private readonly IRoomBookingGetService _getListRoomBookingsService;
        private readonly IRoomBookingDetailRepository _rbdRepository;
        private readonly PayOS _payOS;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly IRoomBookingUpdateService _roomBookingUpdateService;
        public RoomBookingController(IRoomBookingCreateForCustomerService roomBookingCreateService, 
            IRoomBookingGetService getListRoomBookingsService,
            IPaymentHistoryService paymentHistoryService,
            PayOS payOS,
            IRoomBookingUpdateService roomBookingUpdateService,
            IRoomBookingDetailRepository rbdRepository)
        {
            _roomBookingCreateService = roomBookingCreateService;
            _getListRoomBookingsService = getListRoomBookingsService;
            _paymentHistoryService = paymentHistoryService;
            _payOS = payOS;
            _roomBookingUpdateService = roomBookingUpdateService;
            _rbdRepository = rbdRepository;
        }
        [HttpPost("CreateRoomBookingForCustomer")]
        public async Task<ActionResult<Guid>> CreateRoomBookingForCustomer([FromBody]RoomBookingCreateRequestForCustomer request)
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
                //await UpdatePaymentHistory();
                return await _getListRoomBookingsService.GetListRoomBookingByCustomerId(request);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of room bookings could not be retrieved", e);
            }
        }
    }
}
