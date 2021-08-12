<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.User>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Index of Users
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(function() {
            // Dialog
            $('#dialog').dialog({
                autoOpen: false,
                width: 400,
                height: 180,
                modal: true,
                resizable: false,
                draggable: false,
                buttons: {
                    "No": function() {
                        $(this).dialog("close");
                    },
                    "Yes": function() {
                        $(this).dialog("close");
                        $("#resetPasswordForm").attr('action', '<%= Url.Action("IndexPasswordReset") %>/' + $(this).dialog('option', 'userid'));
                        $("#resetPasswordForm").submit();
                    }
                }
            })

            // Dialog Link
            $('.resetPwdLink').click(function() {
                $('#dialog').dialog('option', { userid: $(this).attr('userid') });
                $('#dialog').dialog('open');
            });
        });
    </script>

    <h2 class="title-icon icon-users">Users</h2>
    
    <p class="noPrint">Would you like to <strong><%= Html.ActionLink("Create A New User", "Create") %></strong>?</p>
    
    	<!-- Search Area -->
	<div class="searchArea">
		<form method="get">
			<div class="searchField">
				<label for="role_id">Role:</label>
				<%= Html.DropDownList("role_id", "") %>
				<label for="is_active">Filter by Active:</label>
				<%= Html.DropDownList("is_active", "") %>
			</div>
			<input type="submit" class="btnSearch" value="Search" /> 
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>

	
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		<div class="buttonList">
			<%= Html.ActionButton("Create New", "Create") %>
		</div>
	</div>
    
    <table id="sortable-index" cellpadding="0" cellspacing="0" border="0" width="100%" class="usersTable">
        <tr>
            <th class="no-sort">
                Actions
            </th>
            <th>
                Role
            </th>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
            <th>
                Email
            </th>
            <th class="no-sort">
                Phone Number
            </th>
            <th>
                Active
            </th>
        </tr>
        <% foreach (var item in Model) { %>
        <tr>
            <td class="tableActionLinks" style="width: 350px;">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id = item.user_id })%></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id = item.user_id })%></span>
                 <span class="tableActionEdit"><%= Html.ActionLink("Previlege", "Index","Previlege", new { id = item.user_id },null)%> </span>
                <span class="tableActionPasswordReset">
                    <a class="resetPwdLink" userid="<%= item.user_id %>" href="javascript:void(0);" title="Reset Password">
                        <span>Reset&nbsp;Password</span>
                    </a>
               
            </td>
            <td>
                <%= Html.Encode(item.Role)%>
            </td>
            <td>
                <%= Html.Encode(item.first_name)%>
            </td>
            <td>
                <%= Html.Encode(item.last_name)%>
            </td>
            <td>
                <%= Html.Encode(item.email)%>
            </td>
            <td>
                <%= Html.Encode(item.phone_number)%>
            </td>
            <td>
                <%= Html.Encode(item.active.ToHumanBool())%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <!-- ui-dialog -->
    <div id="dialog" class="modalPopup" title="Password Reset?">
        <p>Confirm password reset.</p>
        <form id="resetPasswordForm" name="resetPasswordForm" method="post">
        </form>
    </div>
    <!-- End Content -->
</asp:Content>