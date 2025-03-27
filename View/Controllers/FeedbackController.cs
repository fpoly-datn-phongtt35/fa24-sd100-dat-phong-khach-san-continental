using Azure.Core;
using Domain.DTO.Customer;
using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.DTO.Role;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IRoom;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;

namespace View.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ICustomerService _customerService;
        private readonly IRoomGetService _roomGetService;
        public FeedbackController(IFeedbackService feedbackService, ICustomerService customerService,IRoomGetService roomGetService)
        {
            _feedbackService = feedbackService;
            _customerService = customerService;
            _roomGetService = roomGetService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetListFeedbacks(FeedbackGetRequest request) 
        {
            var lst = new ResponseData<FeedbackDto>();
            try
            {
                lst = await _feedbackService.GetListFeedback(request);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return  View(lst);
        }

        public async Task<ResponseData<Customer>> GetListCustomers(string txt_search)
        {
            var lst = new ResponseData<Customer>();
            CustomerGetRequest request = new CustomerGetRequest();
            if(txt_search != null) request.Email = txt_search;
            try
            {
                lst = await _customerService.GetAllCustomer(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        public async Task<int> UpdateVisibled(int status,Guid id) 
        {
            try
            {
                FeedbackUpdateRequest request = new FeedbackUpdateRequest() 
                {
                    Status = (EntityStatus)status,
                    Id = id,
                    ModifiedTime = DateTime.Now,
                    Comments = null
                };
                var rs = await _feedbackService.UpdateFeedback(request);
                return rs;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public async Task<ResponseData<RoomResponse>> GetListRooms(string txt_search)
        {
            var lst = new ResponseData<RoomResponse>();
            RoomRequest request = new RoomRequest();
            if (txt_search != null) request.Name = txt_search;
            try
            {
                lst = await _roomGetService.GetAllRooms(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
    }
}
