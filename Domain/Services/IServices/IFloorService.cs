using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IFloorService
    {
        Task<int> AddFloor(FloorCreateRequest request);
        Task<int> UpdateFloor(FloorUpdateRequest request);
        Task<int> DeleteFloor(FloorDeleteRequest request);
        Task<Floor> GetFloorById(Guid Id);
        Task<ResponseData<Floor>> GetFloor(FloorGetRequest request);
    }
}
