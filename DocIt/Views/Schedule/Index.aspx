<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Schedule>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Schedules
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-schedules">Index of Schedules</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create A New Schedule", "Create") %></strong>?</p>
    
	<!-- Search Area -->
	<div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="location_id">Location:</label>
				<%= Html.DropDownList("location_id", "")%>
			</div>
			<div class="searchField">
				<label for="contact_id">Contact:</label>
				<%= Html.DropDownList("contact_id", "")%>
			</div>
			<div class="searchField">
				<label for="sample_type_id">Sample Type:</label>
				<%= Html.DropDownList("sample_type_id", "")%>
			</div>
			<div class="searchField">
				<label for="interval_id">Interval</label>
				<%= Html.DropDownList("interval_id", "")%>
			</div>
			<div class="searchField">
				<label for="is_active">Active</label>
				<%= Html.DropDownList("is_active", "")%>
			</div>
			<input type="submit" class="btnSearch" value="Search" />
			<%= Html.Hidden("sort", ViewData["sort"])%>			   
		</form>
	</div>
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionLink("Create New", "Create") %>
		</div>
	</div>

	<!-- Data table -->
    <table id="sortable-index">
    <thead>
        <tr>
            <th class="no-sort">Actions</th>
            <th>
                Name
            </th>
            <th>
                Location
            </th><!--
            <th>
                Contact
            </th>-->
            <th>
                Sample Type
            </th>
            <th>
                Start Date
            </th>
            <th>
                End Date
            </th>
            <th>
                Interval
            </th>
            <th>
                Active
            </th>
        </tr>
    </thead>
    
    <tbody>
    
    <% foreach (var item in Model) { %>
        <tr>
            <td class="tableActionLinks" style="min-width:140px;">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.schedule_id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.schedule_id })%></span>
            </td>
            <td>
                <%= Html.Encode(item.name) %>
            </td>
            <td>
                <% if (String.IsNullOrEmpty(item.location_id)) { %>None<% } else { %><%= Html.ActionLink(Html.Encode(item.Location), "Details", new { controller = "Location", id = item.location_id })%><% } %>
            </td><!--
            <td>
                <%= Html.Encode(item.contact.display_name) %>
            </td>-->
            <td>
                <%= Html.Encode(item.SampleType.name) %>
            </td>
            <td>
                <%= Html.Encode(item.DateStart) %>
            </td>
            <td>
                <%= Html.Encode(item.DateEnd) %>
            </td>
            <td>
                <%= Html.Encode(item.interval.ToSentence(item.frequency_data)) %>
            </td>
            <td>
                <%= item.is_active.ToHumanBool() %>
            </td>
        </tr>
    <% } %>
    
    </tbody>    
    </table>

	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionLink("Create New", "Create") %>
    </div>

</asp:Content>