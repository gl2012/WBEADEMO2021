<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Parameter>" %>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
			<table cellpadding="0" cellspacing="0" border="0">
			<tr>
                <td><label for="name">Name:</label></td>
                <td>
                <%= Html.TextBox("name", Model.name) %>
                <%= Html.ValidationMessage("name", "*") %>
                </td>
			</tr>
			<tr>
                <td><label for="description">Description:</label></td>
                <td>
                <%= Html.TextBox("description", Model.description) %>
                <%= Html.ValidationMessage("description", "*") %>
                </td>
			</tr>
			<!--
			<tr>
                <td><label for="hidden">Hidden:</label><br />(from dropdowns)</td>
                <td>
                <%= Html.CheckBox("hidden", Model.hidden)%>
                <%= Html.ValidationMessage("hidden", "*")%>
                </td>
			</tr>
			-->
			</table>
             
            <br /><input type="submit" value="Save" />
        </fieldset>

    <% } %>