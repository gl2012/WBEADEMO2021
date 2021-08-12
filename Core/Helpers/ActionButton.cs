
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace WBEADMS.Helpers
{
    public static class ActionButtonExtension
    {
        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return ActionButton(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return ActionButton(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return ActionButton(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return ActionButton(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return ActionButton(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return ActionButton(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return ActionButton(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static string ActionButton(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("linkText cannot be empty");
            }

            // TODO: This makes no sense do you want to thow an exception or do you want to set it to empty string?!?!?!?
            string innerHtml;
            if (!String.IsNullOrEmpty(linkText))
            {
                innerHtml = "<span>" + linkText + "</span>"; // TODO: Get Rod to edit this to add his span tags
            }
            else
            {
                innerHtml = String.Empty;
            }

            if (!controllerName.IsBlank())
            {
                routeValues.Add("controller", controllerName);
            }

            if (!actionName.IsBlank())
            {
                routeValues.Add("action", actionName);
            }

            UrlHelper urlhelper = new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection);
            string url = urlhelper.RouteUrl(routeValues);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = innerHtml
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttribute("title", linkText);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }
}