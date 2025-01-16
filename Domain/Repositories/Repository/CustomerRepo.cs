using Domain.DTO.Athorization;
using Domain.DTO.Customer;
using Domain.DTO.ServiceType;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System.Data;
using Utilities;
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
                    new SqlParameter("@Password", ! string.IsNullOrEmpty(request.Password) ? PasswordHashingHelper.HashPassword(request.Password) : PasswordHashingHelper.HashPassword("123456")),
                    new SqlParameter("@FirstName", string.IsNullOrEmpty(request.FirstName) ? DBNull.Value : (object)request.FirstName),
                    new SqlParameter("@LastName", string.IsNullOrEmpty(request.LastName) ? DBNull.Value : (object)request.LastName),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(request.PhoneNumber) ? DBNull.Value : (object)request.PhoneNumber),
                    new SqlParameter("@Gender", SqlDbType.Int) { Value = request.Gender },
                    new SqlParameter("@DateOfBirth", request.DateOfBirth == default(DateTime) ? DBNull.Value : (object)request.DateOfBirth),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@CreatedTime", request.CreatedTime),
                    new SqlParameter("@CreatedBy", request.CreatedBy)
                };

                var work =  _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertCustomer, sqlParameters);
                return work;
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
            var existingCustomer = GetCustomerById(request.Id);
            if (existingCustomer == null)
            {
                throw new Exception("Customer could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@UserName", request.UserName),
                    new SqlParameter("@FirstName", string.IsNullOrEmpty(request.FirstName) ? DBNull.Value : (object)request.FirstName),
                    new SqlParameter("@LastName", string.IsNullOrEmpty(request.LastName) ? DBNull.Value : (object)request.LastName),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(request.PhoneNumber) ? DBNull.Value : (object)request.PhoneNumber),
                    new SqlParameter("@Gender", SqlDbType.Int) { Value = request.Gender },
                    new SqlParameter("@DateOfBirth", request.DateOfBirth == default(DateTime) ? DBNull.Value : (object)request.DateOfBirth),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status },
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };
                using (var reader = await _DbWorker.ExecuteReaderAsync(StoredProcedureConstant.SP_UpdateCustomer, sqlParameters))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0); 
                    }
                    else
                    {
                        throw new Exception("No rows were returned by the stored procedure.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DataTable> GetAllCustomer(CustomerGetRequest customer)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@UserName", customer.UserName),
                new SqlParameter("Email", customer.Email),
                new SqlParameter("PhoneNumber", customer.PhoneNumber),
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

       
        public async Task<DataTable> ClientLogin(LoginSubmitModel request)
        {
            try
            {
                string hashedPassword = PasswordHashingHelper.HashPassword(request.Password);


                SqlParameter[] sqlParameters = new SqlParameter[]
                {
            new SqlParameter("@UserName", request.UserName),
            new SqlParameter("@Password", hashedPassword)
                };

                return await _DbWorker.GetDataTableAsync(StoredProcedureConstant.SP_ClientLogin, sqlParameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> ClientRegister(RegisterSubmitModel register)
        {
            try
            {
                string hashedPassword = PasswordHashingHelper.HashPassword(register.Password);


                SqlParameter[] sqlParameters = new SqlParameter[]
                {   
                    new SqlParameter("@UserName", register.UserName),
                    new SqlParameter("@Password", hashedPassword),
                    new SqlParameter("@Email", register.Email),
                    new SqlParameter("@PhoneNumber", register.PhoneNumber),
                   new SqlParameter("@CreatedTime", DateTimeOffset.Now),
                };

                return await _DbWorker.GetDataTableAsync(StoredProcedureConstant.SP_ClientRegister, sqlParameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> ClientInsertCustomer(ClientCreateCustomerRequest request)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName", request.UserName),
                    new SqlParameter("@Password", ! string.IsNullOrEmpty(request.Password) ? PasswordHashingHelper.HashPassword(request.Password) : PasswordHashingHelper.HashPassword("123456")),
                    new SqlParameter("@FirstName", string.IsNullOrEmpty(request.FirstName) ? DBNull.Value : (object)request.FirstName),
                    new SqlParameter("@LastName", string.IsNullOrEmpty(request.LastName) ? DBNull.Value : (object)request.LastName),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(request.PhoneNumber) ? DBNull.Value : (object)request.PhoneNumber),
                    new SqlParameter("@CreatedTime", request.CreatedTime)
                };

                return await _DbWorker.GetDataTableAsync(StoredProcedureConstant.SP_ClientInsertCustomer, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> ClientUpdatePassword(ClientUpdatePassword request)
        {
            try
            {
                string hashedPassword = PasswordHashingHelper.HashPassword(request.Password);
                string newPasswordHashed = PasswordHashingHelper.HashPassword(request.NewPassword);

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserId", request.Id),
                    new SqlParameter("@Password", hashedPassword),
                    new SqlParameter("@NewPassword", newPasswordHashed)
                };

                return await _DbWorker.GetDataTableAsync(StoredProcedureConstant.SP_UpdatePassword, sqlParameters);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
