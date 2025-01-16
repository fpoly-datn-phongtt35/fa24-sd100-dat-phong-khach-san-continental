using Domain.Enums;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace View.Views.Shared.Helper;

public static class StatusHelper
{
    public static IHtmlContent DisplayStatusBadge(EntityStatus status)
    {
        switch (status)
        {
            case EntityStatus.Active:
                return new HtmlString("<span class='badge bg-success'>Hoạt động</span>");
            case EntityStatus.InActive:
                return new HtmlString("<span class='badge bg-secondary'>Không hoạt động</span>");
            case EntityStatus.Deleted:
                return new HtmlString("<span class='badge bg-danger'>Đã xóa</span>");
            case EntityStatus.Pending:
                return new HtmlString("<span class='badge bg-secondary'>Hoãn</span>");
            case EntityStatus.PendingForActivation:
                return new HtmlString("<span class='badge bg-secondary'>Đang chờ kích hoạt</span>");
            case EntityStatus.PendingForConfirmation:
                return new HtmlString("<span class='badge bg-secondary'>Đang chờ xác nhận</span>");
            case EntityStatus.PendingForApproval:
                return new HtmlString("<span class='badge bg-secondary'>Đang chờ phê duyệt</span>");
            case EntityStatus.Locked:
                return new HtmlString("<span class='badge bg-gradient'>Đã khóa</span>");
            default:
                return HtmlString.Empty;
        }
    }
    
    public static IHtmlContent DisplayRoomBookingStatusBadge(RoomBookingStatus roomBookingStatus)
    {
        switch (roomBookingStatus)
        {
            case RoomBookingStatus.NEW:
                return new HtmlString("<span class='badge bg-secondary'>Mới đặt</span>");
            case RoomBookingStatus.PENDING:
                return new HtmlString("<span class='badge bg-secondary'>Chờ thanh toán</span>");
            case RoomBookingStatus.PAID:
                return new HtmlString("<span class='badge bg-success'>Hoàn thành</span>");
            case RoomBookingStatus.CANCELLED:
                return new HtmlString("<span class='badge bg-danger'>Đã hủy</span>");
            case RoomBookingStatus.FAILED:
                return new HtmlString("<span class='badge bg-danger'>Thất bại</span>");
            case RoomBookingStatus.DEPOSITED:
                return new HtmlString("<span class='badge bg-danger'>Đã cọc</span>");
            default:
                return HtmlString.Empty;
        }
    }
    
    public static IHtmlContent DisplayNameEntityStatus(this EntityStatus status)
    {
        return status switch
        {
            EntityStatus.Active => new HtmlString("Hoạt động"),
            EntityStatus.InActive =>  new HtmlString("Không hoạt động"),
            EntityStatus.Deleted =>  new HtmlString("Đã xóa"),
            EntityStatus.Pending => new HtmlString("Hoãn"),
            EntityStatus.PendingForActivation => new HtmlString("Chờ kích hoạt"),
            EntityStatus.PendingForConfirmation => new HtmlString("Chờ xác nhận"),
            EntityStatus.PendingForApproval => new HtmlString("Chờ phê duyệt"),
            EntityStatus.Locked => new HtmlString("Khóa"),
            _ => new HtmlString("null")
        };
    }
    
    public static IHtmlContent DisplayNameForEditHistory(this For forHistory)
    {
        return forHistory switch
        {
            For.Expenses => new HtmlString("Phí hư tổn"),
            For.CheckInReality =>  new HtmlString("Ngày nhận thực tế"),
            For.CheckOutReality =>  new HtmlString("Ngày trả thực tế"),
            _ => new HtmlString("null")
        };
    }
    
    public static IHtmlContent DisplayNameRoomStatus(this RoomStatus status)
    {
        return status switch
        {
            RoomStatus.Vacant => new HtmlString("Trống"),
            RoomStatus.OutOfOrder=>  new HtmlString("Hết phòng"),
            RoomStatus.Deleted =>  new HtmlString("Đã xóa"),
            RoomStatus.Occupied => new HtmlString("Không trống"),
            RoomStatus.Reserved => new HtmlString("Đạng dọn dẹp"),
            RoomStatus.Cleaned => new HtmlString("Sạch sẽ"),
            RoomStatus.Dirty => new HtmlString("Bẩn"),
            RoomStatus.Inspected => new HtmlString("Đã kiểm tra"),
            RoomStatus.DoNotDisturb => new HtmlString("Đừng làm phiền"),
            RoomStatus.CheckIn => new HtmlString("Check In"),
            RoomStatus.CheckOut => new HtmlString("Check Out"),
            RoomStatus.AwaitingConfirmation => new HtmlString("Chờ xác nhận"),
            _ => new HtmlString("null")
        };
    }
    public static IHtmlContent DisplayRoomStatusBadge(RoomStatus roomStatus)
    {
        switch (roomStatus)
        {
            case RoomStatus.Vacant:
                return new HtmlString("<span class='badge bg-success'>Trống</span>");
            case RoomStatus.OutOfOrder:
                return new HtmlString("<span class='badge bg-secondary'>Hết phòng</span>");
            case RoomStatus.Deleted:
                return new HtmlString("<span class='badge bg-danger'>Đã xóa</span>");
            case RoomStatus.Occupied:
                return new HtmlString("<span class='badge bg-secondary'>Không trống</span>");
            case RoomStatus.Reserved:
                return new HtmlString("<span class='badge bg-secondary'>Đạng dọn dẹp</span>");
            case RoomStatus.Cleaned:
                return new HtmlString("<span class='badge bg-secondary'>Sạch sẽ</span>");
            case RoomStatus.Dirty:
                return new HtmlString("<span class='badge bg-secondary'>Bẩn</span>");
            case RoomStatus.Inspected:
                return new HtmlString("<span class='badge bg-secondary'>Đã kiểm tra</span>");
            case RoomStatus.DoNotDisturb:
                return new HtmlString("<span class='badge bg-secondary'>Đừng làm phiền</span>");
            case RoomStatus.CheckIn:
                return new HtmlString("<span class='badge bg-secondary'>Check In/span>");
            case RoomStatus.CheckOut:
                return new HtmlString("<span class='badge bg-secondary'>Check Out/span>");
            case RoomStatus.AwaitingConfirmation:
                return new HtmlString("<span class='badge bg-secondary'>Chờ xác nhận</span>");
            default:
                return HtmlString.Empty;
        }
    }
    public static IHtmlContent DisplayStatusBadge1(RoomStatus status)
    {
        switch (status)
        {
            case RoomStatus.Vacant:
                return new HtmlString("<span class='badge bg-success'>Hoạt động</span>");
            case RoomStatus.OutOfOrder:
                return new HtmlString("<span class='badge bg-secondary'>Không hoạt động</span>");
            case RoomStatus.Deleted:
                return new HtmlString("<span class='badge bg-danger'>Đã xóa</span>");
            case RoomStatus.Occupied:
                return new HtmlString("<span class='badge bg-secondary'>Hoãn</span>");
            case RoomStatus.Reserved:
                return new HtmlString("<span class='badge bg-secondary'>Chờ kích hoạt</span>");
            case RoomStatus.Cleaned:
                return new HtmlString("<span class='badge bg-secondary'>Chờ xác nhận</span>");
            case RoomStatus.Dirty:
                return new HtmlString("<span class='badge bg-secondary'>Chờ phê duyệt</span>");
            case RoomStatus.Inspected:
                return new HtmlString("<span class='badge bg-secondary'>Chờ phê duyệt</span>");
            case RoomStatus.DoNotDisturb:
                return new HtmlString("<span class='badge bg-secondary'>Chờphê duyệt</span>");
            case RoomStatus.CheckIn:
                return new HtmlString("<span class='badge bg-secondary'>Chờ phê duyệt</span>");
            case RoomStatus.CheckOut:
                return new HtmlString("<span class='badge bg-secondary'>Chờ phê duyệt</span>");
            case RoomStatus.AwaitingConfirmation:
                return new HtmlString("<span class='badge bg-secondary'>Chờ phê duyệt</span>");
            default:
                return HtmlString.Empty;
        }
    }
    public static IHtmlContent DisplayNameForGender(this GenderType gender)
    {
        return gender switch
        {
            GenderType.Unknown => new HtmlString("Không rõ"),
            GenderType.Nam => new HtmlString("Nam"),
            GenderType.Nữ => new HtmlString("Nữ"),
            _ => new HtmlString("null")
        };
    }
}
