<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Schedule>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details for <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-schedules">Details for <%= Model %></h2>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.schedule_id }) %></li>
			<%--  <li><%= Html.ActionLink("Create Chain of Custody", "CreateWithSchedule", new { controller = "ChainOfCustody", id = Model.schedule_id, date_sample_start = DateTime.Today.ToISODate()})%></li> --%>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
			
		</ul>
		<div class="clear-fix"></div>
    </div>
	
	<div class="colSetup">
		<div class="leftCol">
			<fieldset>
				<legend>Fields</legend>
				
				<table cellpadding="0" cellspacing="0" border="0">
					<tr>
						<td class="label">Name:</td>
						<td><%= Html.Encode(Model.name) %></td>
					</tr>
					<tr>
						<td class="label">Location:</td>
						<td><%= Html.ActionLink(Html.Encode(Model.Location.name), "Details", new {controller = "Location", id = Model.location_id}) %></td>
					</tr>
					<tr>
						<td class="label">Contact:</td>
						<td><%= Html.Encode(Model.contact.display_name) %></td>
					</tr>
					<tr>
						<td class="label">Sample Type:</td>
						<td><%= Html.Encode(Model.SampleType.name) %></td>
					</tr>
					<tr>
						<td class="label">Start Date:</td>
						<td><%= Html.Encode(Model.DateStart) %></td>
					</tr>
					<tr>
						<td class="label">End Date:</td>
						<td><%= Html.Encode(Model.DateEnd) %></td>
					</tr>
					<tr>
						<td class="label">Interval:</td>
						<td><%= Html.Encode(Model.interval.ToSentence(Model.frequency_data)) %></td>
					</tr>
					<tr>
						<td class="label">Comments:</td>
						<td><%= Html.Encode(Model.comments) %></td>
					</tr>
					<tr>
						<td class="label">Active:</td>
						<td>
					<%= Model.is_active.ToHumanBool() %>
						</td>
					</tr>
					<tr>
						<td class="label">Created By:</td>
						<td>
							<% if (!String.IsNullOrEmpty(Model.created_by)) { %>
							<%= Model.created_user %>
							<% } %>
						</td>
					</tr>
					<tr>
						<td class="label">Date Created:</td>
						<td><%= Html.Encode(Model.date_created) %></td>
					</tr>
					<tr>
						<td class="label">Last Modifed By:</td>
						<td>
							<% if (!String.IsNullOrEmpty(Model.modified_by)) { %>
							<%= Model.modified_user %>
							<% } %>
						</td>
					</tr>
					<tr>
						<td class="label">Date Last Modified:</td>
						<td><%= Html.Encode(Model.date_modified)%></td>
					</tr>
				</table>
			</fieldset>
		</div>
		
		<div class="rightCol">
		    <% Html.RenderPartial("NoteCommentBlock", Model.Notes); %>
		</div>
	</div>

</asp:Content>