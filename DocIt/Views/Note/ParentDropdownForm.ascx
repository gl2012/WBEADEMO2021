<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<span id="parent_id_span"><%= Html.DropDownList("parent_id", (IEnumerable<SelectListItem>)ViewData["parent_id"], "", new { title = "Leave blank if this note is not associated with an Item." })%> (leave blank if none)</span>