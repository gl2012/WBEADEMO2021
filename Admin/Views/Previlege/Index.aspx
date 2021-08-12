<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.User>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Previlege of User <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="role_id">Role:</label>
				<%//= Html.DropDownList("role_id", "") %>
				<label for="is_active">Filter by Active:</label>
				<%//= Html.DropDownList("is_active", "") %> -->
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
        <table id="sortable-index" cellpadding="0" cellspacing="0" border="0" width="100%" class="usersTable">
        <tr>
            <th class="no-sort">
                Actions
            </th>
            <th>
                Name
            </th>
            <th class="no-sort">
                Previlege_Edit
            </th>
            <th class="no-sort">
                Previlege_View
            </th>
            <th class="no-sort">
                Previlege_Create
            </th>
            <th class="no-sort">
                DelegateTo
            </th>
           
        </tr>
            </table>
	</div>
</asp:Content>
