using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Models;

namespace ViewClient.Models.DTO
{
    public class RoomDetailViewModel
    {
        public RoomResponse Room { get; set; }
        public IEnumerable<Floor> Floors { get; set; }
        public IEnumerable<RoomTypeResponse> RoomTypes { get; set; }
        public IEnumerable<Service> Services { get; set; }
    }
}
