﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.VoucherDetail
{
    public class VoucherDetailCreateRequest
    {
        public Guid RoomBookingId { get; set; }
        public Guid VoucherId { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public EntityStatus? Status { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
