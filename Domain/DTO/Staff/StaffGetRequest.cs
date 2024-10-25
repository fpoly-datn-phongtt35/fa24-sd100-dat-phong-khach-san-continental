using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Staff
{
    public class StaffGetRequest : PagingRequest
    {
        public string? search { get; set; } 
    }
}
