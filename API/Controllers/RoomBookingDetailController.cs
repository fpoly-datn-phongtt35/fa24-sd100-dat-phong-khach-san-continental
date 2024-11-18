using Domain.DTO.Customer;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.Services.RoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingDetailController : ControllerBase
    {
        private readonly IRoomBookingDetailServiceForCustomer _roomBookingDetailService;

        public RoomBookingDetailController(IRoomBookingDetailServiceForCustomer roomBookingDetailService)
        {
            _roomBookingDetailService = roomBookingDetailService;
        }

        [HttpPost(nameof(GetRoomBookingDetailById))]
        public async Task<RoomBookingDetail> GetRoomBookingDetailById(Guid id)
        {
            try
            {
                return await _roomBookingDetailService.GetById(id);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Not found the room booking detail", e);
            }
        }
        [HttpPost(nameof(GetRoomBookingDetailByRoomBookingId))]
        public async Task<List<RoomBookingDetailGetByIdRoomBooking>> GetRoomBookingDetailByRoomBookingId(Guid id)
        {
            try
            {
                return await _roomBookingDetailService.GetListRoomBookingDetailByRoomBookingId(id);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Not found the room booking detail", e);
            }
        }
        [HttpPost("CreateRoomBookingDetail")]
        public async Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request)
        {
            try
            {
                return await _roomBookingDetailService.CreateRoomBookingDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateRoomBookingDetail")]
        public async Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request)
        {
            try
            {
                return await _roomBookingDetailService.UpdateRoomBookingDetail(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
