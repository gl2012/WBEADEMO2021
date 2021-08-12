<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ItemModel>" %>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Details</legend>
			
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="make_id">Make:</label></td>
					<td>
						<%= Html.DropDownList("make_id", "")%>
						<%= Html.ValidationMessage("make_id", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="name">Name:</label></td>
					<td>
						<%= Html.TextBox("name") %>
						<%= Html.ValidationMessage("name", "*") %>
					</td>
				</tr>
			</table>

            <br /><input type="submit" value="Save" />
        </fieldset>

    <% } %>