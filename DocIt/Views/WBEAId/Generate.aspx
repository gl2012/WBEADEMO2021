<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Generate WBEA Sample IDs
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-wbeaids">Generate WBEA Sample IDs</h2>

    <p>Generate a CSV containing a new set of unused WBEA Sample IDs. 
    The set size contains the number of IDs that will be generated and put into the CSV file.
    Clicking the Download button will perform the WBEA Sample ID generation and download the CSV file.</p>

    <%= Html.BoxedValidationSummary("Generate was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>

        <fieldset>
            <legend>WBEA Sample ID</legend>
			
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="set_size">Set Size:</label></td>
					<td><%= Html.TextBox("set_size") %> <%= Html.ValidationMessage("set_size", "*")%></td>
				</tr>
				<tr>
					<td><input type="submit" value="Download" onclick="if (!performDownloadConfirmation()) { return false; }; setTimeout('location=location', 2500);"/></td>
					<td></td>
				</tr>
			</table>
        </fieldset>
    
    <% } %>
    <script type="text/javascript">
        function performDownloadConfirmation() {
            var setSize = $('#set_size').val();
            if (setSize * 1) { // ignore zero or non-numerical values
                return confirm('Are you sure you want to download ' + setSize + ' WBEA IDs?');
            }
            
            return true;
        }
    </script>
</asp:Content>