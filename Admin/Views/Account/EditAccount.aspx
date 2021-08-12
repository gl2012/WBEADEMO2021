<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="user_name">User Name:</label>
                <%= Html.TextBox("user_name", Model.user_name, new { disabled = "disabled" })%>
                <%= Html.ValidationMessage("user_name", "*") %>
            </p>
            <p>
                <label for="first_name">First Name:</label>
                <%= Html.TextBox("first_name", Model.first_name) %>
                <%= Html.ValidationMessage("first_name", "*") %>
            </p>
            <p>
                <label for="last_name">Last Name:</label>
                <%= Html.TextBox("last_name", Model.last_name) %>
                <%= Html.ValidationMessage("last_name", "*") %>
            </p>
            <p>
                <label for="email">Email:</label>
                <%= Html.TextBox("email", Model.email) %>
                <%= Html.ValidationMessage("email", "*") %>
            </p>
            <p>
                <label for="phone_number">Phone Number:</label>
                <%= Html.TextBox("phone_number", Model.phone_number) %>
                <%= Html.ValidationMessage("phone_number", "*") %>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to My Account", "MyAccount") %>
    </div>

</asp:Content>