<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.DocIt.Controllers.ScheduleCalendar>" %>
<%@ Import Namespace="WBEADMS.Helpers.CalendarHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="calLegend calendarStyles">
		<span class="forecasted">Forecasted</span>&nbsp;&nbsp;&nbsp;<span class="saved">Saved</span>&nbsp;&nbsp;&nbsp;&nbsp;<span class="retrieved">Retrieved</span>&nbsp;&nbsp;&nbsp;<span class="committed">Committed</span>
	</div>

    <h2 class="title-icon icon-schedules-calendar">Calendar: <%= new DateTime((int)ViewData["this_year"], (int)ViewData["this_month"], 1).ToString("MMMM yyyy")%></h2>
    <br />
	
	<!--<div id="calLocation2">Location: <%= Html.DropDownList("location_filter", "-- all --") %></div>-->
	<div class="clear"></div>
    
	<div id="calendarInfo" class="miniCalendarWrapper">
		<div class="miniCalInner">
			<div id="calLocation3">Location: <%= Html.DropDownList("location_filter", "-- all --") %></div>
			<!-- DatePicker container -->			
			<div id="calDatePicker"><!-- --></div>
			<p><i>Click on dates to scroll calendar.</i></p>
		</div>
    </div>
       
    <div id="calendar" class="largeCalendarWrapper calendarStyles">
	
		<!-- Calendar Topper -->
        <div id="calTopper">
			<!-- Faux TABS -->
			<div class="fauxTabs ui-tabs">
				<%
					string selectedSampleTypeId = ViewData["selected_sample_type_id"] as string;
				%>
				<ul id="sampleTypeFilterTab" class="ui-tabs ui-tabs-nav ui-helper-reset ui-widget-header">
					<!--<li id="calLocation">Location: <%= Html.DropDownList("location_filter", "-- all --") %></li>-->
					<li class="ui-corner-top ui-state-default <%= selectedSampleTypeId.IsBlank() ? "ui-tabs-selected ui-state-active" : "" %>"><a href="javascript:void(0);" sample_type_id="">All</a></li>
				<% 					
    foreach (var kvp in (Dictionary<string, string>)ViewData["sample_type_filter"])
    {
        string sampleTypeId = kvp.Key;
        string sampleTypeName = kvp.Value;
        bool isSelected = selectedSampleTypeId == sampleTypeId;
        foreach (var i in (List <string>)ViewData["sample_type_custom"])
        {
            string sampleTypeNameCustom = i;
            if (sampleTypeName == sampleTypeNameCustom)
            {%>
					     <li class="ui-corner-top ui-state-default <%= isSelected ? "ui-tabs-selected ui-state-active" : "" %>"><a href="javascript:void(0);" sample_type_id="<%= sampleTypeId %>"><%= sampleTypeName %></a></li>
				<%}
        }
    }%>
				</ul>
			</div>
        </div>

		<!-- Actual Calendar -->
        <table id="calTable" cellpadding="0" cellspacing="0" border="0">
            <tr id="calTableHeading">
                <th id="calTh0" class="calCell0">Sun</th><th id="calTh1" class="calCell1">Mon</th><th id="calTh2" class="calCell2">Tue</th><th id="calTh3" class="calCell3">Wed</th><th id="calTh4" class="calCell4">Thu</th><th id="calTh5" class="calCell5">Fri</th><th id="calTh6" class="calCell6">Sat</th>
            </tr>
            <% foreach (var week in Model.Weeks) { %>
            <tr>
                <% foreach(var cell in week) { %>
                <td class="<%= Html.CalendarCellCss(cell) %>">
                    <div class="calCellDate">
                        <span name="<%= cell.Date.Month + "-" + cell.Date.Day %>"><%= cell.Date.Day %></span>
                    </div>
                    
                    <div class="calCellContent">
                    <% if (cell.ChainOfCustodys.Count > 0) { %>
                        <ul>
                             
                        <% foreach (var coc in cell.ChainOfCustodys)
                             {
                                string linkName;
                                    if (((List<string>)ViewData["sample_type_custom"]).Contains(coc.SampleType.name))
                                            {
                                                    if (coc.Schedule != null)
                                                     {
                                                                                    linkName = coc.Schedule.name;
                                                     }
                                                     else
                                                     {
                                                          string location;
                                                            if (coc.Deployment.Location != null)
                                                            {
                                                                 location = coc.Deployment.Location.name;
                                                            }
                                                             else
                                                            {
                                                                location = "[Other]";
                                                            }
                                                        linkName = location + " " + coc.SampleType.name + " (adhoc)";
                                                     }                                            
                               %>
                            <li>
                                <%                  if (coc.id.IsBlank())
                                                    { %>
                                    <%= Html.ActionLink(linkName, "CreateWithSchedule", new { controller = "ChainOfCustody", id = coc.schedule_id, date_sample_start = cell.Date.ToISODate() }, new { Class = Html.CalendarLinkCss(coc) })%>
                                                   <% }
                                                    else
                                                    { %>
                                    <%= Html.ActionLink(linkName, "Open", new { controller = "ChainOfCustody", id = coc.id }, new { Class = Html.CalendarLinkCss(coc) })%>
                                                  <% } %>
                            </li>
                        <% }
                                    }%>
                        </ul>
                    <% } %>
                    </div>
                </td>
                <% } %>
            </tr>
            <% } %>
        </table>
        
    </div>

    <br class="clear" />

    <script type="text/javascript">
    
    function this_year() { return '<%= ViewData["this_year"] %>' * 1; } //hack to get Visual Studio to autoformat selection
    function this_month() { return '<%= ViewData["this_month"] %>' * 1; } //hack to convert string to number

    var calInfoAnimating = false;
    var calInfoAnimateTimeout;
    var calDateScrolling = false;

    $(document).ready(function() {


        //fill complete table cell if has samples
        $(".calCellContent ul").parents("td").css('background-color', "#fff");

        //initial calendar options
        $('#calDatePicker').datepicker({
            dateFormat: 'yy-mm-dd',
            minDate: new Date(2005, 0, 1),
            maxDate: '+3m',
            changeMonth: true,
            changeYear: true,
            yearRange: '2005:<%= DateTime.Today.AddMonths(3).Year %>',
            //showMonthAfterYear: true, //showMonthAfterYear seems to be broken on default skin
            showButtonPanel: true,
            dateFormat: 'm-d' //used for date scrolling
        });

        //date scrolling
        $('#calendarInfo').css('margin-top', '0px');
        $('#calDatePicker').datepicker('option', 'onSelect', function(dateText, instance) {
            calDateScrolling = true;
            var target = $('span[name=' + dateText + ']');

            // add highlighting around selected cell
            $('#calTable td.selected').removeClass('selected');
            target.parents('td').addClass('selected');

            var targetOffset = target.offset().top;
            var calTableOffset = $('#calTable').offset().top;
            var calInfoMargin = targetOffset - calTableOffset;
            var maxCalInfoMargin = $('#calTable').offset().top - $('#calendar').offset().top + $('#calTable').height() - $('#calendarInfo').height() - 2;
            if (calInfoMargin > maxCalInfoMargin) { calInfoMargin = maxCalInfoMargin; }
            if (calInfoMargin >= 0) {
                var oldCalInfoMargin = $('#calendarInfo').css('margin-top').replace('px', '') * 1;
                if (Math.round(calInfoMargin) != oldCalInfoMargin) {
                    /*
                    // calendar scrolling animation; beware: this looks choppy on Firefox
                    var calInfoAnimateDuration = (oldCalInfoMargin > calInfoMargin) ? 300 : 300; //calendar up duration vs down duration
                    var bodyScrollDuration = 500;
                    $('html, body').animate({ scrollTop: targetOffset }, bodyScrollDuration);
                    $('#calendarInfo').animate({ margin: calInfoMargin + 'px 0px 0px' }, calInfoAnimateDuration);
                    */

                    // calendar fadeIn/fadeOut animation
                    var calInfoFadeOutDuration = 250;
                    var calInfoFadeInDuration = 250;
                    var bodyScrollDuration = 750;
                    $('#calendarInfo').fadeOut(calInfoFadeOutDuration, function() {
                        $('html, body').animate({ scrollTop: targetOffset }, bodyScrollDuration);
                        setTimeout("$('#calendarInfo').css('margin', '" + calInfoMargin + "px 0px 0px'); $('#calendarInfo').fadeIn(" + calInfoFadeInDuration + "); calDateScrolling = false;", bodyScrollDuration + 10);
                    });
                }
            }
        });

        //document scrolling
        $(window).scroll(function() {
            if (calDateScrolling) { return; }

            var windowTop = $(window).scrollTop();
            var calTableTop = $('#calTable').offset().top;

            var calInfoAnimateDuration = 250;

            if (calInfoAnimating) {
                clearTimeout(calInfoAnimateTimeout);
            } else {
                setTimeout("calInfoAnimating = false", calInfoAnimateDuration);
            }

            var calInfoMargin = windowTop - calTableTop;
            if (calInfoMargin < 0) { calInfoMargin = 0; }

            calInfoAnimating = true;
            calInfoAnimateTimeout = setTimeout("$('#calendarInfo').animate({ marginTop:'" + calInfoMargin + "px'}, " + calInfoAnimateDuration + ");", calInfoAnimateDuration);

        });

        //month/year picking
        $('#calDatePicker').datepicker('option', 'onChangeMonthYear', function(year, month) {
            if (month != this_month() || year != this_year()) {
                setUrlParam([{ 'name': 'month', 'value': month }, { 'name': 'year', 'value': year}]);
            }
        });

        //set current date (must be done after all options have been set)
        $('#calDatePicker').datepicker('setDate', new Date(this_year(), this_month() - 1, 1));

        //location filter
        $('#location_filter').change(function() {
            var location = $(this).find('option:selected').val();
            if (!location) { location = 'all'; }
            setUrlParam([{ 'name': 'location', 'value': location + ''}]);
        });

        //schedule filter
        $('#sample_type_filter').change(function() {
            setUrlParam([{ 'name': 'sample_type_id', 'value': $(this).find('option:selected').val()}]);
        });
        $('#sampleTypeFilterTab li a').click(function() {
            setUrlParam([{ 'name': 'sample_type_id', 'value': $(this).attr('sample_type_id')}]);
        });

    });
    
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>