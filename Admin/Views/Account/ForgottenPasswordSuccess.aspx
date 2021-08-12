<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ForgottenPasswordSuccess
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Forgotten Password</h2>
    <p>
        An new password has been sent to the email registered to [<%= TempData["forgotten_username"] %>].
    </p>
    <p>
        Please go back to the <%= Html.ActionLink("Log on page", "LogOn", "Account") %> after you receive your email.
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>