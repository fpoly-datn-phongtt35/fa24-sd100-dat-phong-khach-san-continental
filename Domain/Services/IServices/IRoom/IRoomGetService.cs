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
        Task<bool> CheckedAvailableRooms(List<Guid> LstRoomId /*đây là list room hiện tại muốn đặt*/,
            SearchRoomsRequest request);
        //Task<RoomResponse?> GetRoomTypeWithAmenityRoomById(Guid roomId);
        Task<bool> CheckedAvailableRooms2(List<Guid> LstRoomId, RoomAvailableRequest request);
        Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request);
        Task<List<TopRoomBookingViewModel>> GetTopBookingRoomsAsync(int SelectedMonthRoom, int SelectedYearRoom);
        Task<List<TopCustomerBooking>> GetTopCustomerBookings(int SelectedMonthCustomer, int SelectedYearCustomer);
        Task<List<GetRevenue>> GetRevenueAsync(string revenueFilterType);
        Task<List<MonthlyCoverageDto>> GetMonthlyCoverage();
        Task<List<WeeklyCoverageDto>> GetWeeklyCoverage();
        Task<List<TopBookedRoom>> GetTop3MostBookedRoomsAsync();
        Task<HotelInfoDto> HotelInfo();
    }
}
