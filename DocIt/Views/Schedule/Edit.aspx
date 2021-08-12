<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Schedule>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Edit Schedule <%= Model.name %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-schedules-edit">Edit <%= Model.name %></h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

    <% Html.RenderPartial("ScheduleForm", Model); %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>