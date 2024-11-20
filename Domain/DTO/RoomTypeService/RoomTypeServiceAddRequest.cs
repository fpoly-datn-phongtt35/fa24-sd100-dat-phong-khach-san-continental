using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.RoomTypeService;

public class RoomTypeServiceAddRequest
{
    [Required(ErrorMessage = "Loại phòng không được để trống")]
    public Guid RoomTypeId { get; set; }
    
    [Required(ErrorMessage = "Dịch vụ không được để trống")]
    public Guid ServiceId { get; set; }
    
    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Amount { get; set; }
    
    public EntityStatus Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Convert the current object of RoomTypeServiceAddRequest into a
    /// new object of RoomTypeService type
    /// </summary>
    /// <returns>RoomTypeService object</returns>
    public Models.RoomTypeService ToRoomTypeService()
    {
        return new Models.RoomTypeService()
        {
            RoomTypeId = RoomTypeId,
            ServiceId = ServiceId,
            Amount = Amount,
            Status = Status,
            CreatedTime = CreatedTime,
            CreatedBy = CreatedBy
        };
    }
}