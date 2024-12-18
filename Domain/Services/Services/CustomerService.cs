using Domain.DTO.Athorization;
using Domain.DTO.Client;
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

        public async Task<ClientInsertCustomerViewModel> ClientInsertCustomer(ClientCreateCustomerRequest request)
        {
            var model = new ClientInsertCustomerViewModel();
            try
            {
                DataTable table = await _customerRepo.ClientInsertCustomer(request);
                if (table != null && table.Rows.Count > 0)
                {
                    var row = table.Rows[0];
                    if (table.Columns.Contains("Id"))
                    {
                        model.Id = row.Field<Guid>("Id");
                    }
                    else
                    {
                        model.Messenger = "Thông tin người dùng cần xem lại.";
                    }
                }
            }
            catch (Exception ex)
            {
                model.Messenger = ex.InnerException.Message.ToString();
            }
            return model;
        }

        public async Task<ClientAuthenicationViewModel> ClientLogin(LoginSubmitModel request)
        {
            var model = new ClientAuthenicationViewModel();
            try
            {
                DataTable table = await _customerRepo.ClientLogin(request);
                if (table != null && table.Rows.Count > 0)
                {
                    model = (from row in table.AsEnumerable()
                             select
                             new ClientAuthenicationViewModel
                             {
                                 Id = row.Field<Guid>("Id"),
                                 UserName = row.Field<string>("UserName"),
                                 Status = row.Field<EntityStatus>("Status"),
                             }).FirstOrDefault();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientAuthenicationViewModel> ClientRegister(RegisterSubmitModel register)
        {
            var model = new ClientAuthenicationViewModel();
            try
            {
                DataTable table = await _customerRepo.ClientRegister(register);
                if (table != null && table.Rows.Count > 0)
                {
                    model = (from row in table.AsEnumerable()
                             select
                             new ClientAuthenicationViewModel
                             {
                                 Id = row.Field<Guid>("Id"),
                                 UserName = row.Field<string>("UserName"),
                                 Status = row.Field<EntityStatus>("Status"),
                             }).FirstOrDefault();
                }
                return model;
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

        public async Task<ResponseData<Customer>> GetAllCustomer(CustomerGetRequest customer)
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
                                  Password = row.Field<string>("Password"),
                                  Email = row.Field<string>("Email"),
                                  PhoneNumber = row.Field<string>("PhoneNumber"),
                                  Gender = row.Field<GenderType?>("Gender"),
                                  DateOfBirth = row.Field<DateTime?>("DateOfBirth"),
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
                                   DateOfBirth = row.Field<DateTime?>("DateOfBirth"),
                                   Gender = row.Field<GenderType?>("Gender") ?? GenderType.Unknown,
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
