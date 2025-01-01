using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.DTO.Room
{
    public class TopCustomerBooking
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType? Gender { get; set; }
        public int BookingCount { get; set; }
        public decimal TotalPrice { get; set; } // Số lần đặt phòng
    }
}
