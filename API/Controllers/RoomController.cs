using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenityRoom;
using Domain.Services.IServices.IRoom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomCreateService _roomAddService;
        private readonly IRoomDeleteService _roomDeleteService;
        private readonly IRoomUpdateService _roomUpdateService;
        private readonly IRoomGetService _roomGetService;
        private readonly IRoomRepo _roomRepo;
        public RoomController(IRoomCreateService roomAddService,
            IRoomDeleteService roomDeleteService,
            IRoomUpdateService roomUpdateService,
            IRoomGetService roomGetService,
            IRoomRepo roomRepo)
        {
            _roomAddService = roomAddService;
            _roomDeleteService = roomDeleteService;
            _roomUpdateService = roomUpdateService;
            _roomGetService = roomGetService;
            _roomRepo = roomRepo;
        }

        [HttpPost(nameof(CreateRoom))]
        public async Task<RoomResponse> CreateRoom(RoomCreateRequest roomAddService)
        {
            try
            {
                return await _roomAddService.AddRoomService(roomAddService);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost(nameof(GetRoomById))]
        public async Task<RoomResponse?> GetRoomById(Guid roomId)
        {
            try
            {
                return await _roomGetService.GetRoomById(roomId);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Not found the room", e);
            }
        }

        [HttpPost(nameof(GetAllRooms))]
        public async Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest)
        {
            try
            {
                return await _roomGetService.GetAllRooms(roomRequest);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of  rooms could not be retrieved", e);
            }
        }
        [HttpPost(nameof(SearchRooms))]
        public async Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request)
        {
            try
            {
                return await _roomGetService.SearchRooms(request);
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of  rooms could not be retrieved", e);
            }
        }
        //[HttpPost(nameof(GetRoomTypeWithAmenityRoomById))]
        //public async Task<RoomResponse?> GetRoomTypeWithAmenityRoomById(Guid roomTypeId)
        //{
        //    try
        //    {
        //        return await _roomGetService.GetRoomTypeWithAmenityRoomById(roomTypeId);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new NullReferenceException("The list of room types could not be retrieved", e);
        //    }
        //}
        [HttpPut(nameof(DeleteRoom))]
        public async Task<RoomResponse?> DeleteRoom(RoomDeleteRequest roomDeleteRequest)
        {
            try
            {
                return await _roomDeleteService.DeleteRoomService(roomDeleteRequest);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut(nameof(UpdateRoom))]
        public async Task<RoomResponse?> UpdateRoom(RoomUpdateRequest roomUpdateRequest)
        {
            try
            {
                return await _roomUpdateService.UpdateRoomService(roomUpdateRequest);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPut(nameof(UpdateRoomStatus))]
        public async Task<int> UpdateRoomStatus(RoomUpdateStatusRequest request)
        {
            try
            {
                return await _roomRepo.UpdateRoomStatus(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
