<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ItemMake>" %>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Make Details</legend>
			<table cellspacing="0" cellpadding="0" border="0">
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