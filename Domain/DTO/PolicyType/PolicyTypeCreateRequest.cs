using Domain.Enums;

namespace Domain.DTO.PolicyType
{
    public class PolicyTypeCreateRequest
    {
        public string? TitleOfType { get; set; }
        public string? Content { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
