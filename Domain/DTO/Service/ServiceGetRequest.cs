using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Service
{
    public class ServiceGetRequest : PagingRequest
    {
        public string? Name { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
