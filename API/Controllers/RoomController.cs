using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.Models;
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

        public RoomController(IRoomCreateService roomAddService,
            IRoomDeleteService roomDeleteService,
            IRoomUpdateService roomUpdateService,
            IRoomGetService roomGetService)
        {
            _roomAddService = roomAddService;
            _roomDeleteService = roomDeleteService;
            _roomUpdateService = roomUpdateService;
            _roomGetService = roomGetService;
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
        public async Task<List<RoomResponse>> GetAllRooms()
        {
            try
            {
                return await _roomGetService.GetAllRooms();
            }
            catch (Exception e)
            {
                throw new NullReferenceException("The list of  rooms could not be retrieved", e);
            }
        }

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
    }
}
