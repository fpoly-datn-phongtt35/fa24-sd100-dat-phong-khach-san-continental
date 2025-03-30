using Microsoft.AspNetCore.Mvc;
using ViewClient.Repositories.IRepository;

namespace ViewClient.ViewComponents
{
    public class TopBookedRoomsViewComponent : ViewComponent
    {
        private readonly IRoom _roomRepo;

        public TopBookedRoomsViewComponent(IRoom roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rooms = await _roomRepo.GetTop3MostBookedRoomsAsync();
            Console.WriteLine($"🔥 Lấy được {rooms.Count} phòng!");
            return View(rooms);
        }
    }
}
