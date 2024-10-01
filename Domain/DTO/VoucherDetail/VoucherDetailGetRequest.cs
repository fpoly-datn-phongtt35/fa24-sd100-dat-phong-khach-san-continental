using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.VoucherDetail
{
    public class VoucherDetailGetRequest : PagingRequest
    {
        public Guid? RoomBookingId { get; set; }
        public Guid? VoucherId { get; set; }
    }
}
