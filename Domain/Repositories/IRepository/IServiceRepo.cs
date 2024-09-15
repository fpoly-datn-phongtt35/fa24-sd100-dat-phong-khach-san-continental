using Domain.DTO.Service;
using Domain.DTO.ServiceType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IServiceRepo
    {
        Task<int> AddService(ServiceCreateRequest request);
        Task<int> UpdateService(ServiceUpdateRequest request);
        Task<int> DeleteService(ServiceDeleteRequest request);
        Task<DataTable> GetServices(ServiceGetRequest request);
    }
}
