using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceType
{
    public class ServiceTypeGetByIdRequest : PagingRequest
    {
        public Guid Id { get; set; }
    }
}
