using Domain.Enums;

namespace Domain.DTO.Policy
{
    public class PolicyCreateRequest
    {
        public Guid PolicyTypeId { get; set; }
        public Guid StaffId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
