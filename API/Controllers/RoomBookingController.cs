using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomBookingController : Controller
{
    private readonly IRoomBookingGetService _roomBookingGetService;
    private readonly IRoomBookingUpdateService _roomBookingUpdateService;
    private readonly IRoomBookingCreateService _roomBookingCreateService;

    public RoomBookingController(IRoomBookingGetService roomBookingGetService, 
        IRoomBookingUpdateService roomBookingUpdateService,
         IRoomBookingCreateService roomBookingCreateService)
    {
        _roomBookingGetService = roomBookingGetService;
        _roomBookingUpdateService = roomBookingUpdateService;
        _roomBookingCreateService = roomBookingCreateService;
    }
    [HttpPost("CreateRoomBooking")]
    public async Task<int> CreateRoomBooking(RoomBookingCreateRequest request)
    {
        try
        {
            return await _roomBookingCreateService.CreateRoomBooking(request);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [HttpPost(nameof(GetFilteredRoomBookings))]
    public async Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBookings
        (RoomBookingGetRequest roomBookingGetRequest)
    {
        try
        {
            return await _roomBookingGetService.GetFilteredRoomBooking(roomBookingGetRequest);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room bookings could not be retrieved", e);
        }
    }
    
    [HttpPost(nameof(GetRoomBookingById))]
    public async Task<RoomBookingResponse?> GetRoomBookingById(Guid roomBookingId)
    {
        try
        {
            return await _roomBookingGetService.GetRoomBookingById(roomBookingId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("Not found the room booking", e);
        }
    }

    [HttpPut(nameof(UpdateRoomBooking))]
    public async Task<RoomBookingResponse?> UpdateRoomBooking(RoomBookingUpdateRequest roomBookingUpdateRequest)
    {
        try
        {
            return await _roomBookingUpdateService.UpdateRoomBookingAsync(roomBookingUpdateRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}