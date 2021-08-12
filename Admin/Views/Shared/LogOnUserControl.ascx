<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
        var HANDLE_RESET = true;
        if (Session["user"] == null) {
            if (!HANDLE_RESET) {
                Page.Response.Redirect(Url.RouteUrl(new { conroller = "Account", action = "LogOff" }));  //this is a hack to redirect when server session is reset, yet cookie says browser is logged in  // TODO: check if we can make form authentication by session so we don't have this session/cookie problem; note that the Url is hardcoded, this is bad if the route is /controller.aspx/action/id
            }
            else {
                Session["user"] = WBEADMS.Models.User.FetchByName(Page.User.Identity.Name);
            }
        }
        
        var user = (WBEADMS.Models.User)Session["user"];

        if (!user.Role.Policy.AdminPanelAccess) {
            TempData["notice"] = user.user_name + " does not have access to Admin Panel.";
            Page.Response.Redirect(Url.Action("LogOff", "Account"));
        }
%>
        <div id="welcomeWrapper">
			<span id="welcomeMsg">Welcome <%= Html.Encode(user.display_name) %>!&nbsp;[<%= Html.ActionLink("My Account", "MyAccount", "Account") %>]</span>
			<span id="headerTime"><%= DateTime.Now.ToString(WBEADMS.ViewsCommon.FetchDateTimeFormat()+":ss")%> MST</span>
			<span class="buttonList"><%= Html.ActionButton("Log Off", "LogOff", "Account") %></span>
		</div>
		
        <script type="text/javascript" language="javascript">
            var currentTime = new Date();
            UpdateFromSystemDate();
            setInterval("UpdateHeaderDate();", 1000); //add 1 second to currentTime, every second; and update the header
            setInterval("UpdateFromSystemDate();", 300001); //set to update every five minutes; offset by 1 millisecond to not execute at the same time as the other Interval

            function UpdateFromSystemDate() {
                $.getJSON('<%= Url.Action("GetDateTimeRFC1123", "Account") %>', {}, function(data) {
                    currentTime = new Date(data.updateddatetime);
                });
            }

            function UpdateHeaderDate() {
                var next_sec = currentTime.getSeconds() + 1;
                currentTime.setSeconds(next_sec, 0);
                $('#headerTime').html(ISODate(currentTime) + ' MST');
            }

            // ISO 8601 DateTime format: yyyy-MM-dd HH:mm:ss <%-- TODO: we should initially read FetchDateTimeFormat from webconfig and use it; but this seems overly complicated --%>
            function ISODate(date) {
                var curr_year = date.getFullYear();
                var curr_month = date.getMonth() + 1;
                var curr_day = date.getDate();
                var curr_hour = date.getHours();
                var curr_min = date.getMinutes();
                var curr_sec = date.getSeconds();
                return curr_year + '-' + leadZero(curr_month) + '-' + leadZero(curr_day) + ' ' +
                    leadZero(curr_hour) + ':' + leadZero(curr_min) + ':' + leadZero(curr_sec);
            }

            function leadZero(number) {
                if (number < 10) {
                    return "0" + number;
                } else {
                    return number;
                }
            }
        </script>
<%
    }
    else {
%> 
		<div class="buttonList">
			<!-- Removing for now: RDM 
			<%= Html.ActionButton("Log On", "LogOn", "Account") %>
			-->
		</div>
<%
    }
%>