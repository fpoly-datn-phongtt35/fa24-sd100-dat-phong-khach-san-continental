using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitUpdateRequest
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Tên đơn vị không được để trống")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Mô tả không được để trống")]
    public string? Description { get; set; }
    public EntityStatus Status { get; set; }
    
    public Models.Unit ToUnit()
    {
        return new Models.Unit()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Status = Status
        };
    }
}