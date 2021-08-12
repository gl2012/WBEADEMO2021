

using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WBEADMS.Helpers
{
    public static class BoxedValidationSummaryHelper
    {
        public static string BoxedValidationSummary(this HtmlHelper htmlHelper, string message)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
            {
                return "<div class=\"errorState\">" + htmlHelper.ValidationSummary(message) + "</div>";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string BoxedValidationSummary1(this HtmlHelper htmlHelper, string message)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
            {
                return "<div class=\"errorState\">" + htmlHelper.ValidationSummary(message) + "</div>";
            }
            else
            {
                return string.Empty;
            }
        }

    }
}