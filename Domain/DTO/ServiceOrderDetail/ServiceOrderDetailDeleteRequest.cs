using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrderDetail
{
    public class ServiceOrderDetailDeleteRequest
    {
        public Guid Id { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
