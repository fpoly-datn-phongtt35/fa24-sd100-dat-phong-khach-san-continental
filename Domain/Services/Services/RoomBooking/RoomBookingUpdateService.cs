
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomBooking;

namespace Domain.Services.Services.RoomBooking;

public class RoomBookingUpdateService : IRoomBookingUpdateService
{
    private readonly IRoomBookingRepository _roomBookingRepository;

    public RoomBookingUpdateService(IRoomBookingRepository roomBookingRepository)
    {
        _roomBookingRepository = roomBookingRepository;
    }

    public async Task<int> CheckDepositRoomBooking()
    {
        try
        {
           return await _roomBookingRepository.CheckDepositRoomBooking();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task<RoomBookingResponse?> UpdateRoomBookingAsync(RoomBookingUpdateRequest roomBookingUpdateRequest)
    {
        if (roomBookingUpdateRequest is null)
            throw new ArgumentNullException(nameof(roomBookingUpdateRequest));

        var existingRoomBooking = await _roomBookingRepository
            .GetRoomBookingById(roomBookingUpdateRequest.Id);
        if (existingRoomBooking is null)
            throw new Exception("Room booking not found");

        existingRoomBooking.Status = roomBookingUpdateRequest.Status;
        existingRoomBooking.ModifiedTime = roomBookingUpdateRequest.ModifiedTime;
        existingRoomBooking.ModifiedBy = roomBookingUpdateRequest.ModifiedBy;
        existingRoomBooking.TotalServicePrice = roomBookingUpdateRequest.TotalServicePrice;
        existingRoomBooking.TotalPrice = roomBookingUpdateRequest.TotalPrice;
        existingRoomBooking.TotalExtraPrice = roomBookingUpdateRequest.TotalExtraPrice;
        existingRoomBooking.TotalExpenses = roomBookingUpdateRequest.TotalExpenses;
        existingRoomBooking.TotalPriceReality = roomBookingUpdateRequest.TotalPriceReality;

        await _roomBookingRepository.UpdateRoomBooking(existingRoomBooking);
        return existingRoomBooking.ToRoomBookingResponse();
    }

    public async Task<int> UpdateRoomBookingPrice(Guid id)
    {
        try
        {
            return await _roomBookingRepository.UpdateRoomBookingPrice(id);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Task<int> UpdateRoomBookingStatus(Guid id, int status)
    {
        try
        {
            return _roomBookingRepository.UpdateRoomBookingStatus(id, status);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
