using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomAvailableResponse 
    {
        public int TotalRoom {  get; set; }
        public int TotalOccupancy {  get; set; }
        public List<RoomResponse> LstRoom { get; set; }
    }
}
