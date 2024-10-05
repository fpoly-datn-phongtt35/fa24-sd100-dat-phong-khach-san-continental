using Domain.DTO.Staff;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
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
    public class StaffRepository : IStaffRepository
    {
        private readonly DbWorker _DbWorker;
        private readonly IConfiguration _configuration;

        public StaffRepository(IConfiguration configuration)
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }
        public async Task<int> AddStaff(StaffCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName",!string.IsNullOrEmpty(request.UserName) ? request.UserName : DBNull.Value),
                    new SqlParameter("@Password",! string.IsNullOrEmpty(request.Password) ? request.Password : DBNull.Value),
                    new SqlParameter("@FirstName",! string.IsNullOrEmpty(request.FirstName) ? request.FirstName : DBNull.Value),
                    new SqlParameter("@LastName",! string.IsNullOrEmpty(request.LastName) ? request.LastName : DBNull.Value),
                    new SqlParameter("@Email", ! string.IsNullOrEmpty(request.Email) ? request.Email : DBNull.Value),
                    new SqlParameter("@PhoneNumber", ! string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : DBNull.Value),
                    new SqlParameter("@RoleId", request.RoleId != Guid.Empty ? request.RoleId : DBNull.Value),
                    new SqlParameter("@Status", EntityStatus.Active),
                    new SqlParameter("@CreatedTime", DateTime.Now),
                    new SqlParameter("@Deleted", false)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_CreateStaff, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteStaff(StaffDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteStaff, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetStaff(StaffGetRequest Search)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@SearchText",Search.search!= null? Search.search : DBNull.Value),
                    new SqlParameter("@PageSize",Search.PageSize),
                    new SqlParameter("@PageIndex",Search.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetAllStaff, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetStaffId(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", id),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetStaffById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateStaff(StaffUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@UserName",!string.IsNullOrEmpty(request.UserName) ? request.UserName : DBNull.Value),
                    new SqlParameter("@Password",! string.IsNullOrEmpty(request.Password) ? request.Password : DBNull.Value),
                    new SqlParameter("@FirstName",! string.IsNullOrEmpty(request.FirstName) ? request.FirstName : DBNull.Value),
                    new SqlParameter("@LastName",! string.IsNullOrEmpty(request.LastName) ? request.LastName : DBNull.Value),
                    new SqlParameter("@Email", ! string.IsNullOrEmpty(request.Email) ? request.Email : DBNull.Value),
                    new SqlParameter("@PhoneNumber", ! string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : DBNull.Value),
                    new SqlParameter("@RoleId", request.RoleId != Guid.Empty ? request.RoleId : DBNull.Value),
                    new SqlParameter("@Status", EntityStatus.Active),
                    new SqlParameter("@UpdatedTime", DateTime.Now),
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateStaff, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
