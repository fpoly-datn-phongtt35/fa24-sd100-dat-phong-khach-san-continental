using Domain.Enums;

namespace Domain.DTO.EditHistory;

public class EditHistoryUpdateRequest
{
    public int Id { get; set; }
    public Guid RoomBookingDetailId { get; set; }
    public For For { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    public Models.EditHistory ToEditHistory()
    {
        return new Models.EditHistory()
        {
            Id = Id,
            RoomBookingDetailId = RoomBookingDetailId,
            For = For,
            Content = Content,
            Description = Description,
            ModifiedAt = ModifiedAt
        };
    }
}