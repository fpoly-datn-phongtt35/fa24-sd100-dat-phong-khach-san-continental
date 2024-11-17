using Domain.DTO.Athorization;
using Domain.DTO.Client;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ICustomerService _CustomerRepo;
        public RegisterController(ICustomerService CustomerRepo)
        {
            _CustomerRepo = CustomerRepo;
        }
        [HttpPost]
        public async Task<ClientAuthenicationViewModel> Register(RegisterSubmitModel register)
        {
            try
            {
                return await _CustomerRepo.ClientRegister(register);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
