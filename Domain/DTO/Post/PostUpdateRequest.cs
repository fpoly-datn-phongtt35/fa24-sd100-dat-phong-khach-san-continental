using Domain.Enums;

namespace Domain.DTO.Post
{
    public class PostUpdateRequest
    {
        public Guid Id { get; set; }
        public Guid PostTypeId { get; set; }
        public Guid? StaffId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
