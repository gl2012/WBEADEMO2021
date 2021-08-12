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
    <h2 class="title-icon icon-dashboard">Admin Dashboard</h2>
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>    

	<fieldset>
		<legend>Quick Links</legend>
		
		<div class="userActionList">
			<div class="userActions">
				<div class="section-icon icon-users">Users</div>
				<ul>
					<li><%= Html.ActionLink("View Users", "Index", "User")%><%-- sample link specifying both action and controller --%></li>
					<li><%= Html.ActionLink("Create New Users", "Create", "User")%></li>
				</ul>
			</div>
			<div class="userActions">
				<div class="section-icon icon-schedules">Locations</div>
				<ul>
					<li><%= Html.ActionLink("View Locations", "Index", "Location")%></li>
                    <li><%= Html.ActionLink("Create New Locations", "Create", "Location")%></li>
				</ul>
			</div>
			<div class="userActions">
				<div class="section-icon icon-items">Items</div>
				<ul>
					<li><%= Html.ActionLink("View Items", "Index", new { controller = "Item", page = 1 })%> <%-- sample link specifying url parameter of ?page=1 --%></li>
					<li><%= Html.ActionLink("Create New Item", "Create", "Item") %></li>
				</ul>
			</div>
		</div>
	</fieldset>

    
</asp:Content>