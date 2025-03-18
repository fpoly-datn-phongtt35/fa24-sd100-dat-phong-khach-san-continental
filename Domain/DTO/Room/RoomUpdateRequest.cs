using Domain.Enums;
using Domain.Models;
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
        public List<string> Images { get; set; } = new();
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
                FloorId = FloorId,
                RoomTypeId = RoomTypeId,
                Status = Status,
                ModifiedTime = ModifiedTime,
                ModifiedBy = ModifiedBy,
                Images = Images.Select(imageUrl => new Images { Image = imageUrl }).ToList() // Ánh xạ List<string> thành List<Images>
            };
        }
    }

}
