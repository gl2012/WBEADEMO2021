<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Generate Sample Media IDs
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-wbeaids">Generate Sample Media IDs</h2>

    <p>Generate a CSV containing a new set of unused Sample Media IDs. 
    The set size contains the number of IDs that will be generated and put into the CSV file.
    Clicking the Download button will perform the Sample Media ID generation and download the CSV file.</p>

    <%= Html.BoxedValidationSummary("Generate was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>

        <fieldset>
            <legend>Sample Media ID</legend>
		 	
			<style type="text/css">.subscript{font-size:xx-small; vertical-align:bottom;}</style>
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
			        <td><label for="parameter">Year:</label></td>
			        <td>
			            <% ViewData["year"] = ViewData["year"] ?? DateTime.Now.Year.ToString(); %>
			            <%= Html.DropDownList("year", new SelectList(new List<String> { DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.AddYears(1).Year.ToString() }, ViewData["year"]))%>
			        </td>
			    </tr>
			    <tr>
			        <td><label for="parameter">Month:</label></td>
			        <td>
			        <%
                        Dictionary<string, string> months = new Dictionary<string, string>();
                        for (int i = 0; i < 12; i++) {
                            months.Add(DateTime.Now.AddMonths(i).ToString("MMM"), DateTime.Now.AddMonths(i).Month.ToString());
                        }                                
                    %>
			        <%= Html.DropDownList("month", new SelectList(months, "value", "key"), ViewData["month"])%>
			        </td>
			    </tr>
			    <tr>
			        <td><label for="parameter">Parameter:</label></td>
			        <td>
			        <%= Html.DropDownList("parameter", new SelectList(SampleMediaIdGenerator.ParameterList, ViewData["parameter"]))%>
			        </td>
			    </tr>
				<tr>
					<td><label for="set_size">Set Size:</label></td>
					<td><%= Html.TextBox("set_size") %> <%= Html.ValidationMessage("set_size", "*")%></td>
				</tr>
				<tr>
					<td><input type="submit" value="Download" onclick="setTimeout('location=location', 2500);"/></td>
					<td></td>
				</tr>
			</table>
        </fieldset>
    
    <% } %>
</asp:Content>