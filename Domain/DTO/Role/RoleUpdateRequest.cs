using Domain.Enums;

namespace Domain.DTO.Role
{
    public class RoleUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? RoleCode { get; set; } = string.Empty;
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
