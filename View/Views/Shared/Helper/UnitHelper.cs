using Domain.Enums;
using Microsoft.AspNetCore.Html;

namespace View.Views.Shared.Helper
{
    public class UnitHelper
    {
        public static IHtmlContent DisplayUnit(UnitType type)
        {
            switch (type)
            {
                case UnitType.Times:
                    return new HtmlString("<span>Lần</span>");
                case UnitType.Ticket:
                    return new HtmlString("<span>Vé</span>");
                case UnitType.Gram:
                    return new HtmlString("<span'>Gram</span>");
                case UnitType.Kilo:
                    return new HtmlString("<span>Kilogram</span>");
                case UnitType.Ton:
                    return new HtmlString("<span>Tấn</span>");
                case UnitType.Hour:
                    return new HtmlString("<span>Giờ</span>");
                case UnitType.Bag:
                    return new HtmlString("<span>Túi</span>");
                case UnitType.Bottle:
                    return new HtmlString("<span>Chai</span>");
                case UnitType.Bowl:
                    return new HtmlString("<span>Tô</span>");
                case UnitType.Cup:
                    return new HtmlString("<span>Ly</span>");
                case UnitType.Carton:
                    return new HtmlString("<span>Thùng</span>");
                case UnitType.Serving:
                    return new HtmlString("<span>Phần</span>");
                default:
                    return HtmlString.Empty;
            }
        }
    }
}
