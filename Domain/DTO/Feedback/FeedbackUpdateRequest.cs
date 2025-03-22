using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Feedback
{
    public class FeedbackUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Comments { get; set; }
        public int? Rating { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public bool? Deleted { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; } = DateTimeOffset.UtcNow;
        public Guid? ModifiedBy { get; set; }
    }
}
