using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Domain.DTO.Customer;
using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.DTO.RoomType;
using Domain.DTO.RoomTypeService;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrderDetail;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.IServices.IRoomType;
using Domain.Services.IServices.IRoomTypeService;
using Domain.Services.Services;
using Domain.Services.Services.Room;
using Domain.Services.Services.RoomTypeService;
using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;

namespace View.Controllers;
[CustomAuthorize]
public class RoomBookingController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IFloorService _floorService;
    private readonly IRoomGetService _roomGetService;
    private readonly IServiceService _serviceService;
    private readonly IRoomUpdateStatusService _roomUpdateStatusService;
    private readonly IRoomBookingUpdateService _roomBookingUpdateService;
    private readonly IRoomBookingGetService _roomBookingService;
    private readonly IServiceTypeService _serviceTypeService;
    private readonly IRoomTypeGetService _roomTypeGetService;
    private readonly IServiceOrderDetailService _serviceOrderDetailService;
    private readonly IRoomBookingCreateForCustomerService _roomBookingCreateService;
    private readonly IRoomBookingDetailServiceForCustomer _roomBookingDetailServiceForCustomer;

    public RoomBookingController(IRoomTypeGetService roomTypeServiceGetService,IFloorService floorService,IRoomBookingUpdateService roomBookingUpdateService,IServiceOrderDetailService serviceOrderDetailService,IServiceTypeService serviceTypeService,IRoomUpdateStatusService roomUpdateStatusService, IRoomBookingCreateForCustomerService roomBookingCreateService, IServiceService serviceService,ICustomerService customerService,IRoomGetService roomGetService,IRoomBookingGetService roomBookingGetService, IRoomBookingDetailServiceForCustomer roomBookingDetailServiceForCustomer)
    {
        _roomBookingDetailServiceForCustomer = roomBookingDetailServiceForCustomer;
        _customerService = customerService;
        _roomTypeGetService = roomTypeServiceGetService;
        _floorService = floorService;
        _roomBookingUpdateService = roomBookingUpdateService;
        _serviceOrderDetailService = serviceOrderDetailService;
        _roomUpdateStatusService = roomUpdateStatusService;
        _serviceTypeService = serviceTypeService;
        _roomBookingCreateService = roomBookingCreateService;
        _roomBookingService = roomBookingGetService;
        _serviceService = serviceService;
        _roomGetService = roomGetService;
    }

    public async Task<List<ServiceType>> GetlistServiceType(string txt_search) 
    {
        try
        {
            var request = new ServiceTypeGetRequest();
            if(txt_search != null) 
            {
                request.Name = txt_search;
            }
            var response = await _serviceTypeService.GetServiceTypes(request);
            return response.data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<List<Floor>> GetFloorSuggestion()
    {
        var model = new ResponseData<Floor>();
        try
        {
            FloorGetRequest request = new FloorGetRequest() 
            {
                PageSize = 100,
            };
            model = await _floorService.GetFloor(request);
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
        return model.data;
    }

    public async Task<List<RoomTypeResponse>> GetRoomTypeSuggestion() 
    {
        var model = new ResponseData<RoomTypeResponse>();
        try
        {
            RoomTypeGetRequest request = new RoomTypeGetRequest()
            {
                PageSize = 100,
            };
            model = await _roomTypeGetService.GetFilteredRoomTypes(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return model.data;
    }

    public async Task<Service> GetServiceById(Guid Id) 
    {
        try
        {
            var response = await _serviceService.GetServiceById(Id);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<List<ServiceOrderDetailResponse>> GetSerOrderDetailRelated(Guid RoomBooking) 
    {
        try
        {
            var response = await _serviceOrderDetailService.GetListServiceOrderDetailByRoomBookingI(RoomBooking);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<List<Service>> GetServiceSuggestion(ServiceGetRequest request) 
    {
        try
        {
            var response = await _serviceService.GetServices(request);
            return response.data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<int> submit(RoomBooking bookingcreaterequest, List<RoomBookingDetail> lstupsert,List<ServiceOrderDetail> lstSerOrderDetail)
    {
        try
        {
            Guid idroombooking = Guid.Empty;
            if (bookingcreaterequest.Id == Guid.Empty) 
            {
                var RoomBooking = new RoomBookingCreateRequestForCustomer() 
                {
                    BookingType = BookingType.Offline,
                    StaffId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    CustomerId = bookingcreaterequest.CustomerId,
                    TotalExtraPrice = bookingcreaterequest.TotalExtraPrice,
                    TotalPrice = bookingcreaterequest.TotalPrice,
                    Status = EntityStatus.Active,
                    TotalServicePrice = bookingcreaterequest.TotalServicePrice,
                    TotalRoomPrice = bookingcreaterequest.TotalRoomPrice,
                    CreatedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                };
                idroombooking = await _roomBookingCreateService.CreateRoomBookingForCustomer(RoomBooking);
            }
            else 
            {
                var roomBooking = new RoomBookingUpdateRequest()
                {
                    Id = bookingcreaterequest.Id,
                    TotalExtraPrice = bookingcreaterequest.TotalExtraPrice,
                    TotalPrice = bookingcreaterequest.TotalPrice,
                    TotalServicePrice = bookingcreaterequest.TotalServicePrice,
                    Status = bookingcreaterequest.Status,
                    ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    ModifiedTime = DateTime.Now,
                };
                await _roomBookingUpdateService.UpdateRoomBookingAsync(roomBooking);
            }
            foreach(var i in lstupsert) 
            {
                i.RoomBookingId = idroombooking;
                i.CreatedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _roomBookingDetailServiceForCustomer.UpSertRoomBookingDetail(i);
                var updateStatusRquest = new RoomUpdateStatusRequest()
                {
                    Id = i.RoomId,
                    ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    ModifiedTime = DateTime.Now,
                };
                if (i.Status == EntityStatus.Deleted || i.Status == EntityStatus.Locked)
                {
                    updateStatusRquest.Status = RoomStatus.Dirty;
                }
                else if (i.Status == EntityStatus.InActive) 
                {
                    updateStatusRquest.Status = RoomStatus.Occupied;
                }
                await _roomUpdateStatusService.UpdateRoomStatus(updateStatusRquest);
            }
            foreach (var i in lstSerOrderDetail) 
            {
                if (i.Id == Guid.Empty)
                {
                    i.RoomBookingId = idroombooking;
                    i.CreatedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    i.Status = EntityStatus.Active;
                    await _serviceOrderDetailService.UpsertServiceOrderDetail(i);
                }
                else 
                {
                    i.RoomBookingId = idroombooking;
                    i.ModifiedTime = DateTime.Now;
                    i.ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    await _serviceOrderDetailService.UpsertServiceOrderDetail(i);
                }
            }
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<List<RoomBookingDetailGetByIdRoomBooking>> GetRoomRelated(Guid Id) 
    {
        try
        {
            var response = await _roomBookingDetailServiceForCustomer.GetListRoomBookingDetailByRoomBookingId(Id);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<int> Cancel(Guid Id,Guid IdRoom) 
    {
        try
        {
            var RoomBDUpdate = new RoomBookingDetailUpdateRequest()
            {
                Id = Id,
                Status = EntityStatus.Locked,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedTime = DateTime.UtcNow
            };
            var response = await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail(RoomBDUpdate);
            var updateStatusRquest = new RoomUpdateStatusRequest()
            {
                Id = IdRoom,
                Status = RoomStatus.Vacant,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedTime = DateTime.Now,
            };
            await _roomUpdateStatusService.UpdateRoomStatus(updateStatusRquest);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<int> CheckIn(Guid Id)
    {
        try
        {
            var RoomBDUpdate = new RoomBookingDetailUpdateRequest()
            {
                Id = Id,
                Status = EntityStatus.InActive,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CheckInReality = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow
            };
            var response = await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail(RoomBDUpdate);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<int> CheckOut(Guid Id, Guid IdRoom)
    {
        try
        {
            var RoomBDUpdate = new RoomBookingDetailUpdateRequest()
            {
                Id = Id,
                Status = EntityStatus.Locked,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CheckOutReality = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow
            };
            var response = await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail(RoomBDUpdate);
            var updateStatusRquest = new RoomUpdateStatusRequest()
            {
                Id = IdRoom,
                Status = RoomStatus.Vacant,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedTime = DateTime.Now,
            };
            var rs = await _roomUpdateStatusService.UpdateRoomStatus(updateStatusRquest);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    [Route("/BookingRoom/Id={IdRoomBooking}&&Client={IdClient}")]
    public async Task<IActionResult> BookingForm(Guid IdRoomBooking,Guid IdClient) 
    {
        ViewBag.IdRoomBooking = null;
        ViewBag.IdClient = null;
        ViewBag.Client = null;
        if (IdRoomBooking != Guid.Empty) 
        {
            ViewBag.IdRoomBooking = IdRoomBooking;
        }
        if(IdClient != Guid.Empty) 
        {
            ViewBag.IdClient = IdClient;
            var Client = await _customerService.GetCustomerById(IdClient);
            ViewBag.Client = Client;
        }
        return View();
    }

    public async Task<RoomResponse> GetRoomById(Guid Id) 
    {
        try
        {
            var response = await _roomGetService.GetRoomById(Id);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<RoomAvailableResponse> GetAvailableRooms(RoomAvailableRequest roomRequest)
    {
        var response = new RoomAvailableResponse();
        try
        {
            response = await _roomGetService.GetAvailableRooms(roomRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return response;
    }

    [HttpPost]
    public async Task<Customer> GetCustomerById(Guid Id) 
    {
        try
        {
            var response = await _customerService.GetCustomerById(Id);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<ResponseData<Customer>> GetCustomerSuggestion(string txt_search) 
    {   var response = new ResponseData<Customer>();
        try 
        {
            var request = new CustomerGetRequest();
            if (Regex.IsMatch(txt_search, @"^\d+$")) 
            {
                request.PhoneNumber = txt_search;
                request.UserName = null;
                request.Email = null;
            }
            else if(Regex.IsMatch(txt_search, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))    
            {
                request.PhoneNumber = null;
                request.UserName = null;
                request.Email = txt_search;
            }
            else 
            {
                request.PhoneNumber = null;
                request.UserName = txt_search;
                request.Email = null;
            }
            response = await _customerService.GetAllCustomer(request);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return response;
        }
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    public async Task<IActionResult> ListRoomBooking(RoomBookingGetRequest Request) 
    {
        try
        {
            var response = await _roomBookingService.GetFilteredRoomBooking(Request);
            return View(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}