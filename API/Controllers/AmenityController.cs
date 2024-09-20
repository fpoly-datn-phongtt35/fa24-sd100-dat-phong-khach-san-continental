using Domain.DTO.Amenity;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }
    
    [HttpPost("CreateAmenity")]
    public async Task<AmenityResponse> CreateAmenity(AmenityCreateRequest amenityCreateRequest)
    {
        try
        {
            return await _amenityService.AddAmenity(amenityCreateRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    [HttpGet("GetAllAmenities")]
    public async Task<List<AmenityResponse>> GetAllAmenities()
    {
        try
        {
            return await _amenityService.GetAllAmenities();
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("The list of amenities could not be retrieved", ex);
        }
    }
    
    [HttpGet("GetAmenityById")]
    public async Task<AmenityResponse?> GetAmenityById(Guid amenityId)
    {
        try
        {
            return await _amenityService.GetAmenityById(amenityId);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("Not found the amenity", ex);
        }
    }

    [HttpPut("UpdateAmenity")]
    public async Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        try
        {
            return await _amenityService.UpdateAmenity(amenityUpdateRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPut("DeleteAmenity")]
    public async Task<AmenityResponse?> DeleteAmenity(AmenityDeleteRequest amenityDeleteRequest)
    {
        try
        {
            return await _amenityService.DeleteAmenityById(amenityDeleteRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Amenity cannot be deleted", e);
        }
    }
}