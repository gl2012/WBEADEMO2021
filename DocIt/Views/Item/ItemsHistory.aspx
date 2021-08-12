<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<WBEADMS.Models.ItemHistory>>" %>
<%@ Import Namespace="WBEADMS.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Items History
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2> Items History</h2>
    
    <!-- Search Area -->
	<div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="location_id">Location:</label>
				<%= Html.DropDownList("location_id", "All")%>
			</div>
			<div class="searchField">
			    <label>Install Date</label>
				<label for="start_install_date">From:</label>
				<%= Html.DatePicker("start_install_date")%>
				<label for="end_install_date">To:</label>
				<%= Html.DatePicker("end_install_date")%>
			</div>
			<%= Html.Hidden("sort", ViewData["sort"])%>
			<input type="submit" class="btnSearch" value="Search" />
		</form>
	</div>
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
		</div>
	</div>
    
    <% if (Model.Count > 0) { %>
    <table id="sortable-index">
        <tr>
            <th>
                Item Name
            </th>
            <th>
                Location
            </th>
            <th>
                Install Date
            </th>
            <th>
                Removal Date
            </th>
        </tr>
        <% foreach (ItemHistory history in Model) { %>
        <tr>
            <td>
                <%= Html.ActionLink(history.Item.DisplayName, "Details", new { controller = "Item", id = history.Item.item_id }) %>
            </td>
            <td>
                <%= Html.ActionLink(history.Location.name, "Details", new { controller = "Location", id = history.Location.location_id }) %>
            </td>
            <td>
                <%= history.DateInstalled%>
            </td>
            <td>
                <%= history.DateRemoved%>
            </td>
        </tr>
        <% } %>
    </table>
    <% }
       else { %>
    No history found.
    <% }%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>