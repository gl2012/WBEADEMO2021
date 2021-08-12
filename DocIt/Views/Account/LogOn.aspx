<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-account-logon logOn">Log On  </h2>
    <h2 class="title-icon icon-account-logon guestBook" style="display: none">GuestBook</h2>
    <p>
        <span class="logOn">
            Please enter your username and password. <!-- <%= Html.ActionLink("Register", "Register") %> if you don't have an account.-->
            (<a class="tBold" style="cursor:pointer" onclick="$('.logOn').hide(); $('.guestBook').show();">Guests, click here</a> to submit a note.)
        </span>
        <span class="guestBook" style="display: none">
            Please enter your name and a reason why you are here.
            (<a class="tBold" style="cursor:pointer" onclick="$('.logOn').show(); $('.guestBook').hide();">Technicians, click here</a> to login.)
        </span>
    </p>
    <p><%=  ViewData["name"] %></p>
    <%= Html.BoxedValidationSummary("Please correct the errors and try again.")%>

	<div class="colSetup">
		<div class="leftCol">
			<div class="logOn">
			<% using (Html.BeginForm("LogOn", "Account")) { %>
				<fieldset>
					<legend>Account Information</legend>
					<p>
						<label for="username">Username:</label>
						<%= Html.TextBox("username") %>
						<%= Html.ValidationMessage("username", "*") %>
					</p>
					<p>
						<label for="password">Password:</label>
						<%= Html.Password("password") %>
						<%= Html.ValidationMessage("password", "*")%>
					</p>
					<p>
						<%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe">Remember me?</label>
					</p>
					<p>
						<input type="submit" value="Log On" />
						<%= Html.Hidden("returnUrl") %>
					</p>
					<br />
					<div><%= Html.ActionLink("Forgotten Password", "ForgottenPassword", "Account") %></div>
                 
				</fieldset>
			<% } %>
			</div>
			
			<div class="guestBook" style="display: none">
			<% using (Html.BeginForm("GuestBook", "Account")) { %>
				<fieldset>
					<legend>GuestBook</legend>
					<p>
						<label for="username">Name:</label>
						<%= Html.TextBox("name") %>
						<%= Html.ValidationMessage("name", "*")%>
					</p>
					<p>
						<label for="body">Note Body:</label>
						<%= Html.TextArea("body", new { rows = "4", cols = "50" })%>
						<%= Html.ValidationMessage("body", "*")%>
					</p>
					<p>
						<input type="submit" value="Save" />
					</p>
				</fieldset>
			<% } %>
			</div>		
		</div>	
	</div>

    
    <script type="text/javascript">
        $(function() {
            var currentAction = '<%= ViewContext.RouteData.Values["action"] %>';
            if (currentAction == 'GuestBook') {
                $('.guestBook').show();
                $('.logOn').hide();
            }
			$('#username').focus();
        });
    </script>
</asp:Content>