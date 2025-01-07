using Domain.Enums;

namespace Domain.DTO.EditHistory;

public class EditHistoryResponse
{
    public int Id { get; set; }
    public Guid RoomBookingDetailId { get; set; }
    public For For { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if(obj.GetType() != typeof(EditHistoryResponse)) return false;

        EditHistoryResponse editHistory = (EditHistoryResponse)obj;
        
        return Id == editHistory.Id && For == editHistory.For && 
               Content == editHistory.Content && Description == editHistory.Description &&
               ModifiedAt == editHistory.ModifiedAt && RoomBookingDetailId == editHistory.RoomBookingDetailId; 
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public EditHistoryUpdateRequest ToEditHistoryUpdateRequest()
    {
        return new EditHistoryUpdateRequest()
        {
            RoomBookingDetailId = RoomBookingDetailId,
            Id = Id,
            For = For,
            Content = Content,
            Description = Description,
            ModifiedAt = ModifiedAt
        };
    }
}

public static class EditHistoryExtensions
{
    public static EditHistoryResponse ToEditHistoryResponse(this Models.EditHistory editHistory)
    {
        return new EditHistoryResponse()
        {
            RoomBookingDetailId = editHistory.RoomBookingDetailId,
            Id = editHistory.Id,
            For = editHistory.For,
            Content = editHistory.Content,
            Description = editHistory.Description,
            ModifiedAt = editHistory.ModifiedAt
        };
    }
}