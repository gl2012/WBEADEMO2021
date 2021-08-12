<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ItemMake>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Index of Makes
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-make">Index of Makes</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionLink("Create a New Make", "Create") %></strong>?</p>
	<br />
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionLink("Create New", "Create") %>
			<%= Html.ActionButton("List Models", "Index", "ItemModel") %>
			<%= Html.ActionButton("List Items", "Index", "Item") %>
		</div>
	</div>

    <table id="sortable-index" class="itemMakeTable tableTwoCol">
        <tr>
            <th class="iconDesc no-sort">
                Actions
			</th>
            <th>
                Name
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id=item.make_id }) %></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.make_id })%></span>
            </td>
            <td>
				<%= Html.ActionLink(item.name, "Details", new { id=item.make_id })%>
            </td>
        </tr>
    
    <% } %>

    </table>
	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionLink("Create New", "Create") %>
    </div>

</asp:Content>