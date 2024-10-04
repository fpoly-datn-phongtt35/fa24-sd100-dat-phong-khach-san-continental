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
        //service order
        public const string SP_InsertServiceOrder = "SP_InsertServiceOrder";
        public const string SP_GetListServiceOrder = "SP_GetListServiceOrder";
        public const string SP_DeleteServiceOrder = "SP_DeleteServiceOrder";
        public const string SP_UpdateServiceOrder = "SP_UpdateServiceOrder";
        public const string SP_GetServiceOrderById = "SP_GetServiceOrderById";
        //service order detail
        public const string SP_InsertServiceOrderDetail = "SP_InsertServiceOrderDetail";
        public const string SP_GetListServiceOrderDetail = "SP_GetListServiceOrderDetail";
        public const string SP_DeleteServiceOrderDetail = "SP_DeleteServiceOrderDetail";
        public const string SP_UpdateServiceOrderDetail = "SP_UpdateServiceOrderDetail";
        public const string SP_GetServiceOrderDetailById = "SP_GetServiceOrderDetailById";

        //room
        public const string SP_InsertRoom = "SP_InsertRoom";
        public const string SP_GetListRoom = "SP_GetListRoom";
        public const string SP_DeleteRoom = "SP_DeleteRoom";
        public const string SP_GetRoomById = "SP_GetRoomById";
        public const string SP_UpdateRoom = "SP_UpdateRoom";

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
        public const string SP_GetAllAmenities = "SP_GetAllAmenities";
        public const string SP_GetAmenityById = "SP_GetAmenityById";
        public const string SP_UpdateAmenity = "SP_UpdateAmenity";
        public const string SP_DeleteAmenity = "SP_DeleteAmenity";
        public const string SP_RollBackDeletedAmenity = "SP_RollBackDeletedAmenity";
        //roomType
        public const string SP_InsertRoomType = "SP_InsertRoomType";
        public const string SP_GetAllRoomTypes = "SP_GetAllRoomTypes";
        public const string SP_GetRoomTypeById = "SP_GetRoomTypeById";
        public const string SP_UpdateRoomType = "SP_UpdateRoomType";
        public const string SP_DeleteRoomType = "SP_DeleteRoomType";
        public const string SP_RollBackDeletedRoomType = "SP_RollBackDeletedRoomType";
        // Customer
        public const string SP_InsertCustomer = "SP_InsertCustomer";
        public const string SP_GetCustomerById = "SP_GetCustomerById";
        public const string SP_DeleteCustomer = "SP_DeleteCustomer";
        public const string SP_UpdateCustomer = "SP_UpdateCustomer";
        public const string SP_GetAllCustomer = "SP_GetAllCustomers";
        // amenityRoom
        public const string SP_InsertAmenityRoom = "SP_InsertAmenityRoom";
        public const string SP_GetAllAmenityRooms = "SP_GetAllAmenityRooms";
        public const string SP_GetAmenityRoomById = "SP_GetAmenityRoomById";
        public const string SP_UpdateAmenityRoom = "SP_UpdateAmenityRoom";
        public const string SP_DeleteAmenityRoom = "SP_DeleteAmenityRoom";
        public const string SP_GetAmenityRoomsByRoomTypeId = "SP_GetAmenityRoomsByRoomTypeId";
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
        public const string SP_GetAllRoomTypeServices = "SP_GetAllRoomTypeServices";
        public const string SP_GetRoomTypeServiceById = "SP_GetRoomTypeServiceById";
        public const string SP_UpdateRoomTypeService = "SP_UpdateRoomTypeService";
        public const string SP_DeleteRoomTypeService = "SP_DeleteRoomTypeService";
        public const string SP_GetRoomTypeServicesByRoomTypeId = "SP_GetRoomTypeServicesByRoomTypeId";
        //Staff 
        public const string SP_GetAllStaff = "SP_GetStaffList";
        public const string SP_CreateStaff = "SP_InsertStaff";
        public const string SP_DeleteStaff = "SP_DeleteStaff";
        public const string SP_UpdateStaff = "SP_UpdateStaff";
        public const string SP_GetStaffById = "SP_GetStaffById";
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
    }
}
