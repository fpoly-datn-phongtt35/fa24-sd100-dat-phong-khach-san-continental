using Azure;
using Domain.DTO.Paging;
using Domain.DTO.ResidenceRegistration;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
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
    public class ResidenceRegistrationService : IResidenceRegistrationService
    {
        private readonly IResidenceRegistrationRepo _residenceRegistrationRepo;
        private readonly IConfiguration _configuration;

        public ResidenceRegistrationService(IResidenceRegistrationRepo residenceRegistrationRepo, IConfiguration configuration)
        {
            _residenceRegistrationRepo = residenceRegistrationRepo;
            _configuration = configuration;
        }

        public async Task<int> AddResidence(ResidenceAddRequest request)
        {
            try
            {
                return await _residenceRegistrationRepo.AddResidence(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> CheckOut1Residence(Guid id)
        {
            try
            {
                return await _residenceRegistrationRepo.CheckOut1Residence(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> CheckOutResidenceByRBD(Guid roomBookingDetailId)
        {
            try
            {
                return await _residenceRegistrationRepo.CheckOutByRBD(roomBookingDetailId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> DeleteResidence(Guid id)
        {
            try
            {
                return await _residenceRegistrationRepo.DeleteResidence(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> GetMaximumOccupancyByRoomBookingDetailId(Guid roomBookingDetailId)
        {
            try
            {
                int rs = await _residenceRegistrationRepo.GetMaximumOccupancyByRoomBookingDetailId(roomBookingDetailId);
                return rs;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<ResidenceRegistration> GetResidenceById(Guid id)
        {
            ResidenceRegistration rr = new ResidenceRegistration();
            try
            {
                DataTable table = await _residenceRegistrationRepo.GetResidenceById(id);
                rr = (from row in table.AsEnumerable()
                      select new ResidenceRegistration
                      {
                          Id = row.Field<Guid>("Id"),
                          RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
                          FullName = row.Field<string>("FullName"),
                          DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                          Gender = row.Field<GenderType?>("Gender"),
                          IdentityNumber = row.Field<string>("IdentityNumber"),
                          PhoneNumber = row.Field<string>("PhoneNumber"),
                          CheckOutTime = row.IsNull("CheckOutTime") ? null : row.Field<DateTimeOffset?>("CheckOutTime"),
                          IsCheckOut = row.Field<bool?>("IsCheckOut")
                      }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rr;
        }

        public async Task<ResponseData<ResidenceRegistration>> GetResidenceByRoomBookingDetailId(Guid roomBookingDetailId)
        {
            ResidenceGetRequest request = new ResidenceGetRequest
            {
                RoomBookingDetailId = roomBookingDetailId
            };
            var model = new ResponseData<ResidenceRegistration>();
            try
            {
                DataTable table = await _residenceRegistrationRepo.GetResidences(request);
                model.data = (from row in table.AsEnumerable()
                              select new ResidenceRegistration
                              {
                                  Id = row.Field<Guid>("Id"),
                                  RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
                                  FullName = row.Field<string>("FullName"),
                                  DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                                  Gender = row.Field<GenderType?>("Gender"),
                                  IdentityNumber = row.Field<string>("IdentityNumber"),
                                  PhoneNumber = row.Field<string>("PhoneNumber"),
                                  CheckOutTime = row.IsNull("CheckOutTime") ? null : row.Field<DateTimeOffset?>("CheckOutTime"),
                                  IsCheckOut = row.Field<bool?>("IsCheckOut")
                              }).ToList();
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
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

        public async Task<ResponseData<ResidenceRegistration>> GetResidences(ResidenceGetRequest request)
        {
            var model = new ResponseData<ResidenceRegistration>();
            try
            {
                DataTable table = await _residenceRegistrationRepo.GetResidences(request);
                model.data = (from row in table.AsEnumerable()
                              select new ResidenceRegistration
                              {
                                  Id = row.Field<Guid>("Id"),
                                  RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
                                  FullName = row.Field<string>("FullName"),
                                  DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                                  Gender = row.Field<GenderType?>("Gender"),
                                  IdentityNumber = row.Field<string>("IdentityNumber"),
                                  PhoneNumber = row.Field<string>("PhoneNumber"),
                                  CheckOutTime = row.IsNull("CheckOutTime") ? null : row.Field<DateTimeOffset?>("CheckOutTime"),
                                  IsCheckOut = row.Field<bool?>("IsCheckOut")
                              }).ToList();
                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
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

        public async Task<int> UpdateResidence(ResidenceUpdateRequest request)
        {
            try
            {
                return await _residenceRegistrationRepo.UpdateResidence(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
