<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Item>" %>

    <%= Html.BoxedValidationSummary("Save was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

	<fieldset>
		<legend>Item Fields</legend>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td><label for="serial_number">Serial Number:</label></td>
				<td>
					<%= Html.TextBox("serial_number") %>
					<%= Html.ValidationMessage("serial_number", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="name">Name:</label></td>
				<td>
					<%= Html.TextBox("name") %>
					<%= Html.ValidationMessage("name", "*") %>
					(if left blank, default name will be "Location - Item Type")
				</td>
			</tr>
			<tr>
				<td><label for="model_id">Model:</label></td>
				<td>
					<%= Html.DropDownList("model_id", "")%>
					<%= Html.ValidationMessage("model_id", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="range">Range:</label></td>
				<td>
					<%= Html.TextBox("range") %>
					<%= Html.ValidationMessage("range", "*") %>
				</td>
			</tr>
			<tr>
				<td><label for="asset_number">Asset Number:</label></td>
				<td>
					<%= Html.TextBox("asset_number")%>
					<%= Html.ValidationMessage("asset_number", "*")%>
				</td>
			</tr>
			<tr>
			    <td><label for="is_integrated">Sampler:</label></td>
			    <td>
			        <div>
			            <label for="is_integrated_yes" style="font-weight:normal">Integrated:</label> 
			            <% if (Model.IsIntegrated) { %>
			            <%= Html.RadioButton("is_integrated", "True", new { @checked = "checked", id = "is_integrated_yes" })%>
			            <% }else { %>
			            <%= Html.RadioButton("is_integrated", "True", new { id = "is_integrated_yes" })%>
			            <% } %>
			        </div>
			        <div>
			            <label for="is_integrated_no" style="font-weight:normal">Continuous:</label> 
			            <% if (Model.IsIntegrated) { %>
			            <%= Html.RadioButton("is_integrated", "False", new { id = "is_integrated_no" })%>
			            <% }else { %>
			            <%= Html.RadioButton("is_integrated", "False", new { @checked = "checked", id = "is_integrated_no" })%>
			            <% } %>
			        </div>
			    </td>
			</tr>
			<tr id="sample_type_row">
			    <td><label for="sample_type_id">Sample Type:</label></td>
			    <td>
			        <%= Html.DropDownList("sample_type_id", "") %>
			        <%= Html.ValidationMessage("sample_type_id", "*")%>
			    </td>
			</tr>
			<tr id="parameter_list_row">
				<td class="label">Parameters <%= Html.ValidationMessage("parameter_list", "*")%></td>
				<td>
					<ul class="noBullets"> 
					<% foreach(var parameter in WBEADMS.Models.Parameter.FetchAll("hidden", "0")) { %>
						<li>
							<input type="checkbox" name="parameter_list" id="parameter<%= parameter.id %>" value="<%= parameter.id %>" <%= (((List<string>)ViewData["parameter_ids"]).Contains(parameter.id)) ? "checked=\"checked\" " : "" %>/>
							<label style="display:inline" for="parameter<%= parameter.id %>"><%= parameter.name %></label>
						</li>
					<% } %>
					</ul>
					
				</td>
			</tr>
			<tr>
				<td><label for="comment">Comment:</label></td>
				<td>
					<%= Html.TextArea("comment", new { rows = 2, cols = 40 })%>
					<%= Html.ValidationMessage("comment", "*") %>
				</td>
			</tr>
		</table>
	
		<br /><input type="submit" value="Save" />
	</fieldset>

    <% } %>
    
<script type="text/javascript">
    $(document).ready(function() {
        HideShowSamplerType();
    });

    $('[name=is_integrated]').click(function() {
        HideShowSamplerType();
    });

    function HideShowSamplerType() {
        if ($('[name=is_integrated]:checked').val() == 'True') {
            $('#parameter_list_row').css('color', '#CCCCCC');
            $('[name=parameter_list]').each(function() {
                $(this).attr('disabled', 'disabled');
                $(this).removeAttr('checked');
            });
        } else {
            $('#parameter_list_row').css('color', 'black');
            $('[name=parameter_list]').removeAttr('disabled');
        }
    }
</script>