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
using Domain.DTO.PolicyType;

namespace Domain.Services.Services
{
    public class PolicyTypeService : IPolicyTypeService
    {
        private readonly PolicyTypeRepo _policyTypeRepo;
        private readonly IConfiguration _configuration;
        public PolicyTypeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _policyTypeRepo = new PolicyTypeRepo(configuration);
        }
        public async Task<int> AddPolicyType(PolicyTypeCreateRequest request)
        {
            try
            {
                return await _policyTypeRepo.AddPolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeletePolicyType(PolicyTypeDeleteRequest request)
        {
            try
            {
                return await _policyTypeRepo.DeletePolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<PolicyType>> GetAllPolicyType(PolicyTypeGetRequest policyType)
        {
            var model = new ResponseData<PolicyType>();
            try
            {
                DataTable dataTable = await _policyTypeRepo.GetAllPolicyType(policyType);
                model.data = (from row in dataTable.AsEnumerable()
                              select new PolicyType
                              {
                                  Id = row.Field<Guid>("Id"),
                                  TitleOfType = row.Field<string>("TitleOfType"),
                                  Content = row.Field<string>("Content"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = policyType.PageIndex;
                model.PageSize = policyType.PageSize;
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
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / policyType.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<PolicyType> GetPolicyTypeById(Guid Id)
        {
            PolicyType policyType = new();
            try
            {
                DataTable table = await _policyTypeRepo.GetPolicyTypeById(Id);
                policyType = (from row in table.AsEnumerable()
                            select new PolicyType
                            {
                                Id = row.Field<Guid>("Id"),
                                TitleOfType = row.Field<string>("TitleOfType"),
                                Content = row.Field<string>("Content"),
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
            return policyType;
        }

        public async Task<int> UpdatePolicyType(PolicyTypeUpdateRequest request)
        {
            try
            {
                return await _policyTypeRepo.UpdatePolicyType(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
