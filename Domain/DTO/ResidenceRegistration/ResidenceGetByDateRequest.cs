using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ResidenceRegistration
{
    public class ResidenceGetByDateRequest : PagingRequest
    {
        public DateTime? Date { get; set; }
        public string? FullName { get; set; }
        public string? IdentityNumber { get; set; }
        public string? RoomName { get; set; }
        public bool? IsCheckOut { get; set; }
    }
}
