using Domain.DTO.Paging;

namespace Domain.DTO.RoomBooking
{
    public class RoomBookingGetRequestByCustomer : PagingRequest
    {
        public Guid? CustomerId { get; set; }
    }
}
