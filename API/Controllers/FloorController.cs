using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Service;
using Domain.Models;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorservice;

        public FloorController(IFloorService floorservice)
        {
            _floorservice = floorservice;
        }

        [HttpPost("CreateFloor")]
        public async Task<int> CreateFloor(FloorCreateRequest request)
        {
            try
            {
                return await _floorservice.AddFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetListFloor")]
        public async Task<ResponseData<Floor>> GetListFloor(FloorGetRequest request)
        {
            try
            {
                return await _floorservice.GetFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetFloorById")]
        public async Task<Floor> GetFloorById(Guid Id)
        {
            try
            {
                return await _floorservice.GetFloorById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPut("UpdateFloor")]
        public async Task<int> UpdateFloor(FloorUpdateRequest request)
        {
            try
            {
                return await _floorservice.UpdateFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteFloor")]
        public async Task<int> DeleteFloor([FromBody] FloorDeleteRequest request)
        {
            try
            {
                return await _floorservice.DeleteFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
