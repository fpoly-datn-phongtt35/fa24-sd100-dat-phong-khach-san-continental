using Domain.DTO.Floor;
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
    public class FloorService : IFloorService
    {
        private readonly FloorRepo _floorRepo;
        private readonly IConfiguration _configuration;
        public FloorService(IConfiguration configuration)
        {
            _configuration = configuration;
            _floorRepo = new FloorRepo(_configuration);
        }
        public Task<int> AddFloor(FloorCreateRequest request)
        {
            try
            {
                return _floorRepo.AddFloor(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteFloor(FloorDeleteRequest request)
        {
            try
            {
                return await _floorRepo.DeleteFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Floor>> GetFloor(FloorGetRequest request)
        {
            var model = new ResponseData<Floor>();
            try
            {
                DataTable table = await _floorRepo.GetFloor(request);

                model.data = (from row in table.AsEnumerable()
                              select new Floor
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Name = row.Field<string>("Name"),
                                  NumberOfRoom = row.Field<int>("Number"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  BuildingId = row.Field<Guid>("BuildingId"),

                              }).ToList();

                //phân trang
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;

                try
                {
                    //chuyển đổi và gán giá trị tổng số bản ghi
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi total recod = 0
                    model.totalRecord = 0;
                }

                //tổng trang
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }


        public async Task<Floor> GetFloorById(Guid Id)
        {
            Floor floor = new();
            try
            {
                DataTable table = await _floorRepo.GetFloorById(Id);
                floor = (from row in table.AsEnumerable()
                           select new Floor
                           {
                               Id = row.Field<Guid>("Id"),
                               Name = row.Field<string>("Name"),
                               Status = row.Field<EntityStatus>("Status"),
                               CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                               CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty
                               ,
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
            return floor;
        }

        public Task<int> UpdateFloor(FloorUpdateRequest request)
        {
            try
            {
                return _floorRepo.UpdateFloor(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
