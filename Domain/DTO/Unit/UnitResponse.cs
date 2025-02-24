using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO.Unit;

public class UnitResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EntityStatus Status { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() != typeof(UnitResponse)) return false;
        
        UnitResponse unit = (UnitResponse)obj;
        return Id == unit.Id && Name == unit.Name 
                             && Description == unit.Description && Status == unit.Status;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public UnitUpdateRequest ToUnitUpdateRequest()
    {
        return new UnitUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Status = Status
        };
    }

    public UnitDeleteRequest ToUnitDeleteRequest()
    {
        return new UnitDeleteRequest()
        {
            Id = Id,
            Status = Status
        };
    }
}

public static class UnitResponseExtensions
{
    public static UnitResponse ToUnitResponse(this Models.Unit unit)
    {
        return new UnitResponse()
        {
            Id = unit.Id,
            Name = unit.Name,
            Description = unit.Description,
            Status = unit.Status
        };
    }
}