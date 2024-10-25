using Domain.Enums;

namespace Domain.DTO.PostType
{
    public class PostTypeUpdateRequest
    {
        public Guid Id { get; set; }
        public string TitleOfType { get; set; }
        public string Content { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
