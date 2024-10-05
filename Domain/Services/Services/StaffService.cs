using Domain.DTO.Paging;
using Domain.DTO.Staff;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class StaffService : IStaffService
    {
        private readonly StaffRepository _staffRepo;
        private readonly IConfiguration _configuration;
        public StaffService(IConfiguration configuration)
        {
            _configuration = configuration;
            _staffRepo = new StaffRepository(_configuration);
        }

        public Task<int> AddStaff(StaffCreateRequest request)
        {
            try
            {
                return _staffRepo.AddStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> DeleteStaff(StaffDeleteRequest request)
        {
            try
            {
                return _staffRepo.DeleteStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StaffUpdateRequest> GetStaffbyId(Guid id)
        {
            var Data = new StaffUpdateRequest();
            try
            {
                DataTable dt = await _staffRepo.GetStaffId(id);
                if (dt != null & dt.Rows.Count > 0) 
                {
                    Data = (from row in dt.AsEnumerable()
                                select new StaffUpdateRequest
                                {
                                    Id = row.Field<Guid>("Id"),
                                    UserName = row.Field<string>("UserName"),
                                    Password = row.Field<string>("Password"),
                                    FirstName = row.Field<string>("FirstName"),
                                    LastName = row.Field<string>("LastName"),
                                    Email = row.Field<string>("Email"),
                                    PhoneNumber = row.Field<string>("PhoneNumber"),
                                    RoleId = row.Field<Guid>("RoleId"),
                                    Status = (EntityStatus)row.Field<int>("Status"),
                                }).FirstOrDefault();
                }
                return Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Staff>> GetStaffs(StaffGetRequest Search)
        {
            var model = new ResponseData<Staff>();
            try
            {
                DataTable table = await _staffRepo.GetStaff(Search);

                model.data = (from row in table.AsEnumerable()
                              select new Staff
                              {
                                  Id = row.Field<Guid>("Id"),
                                  UserName = row.Field<string>("UserName"),
                                  Password = row.Field<string>("Password"),
                                  FirstName = row.Field<string>("FirstName"),
                                  LastName = row.Field<string>("LastName"),
                                  Email = row.Field<string>("Email"),
                                  PhoneNumber = row.Field<string>("PhoneNumber"),
                                  RoleId = row.Field<Guid>("RoleId"),
                                  Status = (EntityStatus)row.Field<int>("Status"),
                              }).ToList();

                //phân trang
                model.CurrentPage = Search.PageIndex;
                model.PageSize = Search.PageSize;

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
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Search.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public Task<int> UpdateStaff(StaffUpdateRequest request)
        {
            try
            {
                return _staffRepo.UpdateStaff(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
