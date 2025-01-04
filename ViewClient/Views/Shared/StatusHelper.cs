using Domain.Enums;
using Microsoft.AspNetCore.Html;

namespace ViewClient.Views.Shared
{
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

        public static IHtmlContent DisplayNameForEnum(this EntityStatus status)
        {
            return status switch
            {
                EntityStatus.Active => new HtmlString("Hoạt động"),
                EntityStatus.InActive => new HtmlString("Không hoạt động"),
                EntityStatus.Deleted => new HtmlString("Đã xóa"),
                EntityStatus.Pending => new HtmlString("Hoãn"),
                EntityStatus.PendingForActivation => new HtmlString("Đang chờ kích hoạt"),
                EntityStatus.PendingForConfirmation => new HtmlString("Đang chờ xác nhận"),
                EntityStatus.PendingForApproval => new HtmlString("Đang chờ phê duyệt"),
                EntityStatus.Locked => new HtmlString("Khóa"),
                _ => new HtmlString("null")
            };
        }
        public static IHtmlContent DisplayRoomStatusBadge(RoomStatus status)
        {
            switch (status)
            {
                case RoomStatus.Vacant:
                    return new HtmlString("<span class='badge bg-success fs-4'>Trống</span>");
                case RoomStatus.OutOfOrder:
                    return new HtmlString("<span class='badge bg-warning fs-4'>Hỏng</span>");
                case RoomStatus.Deleted:
                    return new HtmlString("<span class='badge bg-danger fs-4'>Đã xóa</span>");
                case RoomStatus.Occupied:
                    return new HtmlString("<span class='badge bg-primary fs-4'>Đang sử dụng</span>");
                case RoomStatus.Reserved:
                    return new HtmlString("<span class='badge bg-info fs-4'>Đã đặt</span>");
                case RoomStatus.Cleaned:
                    return new HtmlString("<span class='badge bg-success fs-4'>Đã dọn</span>");
                case RoomStatus.Dirty:
                    return new HtmlString("<span class='badge bg-danger fs-4'>Bẩn</span>");
                case RoomStatus.Inspected:
                    return new HtmlString("<span class='badge bg-secondary fs-4'>Đã kiểm tra</span>");
                case RoomStatus.DoNotDisturb:
                    return new HtmlString("<span class='badge bg-dark fs-4'>Không làm phiền</span>");
                case RoomStatus.CheckIn:
                    return new HtmlString("<span class='badge bg-primary fs-4'>Đã nhận phòng</span>");
                case RoomStatus.CheckOut:
                    return new HtmlString("<span class='badge bg-secondary fs-4'>Đã trả phòng</span>");
                case RoomStatus.AwaitingConfirmation:
                    return new HtmlString("<span class='badge bg-warning fs-4'>Chờ xác nhận</span>");
                default:
                    return HtmlString.Empty;
            }
        }


        public static IHtmlContent DisplayRoomNameForEnum(this RoomStatus status)
        {
            return status switch
            {
                RoomStatus.Vacant => new HtmlString("Trống"),
                RoomStatus.OutOfOrder => new HtmlString("Hỏng"),
                RoomStatus.Deleted => new HtmlString("Đã xóa"),
                RoomStatus.Occupied => new HtmlString("Đang sử dụng"),
                RoomStatus.Reserved => new HtmlString("Đã đặt"),
                RoomStatus.Cleaned => new HtmlString("Đã dọn"),
                RoomStatus.Dirty => new HtmlString("Bẩn"),
                RoomStatus.Inspected => new HtmlString("Đã kiểm tra"),
                RoomStatus.DoNotDisturb => new HtmlString("Không làm phiền"),
                RoomStatus.CheckIn => new HtmlString("Đã nhận phòng"),
                RoomStatus.CheckOut => new HtmlString("Đã trả phòng"),
                RoomStatus.AwaitingConfirmation => new HtmlString("Chờ xác nhận"),
                _ => new HtmlString("Không xác định")
            };
        }
        public static IHtmlContent DisplayRoomBookingStatus(this RoomBookingStatus status)
        {
            string statusText;
            string badgeClass;

            switch (status)
            {
                case RoomBookingStatus.NEW:
                    statusText = "Mới";
                    badgeClass = "badge bg-info"; // Badge màu xanh
                    break;
                case RoomBookingStatus.PENDING:
                    statusText = "Đang chờ";
                    badgeClass = "badge bg-warning"; // Badge màu vàng
                    break;
                case RoomBookingStatus.PAID:
                    statusText = "Đã thanh toán";
                    badgeClass = "badge bg-success"; // Badge màu xanh lá
                    break;
                case RoomBookingStatus.CANCELLED:
                    statusText = "Đã hủy";
                    badgeClass = "badge bg-danger"; // Badge màu đỏ
                    break;
                case RoomBookingStatus.FAILED:
                    statusText = "Thất bại";
                    badgeClass = "badge bg-danger"; // Badge màu đỏ
                    break;
                case RoomBookingStatus.DEPOSITED:
                    statusText = "Đã đặt cọc";
                    badgeClass = "badge bg-primary"; // Badge màu xanh dương
                    break;
                default:
                    statusText = "Không xác định";
                    badgeClass = "badge bg-secondary"; // Badge màu xám
                    break;
            }

            // Trả về nội dung HTML với thẻ span và lớp badge
            return new HtmlString($"<span class='{badgeClass}'>{statusText}</span>");
        }
        public static IHtmlContent DisplayBookingType(this BookingType? bookingType)
        {
            string typeText;
            string badgeClass;

            switch (bookingType)
            {
                case BookingType.Online:
                    typeText = "Trực tuyến";
                    badgeClass = "badge bg-success"; // Badge màu xanh lá
                    break;
                case BookingType.Offline:
                    typeText = "Ngoại tuyến";
                    badgeClass = "badge bg-secondary"; // Badge màu xám
                    break;
                default:
                    typeText = "Không xác định";
                    badgeClass = "badge bg-danger"; // Badge màu đỏ
                    break;
            }

            // Trả về nội dung HTML với thẻ span và lớp badge
            return new HtmlString($"<span class='{badgeClass}'>{typeText}</span>");
        }
    }
}
