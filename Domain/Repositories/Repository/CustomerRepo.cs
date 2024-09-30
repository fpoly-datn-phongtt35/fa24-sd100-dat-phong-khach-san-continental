using Domain.DTO.Customer;
using Domain.DTO.ServiceType;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private static DbWorker _DbWorker;
        private readonly IConfiguration _configuration;
        public CustomerRepo(IConfiguration configuration) 
        {
            _DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<int> AddCustomer(CustomerCreateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName", request.UserName),
                    new SqlParameter("@Password", request.Password),
                    new SqlParameter("@FirstName", string.IsNullOrEmpty(request.FirstName) ? DBNull.Value : (object)request.FirstName),
                    new SqlParameter("@LastName", string.IsNullOrEmpty(request.LastName) ? DBNull.Value : (object)request.LastName),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(request.PhoneNumber) ? DBNull.Value : (object)request.PhoneNumber),
                    new SqlParameter("@Gender", request.Gender.HasValue ? (object)request.Gender.Value : DBNull.Value),
                    new SqlParameter("@DateOfBirth", request.DateOfBirth == default(DateTime) ? DBNull.Value : (object)request.DateOfBirth),
                    new SqlParameter("@Status", (int)request.Status),
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCustomer(CustomerDeleteRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id != null ? (object)request.Id : DBNull.Value),
                    new SqlParameter("@DeletedTime", DateTime.Now),
                    new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetCustomerById(Guid id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerId", id != null ? id : DBNull.Value ),
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetCustomerById, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateCustomer(CustomerUpdateRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@UserName", request.UserName),
                    new SqlParameter("@Password", request.Password),
                    new SqlParameter("@FirstName", string.IsNullOrEmpty(request.FirstName) ? DBNull.Value : (object)request.FirstName),
                    new SqlParameter("@LastName", string.IsNullOrEmpty(request.LastName) ? DBNull.Value : (object)request.LastName),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(request.PhoneNumber) ? DBNull.Value : (object)request.PhoneNumber),
                    new SqlParameter("@Gender", request.Gender.HasValue ? (object)request.Gender.Value : DBNull.Value),
                    new SqlParameter("@DateOfBirth", request.DateOfBirth == default(DateTime) ? DBNull.Value : (object)request.DateOfBirth),
                    new SqlParameter("@Status", 1),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DataTable> GetAllCustomer(CustomerGetByUserNameRequest customer)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@UserName", !string.IsNullOrEmpty(customer.UserName) ? customer.UserName : DBNull.Value),
                new SqlParameter("@PageSize", customer.PageSize),
                new SqlParameter("@PageIndex", customer.PageIndex)
                };

                return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetAllCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
