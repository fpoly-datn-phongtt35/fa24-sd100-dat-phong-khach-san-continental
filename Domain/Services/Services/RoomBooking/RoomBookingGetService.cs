using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomBooking;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Services.Services.RoomBooking;

public class RoomBookingGetService : IRoomBookingGetService
{
    private readonly IRoomBookingRepository _roomBookingRepository;

    public RoomBookingGetService(IRoomBookingRepository roomBookingRepository)
    {
        _roomBookingRepository = roomBookingRepository;
    }

    public async Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBooking(RoomBookingGetRequest bookingGetRequest)
    {
        return await _roomBookingRepository.GetFilteredRoomBookings(bookingGetRequest);
    }

    public async Task<RoomBookingResponse?> GetRoomBookingById(Guid? roomBookingId)
    {
        if(roomBookingId == null) return null;
        var roomBooking = await _roomBookingRepository.GetRoomBookingById(roomBookingId.Value);
        if(roomBooking == null) return null;

        return roomBooking.ToRoomBookingResponse();
    }

    public static int GenerateOrderCode(Guid roomBookingId)
    {
        // Chuyển GUID thành chuỗi
        string input = roomBookingId.ToString();

        // Sử dụng SHA256 để băm
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Lấy 4 byte đầu (32 bit) và chuyển thành int
        int orderCode = BitConverter.ToInt32(hashBytes, 0); // Dùng ToInt32 để lấy 4 byte đầu tiên

        // Đảm bảo giá trị không âm
        return Math.Abs(orderCode);
    }

    public async Task<Guid?> GetRoomBookingIdByOrderCode(int orderCode)
    {
        try
        {
            var data = await _roomBookingRepository.GetFilteredRoomBookings(new RoomBookingGetRequest
            {
                SearchString = null,
                BookingType = null,
                Status = null,
                StaffId = null,
            });
            foreach (var roomBooking in data.data)
            {
                int calculatedOrderCode = GenerateOrderCode(roomBooking.Id);

                if (calculatedOrderCode == orderCode)
                {
                    return roomBooking.Id;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetRoomBookingIdByOrderCode: {ex.Message}");
            return null;
        }
    }

    public async Task<List<DateTimeOffset>> GetCheckinRoomBookingByRoomBookingId(Guid roomBookingId)
    {
        return await _roomBookingRepository.GetCheckinRoomBookingByRoomBookingId(roomBookingId);
    }
}