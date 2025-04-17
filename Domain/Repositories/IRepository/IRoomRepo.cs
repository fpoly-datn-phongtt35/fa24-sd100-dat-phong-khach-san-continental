using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IRoomRepo
    {
        Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomResponse);
        Task<Room?> GetRoomById(Guid RoomId);
        Task<RoomAvailableResponse> GetAvailableRooms(RoomAvailableRequest roomRequest);
        Task<bool> CheckedAvailableRooms(List<Guid> LstRoomId /*đây là list room hiện tại muốn đặt*/,
            SearchRoomsRequest request);
        Task<Room> AddRoom(Room Room);
        Task<bool> CheckedAvailableRooms2(List<Guid> LstRoomId /*đây là list room hiện tại muốn đặt*/,
            RoomAvailableRequest request);
        Task<Room?> UpdateRoom(Room Room);
        Task<int> UpdateRoomStatus(RoomUpdateStatusRequest request);
        Task<Room?> DeleteRoom(Room Room);
        Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request);
        Task<List<TopRoomBookingViewModel>> GetTopBookingRoomsAsync(int SelectedMonthRoom, int SelectedYearRoom);
        Task<List<TopCustomerBooking>> GetTopCustomerBookings(int SelectedMonthCustomer, int SelectedYearCustomer);
        Task<List<GetRevenue>> GetRevenueAsync(string revenueFilterType);
        Task<List<MonthlyCoverageDto>> GetMonthlyCoverage();
        Task<List<WeeklyCoverageDto>> GetWeeklyCoverage();

        Task<List<TopBookedRoom>> GetTop3MostBookedRoomsAsync();
        Task<HotelInfoDto> HotelInfo();

        Task<List<RoomWithNiceViewDto>> GetRoomsWithNiceViewAsync();
        Task<RoomRatingDto> GetAverageRatingByRoomIdAsync(Guid roomId);
    }
}
