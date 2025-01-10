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
using Domain.DTO.EditHistory;

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
                                CheckInBooking = row.Field<DateTimeOffset?>("CheckInBooking"),
                                CheckOutBooking = row.Field<DateTimeOffset?>("CheckOutBooking"),
                                CheckInReality = row.Field<DateTimeOffset?>("CheckInReality"),
                                CheckOutReality = row.Field<DateTimeOffset?>("CheckOutReality"),
                                Price = row.Field<decimal?>("Price"),
                                ExtraPrice = row.Field<decimal>("ExtraPrice"),
                                Deposit = row.Field<decimal>("Deposit"),
                                Expenses = row.Field<decimal?>("Expenses"),
                                Note = row.Field<string>("Note"),
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
        
        public async Task<RoomBookingDetailResponse?> GetRoomBookingDetailById2(Guid roomBookingDetailId)
        {
            if (roomBookingDetailId == null) return null;
            var roomBookingDetail = await _roomBookingDetailRepository.GetRoomBookingDetailById2(roomBookingDetailId);
            if (roomBookingDetail == null) return null;
            return roomBookingDetail.ToRoomBookingDetailResponse();
        }

        public async Task<RoomBookingDetailResponse?> GetRoomBookingDetailWithEditHistoryById(Guid roomBookingDetailId)
        {
            var roomBookingDetail = await _roomBookingDetailRepository
                    .GetRoomBookingDetailWithEditHistory(roomBookingDetailId);
            if(roomBookingDetail == null) return null;
            
            var roomBookingDetailResponse = roomBookingDetail.ToRoomBookingDetailResponse();
            
            //Convert List EditHistory
            roomBookingDetailResponse.EditHistory = roomBookingDetail.EditHistory
                .Select(editHistory => new EditHistoryResponse
                {
                    Id = editHistory.Id,
                    RoomBookingDetailId = editHistory.RoomBookingDetailId,
                    For = editHistory.For,
                    Content = editHistory.Content,
                    Description = editHistory.Description,
                    ModifiedAt = editHistory.ModifiedAt
                }).ToList();
            
            return roomBookingDetailResponse;
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
                                         Status =row.Field<EntityStatus>("Status"),
                                         RoomStatus = row.Field<RoomStatus>("RoomStatus"),
                                         CheckInReality = row.Field<DateTimeOffset?>("CheckInReality"),
                                         CheckOutReality = row.Field<DateTimeOffset?>("CheckOutReality"),
                                         Price = row.Field<decimal?>("Price"),
                                         ServicePrice = row.Field<decimal?>("ServicePrice"),
                                         ExtraService = row.Field<decimal?>("ExtraService"),
                                         ExtraPrice = row.Field<decimal?>("ExtraPrice"),
                                         Expenses = row.Field<decimal?>("Expenses"),
                                         Note = row.Field<string>("Note"),
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
        
        public async Task<RoomBookingDetailResponse?> UpdateRoomBookingDetail2(RoomBookingDetailUpdateRequest roomBookingDetailUpdateRequest)
        {
            if(roomBookingDetailUpdateRequest == null)
                throw new ArgumentNullException(nameof(roomBookingDetailUpdateRequest));
            
            var existingRoomBookingDetail = await _roomBookingDetailRepository
                .GetRoomBookingDetailById2(roomBookingDetailUpdateRequest.Id);
            if(existingRoomBookingDetail == null)
                throw new ArgumentException("Id Room Booking Detail does not exist");
            
            existingRoomBookingDetail.CheckInReality = roomBookingDetailUpdateRequest.CheckInReality;
            existingRoomBookingDetail.CheckOutReality = roomBookingDetailUpdateRequest.CheckOutReality;
            existingRoomBookingDetail.Expenses = roomBookingDetailUpdateRequest.Expenses;
            existingRoomBookingDetail.ExtraPrice = roomBookingDetailUpdateRequest.ExtraPrice;
            existingRoomBookingDetail.Price = roomBookingDetailUpdateRequest.Price;
            existingRoomBookingDetail.ExtraService = roomBookingDetailUpdateRequest.ExtraService;
            existingRoomBookingDetail.ServicePrice = roomBookingDetailUpdateRequest.ServicePrice;
            existingRoomBookingDetail.Note = roomBookingDetailUpdateRequest.Note;
            existingRoomBookingDetail.Status = roomBookingDetailUpdateRequest.Status;
            existingRoomBookingDetail.Note = roomBookingDetailUpdateRequest.Note;
            existingRoomBookingDetail.ModifiedBy = roomBookingDetailUpdateRequest.ModifiedBy;
            existingRoomBookingDetail.ModifiedTime = roomBookingDetailUpdateRequest.ModifiedTime;
            Console.WriteLine($"Note in Service: {roomBookingDetailUpdateRequest.Note}");

            await _roomBookingDetailRepository.UpdateRoomBookingDetail2(existingRoomBookingDetail);
            return existingRoomBookingDetail.ToRoomBookingDetailResponse();
        }
    }
}
