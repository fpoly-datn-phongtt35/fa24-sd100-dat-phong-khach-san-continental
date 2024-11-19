using Domain.DTO.RoomBooking;
using Microsoft.AspNetCore.Mvc;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoombooking
    {
        Task<Guid> CreateRoomBooking(RoomBookingCreateRequestForCustomer request);
    }
}
