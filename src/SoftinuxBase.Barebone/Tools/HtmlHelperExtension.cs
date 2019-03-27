using Microsoft.AspNetCore.Mvc.Rendering;

namespace SoftinuxBase.Barebone.Tools
{
    public static class HtmlHelperExtension
    {
        public static bool IsDebug(this IHtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}