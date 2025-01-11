using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.StoredProcedure
{
    public class StoredProcedureConstant
    {
        //Tất cả các SP sẽ được khai báo tại đây để quản lí
        public const string EXAMPLE = "SP_Name";
        public const string Continetal = "Data Source=.;Initial Catalog=Conentinal;Integrated Security=True;Trust Server Certificate=True";
        //service type
        public const string SP_InsertServiceType = "SP_InsertServiceType";
        public const string SP_UpdateServiceType = "SP_UpdateServiceType";
        public const string SP_DeleteServiceType = "SP_DeleteServiceType";
        public const string SP_GetListServiceType = "SP_GetListServiceType";
        //service
        public const string SP_InsertService = "SP_InsertService";
        public const string SP_GetListService = "SP_GetListService";
        public const string SP_DeleteService = "SP_DeleteService";
        public const string SP_GetServiceById = "SP_GetServiceById";
        public const string SP_UpdateService = "SP_UpdateService";
        public const string SP_GetServiceByTypeId = "SP_GetServiceByTypeId";
        public const string SP_GetAllServiceNamesGroupedByServiceType = "SP_GetAllServiceNamesGroupedByServiceType";

        //service order detail
        public const string SP_InsertServiceOrderDetail = "SP_InsertServiceOrderDetail";
        public const string SP_GetListServiceOrderDetail = "SP_GetListServiceOrderDetail";
        public const string SP_DeleteServiceOrderDetail = "SP_DeleteServiceOrderDetail";
        public const string SP_UpdateServiceOrderDetail = "SP_UpdateServiceOrderDetail";
        public const string SP_GetServiceOrderDetailById = "SP_GetServiceOrderDetailById";
        public const string SP_GetListServiceOrderDetailByRoomBookingDetailId = "SP_GetListServiceOrderDetailByRoomBookingDetailId";

        //room
        public const string SP_InsertRoom = "SP_InsertRoom";
        public const string SP_GetListRoom = "SP_GetListRoom";
        public const string SP_DeleteRoom = "SP_DeleteRoom";
        public const string SP_GetRoomById = "SP_GetRoomById";
        public const string SP_UpdateRoom = "SP_UpdateRoom";
        public const string SP_UpdateRoomStatus = "SP_UpdateRoomStatus";
        public const string SP_GetAvailableRooms = "SP_GetAvailableRooms";
        public const string SP_SearchRooms = "SP_SearchRooms";

        //statistics
        public const string SP_GetTop5MostBookedRoomsLastMonth = "SP_GetTop5MostBookedRoomsLastMonth";
        public const string SP_GetTopMostBookedRoomsCustomer = "SP_GetTopMostBookedRoomsCustomer";
        public const string SP_GetRevenue = "SP_GetRevenue";
        public const string SP_GetCoverageRatio = "SP_GetCoverageRatio";

        //building
        public const string SP_InsertBuilding = "SP_InsertBuilding";
        public const string SP_GetListBuilding = "SP_GetListBuilding";
        public const string SP_DeleteBuilding = "SP_DeleteBuilding";
        public const string SP_GetBuildingById = "SP_GetBuildingById";
        public const string SP_UpdateBuilding = "SP_UpdateBuilding";
        //floor
        public const string SP_InsertFloor = "SP_InsertFloor";
        public const string SP_GetListFloor = "SP_GetListFloor";
        public const string SP_DeleteFloor = "SP_DeleteFloor";
        public const string SP_GetFloorById = "SP_GetFloorById";
        public const string SP_UpdateFloor = "SP_UpdateFloor";
        //amenity
        public const string SP_InsertAmenity = "SP_InsertAmenity";
        public const string SP_GetFilteredAmenities = "SP_GetFilteredAmenities";
        public const string SP_GetFilteredDeletedAmenities = "SP_GetFilteredDeletedAmenities";
        public const string SP_GetAmenityById = "SP_GetAmenityById";
        public const string SP_UpdateAmenity = "SP_UpdateAmenity";
        public const string SP_DeleteAmenity = "SP_DeleteAmenity";
        public const string SP_RecoverDeletedAmenity= "SP_RecoverDeletedAmenity";
        //roomType
        public const string SP_InsertRoomType = "SP_InsertRoomType";
        public const string SP_GetFilteredRoomTypes = "SP_GetFilteredRoomTypes";
        public const string SP_GetFilteredDeletedRoomTypes = "SP_GetFilteredDeletedRoomTypes";
        public const string SP_GetRoomTypeById = "SP_GetRoomTypeById";
        public const string SP_UpdateRoomType = "SP_UpdateRoomType";
        public const string SP_DeleteRoomType = "SP_DeleteRoomType";
        public const string SP_RecoverDeletedRoomType = "SP_RecoverDeletedRoomType";
        // Customer
        public const string SP_InsertCustomer = "SP_InsertCustomer";
        public const string SP_GetCustomerById = "SP_GetCustomerById";
        public const string SP_DeleteCustomer = "SP_DeleteCustomer";
        public const string SP_UpdateCustomer = "SP_UpdateCustomer";
        public const string SP_GetAllCustomer = "SP_GetAllCustomers";
        // amenityRoom
        public const string SP_InsertAmenityRoom = "SP_InsertAmenityRoom";
        public const string SP_GetFilteredAmenityRooms = "SP_GetFilteredAmenityRooms";
        public const string SP_GetAmenityRoomById = "SP_GetAmenityRoomById";
        public const string SP_UpdateAmenityRoom = "SP_UpdateAmenityRoom";
        public const string SP_DeleteAmenityRoom = "SP_DeleteAmenityRoom";
        public const string SP_GetAmenityRoomsByRoomTypeId = "SP_GetAmenityRoomsByRoomTypeId";
        public const string SP_GetFilteredDeletedAmenityRooms = "SP_GetFilteredDeletedAmenityRooms";
        public const string SP_RecoverDeletedAmenityRoom = "SP_RecoverDeletedAmenityRoom";
        // Voucher
        public const string SP_InsertVoucher = "SP_InsertVoucher";
        public const string SP_GetListVoucher = "SP_GetListVoucher";
        public const string SP_GetVoucherById = "SP_GetVoucherById";
        public const string SP_UpdateVoucher = "SP_UpdateVoucher";
        public const string SP_DeleteVoucher = "SP_DeleteVoucher";
        //voucher detail
        public const string SP_InsertVoucherDetail = "SP_InsertVoucherDetail";
        public const string SP_GetListVoucherDetail = "SP_GetListVoucherDetail";
        public const string SP_UpdateVoucherDetail = "SP_UpdateVoucherDetail";
        public const string SP_DeleteVoucherDetail = "SP_DeleteVoucherDetail";
        // PostType
        public const string SP_InsertPostType = "SP_InsertPostType";
        public const string SP_GetAllPostType = "SP_GetAllPostType";
        public const string SP_GetPostTypeById = "SP_GetPostTypeById";
        public const string SP_UpdatePostType = "SP_UpdatePostType";
        public const string SP_DeletePostType = "SP_DeletePostType";
        // RoomTypeService
        public const string SP_InsertRoomTypeService = "SP_InsertRoomTypeService";
        public const string SP_GetFilteredRoomTypeServices = "SP_GetFilteredRoomTypeServices";
        public const string SP_GetRoomTypeServiceById = "SP_GetRoomTypeServiceById";
        public const string SP_UpdateRoomTypeService = "SP_UpdateRoomTypeService";
        public const string SP_DeleteRoomTypeService = "SP_DeleteRoomTypeService";
        public const string SP_GetRoomTypeServicesByRoomTypeId = "SP_GetRoomTypeServicesByRoomTypeId";
        public const string SP_GetFilteredDeletedRoomTypeServices = "SP_GetFilteredDeletedRoomTypeServices";
        public const string SP_RecoverDeletedRoomTypeService = "SP_RecoverDeletedRoomTypeService";
        //Staff 
        public const string SP_GetAllStaff = "SP_GetStaffList";
        public const string SP_CreateStaff = "SP_InsertStaff";
        public const string SP_DeleteStaff = "SP_DeleteStaff";
        public const string SP_UpdateStaff = "SP_UpdateStaff";
        public const string SP_GetStaffById = "SP_GetStaffById";
        public const string SP_Login = "SP_Login";
        // Role
        public const string SP_InsertRole = "SP_InsertRole";
        public const string SP_GetAllRole = "SP_GetAllRole";
        public const string SP_GetRoleById = "SP_GetRoleById";
        public const string SP_UpdateRole = "SP_UpdateRole";
        public const string SP_DeleteRole = "SP_DeleteRole";
        //Post
        public const string SP_InsertPost = "SP_InsertPost";
        public const string SP_GetAllPost = "SP_GetAllPost";
        public const string SP_GetPostById = "SP_GetPostById";
        public const string SP_UpdatePost = "SP_UpdatePost";
        public const string SP_DeletePost = "SP_DeletePost";
        public const string SP_GetAllTerms = "SP_GetAllTerms";
        //RoomBooking
        public const string SP_GetFilteredRoomBookings = "SP_GetFilteredRoomBookings";
        public const string SP_GetRoomBookingById = "SP_GetRoomBookingById";
        public const string SP_UpdateRoomBooking = "SP_UpdateRoomBooking";
        public const string SP_InsertRoomBookingForCustomer = "SP_InsertRoomBookingForCustomer";
        public const string SP_InsertRoomBooking = "SP_InsertRoomBooking";
        public const string SP_UpdateRoomBookingStatus = "SP_UpdateRoomBookingStatus";
        public const string SP_GetCheckinRoomBookingByRoomBookingId = "SP_GetCheckinRoomBookingByRoomBookingId";
        public const string SP_CheckDepositedRoomBooking = "SP_CheckDepositedRoomBooking";
        public const string SP_GetListRoomBookingsByCustomerId = "SP_GetListRoomBookingsByCustomerId";
        public const string SP_UpdateRoomBookingPrice = "SP_UpdateRoomBookingPrice";

        // RoomBookingDetail
        public const string SP_BookRoomDetailForCustomer = "SP_BookRoomDetailForCustomer";
        public const string SP_InsertRoomBookingDetail = "SP_InsertRoomBookingDetail";
        public const string SP_UpdateRoomBookingDetail = "SP_UpdateRoomBookingDetail";
        public const string SP_GetRoomBookingDetailById = "SP_GetRoomBookingDetailById";
        public const string SP_GetRoomBookingDetailById2 = "SP_GetRoomBookingDetailById2";
        public const string SP_GetListRoomBookingDetailByRoomBookingId = "SP_GetListRoomBookingDetailByRoomBookingId";
        public const string SP_GetRoomBookingDetailsByCustomerId = "SP_GetRoomBookingDetailsByCustomerId";
        //Client 
        public const string SP_ClientLogin = "SP_ClientLogin";
        public const string SP_ClientRegister = "SP_ClientRegister";
        public const string SP_ClientInsertCustomer = "SP_ClientInsertCustomer";
        public const string SP_UpdatePassword = "SP_UpdatePassword";

        //payment history
        public const string SP_InsertPaymentHistory = "SP_InsertPaymentHistory";
        public const string SP_GetPaymentHistoryById = "SP_GetPaymentHistoryById";
        public const string SP_GetListPaymentHistory = "SP_GetListPaymentHistory";
        public const string SP_UpdatePaymentHistoryAmount = "SP_UpdatePaymentHistoryAmount";
        public const string SP_DeletePaymentHistory = "SP_DeletePaymentHistory";
        public const string SP_GetPaymentHistoryByOrderCode = "SP_GetPaymentHistoryByOrderCode";

        //residence registration
        public const string SP_InsertResidenceRegistration = "SP_InsertResidenceRegistration";
        public const string SP_GetListResidence = "SP_GetListResidence";
        public const string SP_DeleteResidence = "SP_DeleteResidence";
        public const string SP_UpdateResidence = "SP_UpdateResidence";
        public const string SP_GetTotalPaidAmountByRoomBookingId = "SP_GetTotalPaidAmountByRoomBookingId";
        public const string SP_GetMaximumOccupancyByRoomBookingDetailId = "SP_GetMaximumOccupancyByRoomBookingDetailId";
        public const string SP_CheckOut1Residence = "SP_CheckOut1Residence";
        public const string SP_CheckOutResidencesByRoomBookingDetail = "SP_CheckOutResidencesByRoomBookingDetail";
        public const string SP_GetResidenceRegistrationByDate = "SP_GetResidenceRegistrationByDate";
        
        //Edit History
        public const string SP_InsertEditHistory = "SP_InsertEditHistory";
        public const string SP_GetEditHistoryByRoomBookingDetailId = "SP_GetEditHistoryByRoomBookingDetailId";
    }
}
