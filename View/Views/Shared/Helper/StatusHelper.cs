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
                return new HtmlString("<span class='badge bg-secondary'>Đang tiến hành kích hoạt</span>");
            case EntityStatus.PendingForConfirmation:
                return new HtmlString("<span class='badge bg-secondary'>Đang tiến hành xác nhận</span>");
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
            EntityStatus.InActive =>  new HtmlString("Không hoạt động"),
            EntityStatus.Deleted =>  new HtmlString("Đã xóa"),
            EntityStatus.Pending => new HtmlString("Hoãn"),
            EntityStatus.PendingForActivation => new HtmlString("Đang tiến hành kích hoạt"),
            EntityStatus.PendingForConfirmation => new HtmlString("Đang tiến hành xác nhận"),
            EntityStatus.PendingForApproval => new HtmlString("Đang chờ phê duyệt"),
            EntityStatus.Locked => new HtmlString("Khóa"),
            _ => new HtmlString("null")
        };
    }
}
