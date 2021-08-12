<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.Note>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Note
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-notes-create"><%= ViewData["noteHeading"] %></h2>
	<p>
		<%=Html.ActionLink("Back to List", "Index") %>
	</p>
	
	<div class="colSetup">
		<div class="leftCol">
			<% Html.RenderPartial("Form1"); %>	
		</div>
		
		<% if (Model.parent_type != WBEADMS.Models.Note.ParentType.None) { %>
		<div class="rightCol">
			<div class="tabs">		
				<ul>
					<li><a href="#parentNoteDetails">Parent:</a></li>
				</ul>
				<div id="parentNoteDetails">
					<% Html.RenderPartial("ParentDetails", Model); %>
				</div>
			</div>
		</div>
		<% } %>
	</div>

	<div>
		<%=Html.ActionLink("Back to List", "Index") %>
	</div>
		
	<script type="text/javascript">
		$(document).ready(function() {
			// Initiate Tabs
			$('.tabs').tabs();
		});
	</script>	


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>