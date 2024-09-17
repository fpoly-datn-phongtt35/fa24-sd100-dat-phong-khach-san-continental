using Domain.DTO.Building;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IBuildingRepo
    {
        Task<int> AddBuilding(BuildingCreateRequest request);
        Task<int> UpdateBuilding(BuildingUpdateRequest request);
        Task<int> DeleteBuilding(BuildingDeleteRequest request);
        Task<DataTable> GetBuilding(BuildingGetRequest Search);
    }
}
