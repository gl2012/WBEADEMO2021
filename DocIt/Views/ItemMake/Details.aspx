<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ItemMake>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details for Make: <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-make">Details for Make: <%= Model %></h2>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.make_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

    <fieldset>
        <legend>Details</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td class="label">Id:</td>
				<td><%= Html.Encode(Model.id) %></td>
			</tr>
			<tr>
				<td class="label">Name:</td>
				<td><%= Html.Encode(Model.name) %></td>
			</tr>
			<tr>
				<td class="label">Models:</td>
				<td>
					<ul>
					<% foreach (var model in Model.models) { %>
						<li><%= model.name %></li>
					<% } %>
					</ul>				
				</td>
			</tr>
		</table>
    </fieldset>

    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.make_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

</asp:Content>