using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrderDetail
{
    public class ServiceOrderDetailCreateRequest
    {
        public Guid RoomBookingId { get; set; }
        public Guid ServiceId { get; set; }
        public int? Amount { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public EntityStatus? Status { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
