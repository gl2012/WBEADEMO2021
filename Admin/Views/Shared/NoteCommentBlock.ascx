<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Note[]>" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>
 
<div class="noteComments">
		<fieldset>
	<% if (Model.Length == 0) { %><%-- Note Comments fieldset begin --%>
			<legend>Notes</legend>
			<p>No comments.</p>
	<% } else if (Model.Length >= 99) { %>
			<legend>Notes (first 99)</legend>
	<% } else { %>
			<legend>Notes (<%= Model.Length %>)</legend>
	<% } %>
		<% if (ViewData["parent_id"] != null) { %>
			<div class="buttonList"><%= Html.ActionButton("Add Comment", "Create", 
				 new { controller = "Note", parent_id = ViewData["parent_id"].ToString(), parent_type = ((int)ViewData["parent_type"]).ToString() })%></div><br class="clear" />
		<% } %>
	<% if (Model.Length != 0) { %><%-- Note Comments table --%>
	
		<br class="clear" />
		<table cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<!--
				<th style="width:65px;" class="iconDesc no-sort">
					<span title="Details">D</span>
					<span title="Starred?">S</span>
				</th>
			    -->
				<th><span class="location_id">Location</span></th>
				<th><span class="parameter_list">Parameters</span></th>
				<th><span class="date_occurred">Date Created</span></th>
				<th><span class="created_by">Author</span></th>
			</tr>

	<% foreach (var item in Model) { %>
	
			<tr class="newRow">
				<!--
				<td class="tableActionLinks">
					<span class="details"><%= Html.NoteDetailsLink(item) %></span>
					<span class="star"><%= Html.NoteStarLink(item)%></span>
					<%-- TODO: have a separate icon when a note is deleted --%><%-- if (item.Nparent.deleted) { } --%>
				</td>
			    -->
				<td><span class="location_id"><%= Html.Encode(item.Location.name)%></span></td>
				<td><span class="parameter_list"><% foreach (var parameter in item.parameters) { %><%= parameter.name%> <% } %>&nbsp;</span></td>
				<td><span class="date_occurred"><%= Html.Encode(item.DateCreated)%></span></td>
				<td><span class="created_by"><%= Html.Encode(item.created_user.display_name)%></span></td>
			</tr>
			<tr>
				<td></td>
				<td colspan="4"><%= Html.Encode(item.body)%></td>
			</tr>
	<% } %>
	
		</table>
		
	</fieldset>
	<% } %>
</div>