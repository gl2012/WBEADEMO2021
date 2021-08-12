<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Helpers.CheckBoxNAHelper" %>
<% 
    DeploymentSection deployment = Model.Deployment;

    if (deployment.date_deployed.IsBlank()) {
        deployment.date_deployed = DateTime.Now.ToISODateTime(); // default date to today on open
    }
	
	string action = ViewContext.RouteData.Values["action"].ToString().ToLower();
       
    string fieldsetClass = "";
    if (Model.Status.name == "Opened" || Model.Status.name == "Prepared" || Model.Status.name == "Preparing" || Model.Status.name == "Deploying" ||
        (action.ToLower() == "details" && (Model.Status.name == "Deployed" || Model.Status.name == "Retrieving"))) {
        fieldsetClass = " fullWidth";
    } else if (Model.Status.name == "Deployed" || Model.Status.name == "Retrieving") {
		fieldsetClass = " lessHalfWidth";
	} else { 
		fieldsetClass = " halfWidth";
	}
%>

    <fieldset class="fieldsetDeployment<%= fieldsetClass %>">
        <legend>Deployment Fields</legend>
		<div class="fieldsetInfo">
        <table cellpadding="0" cellspacing="0" border="0" class="cocTable formTable">
            <% if (Model.SampleType != SampleType.PASS && Model.SampleType != SampleType.PRECIP) { %><tr><th></th><th>N/A</th><th></th></tr><% } %>
            <% if (Model.SampleType.name == "VOC") { %>
            <tr>
                <td>
                    <label for="voc_cannister_pressure">VOC Cannister Pressure:</label>
                </td>
                <td><%= Html.CheckBoxNA("voc_cannister_pressure", deployment.voc_cannister_pressure) %></td>
                <td>
                    <%= Html.TextBox("voc_cannister_pressure", deployment.voc_cannister_pressure)%>
                    <%= Html.ValidationMessage("voc_cannister_pressure", "*") %>
                    <%= Html.DropDownList("voc_cannister_pressure_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", deployment.voc_cannister_pressure_unit), "")%>
                    <%= Html.ValidationMessage("voc_cannister_pressure_unit", "*")%>
                </td>
            </tr>
            <% } %>
            <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_deployed">Date Deployed:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DateTimePicker("date_deployed", deployment.date_deployed)%>
                    <%= Html.ValidationMessage("date_deployed", "*")%>
                </td>
            </tr>
            <% } else { %>
            <tr>
                <td>
                    <label for="deployment_initials">Deployment Tech Initials:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("deployment_initials", deployment.deployment_initials)%>
                    <%= Html.ValidationMessage("deployment_initials", "*")%>
                </td>
            </tr>
            <% } %>
            <tr>
                <td>
                    <label for="location_id">Location:</label>
                </td>
                <td></td>
                <td>
                    <% if (!deployment.schedule_id.IsBlank()) {
                           if (!deployment.location_id.IsBlank()) {
                               Response.Write(deployment.Location);
                           }
                           else {
                               // its assumed that a schedule must have a location_id
                               Response.Write(HtmlHelperExtensions.ToStringOrDefaultTo(deployment.Schedule.Location, "ERROR: NOT SET"));
                           }
                        }
                        else { %>
                            <%= Html.DropDownList("location_id", (SelectList)ViewData["location_id_selectlist"], "", new { onkeyup = "$(this).change()", onchange = @"$.getJSON(""../GetLocationDetails"", { location_id:$('#location_id').val()}, function(data){$('#LocationFullName').html(data.full_name);})" })%> 
                            <%= Html.ValidationMessage("location_id", "*")%>
                            
                            <span id="otherLocationSpan" style="margin-left: 10px">
                                Other Location
                                <%= Html.TextBox("other_location", deployment.other_location)%>
                                <%= Html.ValidationMessage("other_location", "*")%>
                            </span>
                            
                            <script type="text/javascript">
                                $('#location_id').change(function() {
                                    // AJAX fetch Location Full Name and update Sampler dropdown
                                    $.getJSON('<%= Url.Action("GetLocationDetails") %>', { location_id: $('#location_id').val() }, function(data) {
                                        $('#LocationFullName').html(data.full_name);
                                        $('#SamplerSerialNumber').html('');
                                        $('#SamplerMake').html('');
                                        $('#SamplerModel').html('');

                                        $.get('<%= Url.Action("GetSamplerDropdown") %>', { sample_type_id: '<%= Model.sample_type_id %>', location_id: $('#location_id').val() }, function(data) {
                                            $('#sampler_item_id').replaceWith(data);
                                            if (data.indexOf("no samplers at this location", 0) >= 0) {
                                                $('.samplerTR').hide();
                                            } else {
                                                $('.samplerTR').show();
                                            }
                                        });
                                    });

                                    // Enable/Disable "Other Location"
                                    var span = $('#otherLocationSpan');
                                    var otherLoc = $('#other_location');
                                    if ($(this).val() == '') {
                                        span.css('display', 'inline');
                                        span.effect('highlight', {}, 2000);
                                        otherLoc.val(otherLoc.data('old_value'));
                                    } else {
                                        span.hide();
                                        otherLoc.data('old_value', otherLoc.val());
                                        otherLoc.val('');
                                    }
                                });
                            </script>
                        <% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="LocationFullName">Location Full Name:</label>
                </td>
                <td></td>
                <td>
                    <span id="LocationFullName">
                        <% if (!String.IsNullOrEmpty(deployment.location_id)) { Response.Write(deployment.Location.full_name); }
                           else { %>
                        Other
                        <% } %>
                    </span>            
                </td>
            </tr>

            <% bool hideSamplerDetailClass = ((IEnumerable<SelectListItem>)ViewData["sampler_item_id_selectlist"]).Count() == 0; %>
            <tr>
                <td>
                    <label for="sampler_item_id">Sampler:</label>
                </td>
                <td></td>
                <td>
                    <%  if (!hideSamplerDetailClass) { %>
                    <%= Html.DropDownList("sampler_item_id", (IEnumerable<SelectListItem>)ViewData["sampler_item_id_selectlist"], "", new { selected = "3", onkeyup = "$(this).change()", onchange = @"$.getJSON(""../GetSamplerDetails"", { sampler_item_id:$('#sampler_item_id').val()}, function(data){$('#SamplerSerialNumber').html(data.serial_number); $('#SamplerMake').html(data.make); $('#SamplerModel').html(data.model);})" })%>
                    <% } else { %>
                    <span id="sampler_item_id">N/A (no samplers at this location)</span>
                    <script type="text/javascript">
                        $(function() { $('.samplerTR').hide(); });
                    </script>
                    <% } %>
                    <%= Html.ValidationMessage("sampler_item_id", "*")%>
                    <% if (Model.SampleType == SampleType.PASS && !(hideSamplerDetailClass && !Model.schedule_id.IsBlank())) { %><%-- only show (Optional) for Passives but hide if location and samplers are unselectable --%>
                    (Optional)
                    <% } %>
                </td>
            </tr>
            <tr class='samplerTR'>
                <td>
                    <label for="SamplerSerialNumber">Sampler Serial Number:</label>
                </td>
                <td></td>
                <td>
                    <span id="SamplerSerialNumber">
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.serial_number); } else { Response.Write(" "); }%>
                    </span>
                </td>
            </tr>
            <tr class='samplerTR'>
                <td>
                    <label for="SamplerMake">Sampler Make:</label>
                </td>
                <td></td>
                <td>
                    <span id="SamplerMake">
                    
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.make); } else { Response.Write(" "); }%>
                    </span>
                </td>
            </tr>
            <tr class='samplerTR'>
                <td>
                    <label for="SamplerModel">Sampler Model:</label>
                </td>
                <td></td>
                <td>
                    <span id="SamplerModel">
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.model); } else { Response.Write(" "); } %>
                    </span>
                </td>
            </tr>
				<% if (Model.SampleType == SampleType.PASS) { %>
            <tr class='samplerTR'><td>&nbsp;<!-- spacer row, so that when samplerTR rows are hidden, all the rows will still be highlighted properly --></td><td></td><td></td></tr>
				<% } %>
        <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_sampler_last_calibrated">Last Calibrated On:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DatePicker("date_sampler_last_calibrated", deployment.date_sampler_last_calibrated) %>
                    <%= Html.ValidationMessage("date_sampler_last_calibrated", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_sampler_last_leak_checked">Last Leak Checked On:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DatePicker("date_sampler_last_leak_checked", deployment.date_sampler_last_leak_checked)%>
                    <%= Html.ValidationMessage("date_sampler_last_leak_checked", "*")%>
                </td>
            </tr>
            
            <% if(Model.SampleType.name == "PAH") { %>
            <tr>
                <td>
                    <label for="leak_check">Leak Check:</label>
                </td>
                <td><%= Html.CheckBoxNA("leak_check", deployment.leak_check)%></td>
                <td>
                    <% if(deployment.leak_check_unit.IsBlank()) {
                           deployment.leak_check_unit = "3";
                       }
                    %>
                    <%= Html.TextBox("leak_check", deployment.leak_check)%>
                    <%= Html.ValidationMessage("leak_check", "*") %>                    
                    <%= Html.DropDownList("leak_check_unit", WBEADMS.Models.Unit.FetchSelectList("Flow Rate", deployment.leak_check_unit))%>
                    <%= Html.ValidationMessage("leak_check_unit", "*")%>
                </td>
            </tr>
            <% } %>

            <% if (Model.SampleType.name != "PASS" && Model.SampleType.name != "VOC" && Model.SampleType.name != "PRECIP") { %>
            <tr>
                <td>
                    <label for="date_sampling_head_cleaned_on">Sampling Head Cleaned On:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DatePicker("date_sampling_head_cleaned_on", deployment.date_sampling_head_cleaned_on)%>
                    <%= Html.ValidationMessage("date_sampling_head_cleaned_on", "*")%>
                </td>
            </tr>
            <% } %>
            <% if (Model.SampleType.name != "PRECIP") { %>
            <tr>
                <td>
                    <label for="sampler_flowrate">Sampler Flowrate:</label>
                </td>
                <td></td>
                <td>
                    <%  /* HACK: default Flow Rate Unit to cc/min
                         * TODO: defaults should be done thru admin panel settings
                         */
                        if (deployment.sampler_flowrate_unit.IsBlank()) {
                            deployment.sampler_flowrate_unit = "8";
                        }
                    %>
                    <%= Html.TextBox("sampler_flowrate", deployment.sampler_flowrate)%>
                    <%= Html.ValidationMessage("sampler_flowrate", "*") %>                    
                    <%= Html.DropDownList("sampler_flowrate_unit", WBEADMS.Models.Unit.FetchSelectList("Flow Rate", deployment.sampler_flowrate_unit))%>
                    <%= Html.ValidationMessage("sampler_flowrate_unit", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="sampler_setpoint">Sampler Setpoint:</label>
                </td>
                <td><%= Html.CheckBoxNA("sampler_setpoint", deployment.sampler_setpoint)%></td>
                <td>
                    <%= Html.TextBox("sampler_setpoint", deployment.sampler_setpoint)%>
                    <%= Html.ValidationMessage("sampler_setpoint", "*") %>
                </td>
            </tr>
            <% } %>
        <% } %>
            <tr>
                <td>
                    <label for="date_sample_start">Programmed Sample Start Date:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DateTimePicker("date_sample_start", deployment.date_sample_start)%>
                    <%= Html.ValidationMessage("date_sample_start", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_sample_end">Programmed Sample End Date:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DateTimePicker("date_sample_end", deployment.date_sample_end, new { })%>
                    <%= Html.ValidationMessage("date_sample_end", "*")%>
                </td>
            </tr>
        <% if (Model.SampleType.name == "VOC") { %>
            <tr>
                <td>
                    <label for="voc_valve_open">VOC Valve Open:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DropDownList("voc_valve_open")%>
                    <%= Html.ValidationMessage("voc_valve_open", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_cannister_pressure_after_connection">VOC Cannister<br /> Pressure After Connection:</label>
                </td>
                <td><%= Html.CheckBoxNA("voc_cannister_pressure_after_connection", deployment.voc_cannister_pressure_after_connection)%></td>
                <td>
                    <%= Html.TextBox("voc_cannister_pressure_after_connection", deployment.voc_cannister_pressure_after_connection)%>
                    <%= Html.ValidationMessage("voc_cannister_pressure_after_connection", "*") %>
                    <%= Html.DropDownList("voc_cannister_pressure_after_connection_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", deployment.voc_cannister_pressure_after_connection_unit), "")%>
                    <%= Html.ValidationMessage("voc_cannister_pressure_after_connection_unit", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_sampler_pressure_after_connection">VOC Instrument<br /> Pressure After Connection:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("voc_sampler_pressure_after_connection", deployment.voc_sampler_pressure_after_connection)%>
                    <%= Html.ValidationMessage("voc_sampler_pressure_after_connection", "*") %>
                    <%= Html.DropDownList("voc_sampler_pressure_after_connection_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", deployment.voc_sampler_pressure_after_connection_unit), "")%>
                    <%= Html.ValidationMessage("voc_sampler_pressure_after_connection_unit", "*")%>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType == SampleType.VOC || Model.SampleType == SampleType.PASS) { %>
           <tr>
                <td>
                    <label for="collecting_duplicate">Collecting Duplicate:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DropDownList("collecting_duplicate")%>      
                    <%= Html.ValidationMessage("collecting_duplicate", "*")%>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType.name == "PM10" || Model.SampleType.name == "PM10E" || Model.SampleType.name == "PM2.5" || Model.SampleType.name == "PASS") { %>
            <tr>
                <td>
                    <label for="travel_blank_present">Travel Blank Present:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DropDownList("travel_blank_present")%>
                    <%= Html.ValidationMessage("travel_blank_present", "*")%>
                </td>
            </tr>
        <% } %>            
        </table>
        <input type="hidden" name="section" value="Deploy" />
		</div>
    </fieldset>