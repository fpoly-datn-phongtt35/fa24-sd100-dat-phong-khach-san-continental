using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Client
{
    public class ClientInsertCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Messenger { get; set; }
    }
}
