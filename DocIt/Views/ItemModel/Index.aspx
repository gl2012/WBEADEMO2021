<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ItemModel>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Models
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-model">Index of Models</h2>
	<p>Would you like to <strong><%= Html.ActionLink("Create a New Model", "Create") %></strong>?</p>
	<br />
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionLink("Create New", "Create") %>
			<%= Html.ActionButton("List Items", "Index", "Item") %>
			<%= Html.ActionButton("List Makes", "Index", "ItemMake") %>
		</div>
	</div>

    <table id="sortable-index" class="itemModelTable tableThreeCol">
        <tr>
            <th class="iconDesc no-sort">Actions</th>
            <th>
                Name
            </th>
            <th>
                Make
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.model_id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.model_id })%></span>
            </td>
            <td>
				<%= Html.ActionLink(item.name, "Details", new { id=item.model_id })%>
            </td>
            <td>
                <%= Html.Encode(item.make.name) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <div class="buttonList">
		<ul>
			<li><%= Html.ActionLink("Create New", "Create") %></li>
		</ul>
        <div class="clear-fix"></div>
    </div>

</asp:Content>