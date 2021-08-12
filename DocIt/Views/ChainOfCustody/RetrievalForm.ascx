<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Helpers.CheckBoxNAHelper" %>
<% 
    RetrievalSection retrieval = Model.Retrieval;
    SampleType sampleType = Model.SampleType;
    if (retrieval.date_sample_retrieved.IsBlank()) {
        retrieval.date_sample_retrieved = DateTime.Today.ToISODate(); // default date to today on open
    }

	string action = ViewContext.RouteData.Values["action"].ToString().ToLower();

    string fieldsetClass = "";
    if (Model.Status.name == "Deployed" || Model.Status.name == "Retrieving") {
        fieldsetClass = " fullWidth";
    }
%>

    <% if (retrieval.ElapsedSamplingDuration.HasValue) {
        ViewData["elapsed_hours"] = Math.Floor(retrieval.ElapsedSamplingDuration.Value.TotalHours);
        ViewData["elapsed_minutes"] = retrieval.ElapsedSamplingDuration.Value.Minutes;
        ViewData["elapsed_seconds"] = retrieval.ElapsedSamplingDuration.Value.Seconds;
    }%>     
    
    <fieldset class="fieldsetRetrieval <%= action %><%= fieldsetClass %>">
        <legend>Retrieval Fields</legend>
		<div class="fieldsetInfo">
        <table cellpadding="0" cellspacing="0" border="0" class="cocTable formTable">
        <% if (Model.SampleType == SampleType.VOC) { %><tr><th></th><th>N/A</th><th></th></tr><% } %>
        <% if (sampleType != SampleType.PASS && sampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="elapsed_sampling_duration">
                        Elapsed Sampling Duration:<%= Html.ValidationMessage("elapsed_sampling_duration", "*")%>  
                    </label>
                </td>
                <td></td>
                <td><table>
                    <tr>
                        <td>Hours</td>
                        <td>
                            <%= Html.TextBox("elapsed_hours", ViewData["elapsed_hours"], new { maxlength = "5", style="width:50px;" })%>
                            <%= Html.ValidationMessage("elapsed_hours", "*")%>
                        </td>
                    </tr>
                    <tr>
                        <td>Minutes:</td>
                        <td>
                            <%= Html.TextBox("elapsed_minutes", ViewData["elapsed_minutes"], new { maxlength = "6", style = "width:50px;" })%>
                            <%= Html.ValidationMessage("elapsed_minutes", "*")%>
                        </td>
                    </tr>
                    <tr>
                        <td>Seconds:</td>
                        <td>
                            <%= Html.TextBox("elapsed_seconds", ViewData["elapsed_seconds"], new { maxlength = "7", style = "width:50px;" })%>
                            <%= Html.ValidationMessage("elapsed_seconds", "*")%>
                        </td>
                    </tr>
                    <tr></tr>
                </table></td>
            </tr>
        <% } %>
        <% if (Model.SampleType != SampleType.VOC && Model.SampleType != SampleType.PASS && Model.SampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="sample_volume">Sample Volume:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("sample_volume", retrieval.sample_volume)%>
                    <%= Html.ValidationMessage("sample_volume", "*") %>
                    <%= Html.DropDownList("sample_volume_unit", WBEADMS.Models.Unit.FetchSelectList("Volume", retrieval.sample_volume_unit))%>
                    <%= Html.ValidationMessage("sample_volume_unit", "*")%>
                    <% if (Model.SampleType == SampleType.PRECIP) {  %>(Leave blank if N/A)<% } %>
                </td>
            </tr>
        <% } %>
            <tr>
                <td>
                    <label for="date_actual_sample_start">Actual Sample Start Date:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DateTimePicker("date_actual_sample_start", retrieval.date_actual_sample_start)%>
                    <%= Html.ValidationMessage("date_actual_sample_start", "*")%>
                    
                    <% if (Model.Status.name == "Deployed" || Model.Status.name == "Retrieval" || Model.Status.name == "Retrieving") { %>
                    <input id="copyProgrammed" type="button" value="Copy from Deployment" title="Copy Programmed Start/End Dates from the Deployment section" />
                    <script type="text/javascript">
                        $('#copyProgrammed').click(function() {
                            $('#date_actual_sample_start_date').val('<%= Model.Deployment.date_sample_start.IsBlank() ? "" : Model.Deployment.date_sample_start.ToDateFormat() %>');
                            $('#date_actual_sample_start_hour').val('<%= Model.Deployment.date_sample_start.IsBlank() ? "" : Model.Deployment.date_sample_start.ToDateTime().Value.Hour.ToString().PadLeft(2, "0"[0]) %>');
                            $('#date_actual_sample_start_mins').val('<%= Model.Deployment.date_sample_start.IsBlank() ? "" : Model.Deployment.date_sample_start.ToDateTime().Value.Minute.ToString().PadLeft(2, "0"[0]) %>');
                            $('#date_actual_sample_start').val('<%= Model.Deployment.date_sample_start.IsBlank() ? "" : Model.Deployment.date_sample_start %>');
                            $('#date_actual_sample_end_date').val('<%= Model.Deployment.date_sample_end.IsBlank() ? "" : Model.Deployment.date_sample_end.ToDateFormat() %>');
                            $('#date_actual_sample_end_hour').val('<%= Model.Deployment.date_sample_end.IsBlank() ? "" : Model.Deployment.date_sample_end.ToDateTime().Value.Hour.ToString().PadLeft(2, "0"[0]) %>');
                            $('#date_actual_sample_end_mins').val('<%= Model.Deployment.date_sample_end.IsBlank() ? "" : Model.Deployment.date_sample_end.ToDateTime().Value.Minute.ToString().PadLeft(2, "0"[0]) %>');
                            $('#date_actual_sample_end').val('<%= Model.Deployment.date_sample_end.IsBlank() ? "" : Model.Deployment.date_sample_end %>');
                        });
                    </script>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_actual_sample_end">Actual Sample End Date:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DateTimePicker("date_actual_sample_end", retrieval.date_actual_sample_end)%>
                    <%= Html.ValidationMessage("date_actual_sample_end", "*")%>
                </td>
            </tr>
            <% if(sampleType == SampleType.PRECIP) { %>
            <tr>
                <td>
                    <label for="wet_week">Wet Week:</label>
                </td>
                <td></td>
                <td>
                    <% if(retrieval.wet_week != null && retrieval.wet_week.IsBlank()) retrieval.wet_week="Yes"; %>
                    <%= Html.DropDownList("wet_week",  new List<SelectListItem>{ new SelectListItem{ Value="Yes", Text="Yes", Selected=retrieval.wet_week=="Yes"}, new SelectListItem{ Value="No", Text="No", Selected=retrieval.wet_week=="No"}}) %>
                </td>
            </tr>
            <% } %>
        <% if (sampleType != SampleType.PASS && sampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="average_station_temperature">Average Station Temperature:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("average_station_temperature", retrieval.average_station_temperature)%>
                    <%= Html.ValidationMessage("average_station_temperature", "*") %>
                    <%= Html.DropDownList("average_station_temperature_unit", WBEADMS.Models.Unit.FetchSelectList("Temperature", retrieval.average_station_temperature_unit))%>
                    <%= Html.ValidationMessage("average_station_temperature_unit", "*")%>
                    <% if (Model.SampleType == SampleType.PRECIP || Model.SampleType == SampleType.PASS) {  %>(Leave blank if N/A)<% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_ambient_temperature">Ambient Temperature:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("average_ambient_temperature", retrieval.average_ambient_temperature)%>
                    <%= Html.ValidationMessage("average_ambient_temperature", "*")%>
                    <%= Html.DropDownList("average_ambient_temperature_unit", WBEADMS.Models.Unit.FetchSelectList("Temperature", retrieval.average_ambient_temperature_unit))%>
                    <%= Html.ValidationMessage("average_ambient_temperature_unit", "*")%>
                    <% if (Model.SampleType == SampleType.PRECIP || Model.SampleType == SampleType.PASS) {  %>(Leave blank if N/A)<% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_barometric_pressure">Atmospheric Pressure:</label>
                </td>
                <td></td>
                <td>
                    <%  /* HACK: default Atmospheric Pressure to atm
                         * TODO: defaults should be done thru admin panel settings
                         */
                        if (retrieval.average_barometric_pressure_unit.IsBlank()) {
                            retrieval.average_barometric_pressure_unit = "2";
                        }
                    %>
                    <%= Html.TextBox("average_barometric_pressure", retrieval.average_barometric_pressure)%>
                    <%= Html.ValidationMessage("average_barometric_pressure", "*") %>
                    <%= Html.DropDownList("average_barometric_pressure_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", retrieval.average_barometric_pressure_unit), "")%>
                    <%= Html.ValidationMessage("average_barometric_pressure_unit", "*")%>
                    <% if (Model.SampleType == SampleType.PRECIP || Model.SampleType == SampleType.PASS) {  %>(Leave blank if N/A)<% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_relative_humidity">Ambient Relative Humidity:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("average_relative_humidity", retrieval.average_relative_humidity)%> %
                    <%= Html.ValidationMessage("average_relative_humidity", "*") %>
                    <% if (Model.SampleType == SampleType.PRECIP || Model.SampleType == SampleType.PASS) {  %>(Leave blank if N/A)<% } %>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType.name == "VOC") { %>
            <tr>
                <td>
                    <label for="voc_final_cannister_pressure">VOC Final Cannister Pressure:</label>
                </td>
                <td><%= Html.CheckBoxNA("voc_final_cannister_pressure", retrieval.voc_final_cannister_pressure)%></td>
                <td>
                    <%= Html.TextBox("voc_final_cannister_pressure", retrieval.voc_final_cannister_pressure)%>
                    <%= Html.ValidationMessage("voc_final_cannister_pressure", "*") %>
                    <%= Html.DropDownList("voc_final_cannister_pressure_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", retrieval.voc_final_cannister_pressure_unit), "")%>
                    <%= Html.ValidationMessage("voc_final_cannister_pressure_unit", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_final_sampler_pressure">VOC Final Sampler Pressure:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("voc_final_sampler_pressure", retrieval.voc_final_sampler_pressure)%>
                    <%= Html.ValidationMessage("voc_final_sampler_pressure", "*") %>
                    <%= Html.DropDownList("voc_final_sampler_pressure_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", retrieval.voc_final_sampler_pressure_unit), "")%>
                    <%= Html.ValidationMessage("voc_final_sampler_pressure_unit", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_valve_closed">VOC Valve Closed:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DropDownList("voc_valve_closed", (IEnumerable<SelectListItem>)ViewData["yes_no_selectlist"])%>
                    <%= Html.ValidationMessage("voc_valve_closed", "*") %>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_sample_retrieved">Date Retrieved:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.DatePicker("date_sample_retrieved", retrieval.date_sample_retrieved)%>
                    <%= Html.ValidationMessage("date_sample_retrieved", "*")%>
                </td>
            </tr>
         <% } else { %>
            <tr>
                <td>
                    <label for="retrieval_initials">Retrieval Tech Initials:</label>
                </td>
                <td></td>
                <td>
                    <%= Html.TextBox("retrieval_initials", retrieval.retrieval_initials)%>
                    <%= Html.ValidationMessage("retrieval_initials", "*")%>
                </td>
            </tr>
        <% } %>
            <tr>
                <td>
                    <label for="retrieval_flags">Field User Flags:</label>
                </td>
                <td></td>
                <td>
                    <% List<SelectListItem> items = new List<SelectListItem>();
                       foreach (KeyValuePair<string, string> entry in RetrievalSection.FieldUserFlags)
                       {
                           SelectListItem item = new SelectListItem { Text = entry.Value, Value = entry.Key };
                           if(retrieval.field_user_flag == entry.Key) item.Selected = true;
                           else item.Selected = false;
                           items.Add(item);
                       }
                    %>
                    <%= Html.DropDownList("field_user_flag", items) %>
                </td>
            </tr>

        </table>
        <input type="hidden" name="section" value="Retrieve" />
		</div>
    </fieldset>