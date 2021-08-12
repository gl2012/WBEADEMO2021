<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.SlopeOffset>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Slope/Offsets
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-slopeoffsets">Index of Slope/Offsets</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create a New Slope/Offset", "Create") %></strong>?</p>

	<!-- Search Area -->
	<div class="searchArea">
	<form method="get">
		<div class="searchField">
			<label for="station_id">Station:</label>
			<%= Html.DropDownList("station_id", "")%>
		</div>
		<%--
		<div class="searchField">
			<label for="analyzer">Analyzer:</label>
			<%= Html.DropDownList("analyzer", "")%>
		</div>
		<div class="searchField">
			<label for="date_Active">Active Date:</label>
			<%= Html.TextBox("date_Active", "")%>
		</div>
		<div class="searchField">
			<label for="slope">Slope:</label>
			<%= Html.TextBox("slope", "")%>
		</div>
		<div class="searchField">
			<label for="offset">Offset:</label>
			<%= Html.TextBox("offset", "")%>
		</div>
		--%>
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
    <table id="sortable-index" class="itemTable">
        <tr>
            <th class="no-sort">Actions</th>
            <th>Date Created</th>
            <th>Station</th>
            <th>Analyzer</th>
            <th sort_name="date-active">Active Date</th>
            <th class="no-sort">Slope</th>
            <th class="no-sort">Offset</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.id })%></span>
            </td>
            <td>
                <%= Html.Encode(item.date_created.ToDateTimeFormat()) %>
            </td>
            <td>
                <%= Html.Encode(item.station_id) %>
            </td>
            <td>
                <%= Html.Encode(item.analyzer) %>
            </td>
            <td>
                <%= Html.Encode(item.date_active)%>
            </td>
            <td>
                <%= Html.Encode(item.slope) %>
            </td>
            <td>
                <%= Html.Encode(item.offset) %>
            </td>
        </tr>
    
    <% } %>

    </table>

	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionButton("Create New", "Create") %>
    </div>

</asp:Content>