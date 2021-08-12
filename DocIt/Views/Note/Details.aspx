<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Note>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Note Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-notes">Note Details
        <% if (Model.committed) { %><span>[Committed]</span><% } %>
        <% if (Model.deleted) { %><span>[Deleted]</span><% } %>
    </h2>
	
	<div class="buttonList">
		<ul>
			<li><% if (!Model.committed) { %> <%=Html.ActionButton("Edit", "Edit", new { id=Model.note_id }) %><% } %></li>
			<li><% if (!Model.deleted) { %> <%=Html.PostLink("Delete", new { action = "Delete", id = Model.note_id }, new { postFunction = "location='" + Url.RouteUrl(new { action = "Index", controller = "Note" }) + "'" })%><% } %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
	</div>
	
	<div class="colSetup">		
		<div class="leftCol lessWide">		
			<fieldset>
				<legend>Note Details</legend>
				<table cellpadding="0" cellspacing="0" border="0" width="100%">
					<tr>
						<td class="label">Parent:</td>
						<td><%= Html.NoteParentLink(Model) %></td>
					</tr>
					<tr>
						<td class="label">Author:</td>
						<td><%= Html.Encode(Model.created_user) %></td>
					</tr>
					<tr>
						<td class="label">Date Created:</td>
						<td><%= Html.Encode(Model.date_created)%></td>
					</tr>
					<tr>
						<td colspan="2"><div class="horizBorder"></div></td>
					</tr>
					
				<% if (!String.IsNullOrEmpty(Model.modified_by)) { %>					
					<tr>
						<td class="label">Modified By:</td>
						<td><%= Html.Encode(Model.modified_user)%></td>
					</tr>
					<tr>
						<td class="label">Date Modified:</td>
						<td><%= Html.Encode(Model.date_modified)%></td>
					</tr>
					<tr>
						<td colspan="2"><div class="horizBorder"></div></td>
					</tr>
				<% } %>
				
					<tr>
						<td class="label">Starred:</td>
						<td><%= Html.NoteStarLink(Model)%></td>
					</tr>
					<tr>
						<td class="label">Location:</td>
						<td><%= Html.ActionLink(Model.Location.name, "Details", new { controller = "Location", id = Model.location_id }) %></td>
					</tr>
					<tr>
						<td class="label">Parameters:</td>
						<td><%= Html.NoteParameters(Model.parameters) %></td>
					</tr>
					<tr>
						<td class="label">Occurred:</td>
						<td><%= Html.Encode(Model.DateOccurred) %></td>
					</tr>
				</table>
				<p>
					<div class="noteBody">
                        <%= Html.Encode(Model.body).Replace(Environment.NewLine, "<br/>")%>
                        <br />
					Attachment: <%= Model.GetAttachmentLinks() %>
				</p>
			</fieldset>
		
		</div>
		
		<div class="rightCol wider">
			<% Html.RenderPartial("NoteCommentBlock", Model.notes); %>
		</div>
	</div>

	<div class="buttonList">
		<ul>
			<li><% if (!Model.committed) { %> <%=Html.ActionButton("Edit", "Edit", new { id=Model.note_id }) %><% } %></li>
			<li><% if (!Model.deleted) { %> <%=Html.PostLink("Delete", new { action = "Delete", id = Model.note_id }, new { postFunction = "location='" + Url.RouteUrl(new { action = "Index", controller = "Note" }) + "'" })%><% } %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
	</div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>