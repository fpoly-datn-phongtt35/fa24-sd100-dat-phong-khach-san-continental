using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomAddRequest
{
    [Required(ErrorMessage = "Tiện nghi không được để trống")]
    public Guid AmenityId { get; set; }
    
    [Required(ErrorMessage = "Loại phòng không được để trống")]
    public Guid RoomTypeId { get; set; }
    
    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Amount { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of AmenityRoomAddRequest into a
    /// new object of AmenityRoom type
    /// </summary>
    /// <returns>Return AmenityRoom object</returns>
    public Models.AmenityRoom ToAmenityRoom()
    {
        return new Models.AmenityRoom()
        {
            AmenityId = AmenityId,
            RoomTypeId = RoomTypeId,
            Amount = Amount,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}