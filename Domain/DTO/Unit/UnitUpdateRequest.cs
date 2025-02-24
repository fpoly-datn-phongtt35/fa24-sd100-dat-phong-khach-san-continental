using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
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