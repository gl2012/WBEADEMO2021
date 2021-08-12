<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (((IEnumerable<SelectListItem>)ViewData["sampler_item_id_selectlist"]).Count() > 0) { %>
<%= Html.DropDownList("sampler_item_id", (IEnumerable<SelectListItem>)ViewData["sampler_item_id_selectlist"], "", new { selected = "3", onkeyup = "$(this).change()", onchange = @"$.getJSON(""../GetSamplerDetails"", { sampler_item_id:$('#sampler_item_id').val()}, function(data){$('#SamplerSerialNumber').html(data.serial_number); $('#SamplerMake').html(data.make); $('#SamplerModel').html(data.model);})" })%>
<% } else { %>
<span id="sampler_item_id">N/A (no samplers at this location)</span>
<% } %>