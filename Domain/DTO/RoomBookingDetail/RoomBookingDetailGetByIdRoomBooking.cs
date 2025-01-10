using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailGetByIdRoomBooking
    {
        public Guid RoomBookingDetailId { get; set; }
        public string Name { get; set; }
        public Guid RoomId { get; set; }
        public EntityStatus Status { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? Expenses { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? ExtraService { get; set; }
        public string? Note { get; set; }
    }
}
