using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IServiceService
    {
        Task<int> AddService(ServiceCreateRequest request);
        Task<int> UpdateService(ServiceUpdateRequest request);
        Task<int> DeleteService(ServiceDeleteRequest request);
        Task<Service> GetServiceById(Guid Id);
        Task<ResponseData<Service>> GetServices(ServiceGetRequest request);
    }
}
