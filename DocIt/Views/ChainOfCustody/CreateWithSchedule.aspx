<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create COC With Schedule
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-coc-create">Create COC With Schedule</h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>COC Fields</legend>
			
			<table cellpadding="0" cellspacing="0" border="0" class="cocTable">
				<tr>
					<td><label for="created_by">Created By:</label></td>
					<td><%= Model.UserCreated %></td>
				</tr>
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
					<td><%= Model.Schedule.SampleType.name %></td>
				</tr>
				<tr>
					<td><label for="schedule_id">Schedule:</label></td>
					<td><%= Model.Schedule.name %></td>
				</tr>
				<tr>
					<td><label for="date_sampling_scheduled">Sampling Scheduled for:</label></td>
					<td><%= Model.date_sampling_scheduled %> <%= Html.ValidationMessage("date_sampling_scheduled", "*")%></td>
				</tr>
				<tr>
					<td><label for="location_id">Location:</label></td>
					<td><%= Model.Schedule.Location.name %></td>
				</tr>
				<tr>
					<td></td>
					<td></td>
				</tr>
			</table>

			<input type="hidden" name="created_by" value="<%= Model.Preparation.created_by %>" />
			<input type="hidden" name="schedule_id" value="<%= Model.schedule_id %>" />
			<input type="hidden" name="date_sampling_scheduled" value="<%= Model.date_sampling_scheduled %>" />
			<br /><input type="submit" value="Create" />
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>