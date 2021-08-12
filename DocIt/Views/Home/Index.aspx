<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-dashboard">Doc-It Dashboard</h2>
	
	<% if (ViewData["location"] == null) { %>
	<p>Would you like to <strong><%= Html.ActionLink("Set A Default Location", "Location") %>?</strong></p>
	<% } else { %>
	<p>This station is set as <%= (Location)ViewData["location"] %>.</p>
	<% } %>
    <%-- sample link specifying only action; controller is defaulted to current controller, which is Home --%>
	
	<fieldset>
		<legend>Quick Links</legend>
		
		<div class="userActionList">
			<div class="userActions">
				<div class="section-icon icon-notes">Notes</div>
				<ul>
					<li><%= Html.ActionLink("View Notes", "Index", "Note") %><%-- sample link specifying both action and controller --%></li>
					<li><%= Html.ActionLink("Create New Note", "Create", "Note") %></li>
				</ul>
			</div>
			<div class="userActions">
				<div class="section-icon icon-schedules">Schedules</div>
				<ul>
					<li><%= Html.ActionLink("View Calendar", "Calendar", "Schedule") %></li>
                    <li><%= Html.ActionLink("View Schedules", "Index", "Schedule")%></li>
				</ul>
			</div>
			<div class="userActions">
				<div class="section-icon icon-items">Items</div>
				<ul>
					<li><%= Html.ActionLink("View Items", "Index", new { controller = "Item", page = 1 })%> <%-- sample link specifying url parameter of ?page=1 --%></li>
					<li><%= Html.ActionLink("Create New Item", "Create", "Item") %></li>
				</ul>
			</div>
			<div class="userActions">
				<div class="section-icon icon-locations">Locations</div>
				<ul>
					<li><%= Html.ActionLink("View Locations", "Index", new { controller = "Location" }, new { @class = "myCssClass", id = "myId" })%><%-- sample link specifying html attributes --%></li>
					<li><%= Html.ActionLink("Set Default Location", "Location") %></li>
				</ul>
			</div>
		</div>
	</fieldset>
</asp:Content>