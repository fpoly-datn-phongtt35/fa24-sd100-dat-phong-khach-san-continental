using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrderDetail
{
    public class ServiceOrderDetailGetRequest : PagingRequest
    {
        public Guid? ServiceOrderId { get; set; }
    }
}
