using Domain.Enums;
using Microsoft.AspNetCore.Html;

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
            case EntityStatus.Locked:
                return new HtmlString("<span class='badge bg-gradient'>Đã khóa</span>");
            default:
                return HtmlString.Empty;
        }
    }
}
