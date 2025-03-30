using Domain.Services.IServices.IRoom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomGetService _roomGetService;
        public RoomController(IRoomGetService roomGetService)
        {
            _roomGetService = roomGetService;
        }

        [HttpGet(nameof(GetTop3MostBookedRooms))]
        public async Task<IActionResult> GetTop3MostBookedRooms()
        {
            try
            {
                var rooms = await _roomGetService.GetTop3MostBookedRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
