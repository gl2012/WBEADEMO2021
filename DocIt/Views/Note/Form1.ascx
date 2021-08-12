<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Note>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% 
        var attrs = new Dictionary<string, object>();
        attrs.Add("enctype", "multipart/form-data");
        attrs.Add("data-ajax", "false");       
    %>
    <%
        using (Html.BeginForm(null, null, FormMethod.Post, attrs))
       {%>

	<fieldset>
		<legend>Note Details</legend>
		
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
			<tr>
				<td><label for="location_id">Location:</label></td>
				<td>
				<%= Html.DropDownList("location_id", "") %>
				<%= Html.ValidationMessage("location_id", "*")%>
				</td>
			</tr>
			<tr>
				<td><label for="parent">Parent:</label></td>
				<% if (Model.id.IsBlank() && !Model.HasParent) { %>
				<td><% Html.RenderPartial("ParentDropdownForm", ViewData); %></td>
				<% } else { %>
				<td><%= Html.NoteParent(Model)%></td>
				<% } %>
			</tr>
			<tr>
				<td><label for="date_occurred">Date&nbsp;Occurred:</label></td>
				<td>
					<%= Html.DatePicker("date_occurred") %>
					<%= Html.ValidationMessage("date_occurred", "*")%>
				</td>
			</tr>
			<tr>
				<td><label for="is_starred">Starred:</label></td>
				<td>
					<%= Html.CheckBox("is_starred", "")%>
					<%= Html.ValidationMessage("is_starred", "*")%>
				</td>
			</tr>
			<tr>
			    <td><label for="sample_head_cleaing">Cyclone&nbsp;Head Cleaning:</label></td>
			    <td>
			        <%= Html.CheckBox("sample_head_cleaning") %>
			        <script type="text/javascript">
			            $(function() {
			                $('#sample_head_cleaning').click(function() {
			                    var body = $('#body');
			                    if (this.checked) {
			                        if (!body.val().match(/^Cyclone head cleaning\.?\s*/i)) {
			                            body.val('Cyclone head cleaning. ' + body.val());
			                        }
			                    } else {
			                        body.val(body.val().replace(/^Sample head cleaning\.?\s*/i, ""));
			                    }
			                });
			            });
			        </script>
			    </td>
			</tr>
			<tr>
				<td><label for="body">Body:</label></td>
				<td>
					<span class="textArea newNote"><%= Html.TextArea("body")%></span>
					<%= Html.ValidationMessage("body", "*")%>
				</td>
			</tr>
			<tr>
				<td class="label">Parameters:</td>
				<td>
		            <ul id="parameter_list_ul" class="noBullets">

		            <% if (Model.location_id.IsBlank() || Model.Location.Parameters.Length == 0) { %>
                        <li id="parameter_list_none"><b>None</b></li>
                    <% } else { foreach (var parameter in Model.Location.Parameters) { %>
			            <li>
				            <input type="checkbox" name="parameter_list" id="parameter<%= parameter.id %>" value="<%= parameter.id %>" <%= (((List<string>)ViewData["parameter_ids"]).Contains(parameter.id)) ? "checked=\"checked\" " : "" %>/>
				            <label style="display:inline" for="parameter<%= parameter.id %>"><%= parameter.name%></label>
			            </li>
		            <% } } %>

		            </ul>
				</td>
			</tr>
            <tr>
                <td class="label">Attachment:</td>
                <td>
                    <input id="fileInput" type="file" runat="server"/>
                    <%=(string)ViewData["cocid"] %><%=ViewData["parent_type"]%>
                </td>
            </tr>
		</table>

		<p>
			 <%=Html.Hidden("cocid", (string)ViewData["cocid"]) %> 
             <%=Html.Hidden("parent_id",ViewData["parent_id"]) %> 
             <%=Html.Hidden("parent_type",ViewData["parent_type"]) %> 
            <input type="submit" value="Save" />
		</p>
		
	</fieldset>

    <% } %>
    
<script type="text/javascript">
    var checked_parameters = '<%= String.Join(",",((List<string>)ViewData["parameter_ids"]).ToArray()) %>'.split(",");

    $('#location_id').change(function() {
        var location_id = $(this).val();
        $.getJSON('<%= Url.Action("GetLocationParameters", "Note")%>/' + location_id, RefreshParameters);
        $.get('<%= Url.Action("ParentDropdownForm", "Note")%>/' + location_id, null, function(dropdownhtml) { $('#parent_id_span').replaceWith(dropdownhtml); });
    });

    if ($.browser.msie) { // hack for IE onChange event
        $('#parent_id').live('click', function() {
            var val = $(this).val();
            var old = $(this).data('old_value');

            //alert('old_value=' + old + '\nval=' + val);
            if (val != old) {
                $.get('<%= Url.Action("GetItemParameter", "Note")%>/' + $(this).val(), null, function(params) { FixParameters(params); });
            }

            $(this).data('old_value', val)
        });
    }

    $('#parent_id').live("change", function() {
        $.get('<%= Url.Action("GetItemParameter", "Note")%>/' + $(this).val(), null, function(params) { FixParameters(params); });
    });

    function RefreshParameters(data) {
        var indexes = new Array();
        for (var i = 0; i < data.length; i++) {
            indexes[i] = data[i].id;
        }

        $('#parameter_list_none').remove();
        if (indexes.length == 0) {
            $('#parameter_list_ul').append('<li id="parameter_list_none"><b>None</b></li>');
        }

        // remove parameter checkboxes
        $('#parameter_list_ul input').each(function() {
            var thisId = $(this).val();
            i = $.inArray(thisId, indexes);
            if (i == -1) {
                $(this).parent('li').remove(); // if existing item not in parameter list, remove it from UL
            } else {
                indexes.splice(i, 1); // if existing item is in list, then remove it from indexes array, so we don't add it later
            }
        });

        // add parameter checkboxes
        $.each(data, function(i, d) {
            var j = $.inArray(d.id, indexes);
            if (j >= 0) {
                var checked = $.inArray(d.id, checked_parameters) >= 0 ? 'checked="checked"' : '';
                var content = '<li><input type="checkbox" name="parameter_list" id="parameter' + d.id +
                    '" value="' + d.id + '" ' + checked + '/> ' +
				    '<label style="display:inline" for="parameter' + d.id + '">' + d.name + '</label></li>';
                $('#parameter_list_ul').append(content);
            }
        });
    }

    function FixParameters(params) {
        if ($.inArray("0", eval(params)) == 0) {
            $('#parameter_list_ul label').css('color', 'black');
            $('#parameter_list_ul input[type=checkbox]').removeAttr('disabled').removeAttr('checked');

        } else {
        $('#parameter_list_ul label').css('color', '#CCCCCC');
            $('#parameter_list_ul input[type=checkbox]').attr('disabled', 'disabled').removeAttr('checked');
            $.each(eval(params), function(i, val) {
                $('label[for=parameter' + val + ']').css('color', 'black');
                $('#parameter_list_ul input[value=' + val + ']').removeAttr('disabled').attr('checked', 'checked');
            });
        }
    }
</script>