using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrderDetail
{
    public class ServiceOrderDetailUpdateRequest
    {
        public Guid Id { get; set; }
        public Guid ServiceOrderId { get; set; }
        public Guid ServiceId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public EntityStatus Status { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
