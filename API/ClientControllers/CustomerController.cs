using Domain.DTO.Athorization;
using Domain.DTO.Client;
using Domain.DTO.Customer;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _CustomerRepo;
        public CustomerController(ICustomerService CustomerRepo)
        {
            _CustomerRepo = CustomerRepo;
        }
        [HttpPost("ClientCreateCustomer")]
        public async Task<ClientInsertCustomerViewModel> ClientCreateCustomer(ClientCreateCustomerRequest request)
        {
            try
            {
                return await _CustomerRepo.ClientInsertCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
