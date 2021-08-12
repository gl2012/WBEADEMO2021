<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Location>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Index of Locations
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-locations">Index of Locations</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create A New Location", "Create") %></strong>?</p>
    
	<!-- Search Area -->
	<div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="is_active">Filter by Active:</label>
				<%= Html.DropDownList("is_active", "") %>
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionButton("Create New", "Create") %>
		</div>
	</div>
    
	<!-- Data table -->
    <table id="sortable-index" class="locationsTable">
        <tr>
            <th class="no-sort">Actions</th>
            <th>Name</th>
            <th>Full Name</th>
            <th>Latitude</th>
            <th>Longitude</th>
            <th>Active</th>
        </tr>
        <% foreach (var item in Model) { %>
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.location_id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.location_id })%></span>
                <span class="tableActionPictures"><%= Html.ActionButton("Pictures", "ListByLocation", new { controller = "LocationImage", id = item.location_id})%></span>
            </td>
            <td>
                <%= Html.Encode(item.name) %>
            </td>
            <td>
                <%= Html.Encode(item.full_name) %>
            </td>
            <td>
                <%= Html.Encode(item.latitude) %>
            </td>
            <td>
                <%= Html.Encode(item.longitude) %>
            </td>
            <td>
                <%= item.active.ToHumanBool() %>
            </td>
        </tr>
        <% } %>
    </table>
	
	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionButton("Create New", "Create") %>
    </div>
</asp:Content>