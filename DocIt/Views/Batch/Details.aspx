<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Batch>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

            
    <h2>Details for Batch: <%= Model.name %></h2>
    
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
                <td class="label">Name:</td>
                <td><%= Html.Encode(Model.name) %></td>
            </tr>
            <tr>
                <td class="label">Related Sample Results:</td>
                <td>
                    <ul>
                    <% foreach (var result in Model.SampleResults) { %>
                    <li><%= Html.ActionLink("Details for " + result.id, "Details", new {controller = "SampleResult", id = result.id}) %> <%= result.date_results_reported %> <%= result.location_name %> <%= result.sample_type %> <%= result.wbea_id %> <%= result.lab_sample_id %></li>
                    <% } %>
                    </ul>
                </td>
            </tr>
        </table>
    </fieldset>
    
    
    <div class="buttonList">
		<ul>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
    </div>

</asp:Content>