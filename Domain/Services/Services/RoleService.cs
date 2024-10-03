using Domain.DTO.Paging;
using Domain.DTO.Role;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Domain.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleRepo _roleRepo;
        private readonly IConfiguration _configuration;
        public RoleService(IConfiguration configuration)
        {
            _configuration = configuration;
            _roleRepo = new RoleRepo(configuration);
        }
        public async Task<int> AddRole(RoleCreateRequest request)
        {
            try
            {
                return await _roleRepo.AddRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteRole(RoleDeleteRequest request)
        {
            try
            {
                return await _roleRepo.DeleteRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Role>> GetAllRole(RoleGetRequest Role)
        {
            var model = new ResponseData<Role>();
            try
            {
                DataTable dataTable = await _roleRepo.GetAllRole(Role);
                model.data = (from row in dataTable.AsEnumerable()
                              select new Role
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Name = row.Field<string>("Name"),
                                  RoleCode = row.Field<string>("RoleCode"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = Role.PageIndex;
                model.PageSize = Role.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Role.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<Role> GetRoleById(Guid Id)
        {
            Role Role = new Role();
            try
            {
                DataTable table = await _roleRepo.GetRoleById(Id);
                Role = (from row in table.AsEnumerable()
                        select new Role
                        {
                            Id = row.Field<Guid>("Id"),
                            Name = row.Field<string>("Name"),
                            RoleCode = row.Field<string>("RoleCode"),
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
            return Role;
        }

        public async Task<int> UpdateRole(RoleUpdateRequest request)
        {
            try
            {
                return await _roleRepo.UpdateRole(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
