using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomCreateRequest
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }

        public Guid FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;
        public List<string> Images { get; set; } = new();
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
                FloorId = FloorId,
                RoomTypeId = RoomTypeId,
                Status = Status,
                CreatedTime = CreatedTime,
                CreatedBy = CreatedBy,
                Images = Images.Select(imageUrl => new Images { Image = imageUrl }).ToList() // Ánh xạ List<string> thành List<Images>
            };
        }
    }
}
