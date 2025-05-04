using Domain.Enums;

namespace View.Models.NewFolder
{
    public class ServiceTypeModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public EntityStatus Status { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
