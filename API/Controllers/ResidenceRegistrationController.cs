using Domain.DTO.Paging;
using Domain.DTO.ResidenceRegistration;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenceRegistrationController : ControllerBase
    {
        private readonly IResidenceRegistrationService _residenceRegistrationService;

        public ResidenceRegistrationController(IResidenceRegistrationService residenceRegistrationService)
        {
            _residenceRegistrationService = residenceRegistrationService;
        }
        [HttpPost("CreateResidenceRegistration")]
        public async Task<int> CreateResidenceRegistration(ResidenceAddRequest request)
        {
            try
            {
                return await _residenceRegistrationService.AddResidence(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetResidenceRegistrations")]
        public async Task<ResponseData<ResidenceRegistration>> GetResidenceRegistrations(ResidenceGetRequest request)
        {
            try
            {
                return await _residenceRegistrationService.GetResidences(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetResidenceRegistrationById")]
        public async Task<ResidenceRegistration> GetResidenceRegistrationById(Guid Id)
        {
            try
            {
                return await _residenceRegistrationService.GetResidenceById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("GetResidenceRegistrationByRoomBookingDetailId")]
        public async Task<ResponseData<ResidenceRegistration>> GetResidenceRegistrationByRoomBookingDetailId(Guid Id)
        {
            try
            {
                return await _residenceRegistrationService.GetResidenceByRoomBookingDetailId(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateResidenceRegistration")]
        public async Task<int> UpdateResidenceRegistration(ResidenceUpdateRequest request)
        {
            try
            {
                return await _residenceRegistrationService.UpdateResidence(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("DeleteResidenceRegistration")]
        public async Task<int> DeleteResidenceRegistration(Guid id)
        {
            try
            {
                return await _residenceRegistrationService.DeleteResidence(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
