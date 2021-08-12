<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.LocationImage>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
		Images for <%= ViewData["location_name"].ToString() %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-locations-images">Images for <%=Html.ActionLink(ViewData["location_name"].ToString(), "Details", new {controller = "Location", id = ViewData["location_id"].ToString()})%></h2>
	<p>&nbsp;</p>
	
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionLink("Upload New", "Upload", new { id = ViewData["location_id"] })%>
			<%= Html.ActionLink("Location List", "Index", "Location")%>
		</div>	
	</div>

    <table id="sortable-index">
        <tr>
            <th class="iconDesc no-sort">
				Actions
			</th>
            <th> </th>
            <th>
                Uploaded By
            </th>
            <th>
                Date Uploaded
            </th>
            <th>
                Modified By
            </th>
            <th>
                Date Modified
            </th>
            <th>
                Comments
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionImagesEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.location_image_id }) %></span>
                <span class="tableActionImageDetails"><%= Html.ActionButton("Details", "Details", new { id=item.location_image_id })%></span>
            </td>
            <td>
                <a href="<%= Url.Action("Details", new {id = item.id}) %>">
                <img src="<%= Url.Action("GetThumbnail", new {id = item.id}) %>" alt="Thumbnail <%= item.id %> for <%= item.Location.name %>" />
                </a>
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.UploadedBy, "Unknown")%>
            </td>
            <td>
                <%= item.date_uploaded.ToDateTimeFormat()%>
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.ModifiedBy, "")%>
            </td>
            <td>
                <%= item.date_modified.ToDateTimeFormat() %>
            </td>
            <td>
                <%= Html.Encode(item.comments) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <div class="buttonList">
		<ul>
			<li><%= Html.ActionLink("Upload New", "Upload", new { id = ViewData["location_id"] })%></li>
		</ul>
        <div class="clear-fix"></div>
    </div>

</asp:Content>