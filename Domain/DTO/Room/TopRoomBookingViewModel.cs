using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.DTO.Room
{
    public class TopRoomBookingViewModel
    {
        public Guid Id { get; set; }           // ID phòng
        public string Name { get; set; }       // Tên phòng
        public string RoomTypeName { get; set; } // Tên loại phòng
        public decimal Price { get; set; }     // Giá phònga
        public string? Address { get; set; }
        public double? RoomSize { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public int BookingCount { get; set; }  // Số lần đặt phòng

    }
 

}
