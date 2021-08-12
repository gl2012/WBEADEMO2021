<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Blackout>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>
    <div class="colSetup">
    <div class="rightCol">
	<fieldset>
		<legend>Help</legend>
	<p>
	    <b>Black Out intervals may overlap.</b><br />
        When entering <b>Station</b> and <b>Analyzer</b> the letter O and 0(zero) are <b>not</b> interchangeable.<br />
        <br />
        <b>Station</b> is the name of the Analyzer station Eg. AMS01, MAMSL11<br />
        <b>Station</b> is <b>not</b> case senstive. Eg. AMS01 is the same as aMs01<br />
        <b>Analyzer</b> is the parameter that is to be blacked out. Eg. H2S, NO2, NOX<br />
        <b>Analyzer</b> is <b>not</b> case senstive. Eg. h2s is the same as H2S<br />
        <b>Start Date</b> is the inclusive start date for the data blackout.<br />
        <b>End Date</b> is the inclusive end date for the data black out to end.<br />
	</p>
	</fieldset>	
	</div>
	<div class="leftCol">
	<fieldset>
		<legend>Blackout Fields</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td><label for="station_id">Station:</label></td>
				<td>
					<%= Html.TextBox("station_id") %>
					<%= Html.ValidationMessage("station_id", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="analyzer">Analyzer:</label></td>
				<td>
					<%= Html.TextBox("analyzer") %>
					<%= Html.ValidationMessage("analyzer", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="date_start">Start Date:</label></td>
				<td>
					<%= Html.DateTimePicker("date_start", Model.date_start)%>
					<%= Html.ValidationMessage("date_start", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="date_end">End Date:</label></td>
				<td>
					<%= Html.DateTimePicker("date_end", Model.date_end)%>
					<%= Html.ValidationMessage("date_end", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="comment">Comment:</label></td>
				<td>
					<%= Html.TextBox("comment")%>
					<%= Html.ValidationMessage("comment", "*")%>
				</td>
			</tr>
		</table>
	
		<br /><input type="submit" value="Save" />
	</fieldset>
	</div>
	</div>
	<% } %>