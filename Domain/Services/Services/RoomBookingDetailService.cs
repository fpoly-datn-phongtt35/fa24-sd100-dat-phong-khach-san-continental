using Domain.DTO.RoomBookingDetail;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
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
    public class RoomBookingDetailService : IRoomBookingDetailServiceForCustomer
    {
        private readonly IRoomBookingDetailRepository _roomBookingDetailRepository;
        private readonly IConfiguration _configuration;
        public RoomBookingDetailService(IConfiguration configuration, IRoomBookingDetailRepository roomBookingDetailRepository)
        {
            _configuration = configuration;
            _roomBookingDetailRepository = roomBookingDetailRepository;
        }
        public async Task<int> UpSertRoomBookingDetail(RoomBookingDetail request)
        {
            try
            {
                return await _roomBookingDetailRepository.UpSertRoomBookingDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request)
        {
            try
            {
                return await _roomBookingDetailRepository.CreateRoomBookingDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request)
        {
            try
            {
                return await _roomBookingDetailRepository.CreateRoomBookingDetailForCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoomBookingDetail> GetById(Guid id)
        {
            RoomBookingDetail roomBookingDetail = new RoomBookingDetail();
            try
            {
                DataTable table = await _roomBookingDetailRepository.GetById(id);
                roomBookingDetail = (from row in table.AsEnumerable()
                            select new RoomBookingDetail
                            {
                                Id = row.Field<Guid>("Id"),
                                RoomId = row.Field<Guid>("RoomId"),
                                RoomBookingId = row.Field<Guid>("RoomBookingId"),
                                CheckInBooking = row.Field<DateTimeOffset>("CheckInBooking"),
                                CheckOutBooking = row.Field<DateTimeOffset>("CheckOutBooking"),
                                CheckInReality = row.Field<DateTimeOffset>("CheckInReality"),
                                CheckOutReality = row.Field<DateTimeOffset>("CheckOutReality"),
                                Price = row.Field<decimal?>("Price"),
                                ExtraPrice = row.Field<decimal>("ExtraPrice"),
                                Deposit = row.Field<decimal>("Deposit"),
                                Status = row.Field<EntityStatus>("Status"),
                                Deleted = row.Field<bool>("Deleted")
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
            return roomBookingDetail;
        }
        public async Task<List<RoomBookingDetailGetByIdRoomBooking>> GetListRoomBookingDetailByRoomBookingId(Guid id)
        {
            List<RoomBookingDetailGetByIdRoomBooking> roomBookingDetail = new List<RoomBookingDetailGetByIdRoomBooking>();
            try
            {
                DataTable table = await _roomBookingDetailRepository.GetListRoomBookingDetailByRoomBookingId(id);
                roomBookingDetail = (from row in table.AsEnumerable()
                                     select new RoomBookingDetailGetByIdRoomBooking
                                     {
                                         RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
                                         RoomId = row.Field<Guid>("RoomId"),
                                         CheckInBooking = row.Field<DateTimeOffset?>("CheckInBooking"),
                                         CheckOutBooking = row.Field<DateTimeOffset?>("CheckOutBooking"),
                                         Status =row.Field<EntityStatus>("Status").ToString(),
                                         RoomStatus = row.Field<RoomStatus>("RoomStatus").ToString(),
                                         CheckInReality = row.Field<DateTimeOffset?>("CheckInReality"),
                                         CheckOutReality = row.Field<DateTimeOffset?>("CheckOutReality"),
                                         Price = row.Field<decimal?>("Price"),
                                         ExtraPrice = row.Field<decimal?>("ExtraPrice"),
                                         Name = row.Field<string>("Name")
                                     }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roomBookingDetail;
        }
 
        public async Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request)
        {
            try
            {
                return await _roomBookingDetailRepository.UpdateRoomBookingDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
