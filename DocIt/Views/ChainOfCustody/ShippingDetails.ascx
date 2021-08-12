<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Models" %>
<% ShippingSection shipping = Model.Shipping; %>
<fieldset class="fieldsetShipping">
    <legend>Shipping Fields</legend>
	<div class="fieldsetInfo">
    <table cellpadding="0" cellspacing="0" border="0" class="cocTable">
        <tr>
            <td class="label">
                Date Shipped To Lab:
            </td>
            <td>
                <%= shipping.date_shipped_to_lab.ToDateTimeFormat()%>
            </td>
        </tr>
        <tr>
            <td class="label">
                Shipped To:
            </td>
            <td>
                <%= shipping.ShippedTo %>
            </td>
        </tr>
        <tr>
            <td class="label">
                Shipping Company:
            </td>
            <td>
                <%= (shipping.shipping_company.IsBlank()) ? "" : (ShippingCompany.Load(shipping.shipping_company)).name %>
            </td>
        </tr>
        <tr>
            <td class="label">
                Waybill Number:
            </td>
            <td>
                <%= shipping.waybill_number %>
            </td>
        </tr>
        <% if (Model.SampleType.name == "VOC") { %>
        <tr>
            <td class="label">
                VOC Cannister Pressure Before Shipping:
            </td>
            <td>
                <%= shipping.voc_cannister_pressure_before_shipping %> <%= shipping.VOCCannisterPressureBeforeShippingUnit %>
            </td>
        </tr>
        <% } %>
        <tr>
            <td class="label">
                Shipped By:
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(shipping.ShippedBy, "unknown") %>
            </td>
        </tr>
        <tr>
            <td class="label">
                Exported:
            </td>
            <td>
                <%= shipping.printed.ToHumanBool() %>
            </td>
        </tr>
        <tr>
            <td class="label">Current Export List <span id="clearListButton" style="cursor:pointer; text-decoration:underline">(Clear List)</span>:</td>
            <td>
                <script type="text/javascript">
                    $(function () {
                        $('#clearListButton').click(function () {
                            $.ajax({
                                url: "/Home.aspx/ClearExportCOC/",
                                success: function (result) {
                                    $('#CoCExportSpan').html(result);
                                }
                            });
                        });

                        $('#CoCExportSpan').html("Loading CoC List... Please wait.");
                        $.ajax({
                            url: "/Home.aspx/GetExportCOC",
                            success: function (result) {
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
	</div>
</fieldset>