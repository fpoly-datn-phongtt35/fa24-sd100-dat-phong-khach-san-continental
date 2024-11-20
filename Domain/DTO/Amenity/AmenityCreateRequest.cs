using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.Amenity;

public class AmenityCreateRequest
{
    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; } 
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of AmenityCreateRequest into a new object of Amenity type
    /// </summary>
    /// <returns>Returns Amenity object</returns>
    public Models.Amenity ToAmenity()
    {
        return new Models.Amenity()
        {
            Name = Name,
            Description = Description,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}