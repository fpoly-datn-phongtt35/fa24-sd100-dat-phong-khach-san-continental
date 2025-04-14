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

        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            switch (type?.ToLower())
            {
                case "top-booked":
                    var topRooms = await _roomRepo.GetTop3MostBookedRoomsAsync();
                    return View("Default", topRooms);

                case "nice-view":
                    var niceViewRooms = await _roomRepo.GetRoomsWithNiceViewAsync();
                    return View("NiceView", niceViewRooms);

                default:
                    return Content("Invalid type");
            }
        }
      

    }
}
