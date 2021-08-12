<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ChainOfCustody>>" %>
<%@ Import Namespace="WBEADMS.Views" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Batch Shipping of COC
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2 class="title-icon icon-coc">Batch Shipping of COC</h2>

    <div class="searchArea noPrint" style="margin-top:0">
			<form method="get" >
			<div class="searchField">
				<label for="sample_type_id">Sample Type:</label>
				<%= Html.DropDownList("sample_type_id", "")%>
			</div>
			<div class="searchField">
				<label for="From">From:</label>
				<% = Html.DatePicker("date_from") %>
			</div>
            <div class="searchField">
				<label for="To">To:</label>
				<% = Html.DatePicker("date_to") %>
			</div>
			
               
        
         
			<label  id="checked_id" style="display: inline;" ></label>
            
			<input type="submit" class="btnSearch" value="Search" /> 
            <%= Html.Hidden("sort", ViewData["sort"])%>
          </form>
         &nbsp;&nbsp; &nbsp;&nbsp;
			 <input type="checkbox" id="select_all" name="select_all"  onclick="checkboxFunc(1)" /><span class="labelFilter1 success"> <b>  Select All  </b></span>   
              <input  id = "btnGet" type="button" class="btnSearch" value="Confirm Selected" />   
         <br />
		
        
     <!--   <a href="/" onclick="return OnIndexCall(this)">Create Batch Shipping</a>
    <form method="post" action="CreateBatchShipping" id="myform">
    <input type="hidden" name="id" id="id"> 
 
</form>-->
      
        </div>
       
	 
     
       <div id="actionArea">
        
		   <div id="createbatch" class="buttonList" >
			<% //= Html.ActionLink("Create Batch Shipping", "CreateBatchShipping" ) %>
            <a href="/ChainOfCustody.aspx/CreateBatchShipping" onclick="return cocIdList(this)">Create Batch Shipping</a>
		</div>
	</div>
   

    <table id="sortable-index">
        <tr>
            <th class="no-sort">Actions</th>
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
        </tr>

    <% foreach (var item in Model) { %>    
        <tr>
            <td class="tableActionLinks">
              <input type="checkbox" class="checkbox" name="parameter_list" id="parameter"/>
            </td>
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
                <% foreach (var sample in item.Samples) { %>
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
        </tr>
    <% } %>

    </table>

   
    

  <script type="text/javascript">
      function OnIndexCall(elem) {
          var id ;
          $("#sortable-index input[ id=parameter]:checked").each(function () {
              var row = $(this).closest("tr")[0];
              id += row.cells[1].innerHTML;
              id = id.trim();
            id += "#";
          });
          $("#id").val(id);
         
          $("#myform").submit();
          return false;
      }

      function cocIdList(elem) {
          var cocid="";
          $("#sortable-index input[id=parameter]:checked").each(function () {
              var row = $(this).closest("tr")[0];
              cocid += row.cells[1].innerHTML.trim();
              cocid = cocid.trim();
              cocid += ","
              cocid = cocid.trim();
          });
         
          $(elem).attr('href', $(elem).attr('href') + '/' + cocid );
      }
    $(function () {
        //Assign Click event to Button.
        function init() {
            document.getElementById("createbatch").style.display = "none"; 
        }
        window.onload = init();
       
        $("#btnGet").click(function () {
            var message = "";
            var label = document.getElementById('checked_id');
            //Loop through all checked CheckBoxes in GridView.
            $("#sortable-index input[id=parameter]:checked").each(function () {
                var row = $(this).closest("tr")[0];
                message += row.cells[1].innerHTML;
               
                message += ",";
            });
            if (message === "") { document.getElementById("createbatch").style.display = "none"; }
            else { document.getElementById("createbatch").style.display = "block"; }
           // label.innerHTML = message;
            
            //Display selected Row data in Alert Box.
           // alert(message);
           // sessionStorage.setItem('cocid', message);
            return false;
        });
    });
</script>
</asp:Content>