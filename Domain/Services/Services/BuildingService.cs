using Domain.DTO.Building;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly BuildingRepo _buildingRepo;
        private readonly IConfiguration _configuration;
        public BuildingService(IConfiguration configuration)
        {
            _configuration = configuration;
            _buildingRepo = new BuildingRepo(_configuration);
        }
        public Task<int> AddBuilding(BuildingCreateRequest request)
        {
            try
            {
                return _buildingRepo.AddBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> DeleteBuilding(BuildingDeleteRequest request)
        {
            try
            {
                return _buildingRepo.DeleteBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Building> GetBuildingById(Guid Id)
        {
            Building building = new();
            BuildingGetByIdRequest request = new BuildingGetByIdRequest()
            {
                Id = Id
            };
            try
            {
                DataTable table = await _buildingRepo.GetBuildingById(request);
                building = (from row in table.AsEnumerable()
                            select new Building
                            {
                                Id = row.Field<Guid>("Id"),
                                Name = row.Field<string>("Name"),
                                Status = row.Field<EntityStatus>("Status"),
                                CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                Deleted = row.Field<bool>("Deleted"),
                                DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                            }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return building;
        }

        public async Task<ResponseData<Building>> GetBuilding(BuildingGetRequest Search)
        {
            var model = new ResponseData<Building>();
            try
            {
                DataTable table = await _buildingRepo.GetBuilding(Search);
                model.data = (from row in table.AsEnumerable()
                              select new Building
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Name = row.Field<string>("Name"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = Search.PageIndex;
                model.PageSize = Search.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Search.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public Task<int> UpdateBuilding(BuildingUpdateRequest request)
        {
            try
            {
                return _buildingRepo.UpdateBuilding(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
