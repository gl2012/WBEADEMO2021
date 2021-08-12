<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.SampleResult>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details for Sample Result: <%= Model.wbea_id %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Details for Sample Result: <%= Model.wbea_id %></h2>
    
    <div class="buttonList">
		<ul>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
        <div class="clear-fix">
        </div>
    </div>
    
    <fieldset>
        <legend>Sample Result Information</legend>
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="label">sample_result_id:</td>
                <td><%= Html.Encode(Model.sample_result_id) %></td>
            </tr>
            <tr>
                <td class="label">lab_name:</td>
                <td><%= Html.Encode(Model.lab_name) %></td>
            </tr>
            <tr>
                <td class="label">client_name:</td>
                <td><%= Html.Encode(Model.client_name) %></td>
            </tr>
            <tr>
                <td class="label">wbea_id:</td>
                <td><%= Html.Encode(Model.wbea_id) %></td>
            </tr>
            <tr>
                <td class="label">lab_sample_id:</td>
                <td><%= Html.Encode(Model.lab_sample_id) %></td>
            </tr>
            <tr>
                <td class="label">sample_media_serial_number:</td>
                <td><%= Html.Encode(Model.sample_media_serial_number) %></td>
            </tr>
            <tr>
                <td class="label">date_sample_received:</td>
                <td><%= Html.Encode(Model.date_sample_received) %></td>
            </tr>
            <tr>
                <td class="label">sample_damaged:</td>
                <td><%= Html.Encode(Model.sample_damaged) %></td>
            </tr>
            <tr>
                <td class="label">location_name:</td>
                <td><%= Html.Encode(Model.location_name) %></td>
            </tr>
            <tr>
                <td class="label">location_id:</td>
                <td><%= Html.Encode(Model.location_id) %></td>
            </tr>
            <tr>
                <td class="label">date_sample_start:</td>
                <td><%= Html.Encode(Model.date_sample_start) %></td>
            </tr>
            <tr>
                <td class="label">date_sample_end:</td>
                <td><%= Html.Encode(Model.date_sample_end) %></td>
            </tr>
            <tr>
                <td class="label">lab_technician_initials:</td>
                <td><%= Html.Encode(Model.lab_technician_initials) %></td>
            </tr>
            <tr>
                <td class="label">voc_received_cannister_pressure:</td>
                <td><%= Html.Encode(Model.voc_received_cannister_pressure) %></td>
            </tr>
            <tr>
                <td class="label">sample_type:</td>
                <td><%= Html.Encode(Model.sample_type) %></td>
            </tr>
            <tr>
                <td class="label">file_name:</td>
                <td><%= Html.Encode(Model.file_name) %></td>
            </tr>
            <tr>
                <td class="label">batch_id:</td>
                <td><%= Html.Encode(Model.batch_id) %></td>
            </tr>
            <tr>
                <td class="label">analysis_id:</td>
                <td><%= Html.Encode(Model.analysis_id) %></td>
            </tr>
            <tr>
                <td class="label">date_results_reported:</td>
                <td><%= Html.Encode(Model.date_results_reported) %></td>
            </tr>
            <tr>
                <td class="label">comments:</td>
                <td><%= Html.Encode(Model.comments) %></td>
            </tr>
            <tr>
                <td class="label">wind_speed:</td>
                <td><%= Html.Encode(Model.wind_speed) %></td>
            </tr>
            <tr>
                <td class="label">relative_humidity:</td>
                <td><%= Html.Encode(Model.relative_humidity) %></td>
            </tr>
            <tr>
                <td class="label">temperature:</td>
                <td><%= Html.Encode(Model.temperature) %></td>
            </tr>
            <tr>
                <td class="label">id:</td>
                <td><%= Html.Encode(Model.id) %></td>
            </tr>
        </table>
    </fieldset>
    
    <% Html.RenderPartial("CompoundList", Model.SampleCompounds); %>
    
    <div class="buttonList">
		<ul>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>