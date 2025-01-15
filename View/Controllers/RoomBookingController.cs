using System.Security.Claims;
using System.Text.RegularExpressions;
using Domain.DTO.Customer;
using Domain.DTO.EditHistory;
using Domain.DTO.Floor;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomBooking;
using Domain.DTO.RoomBookingDetail;
using Domain.DTO.RoomType;
using Domain.DTO.Service;
using Domain.DTO.ServiceOrderDetail;
using Domain.DTO.ServiceType;
using Domain.Enums;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Services.IServices.IEditHistory;
using Domain.Services.IServices.IRoom;
using Domain.Services.IServices.IRoomBooking;
using Domain.Services.IServices.IRoomType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    private readonly IResidenceRegistrationService _residenceRegistrationService;
    private readonly IEditHistoryAddService _editHistoryAddService;

    public RoomBookingController(IRoomTypeGetService roomTypeServiceGetService, IFloorService floorService,
        IRoomBookingUpdateService roomBookingUpdateService, IServiceOrderDetailService serviceOrderDetailService,
        IServiceTypeService serviceTypeService, IRoomUpdateStatusService roomUpdateStatusService,
        IRoomBookingCreateForCustomerService roomBookingCreateService, IServiceService serviceService,
        ICustomerService customerService, IRoomGetService roomGetService, IRoomBookingGetService roomBookingGetService,
        IRoomBookingDetailServiceForCustomer roomBookingDetailServiceForCustomer,
        IEditHistoryAddService editHistoryAddService)
    {
        _roomBookingDetailServiceForCustomer = roomBookingDetailServiceForCustomer;
        _editHistoryAddService = editHistoryAddService;
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
            if (txt_search != null)
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
            var response = await _serviceOrderDetailService.GetListServiceOrderDetailByRoomBookingDetailId(RoomBooking);
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

    [HttpPost]
    public async Task<List<ServiceOrderDetailResponse>> GetListServiceRelated(Guid id)
    {
        try
        {
            var response = await _serviceOrderDetailService.GetListServiceOrderDetailByRoomBookingDetailId(id);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<int> CheckDepositRoomBooking()
    {
        try
        {
            return await _roomBookingUpdateService.CheckDepositRoomBooking();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DateTimeOffset ChangeTime(DateTimeOffset dateTimeOffset, int newHour, int newMinute, int newSecond)
    {
        // Kiểm tra tính hợp lệ của giờ, phút, giây
        if (newHour < 0 || newHour > 23 || newMinute < 0 || newMinute > 59 || newSecond < 0 || newSecond > 59)
        {
            throw new ArgumentOutOfRangeException("Giờ, phút hoặc giây không hợp lệ.");
        }

        // Tạo DateTime mới với giờ, phút, giây được thay đổi
        DateTime newDateTime = new DateTime(
            dateTimeOffset.Year,
            dateTimeOffset.Month,
            dateTimeOffset.Day,
            newHour,
            newMinute,
            newSecond
        );

        // Tạo DateTimeOffset mới với offset giữ nguyên
        return new DateTimeOffset(newDateTime, dateTimeOffset.Offset);
    }

    public async Task<string> submit(RoomBooking bookingcreaterequest,
        List<RoomBookingDetail> lstupsert)
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
                    TotalPriceReality = bookingcreaterequest.TotalPriceReality,
                    TotalExtraPrice = bookingcreaterequest.TotalExtraPrice,
                    TotalPrice = bookingcreaterequest.TotalPrice != 0
                        ? bookingcreaterequest.TotalPrice
                        : bookingcreaterequest.TotalPriceReality,
                    Status = RoomBookingStatus.PENDING,
                    BookingBy = BookingBy.Day,
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
                    TotalPrice = bookingcreaterequest.TotalPrice != 0
                        ? bookingcreaterequest.TotalPrice
                        : bookingcreaterequest.TotalPriceReality,
                    TotalServicePrice = bookingcreaterequest.TotalServicePrice,
                    TotalExpenses = bookingcreaterequest.TotalExpenses,
                    TotalPriceReality = bookingcreaterequest.TotalPriceReality,
                    TotalRoomPrice = bookingcreaterequest.TotalRoomPrice,
                    Status = bookingcreaterequest.Status,
                    ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    ModifiedTime = DateTime.Now,
                };
                await _roomBookingUpdateService.UpdateRoomBookingAsync(roomBooking);
            }

            foreach (var i in lstupsert)
            {
                i.RoomBookingId = idroombooking;
                i.CreatedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                i.CheckInBooking = ChangeTime(i.CheckInBooking.Value,14,0,0);
                i.CheckOutBooking = ChangeTime(i.CheckOutBooking.Value, 12, 0, 0);
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

            return "/BookingRoom/Id=" + (idroombooking != Guid.Empty ? idroombooking : bookingcreaterequest.Id) +
                   "&&Client=" + bookingcreaterequest.CustomerId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "-1";
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

    public async Task<int> Cancel(Guid Id, Guid IdRoom)
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

    public async Task<IActionResult> CheckIn2(Guid id, bool forceCheckIn = false)
    {
        try
        {
            var roomBookingDetail = await _roomBookingDetailServiceForCustomer.GetRoomBookingDetailById2(id);

            // Nếu CheckInReality đã tồn tại và không có forceCheckIn
            if (roomBookingDetail.CheckInReality.HasValue && !forceCheckIn)
            {
                return Json(new { success = false, message = "Phòng đã được Check-In." });
            }

            // Nếu đang check-in sớm hơn ngày đặt và không có forceCheckIn
            if (roomBookingDetail.CheckInBooking.HasValue &&
                DateTime.UtcNow < roomBookingDetail.CheckInBooking.Value && !forceCheckIn)
            {
                return Json(new { success = false, message = "Bạn đang check-in sớm hơn ngày đặt." });
            }

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            var roomBookingDetailUpdate = new RoomBookingDetailUpdateRequest()
            {
                Id = id,
                Status = EntityStatus.InActive,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CheckInReality = localTime,
                ModifiedTime = localTime
            };
            
            var response = await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail2(roomBookingDetailUpdate);
            
            var note = "Check-In thành công"; // Ghi chú cho lịch sử chỉnh sửa
            var success = await AddEditHistory(id, 1, $"Cập nhật thời gian check-in: {localTime}", note);
            if (!success)
                return Json(new { success = false, message = "Check-In thành công nhưng không thể lưu lịch sử chỉnh sửa." });
            
            return Json(new { success = true, message = "Check-In thành công!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }

    public async Task<IActionResult> UpdateCheckInAndCheckOutReality(Guid id, string checkInTime, string checkoutTime,
        string noteCheckin, string noteCheckout, string note, decimal? expenses,decimal? ServicePrice,decimal? ExtraService,
        List<ServiceOrderDetail> lstSerOrderDetail,
        List<Guid>? ListDelete,Guid RB_Id, bool isCheckInForced = false)
    {
        try
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await HandleServiceOrderDetails(id, lstSerOrderDetail, ListDelete, userId);

            if (!TryParseDateTime(checkInTime, out var selectedCheckInTime))
            {
                return Json(new { success = false, message = "Định dạng thời gian Check-In không hợp lệ." });
            }

            DateTimeOffset? selectedCheckoutTime = null;
            if (!string.IsNullOrEmpty(checkoutTime))
            {
                if (!TryParseDateTime(checkoutTime, out var checkoutTimeParsed))
                {
                    return Json(new { success = false, message = "Định dạng thời gian Check-Out không hợp lệ." });
                }

                selectedCheckoutTime = checkoutTimeParsed;
            }

            var roomBookingDetail = await _roomBookingDetailServiceForCustomer.GetRoomBookingDetailById2(id);
            if (roomBookingDetail == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin đặt phòng." });
            }
            
            bool isCheckInChanged = !isCheckInForced && selectedCheckInTime != roomBookingDetail.CheckInReality;
            bool isExpensesChanged = expenses.HasValue && expenses.Value != roomBookingDetail.Expenses;
            bool isCheckOutChanged = selectedCheckoutTime != roomBookingDetail.CheckOutReality;
            
            if (isCheckInChanged && string.IsNullOrWhiteSpace(noteCheckin))
                return Json(new { success = false, message = "Vui lòng nhập mô tả khi chỉnh sửa Ngày nhận thực tế." });
            
            if (isCheckOutChanged && string.IsNullOrWhiteSpace(noteCheckout))
                return Json(new { success = false, message = "Vui lòng nhập mô tả khi chỉnh sửa Ngày trả thực tế." });
            
            if (isExpensesChanged && string.IsNullOrWhiteSpace(note))
                return Json(new { success = false, message = "Vui lòng nhập mô tả khi thêm phí hư tổn." });

            var roomBookingDetailUpdateRequest = new RoomBookingDetailUpdateRequest()
            {
                Id = id,
                Status = EntityStatus.InActive,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CheckInReality = selectedCheckInTime,
                CheckOutReality = selectedCheckoutTime,
                Expenses = expenses,
                ServicePrice = ServicePrice,
                ExtraService = ExtraService,
                Note = note,
                ModifiedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local)
            };
            Console.WriteLine($"Note received in Controller: {note}");

            var response =
                await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail2(roomBookingDetailUpdateRequest);
            if (response == null)
            {
                return Json(new { success = false, message = "Cập nhật thất bại. Vui lòng thử lại sau." });
            }

            try
            {
                await AddEditHistoryIfChanged(id, roomBookingDetail, selectedCheckInTime, selectedCheckoutTime,
                    checkInTime, checkoutTime, expenses, note, noteCheckin, noteCheckout);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

            try
            {
                await _roomBookingUpdateService.UpdateRoomBookingPrice(RB_Id);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }

    public async Task<string> BlockedBill(Guid Id, Guid CusId)
    {
        try
        {
            var roomBooking = new RoomBookingUpdateRequest()
            {
                Id = Id,
                Status = RoomBookingStatus.PAID,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedTime = DateTime.Now,
            };
            await _roomBookingUpdateService.UpdateRoomBookingAsync(roomBooking);
            return "/BookingRoom/Id=" + Id + "&&Client=" + CusId;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private async Task HandleServiceOrderDetails(Guid id, List<ServiceOrderDetail> lstSerOrderDetail,
        List<Guid>? ListDelete, Guid userId)
    {
        foreach (var detail in lstSerOrderDetail)
        {
            if (detail.Id == Guid.Empty)
            {
                detail.RoomBookingDetailId = id;
                detail.CreatedBy = userId;
                detail.Status = EntityStatus.Active;
                await _serviceOrderDetailService.UpsertServiceOrderDetail(detail);
            }
            else
            {
                detail.ModifiedBy = userId;
                await _serviceOrderDetailService.UpsertServiceOrderDetail(detail);
            }
        }

        if (ListDelete != null)
        {
            foreach (var deleteId in ListDelete)
            {
                var deleteRequest = new ServiceOrderDetailDeleteRequest
                {
                    Id = deleteId,
                    DeletedTime = DateTimeOffset.UtcNow,
                    DeletedBy = userId
                };
                await _serviceOrderDetailService.DeleteServiceOrderDetail(deleteRequest);
            }
        }
    }

    private bool TryParseDateTime(string dateTimeString, out DateTimeOffset parsedDateTime)
    {
        return DateTimeOffset.TryParseExact(
            dateTimeString,
            "d/M/yyyy h:mm tt",
            null,
            System.Globalization.DateTimeStyles.None,
            out parsedDateTime);
    }

    private async Task<bool> AddEditHistory(Guid roomBookingDetailId, int forValue, string content, string description)
    {
        try
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            var editHistory = new EditHistoryCreateRequest()
            {
                RoomBookingDetailId = roomBookingDetailId,
                For = (For)forValue,
                Content = content,
                Description = description,
                ModifiedAt = localTime
            };

            var response = await _editHistoryAddService.AddEditHistoryService(editHistory);

            if (response == null)
            {
                throw new Exception("Dịch vụ trả về null.");
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi thêm EditHistory: {ex.Message}");
            return false;
        }
    }

    private async Task AddEditHistoryIfChanged(
        Guid id, RoomBookingDetailResponse roomBookingDetail,
        DateTimeOffset selectedCheckInTime, DateTimeOffset? selectedCheckoutTime, string checkInTime,
        string checkoutTime,
        decimal? expenses, string note, string noteCheckin, string noteCheckout)
    {
        try
        {
            if (selectedCheckInTime != roomBookingDetail.CheckInReality)
            {
                await AddEditHistory(id, 1, $"Cập nhật thời gian check-in: {checkInTime}", noteCheckin);
            }

            if (selectedCheckoutTime != roomBookingDetail.CheckOutReality)
            {
                await AddEditHistory(id, 2, $"Cập nhật thời gian check-out: {checkoutTime}", noteCheckout);
            }

            if (expenses != roomBookingDetail.Expenses)
            {
                await AddEditHistory(id, 3, $"Cập nhật phí hư tổn: {expenses}", note);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi khi thêm lịch sử chỉnh sửa: " + ex.Message);
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

    public async Task<IActionResult> CheckOut2(Guid id, Guid roomId)
    {
        try
        {
            var roomBookingDetail = await _roomBookingDetailServiceForCustomer.GetRoomBookingDetailById2(id);

            if (!roomBookingDetail.CheckInReality.HasValue)
            {
                return Json(new { success = false, message = "Phòng chưa được Check-In." });
            }

            if (roomBookingDetail.CheckOutReality.HasValue)
            {
                return Json(new { success = false, message = "Phòng đã được Check-Out." });
            }

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            var roomBookingDetailUpdate = new RoomBookingDetailUpdateRequest()
            {
                Id = id,
                Status = EntityStatus.Locked,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CheckOutReality = localTime,
                ModifiedTime = localTime
            };
            var response = await _roomBookingDetailServiceForCustomer.UpdateRoomBookingDetail2(roomBookingDetailUpdate);
            var updateStatusRquest = new RoomUpdateStatusRequest()
            {
                Id = roomId,
                Status = RoomStatus.Vacant,
                ModifiedBy = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedTime = DateTime.Now,
            };
            var rs = await _roomUpdateStatusService.UpdateRoomStatus(updateStatusRquest);
            
            var note = "Check-Out thành công"; // Ghi chú cho lịch sử chỉnh sửa
            var success = await AddEditHistory(id, 1, $"Cập nhật thời gian check-out: {localTime}", note);
            if (!success)
                return Json(new { success = false, message = "Check-Out thành công nhưng không thể lưu lịch sử chỉnh sửa." });
            
            return Json(new { success = true, message = "Check-Out thành công!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }

    [Route("/BookingRoom/Id={IdRoomBooking}&&Client={IdClient}")]
    public async Task<IActionResult> BookingForm(Guid IdRoomBooking, Guid IdClient)
    {
        ViewBag.IdRoomBooking = null;
        ViewBag.IdClient = null;
        ViewBag.Client = null;
        var RB = await _roomBookingService.GetRoomBookingById(IdRoomBooking);
        if (RB != null)
        {
            ViewBag.RB = RB;
        }

        if (IdRoomBooking != Guid.Empty)
        {
            ViewBag.IdRoomBooking = IdRoomBooking;
        }

        if (IdClient != Guid.Empty)
        {
            ViewBag.IdClient = IdClient;
            var Client = await _customerService.GetCustomerById(IdClient);
            ViewBag.Client = Client;
        }

        return View();
    }

    [Route("RoomBooking/RoomBookingDetails/RoomBookingDetailId={roomBookingDetailId}")]
    public async Task<IActionResult> RoomBookingDetails(Guid roomBookingDetailId)
    {
        var roomBookingDetailResponse =
            await _roomBookingDetailServiceForCustomer.GetRoomBookingDetailWithEditHistoryById(roomBookingDetailId);
        if (roomBookingDetailResponse == null)
            return View("Error");
        var roomBooking = await _roomBookingService.GetRoomBookingById(roomBookingDetailResponse.RoomBookingId);
        ViewBag.RoomBooking = roomBooking;
        return View(roomBookingDetailResponse);
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
            if (roomRequest.StartDate != null && roomRequest.EndDate != null)
            {
                response = await _roomGetService.GetAvailableRooms(roomRequest);
            }
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

    public async Task<ResponseData<Customer>> GetCustomerSuggestion(string? txt_search)
    {
        var response = new ResponseData<Customer>();
        try
        {
            var request = new CustomerGetRequest();
            if (txt_search == null)
            {
            }
            else if (Regex.IsMatch(txt_search, @"^\d+$"))
            {
                request.PhoneNumber = txt_search;
                request.UserName = null;
                request.Email = null;
            }
            else if (Regex.IsMatch(txt_search, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
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