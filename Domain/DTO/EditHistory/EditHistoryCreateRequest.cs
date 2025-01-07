using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.EditHistory;

public class EditHistoryCreateRequest
{
    public Guid RoomBookingDetailId { get; set; }
    [Required(ErrorMessage = "Mục chỉnh sửa không được để trống. ")]
    public For For { get; set; }
    
    [Required(ErrorMessage = "Nội dung không được để trống.")]
    public string? Content { get; set; }
    
    [Required(ErrorMessage = "Mô tả không được để trống.")]
    public string? Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    
    public Models.EditHistory ToEditHistory()
    {
        return new Models.EditHistory()
        {
            RoomBookingDetailId = RoomBookingDetailId,
            For = For,
            Content = Content,
            Description = Description,
            ModifiedAt = ModifiedAt
        };
    }
}