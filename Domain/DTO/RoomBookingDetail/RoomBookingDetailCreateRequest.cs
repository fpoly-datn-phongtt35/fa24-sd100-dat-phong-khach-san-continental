﻿using Domain.DTO.Client;
using Domain.DTO.Customer;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.RoomBookingDetail
{
    public class RoomBookingDetailCreateRequest
    {
        public Guid RoomId { get; set; }
        public Guid RoomBookingId { get; set; }
        public DateTimeOffset? CheckInBooking { get; set; }
        public DateTimeOffset? CheckOutBooking { get; set; }
        public DateTimeOffset? CheckInReality { get; set; }
        public DateTimeOffset? CheckOutReality { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtraPrice { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Expenses { get; set; } = 0;
        public string? Note { get; set; }
        public RoomBookingStatus? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public List<Domain.Models.ServiceOrderDetail>? SelectedServices { get; set; }
        public ClientCreateCustomerRequest? Customer { get; set; }
        public long? ServicePrice { get; set; }
        public long? TotalPrice { get; set; }
    }
}
