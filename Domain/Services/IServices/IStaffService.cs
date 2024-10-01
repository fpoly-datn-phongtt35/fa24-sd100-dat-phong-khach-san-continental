using Domain.DTO.Paging;
using Domain.DTO.Staff;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IStaffService
    {
        Task<int> AddStaff(StaffCreateRequest request);
        Task<int> UpdateStaff(StaffUpdateRequest request);
        Task<int> DeleteStaff(StaffDeleteRequest request);
        Task<ResponseData<Staff>> GetStaffs(StaffGetRequest Search);
        Task<StaffUpdateRequest> GetStaffbyId(Guid id);
    }
}
