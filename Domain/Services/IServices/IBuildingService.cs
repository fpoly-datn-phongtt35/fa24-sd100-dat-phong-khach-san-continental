using Domain.DTO.Building;
using Domain.DTO.Paging;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IBuildingService
    {
        Task<int> AddBuilding(BuildingCreateRequest request);
        Task<int> UpdateBuilding(BuildingUpdateRequest request);
        Task<int> DeleteBuilding(BuildingDeleteRequest request);
        Task<Building> GetBuildingById(Guid Id);
        Task<ResponseData<Building>> GetBuilding(BuildingGetRequest Search);
    }
}
