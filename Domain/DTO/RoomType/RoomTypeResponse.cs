using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
using Domain.Enums;

namespace Domain.DTO.RoomType;

public class RoomTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaximumOccupancy { get; set; }
    public EntityStatus Status { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset DeletedTime { get; set; }

    public List<AmenityRoomResponse> AmenityRooms { get; set; } = new List<AmenityRoomResponse>();
    public List<RoomTypeServiceResponse> RoomTypeServices { get; set; } = new List<RoomTypeServiceResponse>();
    
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        if(obj.GetType() != typeof(RoomTypeResponse)) return false;
        
        RoomTypeResponse roomType = (RoomTypeResponse)obj;
        return Id == roomType.Id && Name == roomType.Name && 
               Description == roomType.Description && MaximumOccupancy == roomType.MaximumOccupancy &&
               Status == roomType.Status && CreatedTime == roomType.CreatedTime &&
               CreatedBy == roomType.CreatedBy && ModifiedTime == roomType.ModifiedTime &&
               ModifiedBy == roomType.ModifiedBy && Deleted == roomType.Deleted &&
               DeletedTime == roomType.DeletedTime && DeletedBy == roomType.DeletedBy;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public RoomTypeUpdateRequest ToRoomTypeUpdateRequest()
    {
        return new RoomTypeUpdateRequest()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            MaximumOccupancy = MaximumOccupancy,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }

    public RoomTypeDeleteRequest ToRoomTypeDeleteRequest()
    {
        return new RoomTypeDeleteRequest()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}

public static class RoomTypeExtensions
{
    public static RoomTypeResponse ToRoomTypeResponse(this Models.RoomType roomType)
    {
        // roomType => convert => roomTypeResponse
        return new RoomTypeResponse()
        {
            Id = roomType.Id,
            Name = roomType.Name,
            Description = roomType.Description,
            MaximumOccupancy = roomType.MaximumOccupancy,
            Status = roomType.Status,
            CreatedTime = roomType.CreatedTime,
            CreatedBy = roomType.CreatedBy,
            ModifiedTime = roomType.ModifiedTime,
            ModifiedBy = roomType.ModifiedBy,
            Deleted = roomType.Deleted,
            DeletedTime = roomType.DeletedTime,
            DeletedBy = roomType.DeletedBy
        };
    }
}