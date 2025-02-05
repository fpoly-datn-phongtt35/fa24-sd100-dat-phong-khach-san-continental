﻿
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices.IRoom
{
    public interface IRoomGetService
    {
        Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest);
        Task<RoomResponse?> GetRoomById(Guid roomId);
        Task<RoomAvailableResponse> GetAvailableRooms(RoomAvailableRequest roomRequest);
        //Task<RoomResponse?> GetRoomTypeWithAmenityRoomById(Guid roomId);
        Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request);
        Task<List<TopRoomBookingViewModel>> GetTopBookingRoomsAsync(int SelectedMonthRoom, int SelectedYearRoom);
        Task<List<TopCustomerBooking>> GetTopCustomerBookings(int SelectedMonthCustomer, int SelectedYearCustomer);
        Task<List<GetRevenue>> GetRevenueAsync(string revenueFilterType);
        Task<float> GetCoverageRatio(int month, int year);  
    }
}
