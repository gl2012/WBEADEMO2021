<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Models" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details for <%= Model.SampleType %> Chain of Custody: <%= Model.chain_of_custody_id %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeaderContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function() {
			//Add Class to page tag for COC targeting
			$('.page').addClass('cocPage');
			
			//Show/Hide Audit History Section & Move cocSectionNav into place
			$('.cocSection').each(function() {
				$(this).children(".cocSectionAudit").clone().prependTo($(this).find("fieldset:first .fieldsetInfo"));
				$(this).children(".cocSectionNav").clone().css('display','block').prependTo($(this).find("fieldset:first .fieldsetInfo"));
			});				
			$('.showAuditHistory').click(function() {
				$(this).hide();
				$(this).parents('.cocSectionNav').find('.hideAuditHistory').show();
				$(this).parents('.cocSection').find('.cocSectionAudit:first').fadeIn();
			});			
			$('.hideAuditHistory').click(function() {
				$(this).hide();
				$(this).parents('.cocSectionNav').find('.showAuditHistory').show();
				$(this).parents('.cocSection').find('.cocSectionAudit:first').fadeOut();
			});
			
			//Add Show/Hide Buttons to Legends
			$('fieldset fieldset').addClass('innerFieldset');
			$('fieldset:not(.innerFieldset)').each(function() {
				$(this).find('legend:first').prepend("<a href='javascript:void(0);' class='btnShow' title='Expand'><span>+</span></a><a href='javascript:void(0);' class='btnHide' title='Collapse'><span>-</span></a>&nbsp;");
			}); 
			
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

			//COC Overview Status
			var theStatus = "<%= Model.Status.name %>";
			$(".cocOverview li.last").addClass(theStatus.toLowerCase());

		});
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<ul class="cocOverview">
		<li class="first details">Chain of Custody Details:</li>
		<li><span>Type:</span><br /><%= Model.SampleType %></li>
		<li><span>ID:</span><br /><%= Model.chain_of_custody_id %></li>
		<li class="last"><span>Status:</span><br /><span id="cocOverviewStatus"><%= Model.Status.name %></span></li>
	</ul>
	<p> <% =ViewBag.message %></p>
    <div class="buttonList clear">
		<ul> 
			<li><%= Html.ActionButton("Open", "open", new { id = Model.chain_of_custody_id })%></li>
			<li class="noButton"><a href="javascript:void(0);" id="collapseAll">Collapse All</a></li>
			<li class="noButton"><a href="javascript:void(0);" id="expandAll">Expand All</a></li>
			<li class="noButton"><%= Html.ActionLink("Back to List", "Index") %></li>
          <%if (Model.sample_type_id == "9")
                  {%>
            <li class="Button"><%//= Html.ActionLink("Passive Air Sampling", "ViewReport", new { id = Model.chain_of_custody_id, reporttype = "Details" }) %>
                <a href="/ChainOfCustody.aspx/DownloadActiveReport?cocId=<%=Model.chain_of_custody_id %>" style="color:crimson">Download Passive Air Sampling PDF </a>
            </li>
                 
		  <%} %>
        </ul>
          
		<div class="clear-fix"></div>
    </div>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
	
    <fieldset class="clear">
        <legend>Associated Sample(s)</legend>

		<div class="fieldsetInfo">
            <span class="labelFilter1 success">Sample(s) count:<%=Model.Samples.Count %></span>
        <% 
           if (Model.Samples.Count > 0) { %>
              <table id="sortable-index"><tr>
                <th>
                   WBEA Sample ID 

                </th>
                  <th>
                    Prepared By: 
                </th> <th>
                    Received From Lab
                </th>
                  <th>
				    Avg. Storage Temperature
				</th>
                  
                  <th>
                    Media Serial Number
                </th> <th>
                    Lab Sample ID
                </th>
                  <th>Notes</th>
                     </tr>
                  
            <%   foreach (Sample sample in Model.Samples) {
                   //Html.RenderPartial("AssociatedSample", sample);
                   Html.RenderPartial("NewAssociatedSample", sample);
               } %>
             </table>
         <%  }
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
        <% 
               foreach (Sample travelBlank in Model.TravelBlanks) {
                   Html.RenderPartial("AssociatedSample", travelBlank);
               }
        %>
		</div>
    </fieldset>
    <% } %>
    
    <% 
        string currentSection = Model.Status.details_view;
        if (currentSection.IsBlank()) {
            currentSection = Model.Status.PreviousState().details_view;
        }
        
        foreach (string component in Model.GetDetailComponents()) {
			%>
			
			<div class="cocSection">
				<div class="cocSectionNav" style="display:none;">
					<div class="buttonList">
						<ul> 
							<li>
					<%
					if (Model.Status.ToString() != "Historical") {
						Response.Write(Html.ActionButton("Edit", "Edit", new { id = Model.chain_of_custody_id, editSection = component.Replace("Details", "") }));
					}
					%>
							</li>
							<li><a href="javascript:void(0);" class="showAuditHistory"><span>Show Audit History</span></a>
								<a href="javascript:void(0);" class="hideAuditHistory" style="display:none;"><span>Hide Audit History</span></a></li>
						</ul>
					</div>
				</div>
				<%
				Html.RenderPartial(component, Model); 
				%>
				<div class="cocSectionAudit" style="display:none;"><% Html.RenderPartial("AuditHistory", Model.FetchAuditRecords(component.Replace("Details", ""))); %></div>
			</div>
			
			<%
        }
    %>

	<div class="clear">
	    <% Html.RenderPartial("NoteCommentBlock", Model.Notes); %>

	</div>

    <div class="noPrint">
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>
	
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>