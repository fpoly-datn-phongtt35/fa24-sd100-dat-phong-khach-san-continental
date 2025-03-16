using Domain.Enums;

namespace View.Models.Room
{
    public class RoomCreateViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double? RoomSize { get; set; }
        public Guid? FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }
}
