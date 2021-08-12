<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ChainOfCustody>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.SampleType %> Chain of Custody: <%= Model.chain_of_custody_id %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.timepickr.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/ui.timepickr.min.js") %>"></script>
    <link href="<%= Url.Content("~/Content/jquery.timepickr.css") %>" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/autosave.js") %>"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.position.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.menu.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.autocomplete.js") %>"></script>
    
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

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

	<ul class="cocOverview">
		<li class="first open">Chain of Custody Open:</li>
		<li><span>Type:</span><br /><%= Model.SampleType %></li>
		<li><span>ID:</span><br /><%= Model.chain_of_custody_id %></li>
		<li class="last"><span>Status:</span><br /><span id="cocOverviewStatus"><%= Model.Status.name %></span></li>
	</ul>

    <div class="buttonList clear" style="padding-bottom:1em;">
		<ul> 
			<li><%= Html.ActionLink("View Details", "Details", new { id = Model.chain_of_custody_id })%></li>
			<li class="noButton"><a href="javascript:void(0);" id="collapseAll">Collapse All</a></li>
			<li class="noButton"><a href="javascript:void(0);" id="expandAll">Expand All</a></li>
			<li class="noButton"><%= Html.ActionLink("Back to List", "Index") %></li>
            <%if (Model.Previous() != null) %>
            <%{ %>
            <li class="noButton"><%= Html.ActionLink("Previous", "Open", new { id = Model.Previous() })%></li>
            <%} %>
            <%if (Model.Next() != null) %>
            <%{ %>
            <li class="noButton"><%= Html.ActionLink("Next", "Open", new { id = Model.Next() })%></li>
            <%} %>
		</ul>
		<div class="clear-fix"></div>
    </div>
	
	<%-- ASSOCIATED SAMPLES ---%>
    <div id="sample_id_wrapper">
	<% Html.RenderPartial("AssociatedSamplesSection", Model, ViewData);	%>
	</div>
    
    <%-- ASSOCIATED TRAVEL BLANKS---%>
    <% if (Model.SampleType.name == "PM2.5" || Model.SampleType.name == "PM10" || Model.SampleType.name == "PASS") { %>
    <div id="travel_sample_id_wrapper">
	<% Html.RenderPartial("TravelBlanksSection", Model, ViewData); %>
	</div>
    <% } %>
    
    <%-- RENDER SECTIONS --%>
    <% 
       using (Html.BeginForm("Open", "ChainOfCustody")) {
           foreach (string component in Model.GetComponents()) {
               Html.RenderPartial(component, Model);
           }

           if (!Model.Status.IsComplete) { %>
        <p class="noPrint clear">
            <input type="submit" name="form_action" value="Save" />
            <input type="submit" name="form_action" value="Commit" />
        </p>
        <% } %>
        <%= Html.Hidden("auto_save")%>
    <% } %>

    <%-- NOTE COMMENTS --%>
	
	<div class="quickNoteComments">
		<fieldset>
			<legend>Note</legend>
			<div class="fieldsetInfo">
				<% using (Ajax.BeginForm("NoteCreate", new AjaxOptions { UpdateTargetId = "quickNoteComments", OnComplete = "quickNoteCommentSuccess" })) { %>
					<div class="textArea"><%= Html.TextArea("comment", new { no_autosave = "no_autosave" })%> </div>
					<div><input type="submit" value="Add Comment" class="secondaryButton" /></div>
					<% Html.RenderPartial("QuickNoteCommentBlock", Model.Notes); %>
					<%= Html.Hidden("chain_of_custody_id", Model.id) %>
				<% } %>
			</div>
		</fieldset>
	</div>
	
	<script type="text/javascript">
	    function quickNoteCommentSuccess() { //executed on successful post of Note Comment for CoC
	        $('#comment').val('');
	    }
	</script>
	
    <div class="noPrint">
		<br />
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>