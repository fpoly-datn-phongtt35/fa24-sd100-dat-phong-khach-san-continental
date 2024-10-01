using Domain.DTO.Paging;
using Domain.DTO.Voucher;
using Domain.DTO.VoucherDetail;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class VoucherDetailService : IVoucherDetailService
    {
        private readonly VoucherDetailRepo _voucherDetailRepo;
        private readonly IConfiguration _configuration;
        public VoucherDetailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _voucherDetailRepo = new VoucherDetailRepo(_configuration);
        }

        public Task<int> AddVoucherDetail(VoucherDetailCreateRequest request)
        {
            try
            {
                return _voucherDetailRepo.AddVoucherDetail(request);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteVoucherDetail(VoucherDetailDeleteRequest request)
        {
            try
            {
                return await _voucherDetailRepo.DeleteVoucherDetail(request);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<VoucherDetail> GetVoucherDetailById(Guid Id)
        {
            VoucherDetail voucherDetail = new();
            try
            {
                DataTable table = await _voucherDetailRepo.GetVoucherDetailById(Id);
                voucherDetail = (from row in table.AsEnumerable()
                                 select new VoucherDetail
                                 {
                                     Id = row.Field<Guid>("Id"),
                                     RoomBookingId = row.Field<Guid>("RoomBookingId"),
                                     VoucherId = row.Field<Guid>("VoucherId"),
                                     Code = row.Field<string>("Code"),
                                     StartDate = row.Field<DateTimeOffset>("StartDate"),
                                     EndDate = row.Field<DateTimeOffset>("EndDate"),
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
            catch (Exception)
            {

                throw;
            }
            return voucherDetail;
        }

        public async Task<ResponseData<VoucherDetail>> GetVoucherDetails(VoucherDetailGetRequest request)
        {
            var model = new ResponseData<VoucherDetail>();
            try
            {
                DataTable table = await _voucherDetailRepo.GetVoucherDetails(request);
                model.data = (from row in table.AsEnumerable()
                              select new VoucherDetail
                              {
                                  Id = row.Field<Guid>("Id"),
                                  RoomBookingId = row.Field<Guid>("RoomBookingId"),
                                  VoucherId = row.Field<Guid>("VoucherId"),
                                  Code = row.Field<string>("Code"),
                                  StartDate = row.Field<DateTimeOffset>("StartDate"),
                                  EndDate = row.Field<DateTimeOffset>("EndDate"),
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
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    model.totalRecord = 0;
                }
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);

            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }

        public Task<int> UpdateVoucherDetail(VoucherDetailUpdateRequest request)
        {
            try
            {
                return _voucherDetailRepo.UpdateVoucherDetail(request);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
