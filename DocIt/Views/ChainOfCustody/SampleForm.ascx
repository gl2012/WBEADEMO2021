<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Sample>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>
    <% string formId = (string)ViewData["FormId"];  %>
    <% using (Html.BeginForm()) {%>
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
				    <td><%= Model.SampleType%><%= Html.Hidden("sample_type_id", Model.sample_type_id) %></td>
				</tr>
				<tr>
					<td><label for="wbea_id">WBEA Sample ID:</label></td>
					<td>
						<%= Html.TextBox("wbea_id_textbox", "", new { disabled="true"}) %>
                        <%= Html.Hidden("wbea_id", "") %>
						<%= Html.ValidationMessage("wbea_id", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="media_serial_number">Media Serial #:</label></td>
					<td>
						<%= Html.TextBox("media_serial_number") %>
						<%= Html.ValidationMessage("media_serial_number", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="lab_sample_id">Lab Sample ID:</label></td>
					<td>
						<%= Html.TextBox("lab_sample_id") %>
						<%= Html.ValidationMessage("lab_sample_id", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="date_received_from_lab">Date Received From Lab:</label></td>
					<td>
                        
						<%= Html.DatePicker(formId + "_date_received_from_lab")%>
						<%= Html.ValidationMessage(formId + "_date_received_from_lab", "*")%>
						<%= Html.Hidden("date_received_from_lab") %>
					</td>
				</tr>
				<tr>
					<td><label for="average_storage_temperature">Average Storage Temperature:</label></td>
					<td>
						<%= Html.TextBox("average_storage_temperature")%>
						<%= Html.ValidationMessage("average_storage_temperature", "*")%>
						<%= Html.DropDownList("average_storage_temperature_unit", WBEADMS.Models.Unit.FetchSelectList("Temperature", Model.average_storage_temperature_unit))%>
                        <%= Html.ValidationMessage("average_storage_temperature_unit", "*")%>
					</td>
				</tr>
				
				<tr>				
				    <td><label for="is_travel_blank">Travel Blank:</label></td>
				    <td><%= Model.is_travel_blank.ToHumanBool() %><%= Html.Hidden("is_travel_blank", Model.is_travel_blank) %></td>
				</tr>
				
			</table>

    <% } %>