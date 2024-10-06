using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Floor
{
    public class FloorGetRequest : PagingRequest
    {
        public string? Name { get; set; }
        public Guid? BuildingId { get; set; }
        public int? NumberOfRoom { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
