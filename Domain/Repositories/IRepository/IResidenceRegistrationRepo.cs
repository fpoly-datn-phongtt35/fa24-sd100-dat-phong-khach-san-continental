using Domain.DTO.ResidenceRegistration;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IResidenceRegistrationRepo
    {
        Task<int> AddResidence(ResidenceAddRequest request);
        Task<int> UpdateResidence(ResidenceUpdateRequest request);
        Task<int> DeleteResidence(Guid id);
        Task<DataTable> GetResidences(ResidenceGetRequest request);
        Task<DataTable> GetResidencesByDate(ResidenceGetByDateRequest request);
        Task<DataTable> GetResidenceById(Guid id);
        Task<int> GetMaximumOccupancyByRoomBookingDetailId(Guid roomBookingDetailId);
        Task<int> CheckOut1Residence(Guid id);  
        Task<int> CheckOutByRBD(Guid roomBookingDetailId);
    }
}
