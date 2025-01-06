using Domain.Enums;

namespace Domain.DTO.RoomBookingDetail;

public class RoomBookingDetailResponse
{
    public Guid Id { get; set; }        
    public Guid RoomId { get; set; }
    public Guid RoomBookingId { get; set; }
    public DateTimeOffset? CheckInBooking { get; set; }
    public DateTimeOffset? CheckOutBooking { get; set; }
    public DateTimeOffset? CheckInReality { get; set; }
    public DateTimeOffset? CheckOutReality { get; set; }        
    public decimal? Price { get; set; }
    public decimal Deposit { get; set; } = 0;
    public decimal ExtraPrice { get; set; }
    public decimal? Expenses { get; set; } = 0;
    public string? Note { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.Active;
    public DateTimeOffset? CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    
    public string? RoomName { get; set; }
    public int? NumberOfNight { get; set; }
    public decimal? RoomPrice { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if(obj.GetType() != typeof(RoomBookingDetailResponse)) return false;

        RoomBookingDetailResponse roomBookingDetailResponse = (RoomBookingDetailResponse)obj;
        
        return Id == roomBookingDetailResponse.Id && RoomId == roomBookingDetailResponse.RoomId &&
        RoomBookingId == roomBookingDetailResponse.RoomBookingId && CheckInBooking == roomBookingDetailResponse.CheckInBooking && 
        CheckOutBooking == roomBookingDetailResponse.CheckOutBooking &&  CheckInReality == roomBookingDetailResponse.CheckInReality &&
        CheckOutReality == roomBookingDetailResponse.CheckOutReality &&  Price == roomBookingDetailResponse.Price && 
        Deposit == roomBookingDetailResponse.Deposit && ExtraPrice == roomBookingDetailResponse.ExtraPrice &&
        Expenses == roomBookingDetailResponse.Expenses && Note == roomBookingDetailResponse.Note &&
        Status == roomBookingDetailResponse.Status && CreatedTime == roomBookingDetailResponse.CreatedTime &&
        CreatedBy == roomBookingDetailResponse.CreatedBy && ModifiedTime == roomBookingDetailResponse.ModifiedTime &&
        ModifiedBy == roomBookingDetailResponse.ModifiedBy && Deleted == roomBookingDetailResponse.Deleted &&
        DeletedBy == roomBookingDetailResponse.DeletedBy && DeletedTime == roomBookingDetailResponse.DeletedTime;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }

    public RoomBookingDetailUpdateRequest ToRoomBookingDetailUpdateRequest()
    {
        return new RoomBookingDetailUpdateRequest()
        {
            Id = Id,
            CheckInBooking = CheckInBooking,
            CheckOutBooking = CheckOutBooking,
            CheckInReality = CheckInReality,
            CheckOutReality = CheckOutReality,
            ExtraPrice = ExtraPrice,
            Expenses = Expenses,
            Price = Price,
            Note = Note,
            Status = Status,
            ModifiedTime = ModifiedTime,
            ModifiedBy = ModifiedBy,
            Deleted = Deleted
        };
    }
}

public static class RoomBookingDetailResponseExtensions
{
    public static RoomBookingDetailResponse ToRoomBookingDetailResponse(this Models.RoomBookingDetail roomBookingDetail)
    {
        if (roomBookingDetail == null)
            throw new ArgumentNullException(nameof(roomBookingDetail), "RoomBookingDetail không được null.");
        
        var checkInReality = roomBookingDetail.CheckInReality;
        var checkOutReality = roomBookingDetail.CheckOutReality;

        int numberOfNights;

        if (checkInReality == null && checkOutReality == null)
        {
            numberOfNights = (roomBookingDetail.CheckInBooking.HasValue && roomBookingDetail.CheckOutBooking.HasValue)
                ? (int)((roomBookingDetail.CheckOutBooking.Value.Date - roomBookingDetail.CheckInBooking.Value.Date).TotalDays)
                : 0;
        }
        else if (checkOutReality == null)
        {
            numberOfNights = (checkInReality.HasValue && roomBookingDetail.CheckOutBooking.HasValue)
                ? (int)((roomBookingDetail.CheckOutBooking.Value.Date - checkInReality.Value.Date).TotalDays)
                : 0;
        }
        else
        {
            numberOfNights = (checkInReality.HasValue && checkOutReality.HasValue)
                ? (int)((checkOutReality.Value.Date - checkInReality.Value.Date).TotalDays)
                : 0;
        }

        
        return new RoomBookingDetailResponse()
        {
            Id = roomBookingDetail.Id,
            RoomId = roomBookingDetail.RoomId,
            RoomBookingId = roomBookingDetail.RoomBookingId,
            CheckInBooking = roomBookingDetail.CheckInBooking,
            CheckOutBooking = roomBookingDetail.CheckOutBooking,
            CheckInReality = roomBookingDetail.CheckInReality,
            CheckOutReality = roomBookingDetail.CheckOutReality,
            Price = roomBookingDetail.Price,
            Deposit = roomBookingDetail.Deposit,
            ExtraPrice = roomBookingDetail.ExtraPrice,
            Expenses = roomBookingDetail.Expenses,
            Note = roomBookingDetail.Note,
            Status = roomBookingDetail.Status,
            CreatedTime = roomBookingDetail.CreatedTime,
            CreatedBy = roomBookingDetail.CreatedBy,
            ModifiedTime = roomBookingDetail.ModifiedTime,
            ModifiedBy = roomBookingDetail.ModifiedBy,
            Deleted = roomBookingDetail.Deleted,
            DeletedBy = roomBookingDetail.DeletedBy,
            DeletedTime = roomBookingDetail.DeletedTime,
            RoomName = roomBookingDetail.Room?.Name ?? "Không có tên phòng",
            NumberOfNight = numberOfNights,
            RoomPrice = roomBookingDetail.Room?.Price
        };
    }
}

// public static class SurchargeCalculator
// {
//     public static decimal CalculateSurcharge(DateTimeOffset bookingTime, DateTimeOffset realityTime, decimal roomPrice, bool isCheckIn)
//     {
//         // TimeSpan bookingLocalTime = bookingTime.LocalDateTime.TimeOfDay;
//         // TimeSpan realityLocalTime = realityTime.LocalDateTime.TimeOfDay;
//         TimeSpan defaultCheckInTime = new TimeSpan(14, 0, 0); // 2:00 PM
//         TimeSpan defaultCheckOutTime = new TimeSpan(12, 0, 0); // 12:00 PM
//         
//         TimeSpan realityLocalTime = realityTime.LocalDateTime.TimeOfDay;
//         decimal surcharge = 0;
//
//         if (isCheckIn)
//         {
//             if (realityLocalTime >= new TimeSpan(5, 0, 0) && realityLocalTime 
//                 < new TimeSpan(9, 0, 0))
//             {
//                 surcharge = 0.5m * roomPrice;
//             }
//             else if (realityLocalTime >= new TimeSpan(9, 0, 0) && realityLocalTime 
//                      < new TimeSpan(14, 0, 0))
//             {
//                 surcharge = 0.3m * roomPrice;
//             }
//         }
//         else
//         {
//             if (realityLocalTime > defaultCheckOutTime)
//             {
//                 if (realityLocalTime <= new TimeSpan(15, 0, 0)) // Từ 12:00 PM đến 3:00 PM
//                 {
//                     surcharge = 0.3m * roomPrice; // Phụ thu 30%
//                 }
//                 else if (realityLocalTime <= new TimeSpan(18, 0, 0)) // Từ 3:00 PM đến 6:00 PM
//                 {
//                     surcharge = 0.5m * roomPrice; // Phụ thu 50%
//                 }
//                 else if (realityLocalTime > new TimeSpan(18, 0, 0)) // Sau 6:00 PM
//                 {
//                     surcharge = roomPrice; // Phụ thu 100%
//                 }
//             }
//         }
//
//         return surcharge;
//     }
// }

