using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Models;

namespace ViewClient.Models.DTO
{
    public class RoomDetailViewModel
    {
        public RoomResponse Room { get; set; }
        public Floor Floor { get; set; }
        public RoomTypeResponse RoomType { get; set; }
        public IEnumerable<Service> Services { get; set; }
    }
}
