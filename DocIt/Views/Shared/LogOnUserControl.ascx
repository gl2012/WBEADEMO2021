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
                Page.Response.Redirect(Url.RouteUrl(new { controller = "Account", action="LogOff"}));  //this is a hack to redirect when server session is reset, yet cookie says browser is logged in  // TODO: check if we can make form authentication by session so we don't have this session/cookie problem; note that the Url is hardcoded, this is bad if the route is /controller.aspx/action/id
            }
            else {
                Session["user"] = WBEADMS.Models.User.FetchByName(Page.User.Identity.Name);
            }
        }
        var user = (WBEADMS.Models.User)Session["user"];

        if (!user.Role.Policy.DocItAccess) {
            TempData["notice"] = user.user_name + " does not have access to DocIt.";
            Page.Response.Redirect(Url.Action("LogOff", "Account"));
        }
%>
        <div id="welcomeWrapper">
			<span id="welcomeMsg">Welcome <%= Html.Encode(user.display_name) %>!&nbsp;[<%= Html.ActionLink("My Account", "MyAccount", "Account") %>]</span>
			<span id="headerTime"><%= DateTime.Now.ToString(WBEADMS.ViewsCommon.FetchDateTimeFormat()+":ss")%> MST</span>
			<span class="buttonList"><%= Html.ActionButton("Log Off", "LogOff", "Account") %></span>
		</div>

        <script>
            var currentTime = new Date();
            UpdateFromSystemDate();
            setInterval("UpdateHeaderDate();", 1000); //add 1 second to currentTime, every second; and update the header
            setInterval("UpdateFromSystemDate();", 300001); //set to update every five minutes; offset by 1 millisecond to not execute at the same time as the other Interval

            function UpdateFromSystemDate() {
                $.getJSON('<%= Url.Action("GetDateTimeRFC1123", "Account", new { key = DateTime.Now.Ticks })%>', {}, function(data) {
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

            function getoffset(month, date, day, hour) {
                var offset = 7;
                // adjust to MDT offset as needed
                if ((month > 2 && month < 10) || (month === 2 && date > 14)) {
                    offset = 6;
                } else if (month === 2 && date > 7 && date < 15) {
                    if ((day && date - day > 7) || (day === 0 && hour - offset >= 2)) {
                        offset = 6;
                    }
                } else if (month === 10 && date < 8) {
                    if ((day && date - day < 0) || (day === 0 && hour - offset < 1)) {
                        offset = 6;
                    }
                }
                return offset;


            }

            function ISODate1(dt) {
                var year = dt.getFullYear(); // utc year
                var month = dt.getMonth(); // utc month (jan is 0)
                var date = dt.getDate(); // utc date
                var hour = dt.getHours(); // utc hours (midnight is 0)
                var minute = dt.getMinutes(); // utc minutes
                var curr_second = dt.getSeconds(); // utc seconds
                var day = dt.getDay(); // utc weekday (sunday is 0)
                var offset = getOffset(month, date, day, hour);
                if (hour - offset < 0) {
                    hour = 24 + hour - offset;
                    day = day ? day - 1 : 6;
                    if (date === 1) {
                        if (!month) {
                            year -= 1;
                            month = 11;
                        } else {
                            month -= 1;
                        }
                        date = new Date(year, month + 1, 0).getDate();
                    } else {
                        date -= 1;
                    }
                } else {
                    hour -= offset;
                }
                month += 1;
                return year + '-' + leadZero(month) + '-' + leadZero(date) + ' ' +
                    leadZero(hour) + ':' + leadZero(minute) + ':' + leadZero(curr_second);
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