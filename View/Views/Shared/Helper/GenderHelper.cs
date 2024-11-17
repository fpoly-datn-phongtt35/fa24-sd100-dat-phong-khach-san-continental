using Domain.Enums;
using Microsoft.AspNetCore.Html;

namespace View.Views.Shared.Helper
{
    public class GenderHelper
    {
        public static IHtmlContent DisplayGenderBadge(GenderType type)
        {
            switch (type)
            {
                case GenderType.Unknown:
                    return new HtmlString("<span>Không đề cập</span>");
                case GenderType.Nam:
                    return new HtmlString("<span>Nam</span>");
                case GenderType.Nữ:
                    return new HtmlString("<span'>Nữ</span>");
                default:
                    return HtmlString.Empty;
            }
        }

    }
}
