<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Note[]>" %>
<%@ Import Namespace="WBEADMS" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>

	<!-- Search Area -->
	<div class="searchArea forcePrint">
		<form method="get">		
			<div class="searchField">
				<label for="location_id">Location:</label>
				<%= Html.DropDownList("location_id", "")%>
			</div>
			<div class="searchField">
				<label for="parameter_id">Parameter:</label>
				<%= Html.DropDownList("parameter_id", "")%>
			</div>
			<div class="searchField">
				<label for="user_id">Author:</label>
				<%= Html.DropDownList("user_id", "")%>
			</div>
			<div class="searchField">
				<label for="is_starred">Starred:</label>
				<%= Html.DropDownList("is_starred", "")%>
			</div>
			<div class="searchField">
				<label for="search">Date Created:</label>
				<span>
				    <%= Html.DatePicker("date_created_start")%>
				    to
				    <%= Html.DatePicker("date_created_end")%>
				</span>
			</div>
			<div class="searchField">
				<label for="search">Note Body:</label>
				<span class="inputShort"><%= Html.TextBox("search", ViewData["search"])%></span>
			</div>
			<input type="submit" class="btnSearch" value="Search" />
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>
    
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
	    <% if (ViewContext.RouteData.Values["controller"].ToString() == "Note") { %>
			<%= Html.ActionButton("Create New", "Create")%>
		<% } else if (ViewContext.RouteData.Values["controller"].ToString() == "DailySystemCheck") { %>
    		<%= Html.ActionButton("Perform DSC", "SelectDSCLocation")%>
		<% } %>
		<a href="#" title="Print" onclick="window.print();"><span>Print</span></a>
		</div>
	</div>
	
	<!-- Data table -->
    <table id="sortable-index">
        <tr>
            <th style="width:65px;" class="no-sort">Actions</th>
            <th style="min-width:80px;">Location</th>
            <th class="no-sort">Parameter</th>
            <th style="min-width:120px;">Date&nbsp;Created</th>
            <th style="min-width:120px;">Date&nbsp;Occurred</th>
            <th>Author</th>
            <th class="no-sort">Note Body</th>
        </tr>
<% foreach (var item in Model) { %>
        <tr>
            <td class="tableActionLinks">
                <span><%= Html.NoteDetailsLink(item) %></span>
                <span><%= Html.NoteStarLink(item)%></span>
            </td>
            <td>
                <%= Html.ActionLink(item.Location.name, "Details", new { controller = "Location", id = item.Location.location_id })%>
            </td>
            <td>
                <% foreach (var parameter in item.parameters) { %><%= parameter.name%> <% } %>
            </td>
            <td>
                <%= Html.Encode(item.DateCreated)%>
            </td>
            <td>
                <%= Html.Encode(item.DateOccurred)%>
            </td>
            <td>
                <%= Html.Encode(item.created_user)%>
            </td>
            <td>
                <%= Html.Encode(item.body)%>
            </td>
        </tr>
<% } %>
    </table>
    
    <br class="clear" />
    <div class="buttonList">
		<% if (ViewContext.RouteData.Values["controller"].ToString() == "Note") { %>
			<%= Html.ActionButton("Create New", "Create") %>
		<% } else if (ViewContext.RouteData.Values["controller"].ToString() == "DailySystemCheck") { %>
			<%= Html.ActionButton("Perform DSC", "SelectDSCLocation")%>
		<% } %>
	</div>