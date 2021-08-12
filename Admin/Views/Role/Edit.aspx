<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Role>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Edit <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-roles-edit">Edit <%= Model %></h2>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td><label for="name">Name:</label></td>
                <td>
                    <%= Html.TextBox("name", Model.name) %>
                    <%= Html.ValidationMessage("name", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="description">Description:</label></td>
                <td>
                    <%= Html.TextBox("description", Model.description) %>
                    <%= Html.ValidationMessage("description", "*") %>
                </td>
            </tr>
            </table>
            
            <br />
            <input type="submit" value="Save" />
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>