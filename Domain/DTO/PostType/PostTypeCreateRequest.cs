using Domain.Enums;

namespace Domain.DTO.PostType
{
    public class PostTypeCreateRequest
    {
        public string TitleOfType { get; set; }
        public string Content { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
