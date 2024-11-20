using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityResponse
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public EntityStatus Status { get; set; }

    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        if(obj.GetType() != typeof(AmenityResponse)) return false;
        
        AmenityResponse amenity = (AmenityResponse)obj;
        return Id == amenity.Id && Name == amenity.Name &&
               Description == amenity.Description && Status == amenity.Status &&
               CreatedTime == amenity.CreatedTime && CreatedBy == amenity.CreatedBy &&
               ModifiedTime == amenity.ModifiedTime && ModifiedBy == amenity.ModifiedBy &&
               Deleted == amenity.Deleted && DeletedBy == amenity.DeletedBy &&
               DeletedTime == amenity.DeletedTime;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public AmenityUpdateRequest ToAmenityUpdateRequest()
    {
        return new AmenityUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy,
            Deleted = Deleted
        };
    }

    public AmenityDeleteRequest ToAmenityDeleteRequest()
    {
        return new AmenityDeleteRequest()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedBy = DeletedBy,
            DeletedTime = DeletedTime
        };
    }
}

public static class AmenityExtensions
{
    public static AmenityResponse ToAmenityResponse(this Models.Amenity amenity)
    {
        // amenity => convert => AmenityResponse
        return new AmenityResponse()
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Description = amenity.Description,
            Status = amenity.Status,
            CreatedTime = amenity.CreatedTime,
            CreatedBy = amenity.CreatedBy,
            ModifiedTime = amenity.ModifiedTime,
            ModifiedBy = amenity.ModifiedBy,
            Deleted = amenity.Deleted,
            DeletedTime = amenity.DeletedTime,
            DeletedBy = amenity.DeletedBy
        };
    }
}