<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Role>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Edit Policy for <%= Model.name %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-policies-edit">Modify Policy for <%= Model.name %> </h2>
    <h3><%= Model.description %></h3>
    
    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Policies</legend>
		    <table cellpadding="0" cellspacing="0" border="0">
			    <tr>
			        <td><label for="admin_panel_access">Admin Panel Access:</label></td>
                    <td>
                        <%= Html.CheckBox("admin_panel_access", Model.Policy.AdminPanelAccess) %>
                        <%= Html.ValidationMessage("admin_panel_access", "*") %>
                    </td>
                </tr>
			    <tr>
			        <td><label for="docit_access">Doc-IT Access:</label></td>
			        <td>
                        <%= Html.CheckBox("docit_access", Model.Policy.DocItAccess)%>
                        <%= Html.ValidationMessage("docit_access", "*") %>
                    </td>
                </tr>
            </table>
                
            <br /><input type="submit" value="Save" />
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index", new { controller = "Role" })%>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>