<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.SampleResult>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index of Sample Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index of Sample Results</h2>

    <!-- Search Area -->
	<div class="searchArea">
	</div>
	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
		</div>
	</div>
    
    <table id="sortable-index">
        <tr>
            <th class="no-sort">Actions</th>
            <th>
                lab_name
            </th>
            <th>
                client_name
            </th>
            <th>
                wbea_id
            </th>
            <th>
                lab_sample_id
            </th>
            <th>
                sample_media_serial_number
            </th>
            <th>
                date_sample_received
            </th>
            <th>
                sample_damaged
            </th>
            <th>
                location_name
            </th>
            <th>
                location_id
            </th>
            <th>
                date_sample_start
            </th>
            <th>
                date_sample_end
            </th>
            <th>
                lab_technician_initials
            </th>
            <th>
                voc_received_cannister_pressure
            </th>
            <th>
                sample_type
            </th>
            <th>
                file_name
            </th>
            <th>
                batch_id
            </th>
            <th>
                analysis_id
            </th>
            <th>
                date_results_reported
            </th>
            <th>
                comments
            </th>
            <th>
                wind_speed
            </th>
            <th>
                relative_humidity
            </th>
            <th>
                temperature
            </th>
            <th>
                id
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>                
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.sample_result_id })%></span>
            </td>
            <td>
                <%= Html.Encode(item.lab_name) %>
            </td>
            <td>
                <%= Html.Encode(item.client_name) %>
            </td>
            <td>
                <%= Html.Encode(item.wbea_id) %>
            </td>
            <td>
                <%= Html.Encode(item.lab_sample_id) %>
            </td>
            <td>
                <%= Html.Encode(item.sample_media_serial_number) %>
            </td>
            <td>
                <%= Html.Encode(item.date_sample_received) %>
            </td>
            <td>
                <%= Html.Encode(item.sample_damaged) %>
            </td>
            <td>
                <%= Html.Encode(item.location_name) %>
            </td>
            <td>
                <%= Html.Encode(item.location_id) %>
            </td>
            <td>
                <%= Html.Encode(item.date_sample_start) %>
            </td>
            <td>
                <%= Html.Encode(item.date_sample_end) %>
            </td>
            <td>
                <%= Html.Encode(item.lab_technician_initials) %>
            </td>
            <td>
                <%= Html.Encode(item.voc_received_cannister_pressure) %>
            </td>
            <td>
                <%= Html.Encode(item.sample_type) %>
            </td>
            <td>
                <%= Html.Encode(item.file_name) %>
            </td>
            <td>
                <%= Html.Encode(item.batch_id) %>
            </td>
            <td>
                <%= Html.Encode(item.analysis_id) %>
            </td>
            <td>
                <%= Html.Encode(item.date_results_reported) %>
            </td>
            <td>
                <%= Html.Encode(item.comments) %>
            </td>
            <td>
                <%= Html.Encode(item.wind_speed) %>
            </td>
            <td>
                <%= Html.Encode(item.relative_humidity) %>
            </td>
            <td>
                <%= Html.Encode(item.temperature) %>
            </td>
            <td>
                <%= Html.Encode(item.id) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <br class="clear" />
    <div class="buttonList">
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>