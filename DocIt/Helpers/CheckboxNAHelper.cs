
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WBEADMS.Models;

namespace WBEADMS.Helpers.CheckBoxNAHelper
{
    public static class CheckBoxNAHelper
    {
        /// <summary>Returns whether jQuery CheckBox is enabled.</summary>
        public static bool CheckBoxNA(this HtmlHelper helper)
        {
            return ChainOfCustody.jQueryCheckBoxEnabled();
        }

        public static string CheckBoxNA(this HtmlHelper helper, string name, string value)
        {
            bool isChecked = // checked = N/A, unchecked = switched from N/A; jQuery CheckBox - ON=checked, OFF (or N/A)=unchecked
                !value.IsBlank();

            // xxx srsly... what's all this junk?
            //|| // TODO: checkbox should be checked only if value is also associated with field not left blank column
            ////!helper.ViewData[name].ToString().IsBlank() &&
            //((ChainOfCustody)helper.ViewData.Model).Status.OnSave != null;
            if (!CheckBoxNA(helper))
            {
                isChecked = !isChecked; // if jQuery CheckBox is disabled, then reverse the logic: N/A=checked, otherwise unchecked
            }

            return helper.CheckBox("cb-" + name, isChecked, new { @class = "na-checkbox" }).ToString();
        }
    }
}