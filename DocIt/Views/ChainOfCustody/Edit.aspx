<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ChainOfCustody>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit <%= Model.SampleType %> Chain of Custody: <%= Model.chain_of_custody_id %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/wmd-helper.js") %>"></script>
	<script type="text/javascript">
		$(document).ready(function() {
			//Add Class to page tag for COC targeting
			$('.page').addClass('cocPage');

			//Add Show/Hide Buttons to Legends
			$('legend').prepend("<a href='javascript:void(0);' class='btnShow' title='Expand'><span>+</span></a><a href='javascript:void(0);' class='btnHide' title='Collapse'><span>-</span></a>&nbsp;");
			
			//Collapsing Sections
			$('.btnShow').hide();
			$('.btnHide').click(function() {
				$(this).parents('fieldset').eq(0).find('.fieldsetInfo').hide();
				$(this).hide();
				$(this).parents('fieldset').eq(0).find('legend').find('.btnShow').show();
				$(this).parents('fieldset').eq(0).addClass("isCollapsed");
			});
			$('.btnShow').click(function() {
				$(this).parents('fieldset').eq(0).find(".fieldsetInfo").show();
				$(this).hide();
				$(this).parents('fieldset').eq(0).find('legend').find('.btnHide').show();
				$(this).parents('fieldset').eq(0).removeClass("isCollapsed");
			});
			$('#collapseAll').click(function() {
				$('.btnHide').click();				
			});
			$('#expandAll').click(function() {
				$('.btnShow').click();				
			});
			
			//Onload collapse all but the "editForm"
			$('#collapseAll').click();
			$('.editForm .btnShow').click();			
			
			//COC Overview Status
			var theStatus = "<%= Model.Status.name %>";
			$(".cocOverview li.last").addClass(theStatus.toLowerCase());

			//Add N/A background
			if ($('.cocTable tr th:contains(N/A)').length) {
			    $('.formTable > tbody > tr > th:nth-child(2)').addClass('cocNAcell');
			    $('.formTable > tbody > tr > td:nth-child(2)').addClass('cocNAcell');
			    $('.cocNAcell').after('<td></td>');
			}
		});
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

	<ul class="cocOverview">
		<li class="first edit">Chain of Custody Edit:</li>
		<li><span>Type:</span><br /><%= Model.SampleType %></li>
		<li><span>ID:</span><br /><%= Model.chain_of_custody_id %></li>
		<li class="last"><span>Status:</span><br /><span id="cocOverviewStatus"><%= Model.Status.name %></span></li>
	</ul>
    
    <div class="buttonList clear">
		<ul> 
		    <li><%= Html.ActionLink("View Details", "Details", new { id = Model.chain_of_custody_id })%></li>
            <li><%= Html.ActionButton("Open", "open", new { id = Model.chain_of_custody_id })%></li>
			<li class="noButton"><a href="javascript:void(0);" id="collapseAll">Collapse All</a></li>
			<li class="noButton"><a href="javascript:void(0);" id="expandAll">Expand All</a></li>
			<li class="noButton"><%= Html.ActionLink("Back to List", "Index") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
    
    <fieldset>
        <legend>Associated Sample(s)</legend>
		<div class="fieldsetInfo">
        <% 
           if (Model.Samples.Count > 0) {
               foreach (Sample sample in Model.Samples) {
                   Html.RenderPartial("AssociatedSample", sample);
               }
           }
           else {
               Response.Write("There are no associated samples");
           }
        %>
		</div>
    </fieldset>
    
    <% if (Model.TravelBlanks.Count > 0) { %>
    <fieldset>
        <legend>Travel Blank(s)</legend>
			<div class="fieldsetInfo">
				<% foreach (Sample travelBlank in Model.TravelBlanks) { 
                   Html.RenderPartial("AssociatedSample", travelBlank);
				} %>    
			</div>
    </fieldset>
    <% } %>
    <% 
        string editSection = (string)Page.Request.Params["editSection"] ?? "none";

        /* special placement of the commit button (since Deployment and Retrieval is now side by side) TODO: we need to revisit this*/
        foreach (string component in Model.GetDetailComponents()) {
            if (component.StartsWith(editSection)) {
                using (Html.BeginForm("Edit", "ChainOfCustody", new { id = Model.chain_of_custody_id, editSection = editSection }, FormMethod.Post, new { @class = "editForm" })) {
                    Html.RenderPartial(editSection + "Form", Model);
                    if (editSection == "Retrieval")
                        Response.Write("<p style=\"clear:both;float:right\"> <input type=\"submit\" value=\"Commit\" /></p>");
                    else
                        if ((editSection != "Deployment") ||
                            (editSection == "Deployment" && (Model.Status.name.ToLower() == "deployed" || Model.Status.name.ToLower() == "retrieving")))
                            Response.Write("<p style=\"clear:both\"> <input type=\"submit\" value=\"Commit\" /></p>");
                }
            }
            else {
                Html.RenderPartial(component, Model);
                if (component.StartsWith("Retrieval") && editSection == "Deployment")
                    Response.Write("<p style=\"clear:both\"> <input type=\"submit\" value=\"Commit\" onclick=\"$('form').submit();\" /></p>");
            }
        }
    %>

    <div class="clear"></div>
    <div>
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>