
using System;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.Helpers.CalendarHelpers
{
    public static class CalendarHelper
    {
        /// <summary>Returns "forecasted", "saved", or "committed", depending on if it has been not deployed, deployed, or shipped.</summary>
        public static string CalendarLinkCss(this HtmlHelper helper, ChainOfCustody chainOfCustody)
        {
            if (chainOfCustody.id.IsBlank())
            {
                return "forecasted";
            }
            else
            {
                if (chainOfCustody.Status.IsComplete)
                {
                    return "committed";
                }
                else
                {
                    switch (chainOfCustody.Status.name)
                    {
                        case "Retrieved":
                        case "Shipping":
                            return "retrieved";
                        default:
                            return "saved";
                    }
                }
            }
        }

        public static string CalendarCellCss(this HtmlHelper helper, WBEADMS.DocIt.Controllers.CalendarDate cell)
        {
            int dayofweek = (int)cell.Date.DayOfWeek;  // 0~6 for Sunday~Saturday

            // determine css class for cell
            string cellClass = "callCell" + dayofweek.ToString() + " "; // callCell0 ~ callCell6
            if (cell.Date.Month == (int)helper.ViewData["this_month"] && cell.Date.Year == (int)helper.ViewData["this_year"])
            {
                if (cell.Date == DateTime.Today)
                {
                    cellClass += "calCellToday";
                }
                else
                {
                    cellClass += "calCellThisMonth";
                }
            }
            else
            {
                cellClass += "calCellOffMonth";
            }

            return cellClass;
        }
    }
}