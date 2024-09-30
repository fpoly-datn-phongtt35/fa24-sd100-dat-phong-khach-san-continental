using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Domain.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepo _customerRepo;
        private readonly IConfiguration _configuration;
        public CustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _customerRepo = new CustomerRepo(configuration);
        }
        public async Task<int> AddCustomer(CustomerCreateRequest request)
        {
            try
            {
                return await _customerRepo.AddCustomer(request);
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
                return await _customerRepo.DeleteCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseData<Customer>> GetAllCustomer(CustomerGetByUserNameRequest customer)
        {
            var model = new ResponseData<Customer>();
            try
            {
                DataTable dataTable = await _customerRepo.GetAllCustomer(customer);
                model.data = (from row in dataTable.AsEnumerable()
                              select new Customer
                              {
                                  Id = row.Field<Guid>("Id"),
                                  UserName = row.Field<string>("UserName"),
                                  FirstName = row.Field<string>("FirstName"),
                                  LastName = row.Field<string>("LastName"),
                                  Email = row.Field<string>("Email"),
                                  PhoneNumber = row.Field<string>("PhoneNumber"),
                                  Gender = row.Field<int>("Gender"),
                                  DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
                              }).ToList();
                model.CurrentPage = customer.PageIndex;
                model.PageSize = customer.PageSize;
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
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / customer.PageSize);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<Customer> GetCustomerById(Guid Id)
        {
           Customer customer = new Customer();
            try
            {
                DataTable table = await _customerRepo.GetCustomerById(Id);
                customer = (from row in table.AsEnumerable()
                               select new Customer
                               {
                                   Id = row.Field<Guid>("Id"),
                                   UserName = row.Field<string>("UserName"),
                                   Password = row.Field<string>("Password"),
                                   FirstName = row.Field<string>("FirstName"),
                                   LastName = row.Field<string>("LastName"),
                                   PhoneNumber = row.Field<string>("PhoneNumber"),
                                   Email = row.Field<string>("Email"),
                                   DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                                   Gender = row.Field<int>("Gender"),
                                   Status = row.Field<EntityStatus>("Status"),
                                   //CreatedTime = row.Field<DateTime>("CreatedTime"),
                                   //CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                   //ModifiedTime = row.Field<DateTime>("ModifiedTime"),
                                   //ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                   //Deleted = row.Field<bool>("Deleted"),
                                   //DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                   //DeletedTime = row.Field<DateTime>("DeletedTime")
                               }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customer;
        }

        public async Task<int> UpdateCustomer(CustomerUpdateRequest request)
        {
            try
            {
                return await _customerRepo.UpdateCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
