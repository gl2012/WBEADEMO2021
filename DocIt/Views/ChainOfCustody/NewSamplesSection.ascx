<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<WBEADMS.Models.Sample>>" %>
<% 
    string formId = (string)ViewData["formId"];
    ChainOfCustody coc = (ChainOfCustody)ViewData["coc"];
%>
    <fieldset class="<%= ViewData["fieldSetClass"] %>">
        <legend><%= ViewData["legend"] %></legend>
        <%-- sample info and Remove Sample button --%>
		<div class="fieldsetInfo">
                          
        <% 
            var intcount = 0;
            foreach (WBEADMS.Models.Sample sample in Model) {
               intcount=intcount+1 ;
            }
        %>
            <span class="labelFilter1 success"> Simple(s) Count: <%=intcount %></span>
             <table id="sortable-index" class="cocTable sampleTable"><tr>
                <th>
                   WBEA Sample ID 

                </th>
                  <th>
                    Prepared By: 
                </th> <th>
                    Received From Lab
                </th>
                  <th>
				    Avg. Storage Temperature
				</th>
                  
                  <th>
                    Media Serial Number
                </th> <th>
                    Lab Sample ID
                </th>
                 <th>Notes</th>
                 <th colspan="2"> Action
                     </th>
                 </tr>
        <% 
            var lastDate = "";
            string lastTemp = "";
            foreach (WBEADMS.Models.Sample sample in Model) {
                var sampleViewData = new ViewDataDictionary();
                sampleViewData["CoC"] = coc;
               // Html.RenderPartial("AssociatedSample", sample, sampleViewData);
                Html.RenderPartial("NewAssociatedSample", sample, sampleViewData);
                lastDate = sample.date_received_from_lab;
                lastTemp = sample.average_storage_temperature;
            }
        %>
     </table>
        <script type="text/javascript">
            $(document).ready(function() {
                $('#<%= formId %>_wrapper table.sampleTable input[type=submit]').click(function() {
                    $.post(
                        '<%= Url.Action("RemoveSample", "ChainOfCustody", new { id = coc.chain_of_custody_id }) %>',
                        { 'sample_id': $(this).parent().find('input[name=sample_id]').val() },
                        function(returnHtml) {
                            $('#<%= formId %>_wrapper').html(returnHtml);
                            return false;
                        }
                    )
                });

                $('#<%= formId %>_wrapper table.sampleTable input[id=add]').click(function () {
                    $('#<%= formId %>_dialog1').dialog('open');
                });

                $('#<%= formId %>_dialog1').dialog({
                    autoOpen: false,
                    width: 560,
                    height: 425,
                    modal: true,
                    resizable: false,
                    draggable: false,
                    buttons: {
                        "Create": function () {
                            $(this).dialog("close");
                            $("#<%= formId %>_dialog form").attr('action', '<%= Url.Action("Create", new { controller="Sample" }) %>/' + $(this).dialog('option', 'coc_id'));
                            $("#<%= formId %>_dialog [name=date_received_from_lab]").val($("#<%= formId %>_date_received_from_lab").val());
                            $("#<%= formId %>_dialog form").submit();
                        },
                        "Cancel": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
        </script>
      <div id="<%= formId %>_dialog1" style="display: none" class="modalPopup page" title="Sample Note Creation">
             
     
          </div>
      
            
            <%-- Add Sample button and Preview --%>
        <%  
            if (coc.Status.name == "Opened" || coc.Status.name == "Preparing" || coc.Status.name == "Prepared" || coc.Status.name == "Deploying" || coc.Status.name == "Deployed" || coc.Status.name == "Retrieving" || coc.Status.name == "Retrieved" || coc.Status.name == "Shipping") {
                if (Model != null && (Model.Count < (int)ViewData["maxSampleCount"])) { %>
                    <%-- Ajax.ActionLink("Add", "AddSample", new { controller = "ChainOfCustody", id = coc.chain_of_custody_id }, new AjaxOptions() { LoadingElementId = formId + "_loader", UpdateTargetId = formId + "_wrapper",  }, new { @class = "addSample" }) --%>

     
            <div id="<%= formId %>_add_div" class="addSampleBox noPrint">
            <%= Html.DropDownList("sample_id", (IEnumerable<SelectListItem>)ViewData["sample_id"], new { no_autosave = "no_autosave" })%>
            <input class="secondaryButton addSample" type="submit" value="Add Sample" />
            <% if (DateTime.Now.AddDays(-Sample.OldCoCByDate) <= coc.date_opened.ToDateTime().Value) { %>
            <input class="secondaryButton createSample" type="submit" value="Create New" />
            <% } else { %>
            <input class="secondaryButton createSample" type="submit" value="Create New*" title="Warning! If you remove a newly created Sample, you will not find it in the dropdown as the CoC is too old (older than <%= Sample.OldCoCByDate %> days) to be eligible for newly created Samples." />
            <% } %>
            <br />
            <span id="<%= formId %>_preview">
            <% 
                Sample previewSample = null;
                foreach (var sample in (SelectList)ViewData["sample_id"]) {
                    previewSample = Sample.Load(sample.Value); // grab just the first sample in the select list
                    break;
                }
                if (previewSample != null) {
                    var previewSampleViewData = new ViewDataDictionary();
                    previewSampleViewData["CoC"] = coc;
                    previewSampleViewData["Preview"] = true;
                    Html.RenderPartial("AssociatedSample", previewSample, previewSampleViewData);
                   //Html.RenderPartial("NewAssociatedSample", previewSample, previewSampleViewData);
                }
            %>
            </span>
        </div>
        
        <script type="text/javascript">
            $(document).ready(function() {
                /* remove dropdown and add button if list of samples to add is empty */
                if ($('#<%= formId %>_add_div select[name=sample_id] option').length == 0) {
                    $('#<%= formId %>_add_div select').remove();
                    $('#<%= formId %>_add_div input.addSample').remove();
                    $('#<%= formId %>_preview').remove();
                    $('#<%= formId %>_add_div').before('<div class="addSampleBox noSamples noPrint">No available samples to add</div>');
                }

                $('#<%= formId %>_add_div input.addSample').click(function() {
                    $.post(
                        '<%= Url.Action("AddSample", "ChainOfCustody", new { id = coc.chain_of_custody_id }) %>',
                        { 'sample_id': $('#<%= formId %>_wrapper select[name=sample_id]').val() },
                        function(returnHtml) {
                            $('#<%= formId %>_wrapper').html(returnHtml);
                            return false;
                        }
                    )
                });

                $('#<%= formId %>_add_div select').bind('keyup', function() { this.change(); });

                $('#<%= formId %>_add_div select').change(function() {
                  //  $.get("../PreviewSample", { sample_id: $(this).val() }, function(data) { $('#<%= formId %>_preview').html(data); });
                    $.get("../NewPreviewSample", { sample_id: $(this).val() }, function (data) { $('#<%= formId %>_preview').html(data); });
                });
            });
        </script>

        
        <!-- ui-dialog -->
        <div id="<%= formId %>_dialog" class="modalPopup page" title="Sample Creation">
            <% 
                var dialogViewData = new ViewDataDictionary();
                dialogViewData["FormId"] = formId;
                var newSample = new Sample() { 
                    sample_type_id = coc.sample_type_id, 
                    is_travel_blank = (formId == "travel_sample_id").ToString() 
                };
                Html.RenderPartial("SampleForm", newSample, dialogViewData);
            %>
            <hr style="width: 500px; margin: 25px 10px 15px;" />
            <div style="width: 500px; font-size: 0.8em;"><b><i>Warning:</i></b> Creating a new sample will reload the CoC. If you have unsaved changes, cancel this sample creation and save your CoC first.</div>
        </div>
        
        <script type="text/javascript">
            $(function() {
                // Dialog
                $('#<%= formId %>_dialog').dialog({
                    autoOpen: false,
                    width: 560,
                    height: 425,
                    modal: true,
                    resizable: false,
                    draggable: false,
                    buttons: {
                        "Create": function() {
                            $(this).dialog("close");
                            $("#<%= formId %>_dialog form").attr('action', '<%= Url.Action("Create", new { controller="Sample" }) %>/' + $(this).dialog('option', 'coc_id'));
                            $("#<%= formId %>_dialog [name=date_received_from_lab]").val($("#<%= formId %>_date_received_from_lab").val());
                            $("#<%= formId %>_dialog form").submit();
                        },
                        "Cancel": function() {
                            $(this).dialog("close");
                        }
                    }
                })

                // Dialog Link
                $('#<%= formId %>_add_div input.createSample').click(function () {
                    <% if(lastTemp != "") { %>
                    $('#average_storage_temperature').val("<%=lastTemp %>");
                    <% } %>
                    <% if(lastDate != "") { %>
                    $('#<%= formId %>_date_received_from_lab').val("<%= lastDate %>");
                    <% } %>
                    $('#<%= formId %>_dialog').dialog('option', { coc_id: "<%= coc.id %>" });
                    $('#<%= formId %>_dialog').dialog('open');
                });
            });
        </script>

        <%  
                }
            }
        %>
		</div>
    </fieldset>