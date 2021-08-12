<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Lab>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create New Lab
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-labs-create">Create New Lab</h2>
	
    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    
    <% using (Html.BeginForm()) { %>
    <fieldset>
        <legend>Fields</legend>
        <% Html.RenderPartial("Form"); %>
        <br />
        <input type="submit" value="Create" />
    </fieldset>
    <% } %>

    
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>