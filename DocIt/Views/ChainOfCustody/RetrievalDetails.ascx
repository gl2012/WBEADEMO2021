<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Models" %>
<% RetrievalSection retrieval = Model.Retrieval;
   SampleType sampleType = Model.SampleType;

    string action = ViewContext.RouteData.Values["action"].ToString().ToLower();

    string fieldsetClass = "";
    if (Model.Status.name == "Opened" || Model.Status.name == "Prepared" || Model.Status.name == "Preparing" || Model.Status.name == "Deploying" ||
        (action.ToLower() == "details" && (Model.Status.name == "Deployed" || Model.Status.name == "Retrieving"))) {
        fieldsetClass = " fullWidth";
    } else { 
		fieldsetClass = " halfWidth";
	}

%>

    <fieldset class="fieldsetRetrieval">
        <legend>Retrieval Fields</legend>
		<div class="fieldsetInfo">
        <table cellpadding="0" cellspacing="0" border="0" class="cocTable">
        <% if (sampleType != SampleType.PASS && sampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="elapsed_sampling_duration">Elapsed Sampling Duration:</label>
                </td>
                <td>
                <% if (retrieval.ElapsedSamplingDuration.HasValue) { %>
                    <%= Math.Floor(retrieval.ElapsedSamplingDuration.Value.TotalHours) + 
                        ":" + retrieval.ElapsedSamplingDuration.Value.Minutes +
                        ":" + retrieval.ElapsedSamplingDuration.Value.Seconds %>                
                <% } %>
                </td>
            </tr>
        <% } %>
        <% if (sampleType != SampleType.VOC && sampleType != SampleType.PASS && sampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="sample_volume">Sample Volume:</label>
                </td>
                <td>
                    <%= retrieval.sample_volume%> <%= retrieval.SampleVolumeUnit %>
                </td>
            </tr>
        <% } %>
            <tr>
                <td>
                    <label for="date_actual_sample_start">Actual Sample Start Date:</label>
                </td>
                <td>
                    <%= retrieval.date_actual_sample_start.ToDateTimeFormat()%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_actual_sample_end">Actual Sample End Date:</label>
                </td>
                <td>
                    <%= retrieval.date_actual_sample_end.ToDateTimeFormat()%>
                </td>
            </tr>
        <% if (sampleType != SampleType.PASS && sampleType != SampleType.PRECIP) {  %>
            <tr>
                <td>
                    <label for="average_station_temperature">Average Station Temperature:</label>
                </td>
                <td>
                    <%= retrieval.average_station_temperature%> <%= retrieval.AverageStationTemperatureUnit%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_ambient_temperature">Ambient Temperature:</label>
                </td>
                <td>
                    <%= retrieval.average_ambient_temperature%>  <%= retrieval.AverageAmbientTemperatureUnit%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_barometric_pressure">Atmospheric Pressure:</label>
                </td>
                <td>
                    <%= retrieval.average_barometric_pressure%> <%= retrieval.AverageBarometricPressureUnit%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="average_relative_humidity">Ambient Relative Humidity:</label>
                </td>
                <td>
                    <%= retrieval.average_relative_humidity%> %
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType == SampleType.VOC) { %>
            <tr>
                <td>
                    <label for="voc_final_cannister_pressure">VOC Final Cannister Pressure:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(retrieval.voc_final_cannister_pressure, "N/A")%> <%= retrieval.VOCFinalCannisterPressureUnit %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_final_sampler_pressure">VOC Final Sampler Pressure:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(retrieval.voc_final_sampler_pressure)%> <%= retrieval.VOCFinalSamplerPressureUnit%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_valve_closed">VOC Valve Closed:</label>
                </td>
                <td>
                    <%= retrieval.voc_valve_closed.ToHumanBool()%>
                </td>
            </tr>
        <% } %>
            <% if(Model.SampleType == SampleType.PRECIP) { %>
            <tr>
                <td>
                    <label for="wet_week">Wet Week:</label>
                </td>
                <td>
                    <% if(retrieval.wet_week != null && retrieval.wet_week.IsBlank()) retrieval.wet_week="Yes"; %>
                    <%= retrieval.wet_week %>

                </td>
            </tr>
            <% } %>
            <tr>
                <td>
                    <label for="retrieved_by">Retrieved By:</label>
                </td>
                <td>
                    <%= retrieval.RetrievedBy%>
                </td>
            </tr>
        <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_sample_retrieved">Date Retrieved:</label>
                </td>
                <td>
                    <%= retrieval.date_sample_retrieved.ToDateFormat()%>
                </td>
            </tr>    
        <% } else { %>
            <tr>
                <td>
                    <label for="retrieval_initials">Retrieval Tech Initials:</label>
                </td>
                <td>
                    <%= retrieval.retrieval_initials %>
                </td>
            </tr>    
        <% } %>
            <tr>
                <td>
                    <label for="field_user_flag">Field User Flag:</label>
                </td>
                <td>
                    <% string flag = retrieval.field_user_flag;
                       string fieldUserFlagText = "None";
                       if (RetrievalSection.FieldUserFlags.ContainsKey(flag)) fieldUserFlagText = RetrievalSection.FieldUserFlags[flag];
                    %>
                    <%= fieldUserFlagText %>
                </td>
            </tr>
        </table>
		</div>
    </fieldset>