<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ItemModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details for Model: <%= Model %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-model">Details for Model: <%= Model %></h2>
	
    <div class="buttonList">
		<ul>
			<li> <%=Html.ActionLink("Edit", "Edit", new { id=Model.model_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

    <fieldset>
        <legend>Details</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td class="label">Make:</td>
				<td><%= Html.ActionLink(Model.make.name, "Details", new { controller = "ItemMake", id = Model.make_id })%></td>
			</tr>
			<tr>
				<td class="label">Name:</td>
				<td><%= Html.Encode(Model.name) %></td>
			</tr>
			<tr>
				<td class="label">Display Name:</td>
				<td><%= Html.Encode(Model.display_name) %></td>
			</tr> 
		</table>
    </fieldset>

    <div class="buttonList">
		<ul>
			<li> <%=Html.ActionLink("Edit", "Edit", new { id=Model.model_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

</asp:Content>