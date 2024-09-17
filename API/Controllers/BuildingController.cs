using Domain.DTO.Building;
using Domain.DTO.Paging;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;
        public BuildingController(IBuildingService serviceBuilding)
        {
            _buildingService = serviceBuilding;
        }

        [HttpPost("CreateBuilding")]
        public async Task<int> CreateBuilding(BuildingCreateRequest request)
        {
            try
            {
                return await _buildingService.AddBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListBuilding")]
        public async Task<ResponseData<Building>> GetListBuilding(BuildingGetRequest request)
        {
            try
            {
                return await _buildingService.GetBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetBuildingById")]
        public async Task<Building> GetBuildingById(Guid Id)
        {
            try
            {
                return await _buildingService.GetBuildingById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateBuilding")]
        public async Task<int> UpdateBuilding(BuildingUpdateRequest request)
        {
            try
            {
                return await _buildingService.UpdateBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteBuilding")]
        public async Task<int> DeleteBuilding(BuildingDeleteRequest request)
        {
            try
            {
                return await _buildingService.DeleteBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
