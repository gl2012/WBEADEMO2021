<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Location
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-location2">Location</h2>
    
    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Default Location</legend>
        
            <p>
                <% if (ViewData["location"] == null) { %>
                There is no default location currently set on this computer.  Please select one from below to set as default.
                <% } else { %>
                The current location is currently set to <%= ViewData["location"]%>. Newly created notes will default to this location.
                <% } %>
            </p>
            <p>
                <label for="location_id">Location:</label>
                <%= Html.DropDownList("location_id", "")%>
                <%= Html.ValidationMessage("location_id", "*")%>
            </p>
        
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <%} %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>