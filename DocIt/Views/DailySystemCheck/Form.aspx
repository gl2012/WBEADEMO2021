<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="WBEADMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Peform A Daily Systems Check
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        Location thislocation = (Location)ViewData["location"];
        var notes = (Dictionary<string, Note>)ViewData["notes"];
        string dsc_tag = null;
        if (notes != null) {
            var key = notes.Keys.First();
            dsc_tag = notes[key].dsc_tag;
        }
    %>

    <h2 class="title-icon icon-dsc">Daily Systems Check for <%= thislocation %></h2>
    
    <% if (dsc_tag != null) { %>
        <h3><%= dsc_tag %></h3>
    <% } %>

	<%= Html.BoxedValidationSummary("Create was unsuccessful. You must fill out text for General or fill out the text for each parameter.") %>

	<div id="dscCheckByStation">
	<% using (Html.BeginForm()) {%>
		<table cellpadding="0" cellspacing="0" border="0" id="performDSCtable" class="generalTable">
		<thead>
			<tr>
				<th>Parameter</th>
				<th>Starred</th>
				<th>OK</th>
				<th>Text Body</th>
			</tr>
		</thead>
		<tbody>
			<% Html.RenderPartial("FormCell"); %>       
	<% foreach (var parameter in (WBEADMS.Models.Parameter[])ViewData["parameters"]) { %>
			<% Html.RenderPartial("FormCell", parameter); %>
	<% } %>
		</tbody>
		</table>
		<p>
			<input type="submit" name="submit" value="Save and Next" title="Save and move to next station in the list." />
			<input type="submit" name="submit" value="Save" title="Save and keep the DSC notes open for this station."/> 
		</p>
	<% } %>
	</div>

	<div id="SelectDSCLocation">
		<table id="tableSelectDSCLocation" cellpadding="0" cellspacing="0" border="0" class="generalTable">
		<thead>
			<tr><th>Location</th><th>Last Performed DSC</th></tr>
		</thead>
		<tbody>
	<% var lastDSCs = (Dictionary<string, DateTime>)ViewData["lastDSCs"]; %>
	<% foreach (var location in (List<WBEADMS.Models.Location>)ViewData["locations"]) { %>
			<tr>
				<td><%= Html.ActionLink(location.name, "Create", new { location_id = location.id }) %></td>
				<td><% if (lastDSCs.ContainsKey(location.id)) { %><%= Html.ActionLink(lastDSCs[location.id].ToISODateTime(), "Edit", new { location_id = location.id })%><% } else { Response.Write("N/A"); } %></td>
			</tr>
	<% } %>
		</tbody>
		</table>
	</div>

	<div class="clear-fix"></div>
	
	<div>
	    <%= Html.ActionLink("Notes for " + thislocation.name, "Index", new { controller = "Note", location_id = thislocation.id }, new { target = "_blank", title = "Open Notes for " + thislocation.name + " in a new window." }) %>
	</div>

<!-- <p><%= Html.ActionLink("Select a different Location", "SelectDSCLocation")%></p> -->

<script type="text/javascript" language="javascript">
    $(document).ready(function() {
        $('input.ok').click(function() {
            var relatedTextArea = $(this).parent('td').parent('tr').find('textarea');
            if (this.checked) {
                if (!relatedTextArea.val().match(/^OK\.?\s*/i)) {
                    relatedTextArea.val('Status Good. ' + relatedTextArea.val());
                }
            } else {
                relatedTextArea.val(relatedTextArea.val().replace(/^Status Good\.?\s*/i, ""));
            }
        });
    });
    function endsWithOK(body) {
        var re = /\s+OK\s*/i;
        var found = body.match(re);
        return;
    }
</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>