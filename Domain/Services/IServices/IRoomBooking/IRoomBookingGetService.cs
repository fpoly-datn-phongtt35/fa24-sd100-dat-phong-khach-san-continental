﻿using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace Domain.Services.IServices.IRoomBooking;

public interface IRoomBookingGetService
{
    Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBooking
        (RoomBookingGetRequest bookingGetRequest);
    Task<RoomBookingResponse?> GetRoomBookingById(Guid? roomBookingId);
    Task<Guid?> GetRoomBookingIdByOrderCode(int orderCode);
    Task<List<DateTimeOffset>> GetCheckinRoomBookingByRoomBookingId(Guid roomBookingId);
    Task<ResponseData<RoomBookingResponseForCustomer>> GetListRoomBookingByCustomerId(RoomBookingGetRequestByCustomer request);
}