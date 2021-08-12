<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WMD.Model.ChainOfCustody>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Chain Of Custody
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $("#date_sampling_scheduled").datepicker();
        });
	</script>

    <h2 class="title-icon icon-coc-create">Create Chain Of Custody</h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>COC Fields</legend>
			<table cellpadding="0" cellspacing="0" border="0" class="cocTable">
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
					<td>
						<%= Html.DropDownList("sample_type_id") %>
						<%= Html.ValidationMessage("sample_type_id", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="date_sampling_scheduled">Date Sampling Scheduled:</label></td>
					<td>
						<%= Html.TextBox("date_sampling_scheduled") %>
						<%= Html.ValidationMessage("date_sampling_scheduled", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="voc_cannister_pressure">VOC Cannister Pressure:</label></td>
					<td>
						<%= Html.TextBox("voc_cannister_pressure")%>
						<%= Html.ValidationMessage("voc_cannister_pressure", "*")%>
					</td>
				</tr>
			</table>

            <br /><input type="submit" value="Create" />
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>