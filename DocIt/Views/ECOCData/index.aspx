<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.ECOCData_View>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	ECOC Data Export
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-model">ECOC Data Export</h2>
        
	<div class="searchArea ">
		<form method="get"  >	
       
             
			 <% 
                 
                  Session["Selecteddate"] = (string)ViewData["SearchDate"];

                %>
            <table border="0">
            <tr><td>
			<div class="searchField">
				<label for="search">Date Start :</label>
				
				    <% = Html.DatePicker("date_from") %>
                     <%= Html.ValidationMessage("date_from", "Please Enter date", new { @class = "text-danger" })%>
              
                    
				
               
                <label for="search">Date End :</label>
              
                <% = Html.DatePicker("date_to") %>
                 <%= Html.ValidationMessage("date_to", "Please Enter date", new { @class = "text-danger" })%>
                <input type="checkbox" id="chkMonth" name="chkMonth" checked="" >Search period by Month
			</div>
			</td><td>
			<!--<input type="submit" class="btnSearch" value="Search"  onclick="return errValidation();" />-->
            <input type="submit" class="btnSearch" value="Search" />
            <input type="reset" class="btnSearch" value="Reset" />
			<%= Html.Hidden("sort", ViewData["sort"])%>
                </td></tr>
              <tr> <td colspan="2"><%if ((string)ViewData["SearchDate"] != "")
                { %>
           <!--  <a href="/PMData.aspx/ExportPMData">Download PMData <%// =ViewData["SearchDate"]%> CSV Fromat</a> -->
            <a href="/ECOCData.aspx/ExportECOCDataToExcel">Download WBEA ECOC <% =ViewData["SearchDate"]%> DRI Report Excel Format</a>
             
            <%Session["SelectedFrom"]=(string)ViewData["From"];
                    Session["Selectedto"]=(string)ViewData["To"];
                    Session["SelectedMonth"] = (string)ViewData["SearchDate"];
                } %> </td></tr>
                
            </table>
           
		</form>
       
	</div>
     
    
      <!--  <label id="err_id" class="labelFilter1 danger"><%// =(string)ViewData["err"] %></label>-->
        
    
      <div id="actionArea">
		
	</div>
    <table id="sortable-index">
        <tr>
             
            <th style="width:65px;"class="no-sort">SITE</th>
            <th style="min-width:60px;"class="no-sort">DATE</th>
            <th class="no-sort">SIZE</th>
            <th class="no-sort">TVOC</th>
            <th class="no-sort">TVOU</th>
            <th  class="no-sort">WBEAID</th>
            <th class="no-sort">TID</th>
             <th class="no-sort">WBEA &nbsp;NOTES</th>
        </tr>
     
         
     
         <% foreach (var item in Model)
             { %>
            
        <tr>
            
            <td>
               <%=  Html.Encode(item.SITE)%>
            </td>
            <td>
                <%=  Html.Encode(item.DATE)%>
            </td>
            <td>
               <%=  Html.Encode(item.Size)%>
            </td>
            <td>
               <%=  Html.Encode(item.TVOC)%>
            </td>
            <td>
               <%=  Html.Encode(item.TVOU)%>
            </td>
            <td>
                <%=  Html.Encode(item.WEBA_ID)%>
            </td>
            <td>
                <%= Html.Encode(item.TID)%>
            </td>
            <td>
                <%= Html.Encode(item.WEBA_NOTE)%>
            </td>
        </tr>
<% } %>
        
    </table>
   
 <script >
     $(function () {
         var test = localStorage.input === 'true' ? true : false;
         $('input').prop('checked', test || false);
     });

     $('input').on('change', function () {
         localStorage.input = $(this).is(':checked');
         console.log($(this).is(':checked'));
     });
   document.getElementById('chkMonth').onclick = function() {
    // access properties using this keyword
    if ( this.checked ) {
        try {
            var strdate = document.getElementById("date_from").value;

            if ($("#date_from").val() == "") {
                var cday = new Date();
                //alert(cday);
                var lastDate = new Date(cday.getFullYear(), cday.getMonth() + 1, 0).toISOString().split("T")[0];
                document.getElementById("date_to").value = lastDate;
                //   var firstDate = new Date(cday.getFullYear(), cday.getMonth() - 1, 1).toISOString().split("T")[0];
                var firstDate = new Date(cday.getFullYear(), cday.getMonth(), 1).toISOString().split("T")[0];
                document.getElementById("date_from").value = firstDate;
            }
            else {
                //  var stryear = strdate.substring(0, 4);
                //  var strmonth = strdate.substring(5, 7);
                //   var strday = strdate.substring(8, 10);
                //  var cday = new Date(stryear, strmonth, strday);
                var cday = new Date(strdate);
                //  var lastDate = new Date(cday.getFullYear(), cday.getMonth(), 0).toISOString().split("T")[0];
                var lastDate = new Date(cday.getFullYear(), cday.getMonth() + 1, 0).toISOString().split("T")[0];
                document.getElementById("date_to").value = lastDate;
              
                if (strdate.substring(8, 10) != "01") {
                    var firstDate = new Date(cday.getFullYear(), cday.getMonth(), 1).toISOString().split("T")[0];
                    document.getElementById("date_from").value = firstDate;
                    var lastDate = new Date(cday.getFullYear(), cday.getMonth() + 1, 0).toISOString().split("T")[0];
                    document.getElementById("date_to").value = lastDate;
                } else {
                    var lastDate = new Date(cday.getFullYear(), cday.getMonth()+2 , 0).toISOString().split("T")[0];
                    document.getElementById("date_to").value = lastDate;
                }


            }
           
        } catch (err) {
           
        }
        
    } else {
       
        
    }
  }
    

 </script>
     </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeaderContent">
    <style type="text/css">
        .auto-style1 {
            height: 57px;
        }
    </style>
</asp:Content>
