using Domain.DTO.Paging;
using Domain.DTO.Policy;
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
    public class PolicyService : IPolicyService
    {
        private readonly PolicyRepo _policyRepo;
        private readonly IConfiguration _configuration;
        public PolicyService(IConfiguration configuration)
        {
            _configuration = configuration;
            _policyRepo = new PolicyRepo(configuration);
        }
        public async Task<int> AddPolicy(PolicyCreateRequest request)
        {
            try
            {
                return await _policyRepo.AddPolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeletePolicy(PolicyDeleteRequest request)
        {
            try
            {
                return await _policyRepo.DeletePolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Policy>> GetAllPolicy(PolicyGetRequest request)
        {
            var model = new ResponseData<Policy>();
            try
            {
                DataTable dataTable = await _policyRepo.GetAllPolicy(request);
                model.data = (from row in dataTable.AsEnumerable()
                              select new Policy
                              {
                                  Id = row.Field<Guid>("Id"),
                                  Title = row.Field<string>("Title"),
                                  Content = row.Field<string>("Content"),
                                  StaffId = row.Field<Guid>("StaffId"),
                                  PolicyTypeId = row.Field<Guid>("PolicyTypeId"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
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
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public Task<List<PolicyTermsDto>> GetAllPolicyTerms()
        {
            try
            {
                return _policyRepo.GetAllPolicyTerms();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error", ex);
            }
        }

        public async Task<Policy> GetPolicyById(Guid Id)
        {
            Policy Policy = new();
            try
            {
                DataTable table = await _policyRepo.GetPolicyById(Id);
                Policy = (from row in table.AsEnumerable()
                            select new Policy
                            {
                                Id = row.Field<Guid>("Id"),
                                Title = row.Field<string>("Title"),
                                Content = row.Field<string>("Content"),
                                StaffId = row.Field<Guid>("StaffId"),
                                PolicyTypeId = row.Field<Guid>("PolicyTypeId"),
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
            return Policy;
        }

        public async Task<int> UpdatePolicy(PolicyUpdateRequest request)
        {
            try
            {
                return await _policyRepo.UpdatePolicy(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
