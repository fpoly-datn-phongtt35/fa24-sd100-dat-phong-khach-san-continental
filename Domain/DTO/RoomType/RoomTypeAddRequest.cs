using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Services.Services.RoomType;

namespace Domain.DTO.RoomType;

public class RoomTypeAddRequest
{
    [Required(ErrorMessage = "Tên không được để trống.")]
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Số lượng không được để trống.")]
    [RegularExpression("^[1-9]$|^10$", ErrorMessage = "Số lượng phải là một số trong khoảng từ 1 đến 10.")]
    public int? MaximumOccupancy { get; set; }
    public int Quantity { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    public Models.RoomType ToRoomType()
    {
        return new Models.RoomType()
        {
            Name = Name,
            Description = Description,
            MaximumOccupancy = MaximumOccupancy,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy,
            Quantity = Quantity
        };
    }
}


