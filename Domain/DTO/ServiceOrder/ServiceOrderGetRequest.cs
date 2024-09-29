using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ServiceOrder
{
    public class ServiceOrderGetRequest : PagingRequest
    {
        public Guid? RoomBookingId { get; set; }
    }
}
