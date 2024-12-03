using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.PaymentHistory
{
    public class PaymentHistoryCreateRequest
    {
        public Guid RoomBookingId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset? PaymentTime { get; set; }
        public PaymentType? Note { get; set; }
    }
}
