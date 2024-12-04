using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Order
{
    public class PaymentLinkCreateRequest
    {
        public Guid RoomBookingId { get; set; }
        public PaymentType? PaymentType { get; set; }
        public int? Money { get; set; }
    }
}
