<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Schedule>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>

        <fieldset>
            <legend>Schedule Info</legend>
			
			<table cellpadding="0" cellspacing="0" border="0">
			    <% if (Model.id.IsBlank() || Model.ChainOfCustodyCount == 0) {  %>
				<tr>
					<td><label for="location_id">Location:</label></td>
					<td><%= Html.DropDownList("location_id", "")%> <%= Html.ValidationMessage("location_id", "*")%></td>
				</tr>
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
					<td><%= Html.DropDownList("sample_type_id", "")%> <%= Html.ValidationMessage("sample_type_id", "*")%></td>
				</tr>
				<% } else { %>
				<tr>
					<td class="label">Location:</td>
					<td><%= Model.Location %>
				</tr>
				<tr>
					<td class="label">Sample Type:</td>
					<td><%= Model.SampleType %></td>
				</tr>
				<% } %>
				<tr>
					<td><label for="name">Name:</label></td>
					<td><%= Html.TextBox("name") %> <%= Html.ValidationMessage("name", "*") %></td>
				</tr>
				<tr>
					<td><label for="contact_id">Contact:</label></td>
					<td><%= Html.DropDownList("contact_id", "")%> <%= Html.ValidationMessage("contact_id", "*") %></td>
				</tr>
				<tr>
					<td><label for="date_start">Start Date:</label></td>
					<td><%= Html.DatePicker("date_start") %> <%= Html.ValidationMessage("date_start", "*") %></td>
				</tr>
				<tr>
					<td><label for="date_end">End Date:</label></td>
					<td><%= Html.DatePicker("date_end", new { maxDate = "", changeYear = "true", changeMonth = "true" })%> <%= Html.ValidationMessage("date_end", "*") %></td>
				</tr>
				<tr>
					<td><label for="interval_id">Interval Type:</label></td>
					<td><%= Html.DropDownList("interval_id", "")%> <%= Html.ValidationMessage("interval_id", "*") %> </td>
				</tr>
				<tr>
					<td><label for="frequency_data">Frequency:</label></td>
					<td><%= Html.TextBox("frequency_data") %> <%= Html.ValidationMessage("frequency_data", "*") %><%= WBEADMS.ViewsCommon.IconImageTag("clear_button", 8, 15)%><div id="frequency_picker"></div></td>
				</tr>
				<tr>
					<td><label for="comments">Comments:</label></td>
					<td><%= Html.TextArea("comments", new { rows = 2, cols = 40 })%> <%= Html.ValidationMessage("comments", "*") %></td>
				</tr>
				<% if (!Model.id.IsBlank()) { %>
				<tr>
					<td><label for="is_active">Active:</label></td>
					<td><%= Html.CheckBox("is_active")%> <%= Html.ValidationMessage("is_active", "*")%></td>
				</tr>
				<% } %>
				<tr>
					<td><input type="submit" value="Save" /></td>
					<td></td>
				</tr>
			</table>
        </fieldset>
    
    <% } %>

<script type="text/javascript" language="javascript">
    $(document).ready(function() {
        // clicking clear button will reset freq input and clear calendar
        $("[name=clear_button]").css("cursor", "pointer").attr("title", "Click to reset to 1.").click(function() {
            $("#frequency_data").val('1');
            ActivateCellsFrequencyPicker();
        });

        // changing interval will reset freq input
        $("#interval_id").change(function() {
            $("#frequency_data").val('1');  // reset frequency to 1 on change
            CheckIntervalType();
        });

        // on submit, reenable freq data (hack so that frequency_data is always submitted)
        $("input[type=submit]").click(function() {
            $("#frequency_data").removeAttr("disabled");
        });

        // startup functions
        CheckIntervalType();
        ActivateCellsFrequencyPicker();
    });

    // format input fields based on selected interval id
    function CheckIntervalType() {
        var interval_id = $("#interval_id").val();
        if (interval_id == 2 || interval_id == 3) { ActivateFrequency(); } // allow frequency for every n days or every n months
        else if (interval_id == 5) { ActivatePerDay(); } // activate datepicker for within a month
        else {
            if (interval_id == 1 || interval_id == 4 || interval_id == 6) {
                $("#frequency_data").attr("disabled", "disabled");
            }

            $("#frequency_span").hide();
            $("#frequency_picker").hide();
            $("[name=clear_button]").hide();
        }
    }

    // format based on every n days/every n months
    function ActivateFrequency() {
        $("#frequency_span").show();
        $("[name=clear_button]").show();

        var input = $("#frequency_data");
        input.removeAttr("disabled");
        input.attr("title", "Enter the frequency. Default is 1.");

        $("#frequency_picker").hide();
    }

    // set up DatePicker for Within A Month
    function ActivatePerDay() {
        $("#frequency_span").show();
        $("[name=clear_button]").show();

        var input = $("#frequency_data");
        input.attr("disabled", "disabled");
        input.attr("title", "Enter the days of the month, separated by commas.");
        var maxDate = new Date();
        maxDate.setMonth(maxDate.getMonth() + 1); maxDate.setDate(0);  //a hack to get last day of month
        var minDate = new Date();
        minDate.setDate(1);
        $("#frequency_picker").show().datepicker().datepicker('option', {
            duration: 'fast',
            dateFormat: 'd',
            hideIfNoPrevNext: true,
            maxDate: maxDate,
            minDate: minDate,
            onSelect: function(dateText, inst) {
                var freq = $.map($("#frequency_data").val().split(","), function(n, i) { return n * 1; });
                if (freq[0] == "") {
                    freq = [dateText];
                } else {
                    var matchIndex = $.inArray(dateText * 1, freq);
                    if (matchIndex > -1) {
                        freq.splice(matchIndex, 1);
                    } else {
                        freq.push(dateText);
                    }
                    freq.sort(compareNumber);
                }

                $("#frequency_data").val(freq.join(","));
                setTimeout("ActivateCellsFrequencyPicker();", 10);
            }
        });
        //$("#frequency_picker").show().css('height', '40px'); //hack to make Comments wrap to next line; note that datepicker on instantiation will reset height

        ActivateCellsFrequencyPicker();
    }

    // show DatePicker
    function ActivateCellsFrequencyPicker() {
        var freqText = $("#frequency_data").val().split(",");
        $('#frequency_picker td a').removeClass('ui-state-active');
        $('#frequency_picker td a').filter(function(index) { return $.inArray($(this).text(), freqText) >= 0 }).addClass('ui-state-active')
    }

    function compareNumber(a, b) { return a - b; }
</script>