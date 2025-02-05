﻿using Domain.Enums;

namespace Domain.DTO.RoomBooking
{
    public class RoomBookingCreateRequestForCustomer
    {
        public BookingType BookingType { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? StaffId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalRoomPrice { get; set; }
        public decimal? TotalServicePrice { get; set; }
        public decimal? TotalExtraPrice { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? TotalPriceReality { get; set; }
        public BookingBy? BookingBy { get; set; } = Enums.BookingBy.Day;
        public RoomBookingStatus? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? NewId { get; set; }
    }
}
