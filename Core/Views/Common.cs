
using System;
using System.Collections.Generic;

namespace WBEADMS
{

    public static class ViewsCommon
    {
        public const string UIIconImageMapFile = "/content/images/ui-icons_056b93_256x240.png";
        public const string SpacerGifFile = "/content/images/spacer.gif";
        const int IconWidth = 16; // width of icons
        const int IconHeight = 16; // height of icons

        public static string FetchDateFormat(bool isJavaScript)
        {
            if (isJavaScript)
            {
                return System.Configuration.ConfigurationManager.AppSettings["JqueryDateFormat"];
            }
            return System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
        }

        public static string FetchTimeFormat()
        {
            return System.Configuration.ConfigurationManager.AppSettings["TimeFormat"];
        }

        public static string FetchDateTimeFormat()
        {
            return FetchDateFormat(false) + " " + FetchTimeFormat(/*false*/);
        }

        public static List<string> ToStringList<T>(this List<T> list)
        {
            var newlist = new List<string>();
            foreach (T item in list)
            {
                newlist.Add(item.ToString());
            }

            return newlist;
        }

        public static string IconImageBackgroundStyle(int row, int col)
        {
            return IconImageBackgroundStyle(row, col, UIIconImageMapFile);
        }

        public static string IconImageBackgroundStyle(int row, int col, string customImageMap)
        {
            string row_px = (-row * IconHeight).ToString();
            string col_px = (-col * IconWidth).ToString();
            return
                "background: transparent url('" + customImageMap + "') " +
                "no-repeat scroll " + col_px + "px " + row_px + "px";
        }

        /// <summary>Returns an image tag containing an icon from ui-icons.png</summary>
        /// <param name="name">name attribute of image tag</param>
        /// <param name="row">Row of icon. 0 = first row</param>
        /// <param name="col">Column of icon. 0 = first col</param>
        public static string IconImageTag(string name, int row, int col)
        {
            return IconImageTag(name, row, col, UIIconImageMapFile);
        }

        /// <summary>Returns an image tag containing an icon from an image map i.e. ui-icons.png</summary>
        /// <param name="name">name attribute of image tag</param>
        /// <param name="row">Row of icon. 0 = first row</param>
        /// <param name="col">Column of icon. 0 = first col</param>
        /// <param name="customImageMap">image file</param>
        public static string IconImageTag(string name, int row, int col, string customImageMap)
        {
            return "<img name=\"" + name + "\" src=\"" + SpacerGifFile + "\" height=\"" + IconHeight.ToString() + "\" width=\"" + IconWidth.ToString() + "\" " +
                "style=\"" + IconImageBackgroundStyle(row, col, customImageMap) + "\" />";
        }

        public static string Spacer()
        {
            return String.Format("<img src=\"{0}\" height=\"{1}\" width=\"{2}\"",
                SpacerGifFile, IconHeight, IconWidth);
        }
    }
}