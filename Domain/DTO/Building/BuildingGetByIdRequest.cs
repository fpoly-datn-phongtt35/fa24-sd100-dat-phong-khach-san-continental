using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Building
{
    public class BuildingGetByIdRequest: PagingRequest
    {
        public Guid Id { get; set; }
    }
}
