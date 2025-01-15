using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrderDetail
{
    public class ServiceOrderDetailResponse
    {
        public Guid Id { get; set; }
        public Guid RoomBookingId { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public double? Amount { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public Guid ServiceId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        public string? StatusName {  get; set; } 
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
