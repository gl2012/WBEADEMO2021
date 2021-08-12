﻿<%--
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
    <h2>Change Password</h2>
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
                <legend>Account Information</legend>
                <p>
                    <label for="currentPassword">Current password:</label>
                    <%= Html.Password("currentPassword") %>
                    <%= Html.ValidationMessage("currentPassword", "*") %>
                </p>
                <p>
                    <label for="newPassword">New password:</label>
                    <%= Html.Password("newPassword") %>
                    <%= Html.ValidationMessage("newPassword", "*")%>
                </p>
                <p>
                    <label for="confirmPassword">Confirm new password:</label>
                    <%= Html.Password("confirmPassword") %>
                    <%= Html.ValidationMessage("confirmPassword", "*")%>
                </p>
                <p>
                    <input type="submit" value="Change Password" />
                </p>
            </fieldset>
        </div>
    <% } %>
    
    <div>
        <%=Html.ActionLink("Back to My Account", "MyAccount") %>
    </div>
</asp:Content>