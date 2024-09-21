using Domain.DTO.ServiceType;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IServiceTypeRepo
    {
        Task<int> AddServiceType(ServiceTypeCreateRequest request);
        Task<int> UpdateServiceType(ServiceTypeUpdateRequest request);
        Task<int> DeleteServiceType(ServiceTypeDeleteRequest request);
        Task<DataTable> GetServiceTypes(ServiceTypeGetRequest Search);
    }
}
