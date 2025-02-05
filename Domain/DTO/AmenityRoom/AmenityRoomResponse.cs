﻿using System.ComponentModel.DataAnnotations;
using Domain.DTO.Amenity;
using Domain.Enums;

namespace Domain.DTO.AmenityRoom;

public class AmenityRoomResponse
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Không được để trống tiện nghi")]
    public Guid AmenityId { get; set; }
    
    [Required(ErrorMessage = "Không được để trống loại phòng")]
    public Guid RoomTypeId { get; set; }
    public string? AmenityName { get; set; }
    public string? RoomTypeName { get; set; }
    
    [Required(ErrorMessage = "Không được để trống số lượng")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Amount { get; set; }
    
    public EntityStatus Status { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public AmenityResponse Amenity { get; set; } = new AmenityResponse();
    
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        if(obj.GetType() != typeof(AmenityRoomResponse)) return false;
        
        AmenityRoomResponse amenityRoom = (AmenityRoomResponse)obj;
        
        return Id == amenityRoom.Id && AmenityId == amenityRoom.AmenityId &&
               RoomTypeId == amenityRoom.RoomTypeId && Amount == amenityRoom.Amount &&
               Status == amenityRoom.Status && CreatedTime == amenityRoom.CreatedTime &&
               CreatedBy == amenityRoom.CreatedBy && ModifiedTime == amenityRoom.ModifiedTime &&
               ModifiedBy == amenityRoom.ModifiedBy && Deleted == amenityRoom.Deleted &&
               DeletedTime == amenityRoom.DeletedTime && DeletedBy == amenityRoom.DeletedBy;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"AmenityRoomId: {Id}, AmenityId: {AmenityId}, Amenity: {AmenityName} ,RoomTypeId: {RoomTypeId}, " +
               $"RoomType: {RoomTypeName}, Amount: {Amount}, Status: {Status}, CreatedTime: {CreatedTime}," +
               $"CreatedBy: {CreatedBy}, ModifiedTime: {ModifiedTime}, " + $"ModifiedBy: {ModifiedBy}, " +
               $"DeletedTime: {DeletedTime}, DeletedBy: {DeletedBy},"; 
    }

    public AmenityRoomUpdateRequest ToAmenityRoomUpdateRequest()
    {
        return new AmenityRoomUpdateRequest()
        {
            Id = Id,
            AmenityId = AmenityId,
            RoomTypeId = RoomTypeId,
            Amount = Amount,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }

    public AmenityRoomDeleteRequest ToAmenityRoomDeleteRequest()
    {
        return new AmenityRoomDeleteRequest()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}

public static class AmenityRoomResponseExtensions
{
    public static AmenityRoomResponse ToAmenityRoomResponse(this Models.AmenityRoom amenityRoom)
    {
        // amenityRoom => convert => AmenityRoomResponse
        return new AmenityRoomResponse()
        {
            Id = amenityRoom.Id,
            AmenityId = amenityRoom.AmenityId,
            RoomTypeId = amenityRoom.RoomTypeId,
            Amount = amenityRoom.Amount,
            Status = amenityRoom.Status,
            CreatedTime = amenityRoom.CreatedTime,
            CreatedBy = amenityRoom.CreatedBy,
            ModifiedTime = amenityRoom.ModifiedTime,
            ModifiedBy = amenityRoom.ModifiedBy,
            Deleted = amenityRoom.Deleted,
            DeletedTime = amenityRoom.DeletedTime,
            DeletedBy = amenityRoom.DeletedBy,
            
            AmenityName = amenityRoom.Amenity?.Name,
            RoomTypeName = amenityRoom.RoomType?.Name
        };
    }
}