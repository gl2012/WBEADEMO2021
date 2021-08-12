<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.PMData_View>>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	PM Data Export
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-model">PM Data Export</h2>
      <!--  <label id="err_id" class="labelFilter1 danger"><%// =(string)ViewData["err"] %></label>-->
        
	<div class="searchArea forcePrint">
		<form method="get"  >	
            
			 <% 
                 
                  Session["Selecteddate"] = (string)ViewData["SearchDate"];

                %>
			<div class="searchField">
				<label for="search">Date Start :</label>
				<span>
				    <% = Html.DatePicker("date_from") %>

              
                     <!--<input type="text" id="date_search" name="date_search" readonly />  -->
				      <%= Html.ValidationMessage("date_from", "Please Enter date", new { @class = "text-danger" })%>
				</span>
                <label for="search">Date End :</label><span>
                 <% = Html.DatePicker("date_to") %>
                <%= Html.ValidationMessage("date_to", "Please Enter date", new { @class = "text-danger" })%>
               </span>
                <label for="search">Search range by month :</label><span>
                 <% = Html.DropDownList("month_state") %>
               
               </span>
			</div>
			
			<!--<input type="submit" class="btnSearch" value="Search"  onclick="return errValidation();" />-->
            <input type="submit" class="btnSearch" value="Search" />
			<%= Html.Hidden("sort", ViewData["sort"])%>
         
		</form>
           <%if ((string)ViewData["SearchDate"] != "")
                { %>
           <!--  <a href="/PMData.aspx/ExportPMData">Download PMData <%// =ViewData["SearchDate"]%> CSV Fromat</a> -->
             <div><p> <a href="/PMData.aspx/ExportPMDataToExcel">Download WBEA PMData <% =ViewData["SearchDate"]%>  DRI Report Excel Format</a></p>
                </div>
            <%Session["SelectedFrom"]=(string)ViewData["From"];
                    Session["Selectedto"]=(string)ViewData["To"];
                    Session["Selectedmonth"] = (string)ViewData["SearchDate"];
                } %>
      
	</div>
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
    <script type="text/javascript">

        $(function () {
            $("#month_state").change(function () {
                var e = document.getElementById("month_state");
                if (e.options[e.selectedIndex].value == "Yes") {
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
                                var lastDate = new Date(cday.getFullYear(), cday.getMonth() + 2, 0).toISOString().split("T")[0];
                                document.getElementById("date_to").value = lastDate;
                            }
                           

                        }

                    } catch (err) {

                    }
                }
               
            });
        });


        function errValidation() {
         var errmessage = "";
         var boolerr = true;
        
         var datefrom = document.getElementById('date_from').value;
         var dateto = document.getElementById('date_to').value;
        // alert(Date.parse(datefrom));
        
         if (Date.parse(datefrom)==="NaN") { errmessage = errmessage + "From date is Mandatory Field/n"; alert(errmessage); boolerr = false; }


         if (Date.parse(dateto)==="NaN") { errmessage = errmessage + "To date is Mandatory Field/n"; boolerr = false; }

         if ((!Date.parse(dateto) === "NaN") && (Date.parse(datefrom) === "NaN")) {
             dtfrom = new Date(datefrom);
             dtto = new Date(dateto);
             var datediff = Date.UTC(dtto.getFullYear(), dtto.getMonth(), dtto.getDate()) - Date.UTC(dtfrom.getFullYear(), dtfrom.getMonth(), dtfrom.getDate());

             if (datediff < 0) {
                 errmessage = errmessage + "Search To date great than from date/n";
                 boolerr = false;
             }
         }
        

         
      


        /// var lblerr = document.getElementById('err_id')
            //Loop through al checked CheckBoxes in GridView.

            //if (boolerr) { document.getElementById("err_id").style.display = "none"; }
            //else { document.getElementById("err_id").style.display = "block"; }
            //lblerr.innerHTML =errmessage;
            //Display selected Row data in Alert Box.
           // alert(errmessage);

         return boolerr;
            
        }


        $(function () {

          //  function init() {
              //  document.getElementById("err_id").style.display = "none";
           // }
          //  window.onload = init();



            $("#date_from").datepicker({
            
               dateFormat:"yy-mm"
            });

           
         
              var d = new Date();
            var currMonth = d.getMonth();
               var currYear = d.getFullYear();
               var startDate = new Date(currYear, currMonth, 1);


               $("#datepicker").datepicker();
                $("#date_from").datepicker("setDate", startDate);
           


          
           
           


        });    
          //  var currentText = $(".selector").datepicker("option", "currentText");

            // Setter
          //  $(".selector").datepicker("option", "currentText", "Now");
       


</script>
</asp:Content>
