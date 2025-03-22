using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Feedback
{
    public class FeedbackCreateRequest
    {
        public Guid RoomBookingDetailId { get; set; }
        public string? Comments { get; set; }
        public int? Rating { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public DateTimeOffset? CreatedTime { get; set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedBy { get; set; }
    }
}
