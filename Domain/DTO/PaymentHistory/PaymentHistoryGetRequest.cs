using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.PaymentHistory
{
    public class PaymentHistoryGetRequest : PagingRequest
    {
        public int? Note { get; set; }
        public Guid? RoomBookingId { get; set; }
    }
}
