<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Role>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Details for <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    
    <h2 class="title-icon icon-roles">Details for <%= Model %></h2>
    
    <div class="buttonList">
		<ul> 
			<li><%=Html.ActionLink("Edit", "Edit", new { id=Model.id }) %></li>
			<li class="noButton"><%=Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
    
    <div class="colSetup">
	
		<div class="leftCol">
    
            <fieldset>
                <legend>Fields</legend>
				
			    <table cellpadding="0" cellspacing="0" width="100%">
				    <tr>
					    <td class="label">Name:</td>
                        <td><%= Html.Encode(Model.name) %></td>
                    </tr>
                    <tr>
                        <td class="label">Description:</td>
                        <td><%= Html.Encode(Model.description) %></td>
                    </tr>
                    <tr>
                        <td class="label">Users:</td>
                        <td>
                            <ul class="noBullets">
                                <% foreach (User user in WBEADMS.Models.User.FetchByRole(Model.role_id)) {%>
                                <li>
                                    <%= Html.ActionLink(user.display_name, "Details", new { controller = "User", id = user.user_id })%>
                                </li>
                                <% } %>
                            </ul>                    
                        </td>
                    </tr>
                </table>

        </fieldset>
    
    </div>
</asp:Content>