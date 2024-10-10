using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceType
{
    public class ServiceTypeGetRequest : PagingRequest
    {
        public string? Name { get; set; } = string.Empty;
    }
}
