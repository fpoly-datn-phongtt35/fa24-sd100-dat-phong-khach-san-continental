using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomCreateRequest
    {
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public Guid FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public Models.Room ToRoom()
        {
            return new Models.Room()
            {
                Name = Name,
                Price = Price,
                Address = Address,
                Description = Description,
                RoomSize = RoomSize,
                Images = Images,
                FloorId = FloorId,
                RoomTypeId = RoomTypeId,
                Status = Status,
                CreatedTime = CreatedTime,
                CreatedBy = CreatedBy
            };
            
        }
    }
}
