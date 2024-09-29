using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostTypeId { get; set; }
        public Guid StaffId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public PostType PostType { get; set; }
        public Staff Staff { get; set; }
    }
}
