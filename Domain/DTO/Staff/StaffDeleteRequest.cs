using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Staff
{
    public class StaffDeleteRequest
    {
        public Guid Id { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
