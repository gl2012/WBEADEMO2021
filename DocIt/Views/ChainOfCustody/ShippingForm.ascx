<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Helpers.CheckBoxNAHelper" %>
<%@ Register Src="~/Views/ChainOfCustody/AssociatedSamplesSection.ascx" TagPrefix="uc1" TagName="AssociatedSamplesSection" %>

<% 
    ShippingSection shipping = Model.Shipping;

    string action = ViewContext.RouteData.Values["action"].ToString().ToLower();
    
    if (shipping.date_shipped_to_lab.IsBlank()) {
        shipping.date_shipped_to_lab = DateTime.Today.ToISODate(); // default date to today on open
    }
%>
<fieldset class="fieldsetShipping">
    <legend>Shipping Fields</legend>
	<div class="fieldsetInfo">
    <table cellpadding="0" cellspacing="0" border="0" class="cocTable formTable">
        <% if (action == "open" && Model.SampleType == SampleType.VOC) { %><tr><th></th><th>N/A</th><th></th></tr><% } %>
        <tr>
            <td>
                <label for="date_shipped_to_lab">Date Shipped To Lab:</label>
            </td>
            <td></td>
            <td>
                <%= Html.DatePicker("date_shipped_to_lab", shipping.date_shipped_to_lab) %>
                <%= Html.ValidationMessage("date_shipped_to_lab", "*")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="shipped_to">Shipped To:</label>
            </td>
            <td></td>
            <td>
                <%= Html.DropDownList("shipped_to", "") %>
                <%= Html.ValidationMessage("shipped_to", "*")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="shipping_company">Shipping Company:</label>
            </td>
            <td></td>
            <td>
                <%= Html.DropDownList("shipping_company", "")%>
                <%= Html.ValidationMessage("shipping_company", "*") %>
                <script type="text/javascript">
                    $(function() {
                        $('select[name=shipping_company]').change(function() {
                            var val = $(this).val();
                            var dropoffId = $('select[name=shipping_company] option:contains(Drop Off)').val(); 
                            if (val == dropoffId) {
                                if (!$('input[name=waybill_number]').val()) {
                                    $('input[name=waybill_number]').val('N/A');
                                }
                            }
                        });
                    });
                </script>
            </td>
        </tr>
        <tr>
            <td>
                <label for="waybill_number">Waybill Number:</label>
            </td>
            <td></td>
            <td>
                <%= Html.TextBox("waybill_number", shipping.waybill_number, new { title = "Press the down arrow key to get a list of previously used waybill numbers.", style="posotion:relative" })%>
                <%= Html.ValidationMessage("waybill_number", "*") %>
            </td>
        </tr>
        <% if (Model.SampleType.name == "VOC") { %>
        <tr>
            <td>
                <label for="voc_cannister_pressure_before_shipping">VOC Cannister Pressure Before Shipping:</label>
            </td>
            <uc1:AssociatedSamplesSection runat="server" ID="AssociatedSamplesSection" />
            <td><%= Html.CheckBoxNA("voc_cannister_pressure_before_shipping", shipping.voc_cannister_pressure_before_shipping)%></td>
            <td>
                <%= Html.TextBox("voc_cannister_pressure_before_shipping", shipping.voc_cannister_pressure_before_shipping)%>
                <%= Html.ValidationMessage("voc_cannister_pressure_before_shipping", "*") %>
                <%= Html.DropDownList("voc_cannister_pressure_before_shipping_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", shipping.voc_cannister_pressure_before_shipping_unit), "")%>
                <%= Html.ValidationMessage("voc_cannister_pressure_before_shipping_unit", "*")%>
            </td>
        </tr>
        <% } %>
        <tr>
            <td>
                <label for="printed">Exported/Saved:</label>
            </td>
            <td></td>
            <td>
                <%= Html.DropDownList("printed")%>
                <%= Html.ValidationMessage("printed", "*")%>
                <script type="text/javascript">
                    $(function() {
                        $('select[name=printed]').change(function() {
                            if ($(this).val() == 'True') {
                                //window.print();
                                addCOCExport(<%= shipping.chain_of_custody_id %>);
                            }
                            else {
                                deleteCOCExport(<%= shipping.chain_of_custody_id %>);
                            }
                        });
                    });
                </script>
                <span style="cursor:pointer; text-decoration:underline" onclick="addCOCExport(<%= shipping.chain_of_custody_id %>);">Add To CoC Export</span>
            </td>
        </tr>
        <tr>
            <td class="label">Current Export List <span id="clearListButton" style="cursor:pointer; text-decoration:underline">(Clear List)</span>:</td>
            <td></td>
            <td>
                <script type="text/javascript">
                    $(function() {
                        $('#clearListButton').click(function() {
                            $.ajax({
                                url: "/Home.aspx/ClearExportCOC/",
                                    success: function(result) {
                                        $('#CoCExportSpan').html(result);
                                    }
                                });
                        });

                        $('#CoCExportSpan').html("Loading CoC List... Please wait.");
                        $.ajax({
                            url: "/Home.aspx/GetExportCOC",
                            success: function(result) {
                                $('#CoCExportSpan').html(result);
                            }
                        });
                    });
                </script>
                <span id="CoCExportSpan">
                </span>
            </td>
        </tr>
        <tr>
            <td>
                <a href="/Home.aspx/ExportCOC">Download CoC List for Shipping</a>
            </td>
        </tr>

    </table>
    <input type="hidden" name="section" value="Ship" />
	</div>
</fieldset>



<% 
    string[] waybills = ChainOfCustody.FetchRecentWaybill();
    for (int i = 0; i < waybills.Length; i++) {
        waybills[i] = waybills[i].Replace("'", "\\'");
        waybills[i] = waybills[i].Replace("\"", "\\\\\"");
    }
    string waybillJSONstring = "[\"" + String.Join("\",\"", waybills) + "\"]";
%>

<script type="text/javascript">
    /**** Disabled for now. This is broken. Something with the jquery-ui CSS
    $(function () {
        if ($('#waybill_number').autocomplete) {
            $('#waybill_number').addClass('ui-widget ui-widget-content ui-corner-all');
            var source = eval('<%= waybillJSONstring %>');
            $('#waybill_number').autocomplete({
                source: source,
                delay: 300,
                minLength: 0
            });
        }
    });
    ****************/
</script>