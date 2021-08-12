<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.LocationImage>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Image Details
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-locations-images">Image Details</h2>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.location_image_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "ListByLocation", new { id = Model.location_id })%></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

    <fieldset>
        <legend>Image Information</legend>
		
			<a title="Click to Open Image <%= Model.id %> for <%= Model.Location.name %> in New Window" href="<%= Url.RouteUrl(new {controller = "LocationImage", action = "GetImage", id = Model.location_image_id}) %>" target="_blank">
				<img class="imgR" src="<%= Url.RouteUrl(new {controller = "LocationImage", action = "GetImage", id = Model.location_image_id}) %>" alt="Image <%= Model.id %> for <%= Model.Location.name %>" />
			</a>
      
			<table cellpadding="0" cellspacing="0" border="0" width="300px">
				<tr>
					<td class="label">Location:</td>
					<td><%= Html.Encode(Model.Location.name) %></td>
				</tr>
				<tr>
					<td class="label">Size:</td>
				<%if (Model.width.IsBlank() || Model.width.IsBlank()) { %>
				    <td></td>
				<% } else { %>
					<td><%= Html.Encode(Model.width)%>px X <%= Html.Encode(Model.height)%>px</td>
				<% } %>
				</tr>
				<tr>
					<td class="label">Uploaded by:</td>
					<td><%= HtmlHelperExtensions.ToStringOrDefaultTo(Model.UploadedBy, "Unknown")%></td>
				</tr>
				<tr>
					<td class="label">Date Uploaded:</td>
					<td><%= Model.date_uploaded.ToDateTimeFormat()%></td>
				</tr>
				<tr>
					<td class="label">Modified by:</td>
					<td><%= HtmlHelperExtensions.ToStringOrDefaultTo(Model.ModifiedBy, "")%></td>
				</tr>
				<tr>
					<td class="label">Date Modified:</td>
					<td><%= Model.date_modified.ToDateTimeFormat()%></td>
				</tr>
				<tr>
					<td class="label" colspan="2">Comments:</td>
				</tr>
				<tr>
					<td colspan="2"><div class="noteBody"><%= Html.Encode(Model.comments) %></div></td>
				</tr>
			</table>

    </fieldset>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.location_image_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "ListByLocation", new { id = Model.location_id })%></li>
		</ul>
    </div>

</asp:Content>