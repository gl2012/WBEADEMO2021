<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-account-logon">Log On</h2>
    <p>
        Please enter your username and password. <!-- <%= Html.ActionLink("Register", "Register") %> if you don't have an account.-->
    </p>
    <%= Html.BoxedValidationSummary("Login was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm("LogOn", "Account")) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="username">Username:</label>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username", "*")%>
                </p>
                <p>
                    <label for="password">Password:</label>
                    <%= Html.Password("password") %>
                    <%= Html.ValidationMessage("password", "*")%>
                </p>
                <p>
                    <%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe">Remember me?</label>
                </p>
                <p>
                    <input type="submit" value="Log On" />
    				<%= Html.Hidden("returnUrl") %>
                </p>
                <br />
                <div><%= Html.ActionLink("Forgotten Password", "ForgottenPassword", "Account") %></div>
            </fieldset>
        </div>
		
    <script type="text/javascript">
        $(function() {
			$('#username').focus();
        });
    </script>		
    <% } %>
</asp:Content>