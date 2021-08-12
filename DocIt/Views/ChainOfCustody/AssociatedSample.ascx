<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Sample>" %>

       <table cellpadding="0" cellspacing="0" border="0" class="cocTable sampleTable<%= (ViewData["Preview"] == null || (bool)ViewData["Preview"] == false) ? "" : " Preview" %>">
            <tr>
                <td>
                    <b>WBEA Sample ID: </b>
                </td>
                <td>
                    <%= Html.ActionLink(Model.wbea_id, "Details", new { controller = "Sample", id = Model.id }) %>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Prepared By: </b>
                </td>
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(Model.PreparedBy, "Unknown")%>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Received From Lab: </b>
                </td>
                <td>
                    <%= Model.date_received_from_lab%>
                </td>
            </tr>
            <tr>
				<td>
				    <b>Avg. Storage Temperature:</b>
				</td>
				<td>
				    <%= Model.average_storage_temperature%> <%= Model.AverageStorageTemperatureUnit%>
				</td>
			</tr>
            <tr>
                <td>
                    <b>Media Serial Number: </b>
                </td>
                <td>
                    <%= Model.media_serial_number%>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Lab Sample ID: </b>
                </td>
                <td>
                    <%= Model.lab_sample_id%>
                </td>
            </tr>

            <% if (ViewData["CoC"] != null) { %>
            <tr class="noPrint">
                <td colspan="2">
                    <%  
                        var coc = (ChainOfCustody)ViewData["CoC"];
                        string cocStatusName = coc.Status.name;
                        string cocId = coc.chain_of_custody_id;

                        if (cocStatusName == "Opened" || cocStatusName == "Preparing" || cocStatusName == "Prepared" || cocStatusName == "Deploying" || cocStatusName == "Deployed" || coc.Status.name == "Retrieving" || coc.Status.name == "Retrieved" || coc.Status.name == "Shipping") {
                            if (ViewData["Preview"] == null || (bool)ViewData["Preview"] == false) {
                    %>
                        <%= Html.Hidden("sample_id", Model.sample_id)%>
                        <input class="secondaryButton" type="submit" value="Remove" />
                    <% 
                            } 
                        }
                   %>
                </td>
            </tr>
            <% } %>
        </table>