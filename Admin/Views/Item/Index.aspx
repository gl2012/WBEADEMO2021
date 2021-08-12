<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Item>>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Items
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items">Index of Items</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create A New Item", "Create") %></strong>?</p>
	
	<!-- Search Area -->
	<div class="searchArea">
		<form action="Item.aspx" method="get">
			<div class="searchField">
				<label for="model_id">Model:</label>
				<%= Html.DropDownList("model_id", "")%>
			</div>
			<div class="searchField">
				<label for="location_id">Location:</label>
				<%= Html.DropDownList("location_id", "")%>
			</div>
			<div class="searchField">
				<label for="parameter_id">Parameter:</label>
				<%= Html.DropDownList("parameter_id", "")%>
			</div>
			<div class="searchField">
				<label for="is_integrated">Sampler Type:</label>
				<%= Html.DropDownList("is_integrated", "")%>
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionButton("Create New", "Create") %>
			<%= Html.ActionButton("List Models", "Index", "ItemModel") %>
			<%= Html.ActionButton("List Makes", "Index", "ItemMake") %>
		</div>
	</div>
        
	<!-- Data table -->
    <table id="sortable-index" class="itemTable">
        <tr>
            <th class="no-sort">Actions</th>
            <th>Name</th>
            <th class="no-sort">Type</th>
            <th class="no-sort">Make/Model</th>
			<th sort_name="is-integrated">Sampler Type</th>
            <th>Location</th> 
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.item_id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.item_id })%></span>
                <% if (item.Location != null && !item.IsUnassigned) { %>
                    <span class="tableActionMove"><%= Html.ActionButton("Move", "Relocate", new { location_id = item.Location.id, item_id = item.id })%></span>
                <% } %> 
            </td>
            <td>
                <%= Html.Encode(item.DisplayName) %>
            </td>
            <td>
                <% if (item.IsIntegrated) { %>
                    <%= item.SampleType%>
                <% } else { 
                       foreach (var parameter in item.parameters) { %>
                    <%= Html.Encode(parameter.name)%>
                <% } } %>
            </td>
            <td>
                <%= (item.model == null) ? "none" : Html.Encode(item.model.display_name)%>
            </td>
            <td>
                <%= (item.IsIntegrated) ? "Integrated" : "Continuous" %>
            </td>
            <td>
                <% if (item.Location == null) { %>None<% } else { %>
                    <%= Html.ActionLink(item.Location.name, "Details", new { controller = "Location", id = item.Location.id })%>                    
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionButton("Create New", "Create") %>
        <%= Html.ActionButton("List Models", "Index", "ItemModel") %>
        <%= Html.ActionButton("List Makes", "Index", "ItemMake") %>
    </div>

</asp:Content>