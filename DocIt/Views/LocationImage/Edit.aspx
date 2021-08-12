<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.LocationImage>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Edit Image
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-locations-images-edit">Edit Image</h2>
	
    <p>        
        <%=Html.ActionLink("Back to List", "ListByLocation", new { id = Model.location_id })%>
    </p>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Edit", "LocationImage", FormMethod.Post, new { id = Model.location_image_id, enctype="multipart/form-data"})) {%>

        <fieldset>
            <legend>Fields</legend>
			<img class="imgR" src="<%= Url.Action("GetImage", new { controller = "LocationImage", id = Model.location_image_id}) %>" alt="Image <%= Model.id %>for <%= Model.Location.name %>" />
            
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td class="label">Upload New:</td>
					<td>
						<input type="file" name="file" />
						<%= Html.ValidationMessage("file", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="comments">Comments:</label></td>
					<td>
						<%= Html.TextArea("comments", new { rows = 5 })%>
						<%= Html.ValidationMessage("comments", "*") %>					
					</td>
				</tr>
			</table>
			<br /><input type="submit" value="Save" />
        </fieldset>

    <% } %>

    <div>        
        <%=Html.ActionLink("Back to List", "ListByLocation", new { id = Model.location_id })%>
    </div>

</asp:Content>