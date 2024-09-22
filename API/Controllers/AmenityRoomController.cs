using Domain.DTO.AmenityRoom;
using Domain.Services.IServices.IAmenityRoom;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenityRoomController : Controller
{
    private readonly IAmenityRoomAddService _amenityRoomAddService;
    private readonly IAmenityRoomDeleteService _amenityRoomDeleteService;
    private readonly IAmenityRoomUpdateService _amenityRoomUpdateService;
    private readonly IAmenityRoomGetService _amenityRoomGetService;

    public AmenityRoomController(IAmenityRoomAddService amenityRoomAddService, 
        IAmenityRoomDeleteService amenityRoomDeleteService, 
        IAmenityRoomUpdateService amenityRoomUpdateService, 
        IAmenityRoomGetService amenityRoomGetService)
    {
        _amenityRoomAddService = amenityRoomAddService;
        _amenityRoomDeleteService = amenityRoomDeleteService;
        _amenityRoomUpdateService = amenityRoomUpdateService;
        _amenityRoomGetService = amenityRoomGetService;
    }
    
    [HttpPost(nameof(CreateAmenityRoom))]
    public async Task<AmenityRoomResponse> CreateAmenityRoom(AmenityRoomAddRequest amenityRoomAddRequest)
    {
        try
        {
            return await _amenityRoomAddService.AddAmenityRoomService(amenityRoomAddRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    [HttpPost(nameof(GetAmenityRoomById))]
    public async Task<AmenityRoomResponse?> GetAmenityRoomById(Guid amenityRoomId)
    {
        try
        {
            return await _amenityRoomGetService.GetAmenityRoomById(amenityRoomId);
        }
        catch (Exception e)
        {
            throw new NullReferenceException("Not found the amenity room", e);
        }
    }
    
    [HttpPost(nameof(GetAllAmenityRooms))]
    public async Task<List<AmenityRoomResponse>> GetAllAmenityRooms()
    {
        try
        {
            return await _amenityRoomGetService.GetAllAmenityRooms();
        }
        catch (Exception e)
        {
            throw new NullReferenceException("The list of amenity rooms could not be retrieved", e);
        }
    }
    
    [HttpPut(nameof(DeleteAmenityRoom))]
    public async Task<AmenityRoomResponse?> DeleteAmenityRoom(AmenityRoomDeleteRequest amenityRoomDeleteRequest)
    {
        try
        {
            return await _amenityRoomDeleteService.DeleteAmenityRoom(amenityRoomDeleteRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    [HttpPut(nameof(UpdateAmenityRoom))]
    public async Task<AmenityRoomResponse?> UpdateAmenityRoom(AmenityRoomUpdateRequest amenityRoomUpdateRequest)
    {
        try
        {
            return await _amenityRoomUpdateService.UpdateAmenityRoom(amenityRoomUpdateRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}