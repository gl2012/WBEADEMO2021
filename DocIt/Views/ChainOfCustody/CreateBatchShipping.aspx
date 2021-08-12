<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ChainOfCustody>>" %>
<%@ Import Namespace="WBEADMS.Helpers.CheckBoxNAHelper" %>
<%@ Import Namespace="WBEADMS.Views" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	CreateBatchShipping of COC
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.timepickr.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/ui.timepickr.min.js") %>"></script>
    <link href="<%= Url.Content("~/Content/jquery.timepickr.css") %>" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/autosave.js") %>"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.position.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.menu.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.ui.autocomplete.js") %>"></script>
    
	<script type="text/javascript">
        $(document).ready(function () {
            //Add Class to page tag for COC targeting
            $('.page').addClass('cocPage');

            //Add Show/Hide Buttons to Legends
            $('legend').prepend("<a href='javascript:void(0);' class='btnShow' title='Expand'><span>+</span></a><a href='javascript:void(0);' class='btnHide' title='Collapse'><span>-</span></a>&nbsp;");

            //Collapsing Sections
            $('.btnShow').hide();
            $('.btnHide').click(function () {
                $(this).parents('fieldset').eq(0).find('.fieldsetInfo').hide();
                $(this).hide();
                $(this).parents('fieldset').eq(0).find('legend').find('.btnShow').show();
                $(this).parents('fieldset').eq(0).addClass("isCollapsed");
            });
            $('.btnShow').click(function () {
                $(this).parents('fieldset').eq(0).find(".fieldsetInfo").show();
                $(this).hide();
                $(this).parents('fieldset').eq(0).find('legend').find('.btnHide').show();
                $(this).parents('fieldset').eq(0).removeClass("isCollapsed");
            });
            $('#collapseAll').click(function () {
                $('.btnHide').click();
            });
            $('#expandAll').click(function () {
                $('.btnShow').click();
            });

            //COC Overview Status
            var theStatus = "<%//= Model.Status.name %>";
            $(".cocOverview li.last").addClass(theStatus.toLowerCase());

            //Add N/A background
            if ($('.cocTable tr th:contains(N/A)').length) {
                $('.formTable > tbody > tr > th:nth-child(2)').addClass('cocNAcell');
                $('.formTable > tbody > tr > td:nth-child(2)').addClass('cocNAcell');
                $('.cocNAcell').after('<td></td>');
            }
        });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%  var Model1 = new WBEADMS.Models.ChainOfCustody();
        Model1=(ChainOfCustody)ViewData["cocModel"];%>
     <ul class="cocOverview">
		<li class="first edit">Create Batch Shipping of COC:</li>
		<li><span>Type:</span><br /><%= Model1.SampleType %></li>
		
		<li class="last"><span>Status:</span><br /><span id="cocOverviewStatus"><%= Model1.Status.name %></span></li>
	</ul>
  
  <div class="buttonList clear" style="padding-bottom:1em;">
		<ul> 
			
			<li class="noButton"><a href="javascript:void(0);" id="collapseAll">Collapse All</a></li>
			<li class="noButton"><a href="javascript:void(0);" id="expandAll">Expand All</a></li>
			<li class="noButton"><%= Html.ActionLink("Back to List", "BatchShipping") %></li>
		</ul>
		<div class="clear-fix"></div>
    </div>
     
<div >
  
    <fieldset>
			<legend>COCs List</legend>
<div class="fieldsetInfo">
    <table id="sortable-index">
        <tr>
           
            <th>
                ID
            </th>
            <th>
                Sample Type
            </th>
            <th>
                Location
            </th>
            <th sort_name="wbea-id">
                WBEA Sample Id
            </th>
            <th sort_name="media-serial">
                Media Serial #
            </th>
            <th>
                Scheduled Date
            </th>
            <th>
                Status
            </th>
             <% if (Model1.SampleType.name == "VOC")
                 { %>
             <th colspan="2">
                <label for="voc_cannister_pressure_before_shipping">VOC Cannister Pressure Before Shipping</label>
            </th>
            <%} else { %>
             
            <%}%>
        </tr>
    <%  using (Html.BeginForm("CreateBatchShipping", "ChainOfCustody")) { %>
      <% int i = 1; %>
        <% foreach (var item in Model) { %>    
        <tr>
            
            <td>
                <%= Html.Encode(item.chain_of_custody_id) %>
            </td>
            <td>
                <%= Html.Encode(item.SampleType.name) %>
            </td>
            <td>
                <% if (item.Deployment.Location != null) { %>
                    <%= Html.ActionLink(item.Deployment.Location.name, "Details", new { controller = "Location", id = item.Deployment.Location.location_id }) %>
                <% } %>
            </td>
            <td>
                <%

    foreach (var sample in item.Samples) { %>
                   <%= sample.wbea_id %>
                <% } %>
            </td>
            <td>
                <% foreach (var sample in item.Samples) { %>
                <%= sample.media_serial_number %>
                <% } %>
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.Preparation.ScheduledSamplingDate, "Unknown", WBEADMS.ViewsCommon.FetchDateTimeFormat())%>                
            </td>
            <td>
                <%= HtmlHelperExtensions.ToStringOrDefaultTo(item.Status, "NONE")%>                
            </td> 
            <% if (item.SampleType.name == "VOC")
    {%>
             <td><%= Html.CheckBoxNA("voc_cannister_pressure_before_shipping" + i.ToString(), item.Shipping.voc_cannister_pressure_before_shipping)%></td>
            <td>
                <%//= Html.TextBox("voc_cannister_pressure_before_shipping", item.Shipping.voc_cannister_pressure_before_shipping)%>
                <input name="voc_cannister_pressure_before_shipping<%=i%>" value=<%= item.Shipping.voc_cannister_pressure_before_shipping%> >
                <%= Html.ValidationMessage("voc_cannister_pressure_before_shipping", "*") %>
                <%//= Html.DropDownList("voc_cannister_pressure_before_shipping_unit", WBEADMS.Models.Unit.FetchSelectList("Pressure", item.Shipping.voc_cannister_pressure_before_shipping_unit), "")%>
                <%//= Html.ValidationMessage("voc_cannister_pressure_before_shipping_unit", "*")%>
               
               <select name="voc_cannister_pressure_before_shipping_unit" >
                 <% if (item.Shipping.voc_cannister_pressure_before_shipping_unit != null ){ %>
                   <option value=" " >
                        
                    </option>
                  <% } else{ %>
                   <option value=" " selected="selected">
                        
                    </option>

                   <%} %>
                <%foreach (var s in (SelectList)WBEADMS.Models.Unit.FetchSelectList("Pressure", item.Shipping.voc_cannister_pressure_before_shipping_unit))
                          {%>
                    <%if (s.Value == item.Shipping.voc_cannister_pressure_before_shipping_unit)
                          { %>
                     <option value="<%=s.Value%>" selected="selected">

                         <%}
                          else
                          {%>
                    <option value="<%=s.Value%>" >
                        <%} %>
                        <%=s.Text%>
                    </option>
                 
               
                    <%
                              }%>
               </select>
                <%i = 1 + i; %>
                  <%= Html.ValidationMessage("voc_cannister_pressure_before_shipping_unit", "*")%>
            </td>
           
           <% } %>
        </tr>
    <% } %>

    </table>
    </div>
     </fieldset>
    </div>
    <fieldset class="fieldsetShipping">
        <legend>Shipping Fields</legend>
        <div class="fieldsetInfo">
   
    

        <%    Html.RenderPartial("BatchShippingForm" , Model1);
             %>
     <table>
      <tr>
            <td>
                <label for="printed">Exported/Saved:</label>
            </td>
            <td></td>
            <td>
                <%= Html.DropDownList("printed")%>
                <%= Html.ValidationMessage("printed", "*")%>
                <%//=(string)ViewData["cocstrid"] %>
                <script type="text/javascript">
                    $(function () {
                        $('select[name=printed]').change(function () {
                            if ($(this).val() == 'True') {
                                //window.print();
                                addBatchCOCExport(<%= (string)ViewData["cocstrid"] %>);
                            }
                            else {
                                deleteBatchCOCExport(<%= (string)ViewData["cocstrid"] %>);
                            }
                        });
                    });
                </script>
                <span style="cursor:pointer; text-decoration:underline" onclick="addBatchCOCExport(<%=(string) ViewData["cocstrid"] %>);">Add To CoC Export</span>
            </td>
        </tr>
        <tr>
            <td class="label">Current Export List <span id="clearListButton" style="cursor:pointer; text-decoration:underline">(Clear List)</span>:</td>
            <td></td>
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
    
    
     <p class="noPrint clear">
           <input type="hidden" name="passId" value=<%=(string)ViewData["cocstrId"]%> />
            <input type="submit" name="form_action" value="Save"  />
            <input type="submit" name="form_action" value="Commit" />
        </p>
     </div>
    </fieldset>
   
    <% } %> <%//= Html.Hidden("auto_save")%>
    
       
    <div class="quickNoteComments">
		<fieldset>
			<legend>Note</legend>
			<div class="fieldsetInfo">
				<% using (Ajax.BeginForm("BatchNoteCreate", new AjaxOptions { UpdateTargetId = "quickNoteComments", OnComplete = "quickNoteCommentSuccess" })) { %>
					<div class="textArea"><%= Html.TextArea("comment", new { no_autosave = "no_autosave" })%> </div>
					<div><input type="submit" value="Add Comment" class="secondaryButton" />
                         <input type="hidden" name="NoteId" value=<%=(string)ViewData["cocstrId"]%> />

					</div>
					<% //var item = new WBEADMS.Models.Note();
                        Html.RenderPartial("QuickNoteCommentBlock",Model1.Notes); %>
					<%= Html.Hidden("chain_of_custody_id", Model1.id) %>
				<% } %>
			</div>
		</fieldset>
	</div>
	
	<script type="text/javascript">
        function quickNoteCommentSuccess() { //executed on successful post of Note Comment for CoC
            $('#comment').val('');
        }
	</script>
</asp:Content>