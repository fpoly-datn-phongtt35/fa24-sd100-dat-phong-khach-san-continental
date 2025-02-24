using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitCreateRequest
{
    [Required(ErrorMessage = "Tên đơn vị không được để trống")]
    public string Name { get; set; }
    public string Description { get; set; }
    public EntityStatus Status { get; set; }

    public Models.Unit ToUnit()
    {
        return new Models.Unit()
        {
            Name = Name,
            Description = Description,
            Status = Status
        };
    }
}