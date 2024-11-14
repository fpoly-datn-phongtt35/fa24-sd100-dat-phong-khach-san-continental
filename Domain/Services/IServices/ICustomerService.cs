using Domain.DTO.Athorization;
using Domain.DTO.Client;
using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.ServiceType;
using Domain.Models;
using System.Data;

namespace Domain.Services.IServices
{
    public interface ICustomerService
    {
        Task<int> AddCustomer(CustomerCreateRequest request);
        Task<int> UpdateCustomer(CustomerUpdateRequest request);
        Task<int> DeleteCustomer(CustomerDeleteRequest request);
        Task<ResponseData<Customer>> GetAllCustomer(CustomerGetRequest customer);
        Task<Customer> GetCustomerById(Guid Id);
        Task<ClientAuthenicationViewModel> ClientLogin(LoginSubmitModel request);
    }
}
