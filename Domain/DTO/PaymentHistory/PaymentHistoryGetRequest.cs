using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.PaymentHistory
{
    public class PaymentHistoryGetRequest : PagingRequest
    {
        public Guid? RoomBookingId { get; set; }
        public Guid? CustomerId { get; set; }
        public PaymentType? Note { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public decimal? FromAmount { get; set; }
        public decimal? ToAmount { get; set; }
    }
}
