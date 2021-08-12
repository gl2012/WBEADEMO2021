<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ChainOfCustody>>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Chain of Custody Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-coc">Chain of Custody</h2>

    <p class="noPrint"> 
        Would you like to create a <strong><%= Html.ActionLink("New Chain of Custody", "Create") %></strong>?
    </p>
    <table cellpadding="0" cellspacing="0" border="0" class="noPrint">
        <tr>
            <td style="padding-left:0">
                <% using (Html.BeginForm("OpenWithId", "ChainOfCustody")) { %>
                    Open CoC: <%= Html.TextBox("id") %>
                    <input class="secondaryButton" type="submit" value="Open" />
                <% } %>
	        </td>
	        <td>&nbsp;</td>
	        <td>
	            <% using (Html.BeginForm("OpenWithWbeaId", "ChainOfCustody")) { %>
                    Open CoC by WBEA Sample ID: <%= Html.TextBox("wbeaId") %>
                    <input class="secondaryButton" type="submit" value="Open" />
                <% } %>
            </td>
        </tr>
    </table>
	<div class="horizBorder"></div>
	
	<!-- Search Area -->
	<div class="searchArea noPrint" style="margin-top:0">
		<form method="get">
			<div class="searchField">
				<label for="sample_type_id">Sample Type:</label>
				<%= Html.DropDownList("sample_type_id", "")%>
			</div>
			<div class="searchField">
				<label for="location_id">Location:</label>
				<%= Html.DropDownList("location_id", "")%>
			</div>
			<div class="searchField">
				<label for="schedule_id">Schedule:</label>
				<%= Html.DropDownList("schedule_id", "")%>
			</div>
			<div class="searchField">
				<label for="status_id">Status:</label>
				<%= Html.DropDownList("status_id", "")%>
			</div>
			<div class="searchField">
				<label for="progress_state">State:</label>
				<%= Html.DropDownList("progress_state")%>
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>
	
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionLink("Create COC", "Create") %>
		</div>
	</div>

    <table id="sortable-index">
        <tr>
            <th class="no-sort">Actions</th>
            <th>
                ID
            </th>
            <th>
                Sample Type
            </th>
            <th>
                Location
            </th>
            <th sort_name="wbea-id">
                WBEA Sample Id
            </th>
            <th sort_name="media-serial">
                Media Serial #
            </th>
            <th>
                Scheduled Date
            </th>
            <th>
                Status
            </th>
        </tr>

    <% foreach (var item in Model) { %>    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.chain_of_custody_id })%></span>
            <% if(!item.Status.IsComplete) { %>
                <span class="tableActionOpen"><%= Html.ActionButton("Open", "Open", new { id=item.chain_of_custody_id }) %></span>
            <% } %>
            </td>
            <td>
                <%= Html.Encode(item.chain_of_custody_id) %>
            </td>
            <td>
                <%= Html.Encode(item.SampleType.name) %>
            </td>
            <td>
                <% if (item.Deployment.Location != null) { %>
                    <%= Html.ActionLink(item.Deployment.Location.name, "Details", new { controller = "Location", id = item.Deployment.Location.location_id }) %>
                <% } %>
            </td>
            <td>
                <% foreach (var sample in item.Samples) { %>
                <%= sample.wbea_id %>
                <% } %>
            </td>
            <td>
                <% foreach (var sample in item.Samples) { %>
                <%= sample.media_serial_number %>
                <% } %>
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.Preparation.ScheduledSamplingDate, "Unknown", WBEADMS.ViewsCommon.FetchDateTimeFormat())%>                
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.Status, "NONE")%>                
            </td> 
        </tr>
    <% } %>

    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>