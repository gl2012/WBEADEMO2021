/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;


namespace WBEADMS.Views
{
    public static class HtmlHelperExtensions
    {

        #region MenuLinks
        public static string MainMenuLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return helper.ActionLink(linkText, actionName, new { controller = controllerName }, new { @class = "mainlevel" }).ToString();
        }
        #endregion

        #region jquery script stuff

        #region Date Time Helper Functions

        /// <summary>Create a jQuery UI Datepicker with associated text fields for time.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of datetime field</param>
        public static string DateTimePicker(this HtmlHelper helper, string name)
        {
            var options = new { };
            return helper.DateTimePicker(name, options);
        }

        /// <summary>Create a jQuery UI Datepicker with associated text fields for time.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of datetime field</param>
        public static string DateTimePicker(this HtmlHelper helper, string name, string value)
        {
            return DateTimePicker(helper, name, value, null);
        }

        /// <summary>Create a jQuery UI Datepicker with associated text fields for time.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of datetime field</param>
        /// <param name="options">jQuery UI Datepicker options; example: new { changeYear = "true", yearRange = "-1:+3" }</param>
        public static string DateTimePicker(this HtmlHelper helper, string name, object options)
        {
            string value = GetHelperValue(helper, name);
            return DateTimePicker(helper, name, value, options);
        }

        /// <summary>Create a jQuery UI Datepicker with associated text fields for time.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of datetime field</param>
        /// <param name="value">value of field</param>
        /// <param name="options">jQuery UI Datepicker options; example: new { changeYear = "true", yearRange = "-1:+3" }</param>
        public static string DateTimePicker(this HtmlHelper helper, string name, string value, object options)
        {
            Dictionary<string, string> dictOptions = ObjectToDictionary(options);
            return helper.DateTimePicker(name, value, dictOptions);
        }

        private static string DateTimePicker(this HtmlHelper helper, string name, string value, Dictionary<string, string> options)
        {
            string datePickerName = name + "_date";
            string hourName = name + "_hour";
            string minuteName = name + "_mins";

            //if the parse fails blanks will be used.
            string date = "";
            string hours = "";
            string minutes = "";

            DateTime datetime;

            if (!value.IsBlank())
            {
                if (DateTime.TryParse(value, out datetime))
                {
                    date = datetime.Date.ToISODate();
                    hours = datetime.Hour.ToString("00");
                    minutes = datetime.Minute.ToString("00");
                }
                else
                {
                    // Total Hackery, maybe fix this someday...
                    date = value.Split(' ')[0];
                    hours = value.Split(' ')[1].Split(':')[0];
                    minutes = value.Split(':')[1];
                }
            }

            string datePicker = helper.DatePicker(datePickerName, date, options);
            string hourField = helper.TextBox(hourName, hours, new { @class = "inputHours", maxlength = 2 }).ToString();
            string minuteField = helper.TextBox(minuteName, minutes, new { @class = "inputMins", maxlength = 2 }).ToString();
            string dateTimeField = helper.Hidden(name, value, new { @class = "inputHidden" }).ToString();

            return "<span class=\"datetimepicker\">" + datePicker + " " + hourField + " : " + minuteField + " " + dateTimeField + "</span>";
        }

        #endregion

        #region Date Helper Functions

        /// <summary>Create a jQuery UI Datepicker.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of date field</param>
        public static string DatePicker(this HtmlHelper helper, string name)
        {
            var options = new { };
            return DatePicker(helper, name, options);
        }

        /// <summary>Create a jQuery UI Datepicker.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of date field</param>
        /// <param name="value">value of field</param>
        public static string DatePicker(this HtmlHelper helper, string name, string value)
        {
            var options = new { };
            return DatePicker(helper, name, value, options);
        }

        /// <summary>Create a jQuery UI Datepicker.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of date field</param>
        /// <param name="options">jQuery UI Datepicker options; example: new { changeYear = "true", yearRange = "-1:+3" }</param>
        public static string DatePicker(this HtmlHelper helper, string name, object options)
        {
            var value = GetHelperValue(helper, name);
            return DatePicker(helper, name, value, options);
        }

        /// <summary>Create a jQuery UI Datepicker.</summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="name">name/id of date field</param>
        /// <param name="value">value of field</param>
        /// <param name="options">jQuery UI Datepicker options; example: new { changeYear = "true", yearRange = "-1:+3" }</param>
        public static string DatePicker(this HtmlHelper helper, string name, object value, object options)
        {
            var dictOptions = ObjectToDictionary(options);
            return DatePicker(helper, name, value, dictOptions);
        }

        private static string DatePicker(this HtmlHelper helper, string name, object value, Dictionary<string, string> options)
        {
            DateTime date = DateTime.MinValue;

            if (value is string)
            {
                if (DateTime.TryParse((string)value, out date))
                {
                    value = date.ToISODate();
                }
            }

            if (value is DateTime)
            {
                date = (DateTime)value;
                value = date.ToISODate();
            }

            var defaultOptions = new Dictionary<string, string>() {
                { "duration", "fast" },
                { "dateFormat", "yy-mm-dd" }, // ISO 8601 yy-mm-dd
                { "hideIfNoPrevNext", "true" },
               { "changeMonth","true" },
                {  "changeYear", "true"} ,
                { "yearRange","-30:+10" }
                // NOTE: no min/max date as per conversation with Sanjay 2009-12-04
                ////{ "minDate", "new Date(2009,0,1)" }, // default min date is 2009-01-01 (zeroth month is January)
                ////{ "maxDate", "+1Y" },
                ////{ "changeYear", "true" }
        };

            options = defaultOptions.Merge(options);

            /*
            if (!options.ContainsKey("maxDate") && !options.ContainsKey("minDate") && !options.ContainsKey("yearRange")) {
                options = options.Merge(new Dictionary<string, string>() {
                    { "minDate", "new Date(2009,0,1)" }, // default min date is 2009-01-01 (zeroth month is January)
                    { "maxDate", "+3M" },
                });
            }
             */

            var optionList = new List<string>();
            foreach (var item in options)
            {
                string val = item.Value;
                if (val.IsBlank()) { continue; }
                if (!val.IsInt() && !val.IsBool() && !val.Contains("new Date("))
                {
                    val = "'" + val + "'";
                }
                optionList.Add("                " + item.Key + ": " + val);
            }

            if (!options.ContainsKey("onSelect"))
            {
                optionList.Add("                onSelect: function(t, inst) { $(inst.input).change(); }");
            }

            string command = "            $('#TAGID').datepicker().datepicker('option', {\n";
            command += String.Join(",\n", optionList.ToArray());
            command += "\n            });\n";

            if (date > DateTime.MinValue)
            {
                command += "            $('#TAGID').datepicker('setDate', new Date(" + date.Year + "," + (date.Month - 1) + "," + date.Day + "));\n";
            }
            else if (value == null)
            {
                command += "            $('#TAGID').val('');\n";
            }
            else
            {
                command += "            $('#TAGID').val('" + value.ToString() + "');\n";
            }

            JQueryAddCommand(helper, command.Replace("TAGID", name));
            return helper.TextBox(name, value, new { @class = "datepicker", maxlength = 10 }).ToString();
        }

        #endregion

        private static void JQueryAddCommand(HtmlHelper helper, string command)
        {
            var javascript = (List<string>)helper.ViewContext.TempData["JQueryScriptTag"];
            if (javascript == null) { javascript = new List<string>(); }
            javascript.Add(command);
            helper.ViewContext.TempData["JQueryScriptTag"] = javascript;
        }

        public static string JQueryCommands(this HtmlHelper helper)
        {
            var javascript = (List<string>)helper.ViewContext.TempData["JQueryScriptTag"];
            if (javascript == null) { javascript = new List<string>(); }
            return String.Join("\n", javascript.ToArray());
        }

        #endregion

        #region PickerScripts

        public static string DatePickerScript(string name)
        {
            return @"
            $(""#" + name + @""").datepicker({
                dateFormat: '" + ViewsCommon.FetchDateFormat(true) + @"'
            });";
        }

        public static string TimePickerScript(string name)
        {
            return @"
            $('#" + name + @"').timepickr({
                handle: '#trigger-test',
                convention: 24,
                resetOnBlur: false,
                updateLive: true
            });";
        }

        #endregion

        #region ToStringOrDefaultTo functions
        public static string ToStringOrDefaultTo(object target, string defaultValue)
        {
            if (target == null)
            {
                return defaultValue;
            }

            return target.ToString();
        }

        public static string ToStringOrDefaultTo(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return "unspecified";
            }

            return target.ToString();
        }

        public static string ToStringOrDefaultTo(string target, string defaultValue)
        {
            if (string.IsNullOrEmpty(target))
            {
                return defaultValue;
            }

            return target.ToString();
        }

        public static string ToStringOrDefaultTo(DateTime? target, string defaultValue, string format)
        {
            if (target == null)
            {
                return defaultValue;
            }

            return target.Value.ToString(format);
        }
        #endregion

        #region PostLink
        /// <summary>Creates a link that posts via AJAX.</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkText">text for link</param>
        /// <param name="linkRouteValues">object route; this is the action that is posted to; i.e. new {action="Details", id=item.id}</param>
        public static string PostLink(this HtmlHelper helper, string linkText, object linkRouteValues)
        {
            return PostLink(helper, linkText, linkRouteValues, new RouteValueDictionary());
        }

        /// <summary>Creates a link that posts via AJAX.</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkText">text for link</param>
        /// <param name="linkRouteValues">object route; this is the action that is posted to; i.e. new {action="Details", id=item.id}</param>
        /// <param name="htmlAttributes">
        /// html attributes of link tag; The following attributes are specially handled:
        /// onclick=appended before the JQuery post.
        /// postData=object that will be converted into JSON for post.
        /// postFunction=javascript code that will be executed after post, handling the 'data' variable</param>
        public static string PostLink(this HtmlHelper helper, string linkText, object linkRouteValues, object htmlAttributes)
        {
            return PostLink(helper, linkText, linkRouteValues, new RouteValueDictionary(htmlAttributes));
        }

        private static string PostLink(this HtmlHelper helper, string linkText, object linkRouteValues, IDictionary<string, object> htmlAttributes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            return PostLink(helper, linkText, url.RouteUrl(linkRouteValues), htmlAttributes);
        }

        private static string PostLink(this HtmlHelper helper, string linkText, string linkUrl, IDictionary<string, object> htmlAttributes)
        {
            SetPostAttributes(ref htmlAttributes);
            return LinkBuilder(helper.Encode(linkText), linkUrl, htmlAttributes);
        }
        #endregion

        #region ImageLink
        /// <summary>Creates a image link</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkRouteValues">object route</param>
        /// <param name="imageUrl">url of image; will convert virtual paths containing ~ to application paths</param>
        public static string ImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl)
        {
            return ImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>Creates a image link</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkRouteValues">object route</param>
        /// <param name="imageUrl">url of image; will convert virtual paths containing ~ to application paths</param>
        /// <param name="linkHtmlAttributes">html attributes of link tag; </param>
        public static string ImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, object linkHtmlAttributes)
        {
            return ImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(linkHtmlAttributes), new RouteValueDictionary());
        }

        /// <summary>Creates a image link</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkRouteValues">object route</param>
        /// <param name="imageUrl">url of image; will convert virtual paths containing ~ to application paths</param>
        /// <param name="linkHtmlAttributes">html attributes of link tag; The following attributes are specially handled:
        ///     onclick=appended before the JQuery post.
        ///     postData=object that will be converted into JSON for post.
        ///     postFunction=javascript code that will be executed after post, handling the 'data' variable</param>
        /// <param name="imageHtmlAttributes">html attributes of the img tag</param>
        public static string ImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            return ImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(linkHtmlAttributes), new RouteValueDictionary(imageHtmlAttributes));
        }

        private static string ImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, IDictionary<string, object> linkHtmlAttributes, IDictionary<string, object> imageHtmlAttributes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            string imageTag = ImageBuilder(url.Content(imageUrl), imageHtmlAttributes);
            return LinkBuilder(imageTag, url.RouteUrl(linkRouteValues), linkHtmlAttributes);
        }
        #endregion

        #region PostImageLink
        public static string PostImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl)
        {
            return PostImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>Creates a image link that posts via AJAX.</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkRouteValues">object route; this is the action that is posted to</param>
        /// <param name="imageUrl">url of image; will convert virtual paths containing ~ to application paths</param>
        /// <param name="linkHtmlAttributes">html attributes of link tag; The following attributes are specially handled:
        /// <para>onclick=appended before the JQuery post.</para>
        /// <para>postData=object that will be converted into JSON for post.</para>
        /// <para>postFunction=javascript code that will be executed after post, handling the 'data' variable</para>
        /// </param>
        public static string PostImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, object linkHtmlAttributes)
        {
            return PostImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(linkHtmlAttributes), new RouteValueDictionary());
        }

        /// <summary>Creates a image link that posts via AJAX.</summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="linkRouteValues">object route; this is the action that is posted to</param>
        /// <param name="imageUrl">url of image; will convert virtual paths containing ~ to application paths</param>
        /// <param name="linkHtmlAttributes">html attributes of link tag; The following attributes are specially handled:
        /// <para>onclick=appended before the JQuery post.</para>
        /// <para>postData=object that will be converted into JSON for post.</para>
        /// <para>postFunction=javascript code that will be executed after post, handling the 'data' variable</para>
        /// </param>
        /// <param name="imageHtmlAttributes">html attributes of the img tag</param>
        public static string PostImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            return PostImageLink(helper, linkRouteValues, imageUrl, new RouteValueDictionary(linkHtmlAttributes), new RouteValueDictionary(imageHtmlAttributes));
        }

        private static string PostImageLink(this HtmlHelper helper, object linkRouteValues, string imageUrl, IDictionary<string, object> linkHtmlAttributes, IDictionary<string, object> imageHtmlAttributes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            return PostImageLink(helper, url.RouteUrl(linkRouteValues), url.Content(imageUrl), linkHtmlAttributes, imageHtmlAttributes);
        }

        private static string PostImageLink(this HtmlHelper helper, string linkUrl, string imageUrl, IDictionary<string, object> linkHtmlAttributes, IDictionary<string, object> imageHtmlAttributes)
        {
            SetPostAttributes(ref linkHtmlAttributes);
            return ImageLink(helper, linkUrl, imageUrl, linkHtmlAttributes, imageHtmlAttributes);
        }
        #endregion

        #region private methods
        /// <summary>retrieves onclick, postData, postFunction attributes and combines them into a single onclick with JQuery post</summary>
        internal static void SetPostAttributes(ref IDictionary<string, object> linkHtmlAttributes)
        {
            string beforePost = String.Empty;
            if (linkHtmlAttributes.ContainsKey("onclick"))
            {
                beforePost = linkHtmlAttributes["onclick"].ToString();
                if (!beforePost.Trim().EndsWith(";")) { beforePost += ";"; }
            }

            string postData = "{}";
            if (linkHtmlAttributes.ContainsKey("postData"))
            {
                postData = ObjectToJSON(linkHtmlAttributes["postData"]);
                linkHtmlAttributes.Remove("postData");
            }

            string postFunction = String.Empty;
            if (linkHtmlAttributes.ContainsKey("postFunction"))
            {
                postFunction = linkHtmlAttributes["postFunction"].ToString();
                linkHtmlAttributes.Remove("postFunction");
            }

            linkHtmlAttributes["onclick"] = beforePost + "$.post(this.href, " + postData + ", function(data) {" + postFunction + "}); return false;";
        }

        /// <summary>creates image tag with no borders</summary>
        internal static string ImageBuilder(string imageUrl, IDictionary<string, object> imageHtmlAttributes)
        {
            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("src", imageUrl);
            imageBuilder.MergeAttributes(imageHtmlAttributes);
            if (imageBuilder.Attributes.ContainsKey("style"))
            {
                if (!imageBuilder.Attributes["style"].Contains("border"))
                {
                    if (!imageBuilder.Attributes["style"].Trim().EndsWith(";"))
                    {
                        imageBuilder.Attributes["style"] += ";";
                    }

                    imageBuilder.Attributes["style"] += "border:none;";
                }
            }
            else
            {
                imageBuilder.MergeAttribute("style", "border:none;");
            }

            return imageBuilder.ToString(TagRenderMode.SelfClosing);
        }

        /// <summary>creates link tag with no encoding of InnerHtml</summary>
        internal static string LinkBuilder(string innerHtml, string linkUrl, IDictionary<string, object> linkHtmlAttributes)
        {
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", linkUrl);
            linkBuilder.InnerHtml = innerHtml;
            linkBuilder.MergeAttributes(linkHtmlAttributes);
            return linkBuilder.ToString(TagRenderMode.Normal);
        }

        internal static string ObjectToJSON(object data)
        {
            if (data == null) { return "{}"; }
            string json;
#pragma warning disable 0618 // The JavaScriptSerializer type was marked as obsolete prior to .NET Framework 3.5 SP1
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            json = serializer.Serialize(data);
#pragma warning restore 0618
            return json;
        }

        /// <summary>Merge the second array into the first, overwriting common keys</summary>
        /// <param name="x">First dictionary</param>
        /// <param name="y">Second dictionary</param>
        internal static Dictionary<string, string> Merge(this Dictionary<string, string> x, Dictionary<string, string> y)
        {
            if (y == null) { return x; }

            if (x == null) { return y; }

            foreach (var item in y)
            {
                if (x.ContainsKey(item.Key))
                {
                    x[item.Key] = item.Value;
                }
                else
                {
                    x.Add(item.Key, item.Value);
                }
            }

            return x;
        }

        internal static Dictionary<string, string> ObjectToDictionary(object param)
        {
            var keyObjDict = (IDictionary<string, object>)new System.Web.Routing.RouteValueDictionary(param); // TODO: when Microsoft releases source for System.Web.Routing.dll, see how Routing converts anonymous types to IDictionary

            var dict = new Dictionary<string, string>();

            foreach (var item in keyObjDict) { dict.Add(item.Key, item.Value.ToString()); }

            return dict;
        }

        internal static string GetHelperValue(HtmlHelper helper, string name)
        {
            string attemptedValue = (string)GetModelStateValue(helper, name, typeof(string));
            return attemptedValue ?? GetViewDataString(helper, name);
        }

        private static object GetModelStateValue(HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                return modelState.Value.ConvertTo(destinationType, null /* culture */);
            }
            return null;
        }

        private static string GetViewDataString(HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), System.Globalization.CultureInfo.CurrentCulture);
        }
        #endregion
    }
}