using Domain.DTO.RoomType;
using Domain.Enums;
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
    private readonly IRoomTypeUpdateService _roomTypeUpdateService;

    public RoomTypeController(IRoomTypeAddService roomTypeAddService, 
        IRoomTypeUpdateService roomTypeUpdateService,
        IRoomTypeGetService roomTypeGetService, 
        IRoomTypeDeleteService roomTypeDeleteService)
    {
        _roomTypeAddService = roomTypeAddService;
        _roomTypeUpdateService = roomTypeUpdateService;
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
    
    [HttpPost(nameof(GetFilteredRoomTypes))]
    public async Task<List<RoomTypeResponse>> GetFilteredRoomTypes(string? searchString, EntityStatus? status)
    {
        try
        {
            return await _roomTypeGetService.GetFilteredRoomTypes(searchString, status);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room types could not be retrieved", e);
        }
    }
    
    [HttpPost(nameof(GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById))]
    public async Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId)
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
    
    [HttpPost(nameof(GetFilteredDeletedRoomTypes))]
    public async Task<List<RoomTypeResponse>> GetFilteredDeletedRoomTypes(string? searchString)
    {
        try
        {
            return await _roomTypeGetService.GetFilteredDeletedRoomTypes(searchString);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    [HttpPut(nameof(RecoverDeletedRoomType))]
    public async Task<RoomTypeResponse?> RecoverDeletedRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest)
    {
        try
        {
            return await _roomTypeUpdateService.RecoverDeletedRoomType(roomTypeUpdateRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Some errors when recover room type", e);
        }
    }
}