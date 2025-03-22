using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Feedback
{
    public class FeedbackGetRequest : PagingRequest
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? RoomId { get; set; }
        public int? Rating { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
