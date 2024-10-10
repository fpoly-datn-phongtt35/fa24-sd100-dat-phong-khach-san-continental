using Domain.DTO.Amenity;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices.IAmenity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityAddService _amenityAddService;
    private readonly IAmenityDeleteService _amenityDeleteService;
    private readonly IAmenityGetService _amenityGetService;
    private readonly IAmenityUpdateService _amenityUpdateService;

    public AmenityController(IAmenityAddService amenityAddService, IAmenityDeleteService amenityDeleteService,
        IAmenityGetService amenityGetService, IAmenityUpdateService amenityUpdateService)
    {
        _amenityAddService = amenityAddService;
        _amenityDeleteService = amenityDeleteService;
        _amenityGetService = amenityGetService;
        _amenityUpdateService = amenityUpdateService;
    }

    [HttpPost(nameof(CreateAmenity))]
    public async Task<AmenityResponse> CreateAmenity(AmenityCreateRequest amenityCreateRequest)
    {
        try
        {
            return await _amenityAddService.AddAmenity(amenityCreateRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    [HttpPost(nameof(GetFilteredAmenities))]
    public async Task<List<AmenityResponse>> GetFilteredAmenities(EntityStatus? status, string? searchString)
    {
        try
        {
            return await _amenityGetService.GetFilteredAmenities(status, searchString);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("The list of amenities could not be retrieved", ex);
        }
    }
    
    [HttpPost(nameof(GetAmenityById))]
    public async Task<AmenityResponse?> GetAmenityById(Guid amenityId)
    {
        try
        {
            return await _amenityGetService.GetAmenityById(amenityId);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("Not found the amenity", ex);
        }
    }

    [HttpPut(nameof(UpdateAmenity))]
    public async Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        try
        {
            return await _amenityUpdateService.UpdateAmenity(amenityUpdateRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPut(nameof(DeleteAmenity))]
    public async Task<AmenityResponse?> DeleteAmenity(AmenityDeleteRequest amenityDeleteRequest)
    {
        try
        {
            return await _amenityDeleteService.DeleteAmenityById(amenityDeleteRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Amenity cannot be deleted", e);
        }
    }

    [HttpPut(nameof(RecoverDeletedAmenity))]
    public async Task<AmenityResponse?> RecoverDeletedAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        try
        {
            return await _amenityUpdateService.RecoverDeletedAmenity(amenityUpdateRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Some errors when recover amenity", e);
        }
    }

    [HttpPost(nameof(GetFilteredDeletedAmenities))]
    public async Task<List<AmenityResponse>> GetFilteredDeletedAmenities(string? searchString)
    {
        try
        {
            return await _amenityGetService.GetFilteredDeletedAmenities(searchString);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("The list of deleted amenities could not be retrieved", ex);
        }
    }
}