<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<WBEADMS.Models.Audit>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Audit Log
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Audit Log</h2>
    
    <!-- Search Area -->
	<div class="searchArea">
		<form action="Log" method="get">
			<div class="searchField">
				<label for="type">Type:</label>
				<%= Html.DropDownList("auditType", "")%>
			</div>
			<div class="searchField">
				<label for="user_id">User:</label>
				<%= Html.DropDownList("user_id", "")%>
			</div>
			<div class="searchField">
				<label for="field">Field:</label>
				<%= Html.DropDownList("field", "")%>
			</div>
			<%= Html.Hidden("sort", ViewData["sort"])%>
			<input type="submit" class="btnSearch" value="Search" /> 
		</form>
	</div>
	
    <!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
		</div>
	</div>
	
    <%if (Model.Count > 0) { %>
    <table id="sortable-index">   
        <tr>
            <th class="no-sort">Target</th>
            <th>Type</th>
            <th>Date Modified</th>
            <th class="no-sort">User Modified</th>
            <th>Field</th>
            <th class="no-sort">Original Value</th>
            <th></th>
            <th class="no-sort">New Value</th>
        </tr>
        <% foreach (WBEADMS.Models.Audit auditRecord in Model) {%>
        <tr>
            <td>
                <% 
                    switch (auditRecord.type.ToLower()) {
                        case "location":
                            Location location = Location.Load(auditRecord.id);
                            Response.Write(Html.ActionLink(location.name, "Details", new { controller = "Location", id = location.location_id }));
                            break;
                        default:
                            //assume that its a coc
                            ChainOfCustody coc = ChainOfCustody.Load(auditRecord.id);
                            Response.Write(Html.ActionLink("CoC: " + coc.chain_of_custody_id, "Details", new { controller = "ChainOfCustody", id = coc.chain_of_custody_id }));
                            break;
                    }
                %>
            </td>
            <td>
                <%= auditRecord.type%>
            </td>
            <td>
		        <%= auditRecord.DateModified.ToISODateTime()%>
		    </td>
		    <td>
		        <%= auditRecord.User.ToString()%>
		    </td>
			<td>
				<%= Html.Encode(auditRecord.field.ToTitleCase())%>:
			</td>
			<td>
				<%= Html.Encode(auditRecord.original_value)%>
			</td>
			<td class="rightArrow">
			</td>
			<td >
				<%= Html.Encode(auditRecord.new_value)%>
			</td>
		</tr>
        <% } %>
    </table>
    <% } %>
    <% else { %>
        <p>No audits have been made.</p>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>