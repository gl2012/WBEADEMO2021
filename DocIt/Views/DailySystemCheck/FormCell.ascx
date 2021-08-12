<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Parameter>" %>
<% 
    string name = (Model == null) ? "General" : Model.name;
    string id = (Model == null) ? "0" : Model.id;

    string note_id = "0";
    bool starred = false;
    string body = null;
    var notes = (Dictionary<string, Note>)ViewData["notes"];
    if (notes != null && notes.ContainsKey(id)) {
        var note = notes[id];
        note_id = note.id;
        starred = note.starred;
        body = note.body;
    }
        
%>
    <tr>
        <td><%= name %></td>
        <td><%= Html.CheckBox("star_" + id, starred) %></td>
        <td><%= Html.CheckBox("ok_" + id, new { @class = "ok" })%></td>
        <td>
            <span class="textArea"><%= Html.TextArea("body_" + id, body)%></span>
            <%= Html.Hidden("id_" + id, note_id) %>
        </td>
    </tr>