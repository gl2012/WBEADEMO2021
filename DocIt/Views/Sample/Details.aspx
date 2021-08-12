<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Sample>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sample Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-samples">Sample Details</h2>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

    <fieldset>
        <legend>Sample Information</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td class="label">Sample Type:</td>
				<td><%= Html.Encode(Model.SampleType.name) %></td>
			</tr>
			<tr>
				<td class="label">WBEA Sample ID:</td>
				<td><%= Html.Encode(Model.wbea_id) %></td>
			</tr>
			<tr>
				<td class="label">Media Serial Number:</td>
				<td><%= Html.Encode(Model.media_serial_number) %></td>
			</tr>
			<tr>
				<td class="label">Lab Sample ID:</td>
				<td><%= Html.Encode(Model.lab_sample_id) %></td>
			</tr>
			<tr>
				<td class="label">Date Received From Lab:</td>
				<td><%= Model.DateReceivedFromLab%></td>
			</tr>
			<tr>
				<td class="label">Average Storage Temperature:</td>
				<td><%= Model.average_storage_temperature %> <%= Model.AverageStorageTemperatureUnit %></td>
			</tr>
			<tr>
				<td class="label">Prepared By:</td>
				<td><%= Model.PreparedBy %></td>
			</tr>
            <tr>
				<td class="label">Travel Blank:</td>
				<td><%= Model.is_travel_blank.ToHumanBool() %></td>
			</tr>
             <tr>
				<td class="label">Orphabed Sample:</td>
				<td><%= Model.is_orphaned_sample.ToHumanBool() %></td>
			</tr>
			<tr>
				<td class="label">CoC: </td>
				<td>
                <%  int count = 0;
                    foreach (string cocID in Model.ChainOfCustodyIDs) {
                        ChainOfCustody coc = ChainOfCustody.Load(cocID);
                        string linkText = "CoC: " + coc;
                        if(count > 0){
                            Response.Write("<Br />");
                        }
                        
                        count++;
                        %>
                        <%= Html.ActionLink(linkText, "Details", new { controller = "ChainOfCustody", id = coc.chain_of_custody_id })%>
                <% } %>
			</tr>
			<% if (Model.SampleResult != null) { %>
			<tr>
				<td class="label">Sample Result:</td>
				<td>
					<%= Html.ActionLink(Model.SampleResult.ToString(), "Details", new { controller = "SampleResult", id = Model.SampleResult.id})%>
				</td>
			</tr>
			<% } %>
		</table>
    </fieldset>
	
    <div class="buttonList">
		<ul>
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
    </div>

</asp:Content>