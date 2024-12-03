using Domain.Enums;

namespace Domain.DTO.RoomBooking;

public class RoomBookingResponse
{
    public Guid Id { get; set; }
    public BookingType? BookingType { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? TotalRoomPrice { get; set; }
    public decimal? TotalServicePrice { get; set; }
    public decimal? TotalExtraPrice { get; set; }
    public RoomBookingStatus Status { get; set; }
    public string? StaffFullName { get; set; }
    public string? CustomerFullName { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        
        if(obj.GetType() != typeof(RoomBookingResponse)) return false;
        
        RoomBookingResponse roomBookingResponse = (RoomBookingResponse)obj;
        
        return Id == roomBookingResponse.Id && BookingType == roomBookingResponse.BookingType &&
               CustomerId == roomBookingResponse.CustomerId && StaffId == roomBookingResponse.StaffId &&
               TotalPrice == roomBookingResponse.TotalPrice && TotalRoomPrice == roomBookingResponse.TotalRoomPrice &&
               TotalServicePrice == roomBookingResponse.TotalServicePrice && TotalExtraPrice == roomBookingResponse.TotalExtraPrice &&
               Status == roomBookingResponse.Status && CreatedTime == roomBookingResponse.CreatedTime &&
               CreatedBy == roomBookingResponse.CreatedBy && ModifiedTime == roomBookingResponse.ModifiedTime &&
               ModifiedBy == roomBookingResponse.ModifiedBy && Deleted == roomBookingResponse.Deleted &&
               DeletedTime == roomBookingResponse.DeletedTime && DeletedBy == roomBookingResponse.DeletedBy;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public RoomBookingUpdateRequest ToRoomBookingUpdateRequest()
    {
        return new RoomBookingUpdateRequest()
        {
            Id = Id,
            BookingType = BookingType,
            CustomerId = CustomerId,
            StaffId = StaffId,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy
        };
    }

    public RoomBookingDeleteRequest ToRoomBookingDeleteRequest()
    {
        return new RoomBookingDeleteRequest()
        {
            Id = Id,
            CustomerId = CustomerId,
            StaffId = StaffId,
            Status = Status,
            Deleted = Deleted,
            DeletedTime = DeletedTime,
            DeletedBy = DeletedBy
        };
    }
}

public static class RoomBookingResponseExtensions
{
    public static RoomBookingResponse ToRoomBookingResponse(this Models.RoomBooking roomBooking)
    {
        return new RoomBookingResponse()
        {
            Id = roomBooking.Id,
            BookingType = roomBooking.BookingType,
            CustomerId = roomBooking.CustomerId,
            StaffId = roomBooking.StaffId,
            TotalServicePrice = roomBooking.TotalServicePrice,
            TotalExtraPrice = roomBooking.TotalExtraPrice,
            TotalRoomPrice = roomBooking.TotalRoomPrice,
            TotalPrice = roomBooking.TotalPrice,
            Status = roomBooking.Status,
            CreatedTime = roomBooking.CreatedTime,
            CreatedBy = roomBooking.CreatedBy,
            ModifiedTime = roomBooking.ModifiedTime,
            ModifiedBy = roomBooking.ModifiedBy,
            Deleted = roomBooking.Deleted,
            DeletedTime = roomBooking.DeletedTime,
            DeletedBy = roomBooking.DeletedBy,
            // Kiểm tra null trước khi ghép
            StaffFullName = roomBooking.Staff.LastName + " " + roomBooking.Staff.FirstName,
            CustomerFullName = roomBooking.Customer.LastName + " " + roomBooking.Customer.FirstName,
        };
    }
}