<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ShippingCompany>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Edit Shipping Company
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-labs-edit">Edit Shipping Company</h2>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
    <fieldset>
        <legend>Fields</legend>
        <% Html.RenderPartial("Form"); %>
        <br />
        <input type="submit" value="Save" />
    </fieldset>
    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>