using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Service
{
    public class ServiceTypeGroupDto
    {
        public string ServiceTypeName { get; set; }
        public List<Guid> ServiceIds { get; set; }
        public List<string> ServiceNames { get; set; } 
    }
}
