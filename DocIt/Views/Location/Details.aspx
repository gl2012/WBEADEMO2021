<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Location>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Details for <%= Model %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-locations">Details for <%= Model %></h2>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.location_id }) %></li>
			<li><%=Html.ActionLink("Pictures", "ListByLocation", new { controller = "LocationImage", id = Model.location_id })%></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
	
	<div class="colSetup">
		<div class="leftCol lessWide">
			<fieldset>
				<legend>Details</legend>
				
				<table cellpadding="0" cellspacing="0" border="0">
					<tr>
						<td class="label">Name:</td>
						<td><%= Html.Encode(Model.name) %></td>
					</tr>
					<tr>
						<td class="label">Full Name:</td>
						<td><%= Html.Encode(Model.full_name) %></td>
					</tr>
					<tr>
						<td class="label">Latitude:</td>
						<td><%= Html.Encode(Model.latitude) %></td>
					</tr>
					<tr>
						<td class="label">Longitude:</td>
						<td><%= Html.Encode(Model.longitude) %></td>
					</tr>
					<tr>
						<td class="label">Address:</td>
						<td><%= Html.Encode(Model.address) %></td>
					</tr>
					<tr>
						<td class="label">Phone Number:</td>
						<td><%= Html.Encode(Model.phone_number) %></td>
					</tr>
					<tr>
						<td class="label">Comments:</td>
						<td><%= Html.Encode(Model.comments) %></td>
					</tr>
					<tr>
						<td class="label">Terrain Description:</td>
						<td><%= Html.Encode(Model.terrain_description) %></td>
					</tr>
					<tr>
						<td class="label">Tree Canopy Description:</td>
						<td><%= Html.Encode(Model.tree_canopy_description) %></td>
					</tr>
					<tr>
						<td class="label">Nearby Sources:</td>
						<td><%= Html.Encode(Model.nearby_sources) %></td>
					</tr>
					<tr>
						<td class="label">Wet Weather Access:</td>
						<td><%= Html.Encode(Model.wet_weather_access) %></td>
					</tr>
					<tr>
						<td class="label">Winter Access:</td>
						<td><%= Html.Encode(Model.winter_access) %></td>
					</tr>
					<tr>
						<td class="label">Last Modified By:</td>
						<td>
							<% if (Model.UserModified != null) { %>
								<%= Model.UserModified.display_name %>
							<%} %>
						</td>
					</tr>
					<tr>
						<td class="label">Date Last Modified:</td>
						<td><%= Html.Encode(Model.date_modified) %></td>
					</tr>
					<tr>
						<td class="label">Created By: </td>
						<td>
							<% if (Model.UserCreated != null) { %>
								<%= Html.ActionLink(Model.UserCreated.display_name, "Details", new { controller = "User", id = Model.UserCreated.user_id })%>
							<%} %>
						</td>
					</tr>
					<tr>
						<td class="label">Date Created:</td>
						<td><%= Html.Encode(Model.date_created) %></td>
					</tr>
					<tr>
						<td class="label">QA Signoff:</td>
						<td><%= Html.Encode(Model.qa_signoff) %></td>
					</tr>
					<tr>
						<td class="label">Date QA Signoff:</td>
						<td><%= Html.Encode(Model.DateQASignoff) %></td>
					</tr>
					<tr>
						<td class="label">Active:</td>
						<td>
							<% if (Model.active == "True") { %>
								Yes 
							<% } else { %>
								No
							<% } %> 
						</td>
					</tr>
				</table>
			</fieldset>
		</div>
		
		<div class="rightCol wider">

			<div class="tabs">		
				<ul>
					<li><a href="#locationItems">Items:</a></li>
					<li><a href="#locationComments">Notes:</a></li>
					<li><a href="#locationAudit">Audit Log:</a></li>
				</ul>
				
				<div id="locationItems">
					<!-- TODO: shall we move this Item block to another list and make it tabbed like old DocIt? -->

					<fieldset>
						<legend>Items (<%= Model.Items.Count() %>)</legend>
						<!-- TODO: add search/filter/pagination fields like in old DocIt -->
						<table class="generalTable">
						<tr>
							<th class="iconDesc no-sort"><span title="Details">D</span></th>
							<!--<th>Serial Number</th>-->
							<th>Name</th>
							<th>Make - Model</th>
							<th>Parameter</th>
							<th>Install Date</th>
							<!--<th>Removal Date</th>-->
						</tr>
				<% foreach (var item in Model.Items) { %>
						<tr>
							<td class="tableActionLinks"><span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { controller = "Item", id = item.id })%></span></td>
							<!--<td><%= item.serial_number %></td>-->
							<td><%= item.name %></td>
							<td><%= item.make %> - <%= item.model %></td>
							<td><% foreach(var param in item.parameters) { %><%= param %> <% } %></td>
							<td><%= item.DateInstalled%></td>
							<!--<td><%= item.DateRemoved %></td>-->
						</tr>
				<% } %>
						</table>
					</fieldset>
				</div>
				<div id="locationComments">
					<% Html.RenderPartial("NoteCommentBlock", Model.Notes); %>
				</div>
				<div id="locationAudit">
					<% Html.RenderPartial("AuditHistory", Model.FetchAuditRecords()); %>
				</div>
				
			</div>


		</div>	
	</div>
	
	<script type="text/javascript">
		$(document).ready(function() {
			// Initiate Tabs
			$('.tabs').tabs();
		});
	</script>
	
</asp:Content>