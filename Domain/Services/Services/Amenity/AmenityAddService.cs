using Domain.DTO.Amenity;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenity;

namespace Domain.Services.Services.Amenity;

public class AmenityAddService : IAmenityAddService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityAddService(IAmenityRepository amenityRepository)
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
        var exists = await _amenityRepository.GetAmenityExists(amenityCreateRequest.Name);
        if (exists != null)
        {
            // Ném ra exception với thông báo lỗi phù hợp
            throw new Exception($"Đã tồn tại tiện ích với tên '{amenityCreateRequest.Name}'");
        }
        
        var amenity = amenityCreateRequest.ToAmenity();
        
        // Add amenity object to AmenityResponse type
        await _amenityRepository.AddAmenity(amenity);
        return amenity.ToAmenityResponse();
    }
}