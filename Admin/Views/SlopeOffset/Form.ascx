<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.SlopeOffset>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>
    <div class="colSetup">
    <div class="rightCol">
	<fieldset>
		<legend>Help</legend>
	    <p>
            When entering <b>Station</b> and <b>Analyzer</b> the letter O and 0(zero) are <b>not</b> interchangeable.<br />
            <br />
            <b>Station</b> is the name of the Analyzer station Eg. AMS01, MAMSL11<br />
            <b>Station</b> is <b>not</b> case senstive. Eg. AMS01 is the same as aMs01<br />
            <b>Analyzer</b> is the parameter that is to be blacked out. Eg. H2S, NO2, NOX<br />
            <b>Analyzer</b> is <b>not</b> case senstive. Eg. h2s is the same as H2S<br />
            <b>Active Date</b> is the inclusive start date marking when the new slope and offset should take effect.<br />
	    </p>
	</fieldset>	
	</div>
	<div class="leftCol">
	<fieldset>
		<legend>Slope/Offset Fields</legend>
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
				<td><label for="date_active">Active Date:</label></td>
				<td>
					<%= Html.DateTimePicker("date_active", Model.date_active)%>
					<%= Html.ValidationMessage("date_active", "*")%>
				</td>
			</tr>
			<tr>
				<td><label for="slope">Slope:</label></td>
				<td>
					<%= Html.TextBox("slope")%>
					<%= Html.ValidationMessage("slope", "*")%>
				</td>
			</tr>
			<tr>
				<td><label for="offset">Offset:</label></td>
				<td>
					<%= Html.TextBox("offset")%>
					<%= Html.ValidationMessage("offset", "*")%>
				</td>
			</tr>
		</table>
	
		<br /><input type="submit" value="Save" />
	</fieldset>
	</div>
	</div>
	<% } %>