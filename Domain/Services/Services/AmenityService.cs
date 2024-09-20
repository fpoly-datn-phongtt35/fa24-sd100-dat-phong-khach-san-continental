﻿using System.Data;
using Domain.DTO.Amenity;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices;
using Microsoft.Data.SqlClient;

namespace Domain.Services.Services;

public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<AmenityResponse> AddAmenity(AmenityCreateRequest amenityCreateRequest)
    {
        // Check if AmenityCreateRequest is not null
        if (amenityCreateRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityCreateRequest));
        }
        // Convert amenityCreateRequest into Amenity type
        var amenity = amenityCreateRequest.ToAmenity();
        
        amenity.Id = Guid.NewGuid();
        
        // Add amenity object to AmenityResponse type
        await _amenityRepository.AddAmenity(amenity);

        return amenity.ToAmenityResponse();
    }

    public async Task<AmenityResponse?> UpdateAmenity(AmenityUpdateRequest amenityUpdateRequest)
    {
        if (amenityUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityUpdateRequest));
        }
        var existingAmenity = await _amenityRepository.GetAmenityById(amenityUpdateRequest.Id);
        if (existingAmenity == null)
        {
            throw new ArgumentException("Id amenity does not exist");
        }
        
        existingAmenity.Name = amenityUpdateRequest.Name;
        existingAmenity.Description = amenityUpdateRequest.Description;
        existingAmenity.Status = amenityUpdateRequest.Status;
        existingAmenity.ModifiedTime = amenityUpdateRequest.ModifiedTime;
        existingAmenity.ModifiedBy = amenityUpdateRequest.ModifiedBy;
        
        await _amenityRepository.UpdateAmenity(existingAmenity);
        
        return existingAmenity.ToAmenityResponse();
    }
    
    public async Task<AmenityResponse?> DeleteAmenityById(AmenityDeleteRequest amenityDeleteRequest)
    {
        if (amenityDeleteRequest == null)
        {
            throw new ArgumentNullException(nameof(amenityDeleteRequest));
        }
        var existingAmenity = await _amenityRepository.GetAmenityById(amenityDeleteRequest.Id);
        if (existingAmenity == null)
        {
            throw new ArgumentException("Id amenity does not exist");
        }

        existingAmenity.Status = (EntityStatus)3;
        existingAmenity.Deleted = true;
        existingAmenity.DeletedTime = amenityDeleteRequest.DeletedTime;
        existingAmenity.DeletedBy = amenityDeleteRequest.DeletedBy;
        
        await _amenityRepository.DeleteAmenityById(existingAmenity);
        
        return existingAmenity.ToAmenityResponse();
    }

    public async Task<List<AmenityResponse>> GetAllAmenities()
    {
        var amenities = await _amenityRepository.GetAllAmenities();

        var amenityResponses = amenities
            .Select(amenity => amenity.ToAmenityResponse())
            .ToList();
        
        return amenityResponses;
    }

    public async Task<AmenityResponse?> GetAmenityById(Guid? amenityId)
    {
        if (amenityId == null)
            return null;
        
        var amenity = await _amenityRepository.GetAmenityById(amenityId.Value);
        if (amenity == null)
            return null;
        
        return amenity.ToAmenityResponse();
    }

    public string GenerateToken()
    {
        return _amenityRepository.GenerateToken();
    }
}