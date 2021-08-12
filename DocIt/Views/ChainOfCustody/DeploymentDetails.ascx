<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<% 
    DeploymentSection deployment = Model.Deployment;
   
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

	<fieldset class="fieldsetDeployment <%= action %><%= fieldsetClass %>">
        <legend>Deployment Fields</legend>
		<div class="fieldsetInfo">
        <table cellpadding="0" cellspacing="0" border="0" class="cocTable">
            <% if (Model.SampleType.name == "VOC") { %>
            <tr>
                <td>
                    <label for="voc_cannister_pressure">VOC Cannister Pressure:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.voc_cannister_pressure, "N/A")%> <%= deployment.VOCCannisterPressureUnit %>
                </td>
            </tr>
            <% } %>
            <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_deployed">Date Deployed:</label>
                </td>
                <td>
                    <%= deployment.date_deployed.ToDateTimeFormat()%>
                </td>
            </tr>
            <% } else { %>
            <tr>
                <td>
                    <label for="date_deployed">Deployment Tech Initials:</label>
                </td>
                <td>
                    <%= deployment.deployment_initials%>
                </td>
            </tr>
            <% } %>
            <tr>
                <td>
                    <label for="deployed_by">Deployed By:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.DeployedBy, "Unknown")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="location_id">Location Name:</label>
                </td>
                <td>
                    <% if (deployment.Location == null) { %>
                        None
                    <% } else { %>
                        <%= Html.ActionLink(deployment.Location.ToString(), "Details", new { controller = "Location", id = deployment.Location.location_id }) %>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="LocationFullName">Location Full Name:</label>
                </td>
                <td>
                    <% if (!String.IsNullOrEmpty(deployment.location_id)) { Response.Write(deployment.Location.full_name); }%>
                </td>
            </tr>
            <% if (Model.SampleType != SampleType.PASS || deployment.Sampler != null) {  %>
            <tr>
                <td>
                    <label for="sampler_item_id">Sampler Name:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.Sampler, "not specified")%>
                </td>
            </tr>
            <% if (deployment.Sampler != null) { %>
            <tr>
                <td>
                    <label for="SamplerSerialNumber">Sampler Serial Number:</label>
                </td>
                <td>
                    <span id="SamplerSerialNumber">
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.serial_number); } else { Response.Write("unspecified"); }%>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="SamplerMake">Sampler Make:</label>
                </td>
                <td>
                    <span id="SamplerMake">
                    
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.make); } else { Response.Write("unspecified"); }%>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="SamplerModel">Sampler Model:</label>
                </td>
                <td>
                    <span id="SamplerModel">
                        <% if (!String.IsNullOrEmpty(deployment.sampler_item_id)) { Response.Write(deployment.Sampler.model); } else { Response.Write("unspecified"); } %>
                    </span>
                </td>
            </tr>
            <% } %>
            <% } else { %><tr><td><label>Sampler:</label></td><td>N/A</td></tr><% } %>
        <% if (Model.SampleType.name != "PASS") { %>
            <tr>
                <td>
                    <label for="date_sampler_last_calibrated">Last Calibrated Date:</label>
                </td>
                <td>
                    <%= deployment.date_sampler_last_calibrated.ToDateFormat()%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_sampler_last_leak_checked">Date of Last Leak Check:</label>
                </td>
                <td>
                    <%= deployment.date_sampler_last_leak_checked.ToDateFormat()%>
                </td>
            </tr>
            <% if(Model.SampleType.name == "PAH") { %>
            <tr>
                <td>
                    <label for="leak_check">Leak Check:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.leak_check, "N/A")  %> <%if (deployment.leak_check != null && !deployment.leak_check.IsBlank()) {%><%= deployment.LeakCheckFlowRateUnit %> <% } %>
                </td>
            </tr>
            <% } %>

            <% if (Model.SampleType.name != "PASS" && Model.SampleType.name != "VOC" && Model.SampleType.name != "PRECIP") { %>
            <tr>
                <td>
                    <label for="date_sampling_head_cleaned_on">Sampling Head Cleaned On:</label>
                </td>
                <td>
                    <%= deployment.date_sampling_head_cleaned_on.ToDateFormat()%>
                </td>
            </tr>
            <% } %>
            <% if (Model.SampleType.name != "PRECIP") { %>
            <tr>
                <td>
                    <label for="sampler_flowrate">Sampler Flowrate:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.sampler_flowrate)%> <%= deployment.SamplerFlowrateUnit %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="sampler_setpoint">Sampler Setpoint:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.sampler_setpoint, "N/A")%>
                </td>
            </tr>
            <% } %>
        <% } %>
            <tr>
                <td>
                    <label for="date_sample_start">Programmed Sample Start Date:</label>
                </td>
                <td>
                    <%= deployment.date_sample_start.ToDateTimeFormat()%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_sample_end">Programmed Sample End Date:</label>
                </td>
                <td>
                    <%= deployment.date_sample_end.ToDateTimeFormat()%>
                </td>
            </tr>    
        <% if (Model.SampleType.name == "VOC") { %>
            <tr>
                <td>
                    <label for="voc_valve_open">VOC Valve Open:</label>
                </td>
                <td>
                    <%= deployment.voc_valve_open.ToHumanBool() %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_cannister_pressure_after_connection">VOC Cannister Pressure After Connection:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.voc_cannister_pressure_after_connection, "N/A")%> <%= deployment.VOCCannisterPressureAfterConnection%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="voc_sampler_pressure_after_connection">VOC Instrument Pressure After Connection:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(deployment.voc_sampler_pressure_after_connection)%> <%= deployment.VOCSamplerPressureAfterConnectionUnit%>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType.name == "VOC" || Model.SampleType.name == "PASS") { %>
           <tr>
                <td>
                    <label for="collecting_duplicate">Collecting Duplicate:</label>
                </td>
                <td>
                    <%= deployment.collecting_duplicate.ToHumanBool()%>
                </td>
            </tr>
        <% } %>
        <% if (Model.SampleType.name == "PM10" || Model.SampleType.name == "PM10E" ||
               Model.SampleType.name == "PM2.5" || Model.SampleType.name == "PASS") { %>
            <tr>
                <td>
                    <label for="travel_blank_present">Travel Blank Present:</label>
                </td>
                <td>
                    <%= deployment.travel_blank_present.ToHumanBool() %>
                </td>
            </tr>
        <% } %>            
        </table> 
		</div>
    </fieldset>