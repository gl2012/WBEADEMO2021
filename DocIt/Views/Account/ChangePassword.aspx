<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-account-password">Change Password</h2>
    <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    </p>
    <%= Html.BoxedValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Password</legend>
				<table cellpadding="0" cellspacing="0" border="0">
					<tr>
						<td><label for="currentPassword">Current password:</label></td>
						<td>
							<%= Html.Password("currentPassword") %>
							<%= Html.ValidationMessage("currentPassword", "*") %>						
						</td>
					</tr>
					<tr>
						<td><label for="newPassword">New password:</label></td>
						<td>
							<%= Html.Password("newPassword") %>
							<%= Html.ValidationMessage("newPassword", "*")%>						
						</td>
					</tr>
					<tr>
						<td><label for="confirmPassword">Confirm new password:</label></td>
						<td>
							<%= Html.Password("confirmPassword") %>
							<%= Html.ValidationMessage("confirmPassword", "*")%>						
						</td>
					</tr>
				</table>
				<br /><input type="submit" value="Change Password" />
            </fieldset>
        </div>
    <% } %>

    <div>
        <%=Html.ActionLink("Back to My Account", "MyAccount") %>
    </div>
</asp:Content>