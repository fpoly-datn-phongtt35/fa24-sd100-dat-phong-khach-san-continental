using Domain.Enums;
using Domain.Models;

namespace View.Models.Room
{
    public class RoomUpdateViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }
        public Guid? FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public List<string>? ExistingImages { get; set; }
        public List<string>? DeletedImages { get; set; } = new();
        public List<string>? Images { get; set; } = new();
    }
}
