<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Batch>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index of Batches
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index of Batches</h2>
    
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
            <th>Batch Id</th>
            <th>Name</th>            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.batch_id })%></span>
            </td>
            <td>
                <%= Html.Encode(item.batch_id) %>
            </td>
            <td>
                <%= Html.Encode(item.name) %>
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