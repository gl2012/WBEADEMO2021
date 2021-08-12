<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	My Account
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-account">My Account Details</h2>
	
    <div class="buttonList">
		<ul>
		<li><%= Html.ActionButton("Edit My Details", "EditAccount") %></li>
		<li><%= Html.ActionLink("Change Password", "ChangePassword") %></li>
		</ul>
	</div>    
	
    <fieldset>
        <legend>Account Details</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td class="label">User Name:</td>
				<td><%= Html.Encode(Model.user_name) %></td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td><%= Html.Encode(Model.first_name) %></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td><%= Html.Encode(Model.last_name) %></td>
			</tr>
			<tr>
				<td class="label">Email:</td>
				<td><%= Html.Encode(Model.email) %></td>
			</tr>
			<tr>
				<td class="label">Phone Number:</td>
				<td><%= Html.Encode(Model.phone_number) %></td>
			</tr>
			<tr>
				<td class="label">Role:</td>
				<td><%= Model.Role.name %></td>
			</tr>
			<tr>
				<td class="label">Date Created:</td>
				<td><%= Html.Encode(Model.date_created) %></td>
			</tr>
		</table>
    </fieldset>
    
    <div class="buttonList">
		<%= Html.ActionButton("Edit My Details", "EditAccount") %>
		<%= Html.ActionLink("Change Password", "ChangePassword") %>
	</div>    
</asp:Content>