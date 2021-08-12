<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Note>" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>
    <div id="noteParentDetails" style="clear:both;"><!-- TODO: move this style into a CSS file -->
        <fieldset>
            <legend>
                Parent Details 
                [<%= Model.parent_type %>] 
                <% if (Model.parent_type == WBEADMS.Models.Note.ParentType.Note && ((WBEADMS.Models.Note)Model.parent).deleted) { Response.Write("[Deleted]"); }%>
                
            </legend>
			
			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td>Parent:</td>
					<td><%= Html.NoteParentLink(Model)%></td>
				</tr>
				
        <% switch (Model.parent_type) {
            case WBEADMS.Models.Note.ParentType.Note:
                var Nparent = (WBEADMS.Models.Note)Model.parent; %>
				<tr>
					<td>Author:</td>
					<td><%= Nparent.created_user%></td>
				</tr>
				<tr>
					<td>Location:</td>
					<td><%= Nparent.Location%></td>
				</tr>
				<tr>
					<td>Parameters:</td>
					<td><%= Html.NoteParameters(Nparent.parameters)%></td>
				</tr>
				<tr>
					<td>Created:</td>
					<td><%= Nparent.DateCreated%></td>
				</tr>
				<tr>
					<td>Starred:</td>
					<td><%= Html.NoteStarImage(Nparent)%></td>
				</tr>
				<% if (!Nparent.body.IsBlank()) { %>
				<tr>
					<td colspan="2">Body:</td>
				</tr>
				<tr> 
					<td colspan="2" class="noteBody"><%= Nparent.body%></td>
				</tr>
				<% } %>
                <% break;
                   
            case WBEADMS.Models.Note.ParentType.Item:
                var Iparent = (WBEADMS.Models.Item)Model.parent; %>
				<tr>
					<td>Serial #:</td>
					<td><%= Html.Encode(Iparent.serial_number)%></td>
				</tr>
				<tr>
					<td>Name:</td>
					<td><%= Html.Encode(Iparent.name)%></td>
				</tr>
				<tr>
					<td>Parameter:</td>
					<td><%= Html.NoteParameters(Iparent.parameters) %></td>
				</tr>
				<tr>
					<td>Make/Model:</td>
					<td><%= (Iparent.model == null) ? "none" : Html.Encode(Iparent.model.display_name)%></td>
				</tr>
				<% if (!Iparent.comment.IsBlank()) { %>
				<tr>
					<td colspan="2">Comment:</td>
				</tr>
				<tr> 
					<td colspan="2" class="noteBody"><%= Html.Encode(Iparent.comment)%></td>
				</tr>
				<% } %>
                <% break;
                   
            case WBEADMS.Models.Note.ParentType.Schedule:
                var Sparent = (WBEADMS.Models.Schedule)Model.parent;%>
				<tr>
					<td>Name:</td>
					<td><%= Html.Encode(Sparent.name)%></td>
				</tr>
				<tr>
					<td>Location:</td>
					<td><%= Html.Encode(Sparent.Location)%></td>
				</tr>
				<tr>
					<td>Contact:</td>
					<td><%= Html.Encode(Sparent.contact)%></td>
				</tr>
				<tr>
					<td>Sample Type:</td>
					<td><%= Html.Encode(Sparent.SampleType)%></td>
				</tr>
				<tr>
					<td>Start Date:</td>
					<td><%= Html.Encode(Sparent.date_start)%></td>
				</tr>
				<tr>
					<td>End Date:</td>
					<td><%= Html.Encode(Sparent.date_end)%></td>
				</tr>
				<tr>
					<td>Interval:</td>
					<td><%= Html.Encode(Sparent.interval.ToSentence(Sparent.frequency_data))%></td>
				</tr>
				<% if (!Sparent.comments.IsBlank()) { %>
				<tr>
					<td colspan="2">Comments:</td>
				</tr>
				<tr> 
					<td colspan="2" class="noteBody"><%= Html.Encode(Sparent.comments)%></td>
				</tr>
				<% } %>
                <% break;
                   
            default: %>
                <% break;
           } %>
			</table>
        </fieldset>
    </div>