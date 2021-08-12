<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Parameter>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Parameters
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-parameters">Index of Parameters</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create a New Parameter", "Create") %></strong>?</p>

	<!-- Search Area -->
	<div class="searchArea">
	    <%--
		<form method="get">
			<div class="searchField">
				<label for="XXX">XXX:</label>
				<%= Html.DropDownList("xxx", "") %>
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
		--%>
	</div>

	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionButton("Create New", "Create") %>
		</div>
	</div>

	<!-- Data table -->
    <table id="sortable-index" class="parametersTable tableThreeCol">
        <tr>
            <th class="no-sort">Actions</th>            
			<th>Name</th>
            <th>Description</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.id })%></span>
            </td>
            <td>
				<%= Html.ActionLink(item.name, "Details", new { id = item.id })%>
            </td>
            <td>
                <%= Html.Encode(item.description) %>
            </td>
        </tr>
    
    <% } %>

    </table>
	
	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionButton("Create New", "Create") %>
    </div>

</asp:Content>