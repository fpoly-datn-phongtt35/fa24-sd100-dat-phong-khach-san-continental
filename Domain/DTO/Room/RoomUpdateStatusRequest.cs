using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomUpdateStatusRequest
    {
        public Guid Id { get; set; }
        public RoomStatus Status { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
