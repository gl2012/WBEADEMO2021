<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.ChainOfCustody>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	CreateChainOfCustody
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-coc-create">Create Chain Of Custody</h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

    <%= Html.BoxedValidationSummary("Create was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
			<table cellpadding="0" cellspacing="0" border="0" class="cocTable">
				<tr>
					<td><label for="sample_type_id">Sample Type:</label></td>
					<td>
						<%//= Html.DropDownListFor(x=>x.sample_type_id,(SelectList)ViewData["sample_type_id"], new { @id="sample_type_id"}) %>
                        <%= Html.DropDownList("sample_type_id") %>
						<%= Html.ValidationMessage("sample_type_id", "*") %>
					</td>
				</tr>
                <tr>
					<td><label for="schedule_id">Schedule:</label></td>
					<td id="Schedule">
						<%= Html.DropDownList("schedule_id") %>
						<%= Html.ValidationMessage("schedule_id", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="date_sampling_scheduled">Date Sampling Scheduled:</label> </td>
					<td>
						<%= Html.DatePicker("date_sampling_scheduled") %>
						<%= Html.ValidationMessage("date_sampling_scheduled", "*") %>
					</td>
				</tr>
			</table>

            <br /><input type="submit" id="btnSubmit"  value="Create" />
                  <input type="hidden" id="scheduleId" name="scheduleId" value="1" />
           
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

    <script type="text/javascript">  
    $(document).ready(function () {  
        $('#sample_type_id').change(function () {  
           // alert($('#sample_type_id').val());
            var ddl = $("#schedule_id")
           // var versionsProgress = $("#states-loading-progress");
           //  versionsProgress.show();
            $.ajax({  
                type: "GET",  
                url: "/ChainOfCustody.aspx/Getschedule",  
                data: { "stateId": $('#sample_type_id').val() },  
                datatype: "json",  
                traditional: true,  
                success: function (data) {  
                    var ddlschedule = "<select id='schedule_id'>";
                    ddlschedule = ddlschedule + '<option value="">--Select--</option>';
                    for (var i = 0; i < data.length; i++) {
                        ddlschedule = ddlschedule + '<option value=' + data[i].Value + '>' + data[i].Text + '</option>';
                    }
                    ddlschedule = ddlschedule + '</select>';
                    $('#Schedule').html(ddlschedule);  
                  
                }  
            });  
        }); 

    
    }); 
      
        $(function () {
            $("#btnSubmit").click(function () {
                var id = $("#schedule_id").val();
                //alert(id);
                $("#id").val(id);
                document.getElementById('scheduleId').value = id;
            });
        });      
           

       

</script>  

</asp:Content>