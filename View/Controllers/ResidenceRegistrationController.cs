using Domain.DTO.ResidenceRegistration;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class ResidenceRegistrationController : Controller
    {
        private readonly IResidenceRegistrationService _residenceRegistrationService;

        public ResidenceRegistrationController(IResidenceRegistrationService residenceRegistrationService)
        {
            _residenceRegistrationService = residenceRegistrationService;
        }

        public async Task<List<ResidenceRegistration>> GetResidenceRegistrationByRoomBookingDetailId(Guid Id)
        {
            try
            {
                var model = await _residenceRegistrationService.GetResidenceByRoomBookingDetailId(Id);
                return model.data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> GetMaximumOccupancyByRoomBookingDetailId(Guid Id)
        {
            try
            {
                var result = await _residenceRegistrationService.GetMaximumOccupancyByRoomBookingDetailId(Id);
                return Ok(new { maximumOccupancy = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddResidenceRegistration(ResidenceAddRequest request)
        {
            var obj = new ResidenceAddRequest
            {
                RoomBookingDetailId = request.RoomBookingDetailId,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                IdentityNumber = request.IdentityNumber,
                PhoneNumber = request.PhoneNumber,
                //CreatedBy = null,
                //CreatedTime = DateTimeOffset.Now
            };
            try
            {
                var result = await _residenceRegistrationService.AddResidence(obj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteResidenceRegistration(Guid id)
        {
            try
            {
                var result = await _residenceRegistrationService.DeleteResidence(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditResidenceRegistration(ResidenceUpdateRequest request)
        {
            try
            {
                var result = await _residenceRegistrationService.UpdateResidence(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
