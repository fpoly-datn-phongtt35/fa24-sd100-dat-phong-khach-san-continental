using Domain.DTO.Athorization;
using Domain.DTO.Client;
using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICustomerService _CustomerRepo;
        public LoginController(ICustomerService CustomerRepo)
        {
            _CustomerRepo = CustomerRepo;
        }
        [HttpPost]
        public async Task<ClientAuthenicationViewModel> Login(LoginSubmitModel request)
        {
            try
            {
                return await _CustomerRepo.ClientLogin(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
