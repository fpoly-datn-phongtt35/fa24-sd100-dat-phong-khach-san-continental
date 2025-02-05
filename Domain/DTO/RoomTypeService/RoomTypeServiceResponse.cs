﻿using System.ComponentModel.DataAnnotations;
using Domain.DTO.Service;
using Domain.Enums;

namespace Domain.DTO.RoomTypeService;

public class RoomTypeServiceResponse
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Loại phòng không được để trống")]
    public Guid RoomTypeId { get; set; }
    
    [Required(ErrorMessage = "Dịch vụ không được để trống")]
    public Guid ServiceId { get; set; }
    public string? RoomTypeName { get; set; }
    public string? ServiceName { get; set; }
    
    [Required(ErrorMessage = "Số lượng không được để trống")]
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

    public ServiceResponse Service { get; set; } = new ServiceResponse();
    
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() != typeof(RoomTypeServiceResponse)) return false;

        RoomTypeServiceResponse roomTypeService = (RoomTypeServiceResponse)obj;

        return Id == roomTypeService.Id && RoomTypeId == roomTypeService.RoomTypeId &&
               ServiceId == roomTypeService.ServiceId && Amount == roomTypeService.Amount &&
               Status == roomTypeService.Status && CreatedTime == roomTypeService.CreatedTime &&
               CreatedBy == roomTypeService.CreatedBy && ModifiedTime == roomTypeService.ModifiedTime &&
               ModifiedBy == roomTypeService.ModifiedBy && Deleted == roomTypeService.Deleted &&
               DeletedTime == roomTypeService.DeletedTime && DeletedBy == roomTypeService.DeletedBy;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public RoomTypeServiceUpdateRequest ToRoomTypeServiceUpdateRequest()
    {
        return new RoomTypeServiceUpdateRequest()
        {
            Id = Id,
            ServiceId = ServiceId,
            RoomTypeId = RoomTypeId,
            Amount = Amount,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy,
            Deleted = Deleted
        };
    }

    public RoomTypeServiceDeleteRequest ToRoomTypeServiceDeleteRequest()
    {
        return new RoomTypeServiceDeleteRequest()
        {
            Id = Id,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}

public static class RoomTypeServiceExtensions
{
    public static RoomTypeServiceResponse ToRoomTypeServiceResponse(this Models.RoomTypeService roomTypeService)
    {
        // roomTypeService => convert => RoomTypeServiceResponse
        return new RoomTypeServiceResponse()
        {
            Id = roomTypeService.Id,
            ServiceId = roomTypeService.ServiceId,
            RoomTypeId = roomTypeService.RoomTypeId,
            Amount = roomTypeService.Amount,
            Status = roomTypeService.Status,
            CreatedTime = roomTypeService.CreatedTime,
            CreatedBy = roomTypeService.CreatedBy,
            ModifiedTime = roomTypeService.ModifiedTime,
            ModifiedBy = roomTypeService.ModifiedBy,
            Deleted = roomTypeService.Deleted,
            DeletedTime = roomTypeService.DeletedTime,
            DeletedBy = roomTypeService.DeletedBy,
            RoomTypeName = roomTypeService.RoomType?.Name,
            ServiceName = roomTypeService.Service?.Name
        };
    }
}