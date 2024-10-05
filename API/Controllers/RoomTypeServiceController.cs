using Domain.DTO.RoomTypeService;
using Domain.Services.IServices.IRoomTypeService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTypeServiceController : Controller
{
    private readonly IRoomTypeServiceAddService _roomTypeServiceAddService;
    private readonly IRoomTypeServiceDeleteService _roomTypeServiceDeleteService;
    private readonly IRoomTypeServiceUpdateService _roomTypeServiceUpdateService;
    private readonly IRoomTypeServiceGetService _roomTypeServiceGetService;

    public RoomTypeServiceController(IRoomTypeServiceAddService roomTypeServiceAddService, 
        IRoomTypeServiceDeleteService roomTypeServiceDeleteService, 
        IRoomTypeServiceUpdateService roomTypeServiceUpdateService, 
        IRoomTypeServiceGetService roomTypeServiceGetService)
    {
        _roomTypeServiceAddService = roomTypeServiceAddService;
        _roomTypeServiceDeleteService = roomTypeServiceDeleteService;
        _roomTypeServiceUpdateService = roomTypeServiceUpdateService;
        _roomTypeServiceGetService = roomTypeServiceGetService;
    }

    [HttpPost(nameof(AddRoomTypeService))]
    public async Task<RoomTypeServiceResponse> AddRoomTypeService(RoomTypeServiceAddRequest roomTypeServiceAddRequest)
    {
        try
        {
            return await _roomTypeServiceAddService.AddRoomTypeService(roomTypeServiceAddRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost(nameof(GetRoomTypeServiceById))]
    public async Task<RoomTypeServiceResponse?> GetRoomTypeServiceById(Guid roomTypeServiceId)
    {
        try
        {  
            return await _roomTypeServiceGetService.GetRoomTypeServiceById(roomTypeServiceId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("Not found room type service", e);
        }
    }
    
    [HttpPost(nameof(GetAllRoomTypeServices))]
    public async Task<List<RoomTypeServiceResponse>> GetAllRoomTypeServices()
    {
        try
        {  
            return await _roomTypeServiceGetService.GetAllRoomTypeServices();
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of room type services could not be retrieved", e);
        }
    }
    
    [HttpPut(nameof(DeleteRoomTypeService))]
    public async Task<RoomTypeServiceResponse?> DeleteRoomTypeService
        (RoomTypeServiceDeleteRequest roomTypeServiceDeleteRequest)
    {
        try
        {
            return await _roomTypeServiceDeleteService.DeleteRoomTypeService(roomTypeServiceDeleteRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPut(nameof(UpdateRoomTypeService))]
    public async Task<RoomTypeServiceResponse?> UpdateRoomTypeService
        (RoomTypeServiceUpdateRequest roomTypeServiceUpdateRequest)
    {
        try
        {
            return await _roomTypeServiceUpdateService.UpdateRoomTypeService(roomTypeServiceUpdateRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}