<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Note[]>" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>

<div id="quickNoteComments" class="quickNoteComments">
<% if (Model.Length != 0) { %>
    <br class="clear" />
    <table cellpadding="0" cellspacing="0" width="auto">
    <% foreach (var item in Model) { %>
    <tr>
        <td class="tableActionLinks">
            <span class="details"><%= Html.NoteDetailsLink(item)%></span>
            <!-- <span class="star"><%= Html.NoteStarLink(item) %></span> -->
        </td>
        <td><%= Html.Encode(item.body).Replace(Environment.NewLine, "<br/>")%></td>
        <!--
        <td><%= Html.Encode(item.Location.name) %></td>
        <td><%= item.date_occurred %></td>
        <td><%= item.created_user.display_name %></td>
        -->
    </tr>
    <% } %>
    </table>
<% } %>
</div>