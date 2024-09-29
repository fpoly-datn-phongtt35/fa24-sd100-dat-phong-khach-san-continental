using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrder
{
    public class ServiceOrderUpdateRequest
    {
        public Guid Id { get; set; }
        public Guid? RoomBookingId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        public bool Deleted { get; set; } = false;
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
