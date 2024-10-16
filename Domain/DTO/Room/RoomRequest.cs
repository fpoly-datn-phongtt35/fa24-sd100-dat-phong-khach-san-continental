using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomRequest : PagingRequest
    {
        public string? Name { get; set; }
        public Guid? FloorId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public RoomStatus? Status { get; set; }
    }
}
