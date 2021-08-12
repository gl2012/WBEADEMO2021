<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.LocationImage>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Upload Image
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-locations-images-upload">Upload Image for <%= ViewData["location_name"] %></h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "ListByLocation", new { id = ViewData["location_id"] })%>
    </p>
	
    <%= Html.BoxedValidationSummary("Upload was unsuccessful. Please correct the errors and try again.")%>
	
    <form method="post" action="<%=Url.Action("Upload", new { id = ViewData["location_id"].ToString() })%>" enctype="multipart/form-data"> 
        <fieldset>
            <legend>Image Upload</legend>
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="file">Image:</label></td>
					<td>
                        <input type="file" name="file" /> 
                        <%= Html.ValidationMessage("file", "*") %>
				    </td>
				</tr>
				<tr>
					<td><label for="comments">Comments:</label></td>
					<td>
                        <%= Html.TextArea("comments", new { rows = 3, cols = 40 })%>
                        <%= Html.ValidationMessage("comments", "*") %>
					</td>
				</tr>
			</table>
            <br /><input type="submit" value="Save" />
        </fieldset>
    </form> 

    <div>
        <%=Html.ActionLink("Back to List", "ListByLocation", new { id = ViewData["location_id"] })%>
    </div>

</asp:Content>