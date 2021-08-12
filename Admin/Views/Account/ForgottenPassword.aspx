<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forgotten Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Forgotten Password</h2>
    <p>
        Please enter your user name and a new password will be sent to the email used to create that account.
    </p>
    <%= Html.BoxedValidationSummary("Login was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="username">User name:</label>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username", "*") %>
                </p>
                <p>
                    <input type="submit" value="Email New Password" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>