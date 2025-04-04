using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class HotelInfoDto
    {
        public int TotalRoom { get; set; }
        public int AvailableRoom { get; set; }
        public int BookedRoom { get; set; }
        public int HotelMaximumOccupancy { get; set; }
        public int InHotel { get; set; }
        public int AvailableOccupancy { get; set; }
        public double RoomBookedRate { get; set; }
    }
}
