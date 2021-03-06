<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
<%@ Import Namespace="WBEADMS.Helpers.CheckBoxNAHelper" %>
<% 
    ShippingSection shipping = Model.Shipping;

    string action = ViewContext.RouteData.Values["action"].ToString().ToLower();
    
    if (shipping.date_shipped_to_lab.IsBlank()) {
        shipping.date_shipped_to_lab = DateTime.Today.ToISODate(); // default date to today on open
    }
%>

    <fieldset>
	
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
      
       
 
    </table>
        </fieldset>
   <input type="hidden" name="section" value="Ship" />
   <br>
   
	




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
         //   $('#waybill_number').autocomplete({
         //       source: source,
       //         delay: 300,
     //           minLength: 0
     //       });
    //    }
 //   });
   
</script>