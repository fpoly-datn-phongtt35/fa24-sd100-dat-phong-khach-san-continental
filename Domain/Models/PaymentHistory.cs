using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PaymentHistory
    {
        [Key]
        public Guid Id { get; set; }
        public int OrderCode { get; set; }
        public Guid RoomBookingId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset? PaymentTime { get; set; }
        public PaymentType? Note { get; set; }

        public virtual RoomBooking RoomBooking { get; set; }
    }
}
