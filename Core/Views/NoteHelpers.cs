/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.Views.NoteHelpers
{
    public static class NoteHtmlHelperExtensions
    {
        public static string NoteDetailsLink(this HtmlHelper helper, Note item)
        {
            var linkRouteValues =
                new { action = "Details", controller = "Note", id = item.note_id };
            string imageUrl =
                item.committed
                    ? "/content/images/icons/page_red.png"
                    : "/content/Images/icons/page.png";
            string imageTitle =
                item.committed
                    ? "See Details for Committed Note"
                    : "See Details for Note";
            return helper.ImageLink(linkRouteValues, imageUrl, new { title = imageTitle });
        }

        // Used in ParentDetails.ascx for stars
        public static string NoteStarImage(this HtmlHelper helper, Note item)
        {
            string selected_image_src = StarImageUrl(item.starred);
            var imgTag = new TagBuilder("img");
            imgTag.Attributes.Add("src", selected_image_src);
            return imgTag.ToString(TagRenderMode.SelfClosing);
        }

        public static string NoteStarLink(this HtmlHelper helper, Note item)
        {
            string selected_image_src = StarImageUrl(item.starred);
            string selected_image_title = StarImageTitle(item.starred);
            return helper.ImageLink(
                new { action = "Star", controller = "Note", parent_id = item.id },
                selected_image_src,
                new { title = selected_image_title });
        }

        ////public static string PostNoteStarLink(this HtmlHelper helper, Note item) {
        ////    string selected_image_src = StarImg(item.starred);
        ////    string link_id = "starlink" + item.id;
        ////    string link_postFunction =
        ////       "if (!data) { alert('Failed to toggle star.'); } " +
        ////       "else { data = eval('(' + data + ')'); " +
        ////       "$('#starlink' + data.id + ' img').attr('src', 'https://docit.wbea.org/Images/action_' + data.starred + '_white.png'); }";
        ////    //NOTE: data type returned from Star action is {"id":"1", "starred":"true"}, which is used to "generate action_true_white.png" or "action_false_white.png"

        ////    return
        ////        helper.PostImageLink(
        ////            new { action = "Star", controller = "Note", id = item.note_id } /* routeValues */,
        ////            selected_image_src /* imgUrl */,
        ////            new { id = link_id, postFunction = link_postFunction } /* linkHtmlAttributes */
        ////        );
        ////}

        public static string NoteParameters(this HtmlHelper helper, Parameter[] parameters)
        {
            if (parameters.Length != 0)
            {
                var parameterList = new List<string>();
                foreach (var parameter in parameters)
                {
                    parameterList.Add(helper.Encode(parameter.ToString()));
                }

                parameterList.Sort();
                return String.Join(" ", parameterList.ToArray());
            }
            else
            {
                return "None";
            }
        }

        public static string NoteParent(this HtmlHelper helper, Note item)
        {
            if (!item.HasParent) { return "None"; }

            string html = String.Empty;
            string samplename = string.Empty;
            if (item.parent_type != Note.ParentType.Note)
            {
                if (item.parent_type == Note.ParentType.ChainOfCustody)
                {
                    html = "<input Length=\"0\" id=\"parent_id\" name=\"parent_id\" type=\"hidden\" value=\"" + item.parent_id + "\" runat=\"server\"/>";
                    html += "<input Length=\"0\" id=\"parent_type\" name=\"parent_type\" type=\"hidden\" value=\"" + (int)item.parent_type + "\" runat=\"server\"/>";
                    html += "CoC: ";
                }
                else if (item.parent_type == Note.ParentType.Sample)
                {
                    Sample s = Sample.Load(item.parent_id);
                    samplename = item.parent_type.ToString() + ": " + s.SampleType.name + " Sample Prepared By " + s.PreparedBy;
                }
                else
                {
                    html = item.parent_type.ToString() + ": ";
                }
            }
            if (item.parent_type == Note.ParentType.Sample)
                return samplename;
            else
                return html + item.parent.ToString();
        }

        public static string NoteParentLink(this HtmlHelper helper, Note item)
        {
            string html;
            string parentName = string.Empty;
            if (!item.HasParent)
            {
                html = "None";
            }
            else
            {
                html = String.Empty;
                UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

                string parentType = item.parent_type.ToString();
                if (item.parent_type != Note.ParentType.Note)
                {
                    if (item.parent_type == Note.ParentType.ChainOfCustody)
                    {
                        html = "CoC: ";
                    }
                    else
                    {
                        html = item.parent_type.ToString() + ": ";
                    }
                }

                string url = urlHelper.Action("Details", new { controller = parentType, id = item.parent_id });
                if (item.parent_type == Note.ParentType.Sample)
                {
                    Sample s = Sample.Load(item.parent_id);
                    parentName = item.parent_type.ToString() + ": " + s.SampleType.name + " Sample Prepared By " + s.PreparedBy;
                }
                else
                    parentName = item.parent.ToString();
                html += "<a href=\"" + url + "\">" + helper.Encode(parentName) + "</a>";
            }

            return html;
        }

        private static string StarImageUrl(bool starred)
        {
            return
                starred
                    ? "/content/images/icons/asterisk_orange.png"
                    : "/content/images/icons/asterisk_grey.png";
        }

        private static string StarImageTitle(bool starred)
        {
            return
                starred
                    ? "Click to unstar note"
                    : "Click to mark note for Action";
        }
    }
}