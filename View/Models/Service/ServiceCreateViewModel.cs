using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace View.Models.Service
{
    public class ServiceCreateViewModel
    {
        public Guid ServiceTypeId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public Guid UnitId { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }
}
