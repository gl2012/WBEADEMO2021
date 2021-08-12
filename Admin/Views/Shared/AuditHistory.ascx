<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dictionary<DateTime, List<WBEADMS.Models.Audit>>>" %>
<fieldset>
    <legend>Audit History</legend>
    <%if (Model.Count > 0) { %>
        <% foreach (DateTime key in Model.Keys) {%>
            <table cellpadding="0" cellspacing="0" border="0" class="generalTable auditTable">
				<thead>
					<tr>
						<th colspan="2">
							<%= Model[key][0].User.user_name%>
						</th>
						<th style="text-align:right;">
							<span style="font-weight:normal !important"><%= key%></span>
						</th>
					</tr>
				</thead>
                <tbody>
            <% foreach (WBEADMS.Models.Audit auditRecord in Model[key]) {%>
					<tr>
						<td width="30%" class="tBold">
							<%= Html.Encode(auditRecord.field.ToTitleCase())%>:
						</td>
						<td width="25%">
							<%= Html.Encode(auditRecord.original_value)%>
						</td>
						<td class="rightArrow" width="35%">
							<%= Html.Encode(auditRecord.new_value)%>
						</td>
					</tr>
            <% } %>
				</tbody> 
            </table>
        <% } %>
    <% } %>
    <% else { %>
        <p>No audits have been made.</p>
    <% } %>
</fieldset>