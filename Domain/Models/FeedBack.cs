using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FeedBack
    {
        [Key]
        public Guid Id  { get; set; }
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public User User { get; set; }
        public Customer Customer { get; set; }
        public Bill Bill { get; set; }
    }
}
