using System.Text.RegularExpressions;
using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Microsoft.AspNetCore.Mvc;
using WEB.CMS.Customize;

namespace View.Controllers;
[CustomAuthorize]
public class RoomBookingController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IRoomGetService _roomGetService;
    private readonly IRoomBookingGetService _roomBookingService;
    private readonly IRoomBookingDetailServiceForCustomer _roomBookingDetailServiceForCustomer;

    public RoomBookingController(ICustomerService customerService,IRoomGetService roomGetService,IRoomBookingGetService roomBookingGetService)
    {
        _customerService = customerService;
        _roomBookingService = roomBookingGetService;
        _roomGetService = roomGetService;
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

    public async Task<ResponseData<RoomResponse>> GetRoomSuggestion(string txt_search) 
    {
        var response = new ResponseData<RoomResponse>();
        try
        {
            var request = new RoomRequest();
            request.Name = txt_search;
            request.Status = Domain.Enums.RoomStatus.Vacant;
            response = await _roomGetService.GetAllRooms(request);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return response;
        }
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