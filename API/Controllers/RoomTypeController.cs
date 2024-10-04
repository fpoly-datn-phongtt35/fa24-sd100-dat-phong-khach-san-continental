using Domain.DTO.RoomType;
using Domain.Models;
using Domain.Services.IServices.IRoomType;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTypeController : Controller
{
    private readonly IRoomTypeAddService _roomTypeAddService;
    private readonly IRoomTypeDeleteService _roomTypeDeleteService;
    private readonly IRoomTypeGetService _roomTypeGetService;
    private readonly IRoomTypeRollBackService _roomTypeRollBackService;
    private readonly IRoomTypeUpdateService _roomTypeUpdateService;

    public RoomTypeController(IRoomTypeAddService roomTypeAddService, 
        IRoomTypeUpdateService roomTypeUpdateService, 
        IRoomTypeRollBackService roomTypeRollBackService, 
        IRoomTypeGetService roomTypeGetService, 
        IRoomTypeDeleteService roomTypeDeleteService)
    {
        _roomTypeAddService = roomTypeAddService;
        _roomTypeUpdateService = roomTypeUpdateService;
        _roomTypeRollBackService = roomTypeRollBackService;
        _roomTypeGetService = roomTypeGetService;
        _roomTypeDeleteService = roomTypeDeleteService;
    }

    [HttpPost(nameof(CreateRoomType))]
    public async Task<RoomTypeResponse> CreateRoomType(RoomTypeAddRequest roomTypeAddRequest)
    {
        try
        {
            return await _roomTypeAddService.AddRoomType(roomTypeAddRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost(nameof(GetRoomTypeById))]
    public async Task<RoomTypeResponse?> GetRoomTypeById(Guid roomTypeId)
    {
        try
        {
            return await _roomTypeGetService.GetRoomTypeById(roomTypeId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("Not found the room type", e);
        }
    }
    
    [HttpPost(nameof(GetAllRoomTypes))]
    public async Task<List<RoomTypeResponse>> GetAllRoomTypes()
    {
        try
        {
            return await _roomTypeGetService.GetAllRoomTypes();
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room types could not be retrieved", e);
        }
    }
    
    [HttpPost(nameof(GetRoomTypeWithAmenityRoomById))]
    public async Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomById(Guid roomTypeId)
    {
        try
        {
            return await _roomTypeGetService.GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(roomTypeId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room types could not be retrieved", e);
        }
    }
    
    [HttpPut(nameof(DeleteRoomType))]
    public async Task<RoomTypeResponse?> DeleteRoomType(RoomTypeDeleteRequest roomTypeDeleteRequest)
    {
        try
        {
            return await _roomTypeDeleteService.DeleteRoomType(roomTypeDeleteRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPut(nameof(UpdateRoomType))]
    public async Task<RoomTypeResponse?> UpdateRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest)
    {
        try
        {
            return await _roomTypeUpdateService.UpdateRoomType(roomTypeUpdateRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}