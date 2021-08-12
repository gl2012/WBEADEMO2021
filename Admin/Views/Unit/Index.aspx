<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Unit>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Units
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Units</h2>
    
	<!-- Search Area -->
	<div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="type">Filter by Type:</label>
				<%= Html.DropDownList("type", "")%>
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
    <table id="sortable-index">
        <tr>
            <th class="iconDesc no-sort">			
                <span title="Edit">Edit</span>
			</th>
			<th>
                Type
            </th>
            <th>
                Name
            </th>
            <th class="iconDesc no-sort">
                Symbol
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id = item.unit_id })%></span>                
            </td>
            <td>
                <%= Html.Encode(item.type) %>
            </td>
            <td>
                <%= Html.Encode(item.name) %>
            </td>
            <td>
                <%= Html.Encode(item.symbol) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <br class="clear" />
    <div class="buttonList">
        <%= Html.ActionLink("Create New", "Create") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>