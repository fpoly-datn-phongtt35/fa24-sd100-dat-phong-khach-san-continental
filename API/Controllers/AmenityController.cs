using Domain.DTO.Amenity;
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
}