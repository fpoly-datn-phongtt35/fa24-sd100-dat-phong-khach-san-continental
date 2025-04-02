using Domain.DTO.Policy;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository
{
    public class PolicyRepo : IPolicyRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public PolicyRepo(IConfiguration configuration)
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<int> AddPolicy(PolicyCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", request.Title),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@StaffId", (object)request.StaffId),
                    new SqlParameter("@PolicyTypeId", (object)request.PolicyTypeId),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertPolicy, sqlParameters);
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
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeletePolicy, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetPolicyById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetPolicyById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePolicy(PolicyUpdateRequest request)
        {
            var existingPolicy = GetPolicyById(request.Id);
            if (existingPolicy == null)
            {
                throw new Exception("Policy could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Title", request.Title),
                    new SqlParameter("@Content", request.Content),
                    new SqlParameter("@StaffId",(object) request.StaffId),
                    new SqlParameter("@PolicyTypeId",(object) request.PolicyTypeId),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdatePolicy, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DataTable> GetAllPolicy(PolicyGetRequest Policy)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@Title", Policy.Title),
                new SqlParameter("@Content", Policy.Content),
                new SqlParameter("@PageSize", Policy.PageSize),
                new SqlParameter("@PageIndex", Policy.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetAllPolicy, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PolicyTermsDto>> GetAllPolicyTerms()
        {
            try
            {
                var dataTable = await _DbWorker.GetDataTableAsync(
                    StoredProcedureConstant.SP_GetAllTerms,
                    Array.Empty<SqlParameter>()
                );

                var groupedData = dataTable.AsEnumerable()
                    .GroupBy(row => row.Field<string>("PolicyTypeTitle"))
                    .Select(group => new PolicyTermsDto
                    {
                        PolicyTypeTitle = group.Key,
                        PolicyIds = group.Select(row => row.Field<Guid>("PolicyId")).ToList(),
                        PolicyTitles = group.Select(row => row.Field<string>("PolicyTitle")).ToList(),
                        PolicyContents = group.Select(row => row.Field<string>("PolicyContent")).ToList()

                    }).ToList();
                return groupedData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
