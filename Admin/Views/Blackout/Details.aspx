<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Blackout>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details Blackout <%= Model.id %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-blackouts">Details for Blackout <%= Model.id %></h2>

    <div class="buttonList">
		<ul> 
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
    
    <fieldset>
        <legend>Fields</legend>
        <table cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td class="label">Station:</td>
			<td><%= Html.Encode(Model.station_id) %></td>
		</tr>
		<tr>
			<td class="label">Analyzer:</td>
			<td><%= Html.Encode(Model.analyzer)%></td>
		</tr>
		<tr>
			<td class="label">Start Date:</td>
			<td><%= Html.Encode(Model.date_start.ToDateTimeFormat())%></td>
		</tr>
		<tr>
			<td class="label">End Date:</td>
			<td><%= Html.Encode(Model.date_end.ToDateTimeFormat())%></td>
		</tr>
		<tr>
			<td class="label">Comment:</td>
			<td><%= Html.Encode(Model.comment)%></td>
		</tr>
        </table>
    </fieldset>

    <div class="buttonList">
		<ul>
			<li> <%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

</asp:Content>