<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.User>" %>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <table cellpadding="0" cellspacing="0" border="0">
            <tr><td>
            <legend>Edit Fields</legend>
            <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td><label for="user_name">User Name:</label></td>
				<td>
				<% if (Model.id.IsBlank()) { %>
                <%= Html.TextBox("user_name")%>
                <% } else { %>
                <%= Html.Encode(Model.user_name) %>
                <% } %>
                <%= Html.ValidationMessage("user_name", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="first_name">First Name:</label></td>
				<td>
                <%= Html.TextBox("first_name") %>
                <%= Html.ValidationMessage("first_name", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="last_name">Last Name:</label></td>
				<td>
                <%= Html.TextBox("last_name") %>
                <%= Html.ValidationMessage("last_name", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="email">Email:</label></td>
				<td>
                <%= Html.TextBox("email") %>
                <%= Html.ValidationMessage("email", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="phone_number">Phone Number:</label></td>
				<td>
                <%= Html.TextBox("phone_number") %>
                <%= Html.ValidationMessage("phone_number", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="role_id">Role:</label></td>
				<td>
                <%= Html.DropDownList("role_id") %>
                <%= Html.ValidationMessage("role_id", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="comments">Comments:</label></td>
				<td>
                <%= Html.TextBox("comments") %>
                <%= Html.ValidationMessage("comments", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="technician_code">Technician Code:</label></td>
				<td>
                <%= Html.TextBox("technician_code") %>
                <%= Html.ValidationMessage("technician_code", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="active">Active:</label></td>
				<td>
                <%= Html.CheckBox("active") %>
                <%= Html.ValidationMessage("active", "*") %>
                </td>
            </tr>
            </table>
            <br /><input type="submit" value="Save" />
                </td>
                <td> </td>
                    </tr>
                </table>
        </fieldset>

    <% } %>