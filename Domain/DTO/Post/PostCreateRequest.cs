using Domain.Enums;

namespace Domain.DTO.Post
{
    public class PostCreateRequest
    {
        public Guid PostTypeId { get; set; }
        public Guid StaffId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
