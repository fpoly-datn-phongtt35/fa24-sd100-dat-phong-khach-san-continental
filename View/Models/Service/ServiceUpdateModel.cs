using Domain.Enums;

namespace View.Models.Service
{
    public class ServiceUpdateModel
    {
        public Guid Id { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid UnitId { get; set; }
        public EntityStatus Status { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }

        public List<IFormFile>? NewImages { get; set; }  
        public List<string>? ExistingImages { get; set; }
        public List<string>? DeletedImages { get; set; } = new();
        public List<string>? Images { get; set; } = new();
    }
}
