using Domain.DTO.Athorization;
using Domain.DTO.ServiceType;
using Domain.DTO.Staff;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IStaffRepository
    {
        Task<int> AddStaff(StaffCreateRequest request);
        Task<int> UpdateStaff(StaffUpdateRequest request);
        Task<int> DeleteStaff(StaffDeleteRequest request);
        Task<DataTable> GetStaffId(Guid id);
        Task<DataTable> Login(LoginSubmitModel request);
        Task<DataTable> GetStaff(StaffGetRequest Search);
    }
}
