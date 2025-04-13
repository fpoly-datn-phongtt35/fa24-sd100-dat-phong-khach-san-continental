namespace Domain.DTO.Room
{
    public class RoomWithNiceViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public double RoomSize { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
