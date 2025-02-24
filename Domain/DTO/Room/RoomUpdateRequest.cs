using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }
        public Guid? ImagesId { get; set; }
        public Guid? FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }

        public Models.Room ToRoom()
        {
            return new Models.Room()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Address = Address,
                Description = Description,
                RoomSize = RoomSize,
                ImagesId = ImagesId,
                FloorId = FloorId,
                RoomTypeId = RoomTypeId,
                Status = Status,
                ModifiedTime = ModifiedTime,
                ModifiedBy = ModifiedBy
            };
        }
    }
}
