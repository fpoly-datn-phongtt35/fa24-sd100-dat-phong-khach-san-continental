using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailUpdateRequest
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal ExtraPrice { get; set; }
        public decimal? Price { get; set; }
        public decimal? Expenses { get; set; }
        public string? Note { get; set; }
        public EntityStatus Status { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Models.RoomBookingDetail ToRoomBookingDetail()
        {
            return new Models.RoomBookingDetail()
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
}
