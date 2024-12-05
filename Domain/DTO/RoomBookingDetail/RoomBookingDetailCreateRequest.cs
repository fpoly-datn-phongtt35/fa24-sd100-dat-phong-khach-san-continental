using Domain.DTO.Client;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailCreateRequest
    {
        public Guid RoomId { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? Deposit { get; set; }
        public RoomBookingStatus? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public List<Domain.Models.ServiceOrderDetail>? SelectedServices { get; set; }
        public ClientInsertCustomerViewModel? Customer { get; set; }
    }
}
