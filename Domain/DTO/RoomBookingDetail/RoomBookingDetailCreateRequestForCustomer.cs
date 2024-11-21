using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailCreateRequestForCustomer
    {
        public List<Guid> RoomIds { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? Deposit { get; set; }
        public EntityStatus? Status { get; set; }

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public string RoomIdsAsString
        {
            get => string.Join(",", RoomIds.Select(id => id.ToString()));
            set => RoomIds = value.Split(',').Select(Guid.Parse).ToList();
        }
    }
}
