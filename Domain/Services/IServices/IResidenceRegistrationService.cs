using Azure;
using Domain.DTO.Paging;
using Domain.DTO.ResidenceRegistration;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IResidenceRegistrationService
    {
        Task<int> AddResidence(ResidenceAddRequest request);
        Task<int> UpdateResidence(ResidenceUpdateRequest request);
        Task<int> DeleteResidence(Guid id);
        Task<int> CheckOut1Residence(Guid id);
        Task<int> CheckOutResidenceByRBD(Guid roomBookingDetailId, DateTimeOffset checkOutTime);
        Task<ResponseData<ResidenceRegistration>> GetResidences(ResidenceGetRequest request);
        Task<ResponseData<ResidenceResponse>> GetResidencesByDate(ResidenceGetByDateRequest request);
        Task<ResidenceRegistration> GetResidenceById(Guid id);
        Task<ResponseData<ResidenceRegistration>> GetResidenceByRoomBookingDetailId(Guid roomBookingDetailId);
        Task<int> GetMaximumOccupancyByRoomBookingDetailId(Guid roomBookingDetailId);
    }
}
