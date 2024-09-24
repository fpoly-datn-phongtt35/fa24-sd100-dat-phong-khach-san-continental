using Domain.DTO.Paging;
using Domain.DTO.ServiceType;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IServiceTypeService
    {
        Task<int> AddServiceType(ServiceTypeCreateRequest request);
        Task<int> UpdateServiceType(ServiceTypeUpdateRequest request);
        Task<int> DeleteServiceType(ServiceTypeDeleteRequest request);
        Task<ServiceType> GetServiceTypeById(Guid Id);
        Task<ResponseData<ServiceType>> GetServiceTypes(ServiceTypeGetRequest Search);
    }
}
