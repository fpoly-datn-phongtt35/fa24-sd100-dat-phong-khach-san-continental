using Domain.DTO.Floor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IFloorRepo
    {
        Task<int> AddFloor(FloorCreateRequest request);
        Task<int> UpdateFloor(FloorUpdateRequest request);
        Task<int> DeleteFloor(FloorDeleteRequest request);
        Task<DataTable> GetFloor(FloorGetRequest request);
    }
}
