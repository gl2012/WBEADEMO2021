<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Sample>>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Samples Index
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<!--<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css"  
rel="stylesheet" type="text/css" /> -->
   
<!--<script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"></script>  -->
<!--<link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"  
rel="stylesheet" type="text/css" /> --> 
<!--<script src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>  -->
  
   
   
  <!-- <script type="text/javascript" src="<%= Url.Content("~/Content/bootstrap3.0.3/jquery-2.1.3.min.js") %>"></script> 
 <script type="text/javascript" src="<%= Url.Content("~/Content/bootstrap3.0.3/bootstrap-3.3.2.min.js") %>"></script>-->
 
<!-- Include the plugin's CSS and JS: -->
 
  


<!-- <script type="text/javascript" src="<%= Url.Content("~/Content/bootstrap3.0.3/bootstrap-multiselect.js") %>"></script>-->
    <h2 class="title-icon icon-samples">Samples Index</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create A New Sample", "Create") %></strong>?</p>
    <div class="container-fluid">
	<!-- Search Area -->
	<!--<div class="searchArea"> -->
		<form method="get">
			
       
         <table border="0">
             <tr><td>
				<label for="sample_type_id">Sample Type:</label>
				<%= Html.DropDownList("sample_type_id", "")%></td>
		
				<td><label for="user_id">Prepared By:</label>
				<%= Html.DropDownList("user_id", "")%></td>
		
				<td><label for="assigned">Is Assigned:</label>
				<%= Html.DropDownList("assigned", "")%></td>
			
            	
				
           
			<td><%= Html.Hidden("sort", ViewData["sort"])%>
            <%= Html.Hidden("SelectedMediaNo")%>
               <%= Html.Hidden("SelectedLabSampleId")%>
			
                 <input type="submit" id="btnSubmit" class="btnSearch" value="Search" /> 	</td>	</tr>	
		
             
   
          <tr><td> <label for="assigned">Media Serial #:</label>
                <%=Html.TextBox("media_sn") %></td>
               
			<!-- <select id="media_sn_list" multiple   > </select>-->
          
          <td>  <%=Html.DropDownList("media_sn_list", new SelectList(Enumerable.Empty<SelectListItem>()), new { @class = "multiselect", @multiple = "multiple",@style = "display: none;"  }) %></td>
		
            <td>  <label for="lab">  LAB Sample Id:</label>
              <input id="lab_sample" type="text" /></td>
                 
             <td>  <%=Html.DropDownList("lab_sample_list", new SelectList(Enumerable.Empty<SelectListItem>()), new { @class = "multiselect", @multiple = "multiple" ,@style = "display: none;" }) %>
             </td> 
             </tr>

         
       
           </form >
        
	<!-- Action Area + Top Paging-->
  
 
    <div id="actionarea">
            
       <div class="buttonList">
			<%= Html.ActionButton("Create New", "Create") %>
		</div>
	</div>
    
	<!-- Data table -->
    <table id="sortable-index">
        
        <tr>
            <th class="no-sort" width="140">Actions</th>
            <th>Prepared By</th>
            <th>Sample <Br />Type</th><%-- The space after sample is no accident  --%>
            <th sort_name="wbea-id">WBEA Sample ID</th>
            <th sort_name="media-serial">Media Serial #</th>
            <th>Lab Sample ID</th>
            <th>Received From Lab</th>
            <th class="no-sort">Travel<br />Blank</th>
            <th class="no-sort">Assigned To</th>
            <th>Lab Results</th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionEdit"><%= Html.ActionButton("Edit", "Edit", new { id = item.sample_id })%></span>
                <span class="tableActionDetails"><%= Html.ActionButton("Details", "Details", new { id=item.sample_id })%></span>
            </td>
            <td>
                <%= item.PreparedBy %>
            </td>
            <td>
                <%= Html.Encode(item.SampleType.name) %>
            </td>
            <td>
                <%= Html.Encode(item.wbea_id) %>
            </td>
            <td>
                <%= Html.Encode(item.media_serial_number) %>
            </td>
            <td>            
                <%= Html.Encode(item.lab_sample_id) %>
            </td>
            <td>
                <%= Html.Encode(item.DateReceivedFromLab)%>
            </td>
            <td>
                <%= item.is_travel_blank.ToHumanBool() %>
            </td>
            <td>
                <%  int count = 0;
                    foreach (string cocID in item.ChainOfCustodyIDs) {
                        ChainOfCustody coc = ChainOfCustody.Load(cocID);
                        string linkText = "CoC: " + coc;
                        if(count > 0){
                            Response.Write("<Br />");
                        }
                        
                        count++;
                        %>
                        <%= Html.ActionLink(linkText, "Details", new { controller = "ChainOfCustody", id = coc.chain_of_custody_id })%>
                <% } %>
            </td>
            <td>
                <% if (item.SampleResult != null) { %>
                    <%= Html.ActionLink(item.SampleResult.ToString(), "Details", new {controller = "SampleResult", id = item.SampleResult.id}) %>
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>
</div>
	<br class="clear" />
    <div class="buttonList">
        <%= Html.ActionButton("Create New", "Create") %>
    </div>
     
    
    
    <script type="text/javascript">  
       


        $(document).ready(function () {
          
        $('#media_sn').change(function () {  
            //alert($('#media_sn').val());
            $('#media_sn_list').show();

           
           // var versionsProgress = $("#states-loading-progress");
           //  versionsProgress.show();
            $.ajax({  
                type: "GET",  
                url: "/sample.aspx/GetMediaSNList",  
                data: { "stateId": $('#media_sn').val() },  
                datatype: "json",  
                traditional: true,  
                success: function (response) {  
                  
                    if (response.length > 0) {


                        $('#media_sn_list').html('');
                        var options = '';
                       
                        for (var i = 0; i < response.length; i++) {
                            options += '<option value="' + response[i] + '">' + response[i] + '</option>';
                           
                        }
                        $('#media_sn_list').append(options);
                        $('#media_sn_list').multiselect('rebuild');

                    } 
                }  
            });  
        }); 
            $('#lab_sample').click(function () {
                //alert($('#lab_sample').val());
                $('#lab_sample_list').show();


                // var versionsProgress = $("#states-loading-progress");
                //  versionsProgress.show();
                $.ajax({
                    type: "GET",
                    url: "/sample.aspx/GetLabSampleList",
                    data: { "stateId": $('#lab_sample').val() },
                    datatype: "json",
                    traditional: true,
                    success: function (response) {

                        if (response.length > 0) {


                            $('#lab_sample_list').html('');
                            var options = '';

                            for (var i = 0; i < response.length; i++) {
                                options += '<option value="' + response[i] + '">' + response[i] + '</option>';
                              //  alert(response[i]);
                            }
                            $('#lab_sample_list').append(options);
                            $('#lab_sample_list').multiselect('rebuild');

                        } 
                    }
                });
            }); 
    
    }); 
      
        $(function () {
            $("#btnSubmit").click(function () {
                var x = document.getElementById("media_sn_list");
                var y = document.getElementById("lab_sample_list");
                var id = "";
                var labid = "";
                for (var i = 0; i < x.options.length; i++) {
                    if (x.options[i].selected == true) {
                        //alert(x.options[i].value);
                        id = id + "," + "'"+ x.options[i].value+"'"
                    }
                }
                //alert(id);
                document.getElementById('SelectedMediaNo').value = id;


                for (var i = 0; i < y.options.length; i++) {
                    if (y.options[i].selected == true) {
                       // alert(y.options[i].value);
                        labid = labid + "," + "'" + y.options[i].value + "'"
                    }
                }
             // alert(labid);
                document.getElementById('SelectedLabSampleId').value = labid;
            });
        });

       
    </script>  

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>