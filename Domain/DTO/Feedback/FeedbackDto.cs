using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Feedback
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public Guid RoomBookingDetailId { get; set; }
        public string? Comments { get; set; }
        public int? Rating { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public Guid RoomId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
