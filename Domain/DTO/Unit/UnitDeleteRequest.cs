using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitDeleteRequest
{
    public Guid Id { get; set; }
    public EntityStatus Status { get; set; }
    
    public Models.Unit ToUnit()
    {
        return new Models.Unit()
        {
            Id = Id,
            Status = Status
        };
    }
}