<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Details for <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-users">Details for <%= Model %></h2>

    <div class="buttonList">
		<ul>
			<li> <%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>

    <fieldset>
        <legend>Fields</legend>
		<table cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td class="label">User Name:</td>
			<td><%= Html.Encode(Model.user_name) %></td>
		</tr>
		<tr>
			<td class="label">First Name:</td>
			<td><%= Html.Encode(Model.first_name) %></td>
		</tr>
		<tr>
			<td class="label">Last Name:</td>
			<td><%= Html.Encode(Model.last_name) %></td>
		</tr>
		<tr>
			<td class="label">Email:</td>
			<td><%= Html.Encode(Model.email) %></td>
		</tr>
		<tr>
			<td class="label">Phone Number:</td>
			<td><%= Html.Encode(Model.phone_number) %></td>
		</tr>
		<tr>
			<td class="label">Role:</td>
			<td><%= Html.ActionLink(Model.Role.name, "Details", new { controller = "Role", id = Model.role_id})%></td>
		</tr>
		<tr>
			<td class="label">Comments:</td>
			<td><%= Html.Encode(Model.comments) %></td>
		</tr>
		<tr>
			<td class="label">Technician Code:</td>
			<td><%= Html.Encode(Model.technician_code) %></td>
		</tr>
		<tr>
			<td class="label">Active:</td>
			<td><%= Html.Encode(Model.active.ToHumanBool()) %></td>
		</tr>
		<tr>
			<td class="label">Date Created:</td>
			<td><%= Html.Encode(Model.date_created) %></td>
		</tr>
		<tr>
			<td class="tableActionLinks" colspan="2">
			<span class="tableActionPasswordReset"><a class="resetPwdLink" userid="<%= Model.id %>" href="javascript:void(0);" title="Reset Password"><span>Reset&nbsp;Password</span></a></span>
			</td>
			
		</tr>
		</table>
    </fieldset>

    <div class="buttonList">
		<ul>
			<li> <%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>


    <!-- ui-dialog -->
    <div id="dialog" class="modalPopup" title="Password Reset?">
        <p>Confirm password reset.</p>
        <form id="resetPasswordForm" name="resetPasswordForm" method="post"></form>
    </div>
    
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
                        $("#resetPasswordForm").attr('action', '<%= Url.Action("PasswordReset") %>/' + $(this).dialog('option', 'userid'));
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

</asp:Content>