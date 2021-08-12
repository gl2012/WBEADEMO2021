<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Models" %>
<% PreparationSection preparation = Model.Preparation; %>
    
    <fieldset>
        <legend>Chain of Custody Fields</legend>
		<div class="fieldsetInfo">
        <table cellpadding="0" cellspacing="0" border="0" class="cocTable">
            <tr>
                <td>
                    <label for="chain_of_custody_id">Chain of Custody ID:</label>
                </td>
                <td>
                    <%= Model.chain_of_custody_id %>
                </td>
            </tr>
        
            <tr>
                <td>
                    <label for="created_by">Created By:</label>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(preparation.UserCreated, "Unknown")%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="sample_type_id">Sample Type:</label>
                </td>
                <td>
                    <%= Model.SampleType.name %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="schedule_id">Schedule:</label>
                </td>
                <td>
                <% if (Model.Schedule == null) { %>
                    None
                <% } else { %>
                    <%= Html.ActionLink(Model.Schedule.name, "Details", new { controller = "Schedule", id = Model.Schedule.id })%>
                <%}  %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="date_sampling_scheduled">Sampling Scheduled for:</label>
                </td>
                <td>
                    <%= preparation.date_sampling_scheduled.ToDateFormat()%>
                </td>
            </tr>
        </table>
		</div>
    </fieldset>