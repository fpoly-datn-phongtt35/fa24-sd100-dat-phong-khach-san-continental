using Domain.DTO.RoomBookingDetail;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoomBookingDetail
    {
        Task<Guid> CreateRoomBookingDetail(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
        Task<List<RoomBookingDetailGetByIdRoomBooking>> GetRBDWithoutComments(Guid id);
    }
}
