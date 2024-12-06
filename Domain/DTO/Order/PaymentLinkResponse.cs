using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Order
{
    public class PaymentLinkResponse
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
