using Domain.DTO.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ResidenceRegistration
{
    public class ResidenceGetRequest : PagingRequest
    {
        public Guid? Id { get; set; }
        public Guid? RoomBookingDetailId { get; set; }
        public string? FullName { get; set; }
    }
}
