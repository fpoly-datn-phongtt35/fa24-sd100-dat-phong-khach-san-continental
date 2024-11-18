using Domain.Enums;

namespace Domain.DTO.Role
{
    public class RoleCreateRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? RoleCode { get; set; } = string.Empty;
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
