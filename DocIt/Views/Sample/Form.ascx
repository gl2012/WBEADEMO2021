<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Sample>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>

			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
    			<% if (ViewData["coc_id"] == null && (Model.id.IsBlank() || Model.ChainOfCustodyIDs.Count == 0)) { %>
					<td>
						<%= Html.DropDownList("sample_type_id") %>
						<%= Html.ValidationMessage("sample_type_id", "*") %>
					</td>
    			<% } else { %>
				    <td><%= Model.SampleType%></td>
				<% } %>
				</tr>
				<tr>
					<td><label for="wbea_id">WBEA Sample ID:</label></td>
					<td>
						<%= Html.TextBox("wbea_id") %>
						<%= Html.ValidationMessage("wbea_id", "*") %>
						<% if (Model.id.IsBlank()) { %> (Leave blank to auto-generate.)<% } %>
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
						<%= Html.DatePicker("date_received_from_lab") %>
						<%= Html.ValidationMessage("date_received_from_lab", "*") %>
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
				<% if (ViewData["coc_id"] == null && (Model.id.IsBlank() || Model.ChainOfCustodyIDs.Count == 0)) { %>
				    <td><%= Html.CheckBox("is_travel_blank")%></td>
				<% } else { %>
				    <td><%= Model.is_travel_blank.ToHumanBool() %></td>
				<% } %>
				</tr>
				<tr>
                   <td><label for="is_orphaned_sample">Orphaned Sample:</label> </td>
                    <td>
                     <% if(Model.is_orphaned_sample.IsBlank() ) { %>  
                        <%= Html.CheckBox("is_orphaned_sample", false) %>
                        	<% } else { %>
                        	 <%= Html.CheckBox("is_orphaned_sample") %>
                        <% } %>
                   </td>
                  
                </tr>
			</table>
            <br />
            <input type="submit" value="Save" />

        </fieldset>

    <% } %>

    <script type="text/javascript">
        $(function() {
            DisableTravelBlank();
        });
        
        $('#sample_type_id').click(function() { // for IE7
            DisableTravelBlank();
        });

        $('#sample_type_id').change(function() { // for Firefox/Chrome
            DisableTravelBlank();
        });

        $('input[type=submit]').click(function() {
            $('#is_travel_blank').removeAttr('disabled');
        });

        function DisableTravelBlank() {
            var sample_type = $('#sample_type_id').val();
            var is_travel_blank_available = sample_type == '<%= SampleType.PM25.id%>' || sample_type == '<%= SampleType.PM10.id%>' || sample_type == '<%= SampleType.PASS.id%>';
            if ($('#is_travel_blank').length != 0) {
                if (is_travel_blank_available) {
                    $('#is_travel_blank').removeAttr('disabled');
                } else {
                    $('#is_travel_blank').get(0).checked = false;
                    $('#is_travel_blank').attr('disabled', 'disabled');
                }
            }
        }
    </script>