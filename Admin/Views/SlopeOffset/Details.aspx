<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.SlopeOffset>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details Slope/Offset <%= Model.id %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-slopeoffsets">Details for Slope/Offset  <%= Model.id %></h2>

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
			<td class="label">Active Date:</td>
			<td><%= Html.Encode(Model.date_active.ToDateTimeFormat())%></td>
		</tr>
		<tr>
			<td class="label">Slope:</td>
			<td><%= Html.Encode(Model.slope)%></td>
		</tr>
		<tr>
			<td class="label">Offset:</td>
			<td><%= Html.Encode(Model.offset)%></td>
		</tr>
		<tr>
			<td class="label"> Date Created:</td>
			<td><%= Html.Encode(Model.date_created.ToDateTimeFormat())%></td>
		</tr>
		<tr>
			<td class="label">Created By:</td>
			<td><%= Html.Encode(Model.CreatedBy)%></td>
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