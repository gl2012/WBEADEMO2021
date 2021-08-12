<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Location>" %>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
	
    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
			
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="name">Name:</label></td>
					<td><%= Html.TextBox("name") %> <%= Html.ValidationMessage("name", "*") %></td>
				</tr>
				<tr>
					<td><label for="full_name">Full Name:</label></td>
					<td><%= Html.TextBox("full_name") %> <%= Html.ValidationMessage("full_name", "*") %></td>
				</tr>
				<tr>
					<td><label for="latitude">Latitude:</label></td>
					<td><%= Html.TextBox("latitude", String.Format("{0:F}", Model.latitude)) %> <%= Html.ValidationMessage("latitude", "*") %></td>
				</tr>
				<tr>
					<td><label for="longitude">Longitude:</label></td>
					<td><%= Html.TextBox("longitude", String.Format("{0:F}", Model.longitude)) %> <%= Html.ValidationMessage("longitude", "*") %></td>
				</tr>
				<tr>
					<td><label for="address">Address:</label></td>
					<td><%= Html.TextBox("address") %> <%= Html.ValidationMessage("address", "*") %></td>
				</tr>
				<tr>
					<td><label for="phone_number">Phone Number:</label></td>
					<td><%= Html.TextBox("phone_number") %> <%= Html.ValidationMessage("phone_number", "*") %></td>
				</tr>
				<tr>
					<td><label for="comments">Comments:</label></td>
					<td><%= Html.TextArea("comments", new { rows = 2, cols = 40 })%> <%= Html.ValidationMessage("comments", "*") %></td>
				</tr>
				<tr>
					<td><label for="terrain_description">Terrain Description:</label></td>
					<td><%= Html.TextArea("terrain_description", new { rows = 2, cols = 40 })%> <%= Html.ValidationMessage("terrain_description", "*") %></td>
				</tr>
				<tr>
					<td><label for="tree_canopy_description">Tree Canopy Description:</label></td>
					<td><%= Html.TextArea("tree_canopy_description", new { rows = 2, cols = 40 })%> <%= Html.ValidationMessage("tree_canopy_description", "*") %></td>
				</tr>
				<tr>
					<td><label for="nearby_sources">Nearby Sources:</label></td>
					<td><%= Html.TextBox("nearby_sources") %> <%= Html.ValidationMessage("nearby_sources", "*") %></td>
				</tr>
				<tr>
					<td><label for="wet_weather_access">Wet Weather Access:</label></td>
					<td><%= Html.TextBox("wet_weather_access") %> <%= Html.ValidationMessage("wet_weather_access", "*") %></td>
				</tr>
				<tr>
					<td><label for="winter_access">Winter Access:</label></td>
					<td><%= Html.TextBox("winter_access") %> <%= Html.ValidationMessage("winter_access", "*") %></td>
				</tr>
				<tr>
					<td><label for="active">Active:</label></td>
					<td><%= Html.CheckBox("active") %></td>
				</tr>
				<tr>
					<td></td>
					<td></td>
				</tr>
			</table>
			<p> 
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>