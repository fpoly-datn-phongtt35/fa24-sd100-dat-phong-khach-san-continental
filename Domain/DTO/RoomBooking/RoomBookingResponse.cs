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
    public decimal? TotalExpenses { get; set; }
    public decimal? TotalPriceReality { get; set; }
    public BookingBy? BookingBy { get; set; }
    public RoomBookingStatus Status { get; set; }
    public string? StaffFullName { get; set; }
    public string? CustomerFullName { get; set; }
    public string? RoomName { get; set; }
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
               TotalExpenses == roomBookingResponse.TotalExpenses && TotalPriceReality == roomBookingResponse.TotalPriceReality &&
               BookingBy == roomBookingResponse.BookingBy &&
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
            TotalExpenses = roomBooking.TotalExpenses,
            TotalPriceReality = roomBooking.TotalPriceReality,
            BookingBy = roomBooking.BookingBy,
            Status = roomBooking.Status,
            CreatedTime = roomBooking.CreatedTime,
            CreatedBy = roomBooking.CreatedBy,
            ModifiedTime = roomBooking.ModifiedTime,
            ModifiedBy = roomBooking.ModifiedBy,
            Deleted = roomBooking.Deleted,
            DeletedTime = roomBooking.DeletedTime,
            DeletedBy = roomBooking.DeletedBy,
            // Kiểm tra null trước khi ghép
            StaffFullName = roomBooking.Staff != null 
                ? roomBooking.Staff.LastName + " " + roomBooking.Staff.FirstName 
                : "không nhân viên",
            CustomerFullName = roomBooking.Customer != null 
                ? roomBooking.Customer.LastName + " " + roomBooking.Customer.FirstName
                : "No Customer Assigned", 
        };
    }
    
    public static Models.RoomBooking ToRoomBooking(this RoomBookingResponse response)
    {
        return new Models.RoomBooking()
        {
            Id = response.Id,
            BookingType = response.BookingType,
            CustomerId = response.CustomerId,
            StaffId = response.StaffId,
            TotalServicePrice = response.TotalServicePrice,
            TotalExtraPrice = response.TotalExtraPrice,
            TotalRoomPrice = response.TotalRoomPrice,
            TotalPrice = response.TotalPrice,
            TotalExpenses = response.TotalExpenses,
            TotalPriceReality = response.TotalPriceReality,
            BookingBy = response.BookingBy,
            Status = response.Status,
            CreatedTime = response.CreatedTime,
            CreatedBy = response.CreatedBy,
            ModifiedTime = response.ModifiedTime,
            ModifiedBy = response.ModifiedBy,
            Deleted = response.Deleted,
            DeletedTime = response.DeletedTime,
            DeletedBy = response.DeletedBy,
            // Nếu cần thêm các trường khác, ánh xạ chúng tại đây
        };
    }

}