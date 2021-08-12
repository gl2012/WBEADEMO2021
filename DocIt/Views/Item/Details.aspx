<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Item>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details for <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items">Details for <%= Model %></h2>
	
    <div class="buttonList">
		<ul> 
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.item_id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
	
	<div class="colSetup">
	
		<div class="leftCol">

			<fieldset>
				<legend>Fields</legend>
				
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td class="label">Serial Number:</td>
						<td><%= Html.Encode(Model.serial_number) %></td>
					</tr>
					<tr>
						<td class="label">Name:</td>
						<td><%= Html.Encode(Model.name) %></td>
					</tr>
                <% if (Model.IsIntegrated) { %>
                    <tr>
                        <td class="label">Sample Type:</td>
                        <td><%= Model.SampleType %></td>
                    </tr>
                <% } else { %>
				<% if (Model.parameters.Length == 1) { %>
					<tr>
						<td class="label">Parameter:</td>
						<td><%= Model.parameters[0].name%></td>
					</tr>
				<% } else { %>
					<tr>
						<td class="label">Parameters:</td>
						<td>
							<ul>
								<% foreach (var parameter in Model.parameters) { %>
								<li><%= parameter.name %></li>
								<% } %>
							</ul>
						</td>
					</tr>
				<% } %>
				<% } %>
					<tr>
						<td class="label">Make/Model:</td>
						<td><%= (Model.model == null) ? "none" : Html.Encode(Model.model.display_name) %></td>
					</tr>
					<tr>
						<td class="label">Location:</td>
						<td>
							<% if (Model.Location == null) { %>None<% } else { %>
								<%= Html.ActionLink(Model.Location.name, "Details", new { controller = "Location", id = Model.Location.id })%>
								<%= Html.ActionLink("(move)", "Relocate", new { location_id = Model.Location.id, item_id = Model.id })%>
							<% } %>
						</td>
					</tr>
					<tr>
						<td class="label">Date Created:</td>
						<td><%= Html.Encode(Model.date_created) %></td>
					</tr>
					<tr>
						<td class="label" colspan="2">Comment:</td>
					</tr>
					<tr>
						<td colspan="2">
							<div class="noteBody"><%= Html.Encode(Model.comment) %></div>
						</td>
					</tr>
				</table>
			</fieldset>
			
		</div>
		
		<div class="rightCol">
		
			<div class="tabs">		
				<ul>
					<li><a href="#itemHistory">History:</a></li>
					<li><a href="#itemComments">Notes:</a></li>
				</ul>
				
				<div id="itemHistory">
					<fieldset>
						<legend>History</legend>
						<table class="generalTable">
							<tr>
								<th>Location</th>
								<th>Install Date</th>
								<th>Removal Date</th>
							</tr>
					<% foreach (var item in WBEADMS.Models.ItemHistory.GetItemHistorys(Model.id)) { %>
							<tr>
								<td><%= Html.ActionLink(item.Location.name, "Details", new { controller = "Location", id = item.Location.id}) %></td>
								<td><%= item.DateInstalled%></td>
								<td><%= item.DateRemoved%></td>
							</tr>
					<% } %>
						</table>
					</fieldset>
				</div>
				
				<div id="itemComments">
					<% Html.RenderPartial("NoteCommentBlock", Model.notes); %>
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