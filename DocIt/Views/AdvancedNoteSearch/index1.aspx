
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.AdvancedNoteSearch_View>>" %>
<%@ Import Namespace="WBEADMS" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	 Notes Advanced Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">	<!-- Search Area -->
 <h2 class="title-icon icon-samples">Notes Advanced Search</h2>

	<div class="searchArea forcePrint">
		<form method="Get" autocomplete="off">		
		
			<div class="searchField">
				<label for="is_starred">Note Setting Item:</label>
				<%= Html.DropDownList("note_setting_id", "")%>
			</div>
			<div class="searchField">
				<label for="search">Date Occurred:</label>
				<span>
				    <%= Html.DatePicker("date_from")%>
				    to
				    <%= Html.DatePicker("date_to")%>
				</span>
			</div>
		
			<input type="submit" class="btnSearch" value="Search" />
			<%= Html.Hidden("sort", ViewData["sort"])%>
		</form>
	</div>
     <p>
            <label>Description:</label>
          
         <%=Html.TextArea("setting_description", ViewData["setting_description"].ToString(), 1, 150, null )%>
          
        </p>
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
	
		</div>
	
	
	<!-- Data table -->
    <table id="sortable-index">
        <tr>
            <th style="width:65px;" class="no-sort">Actions</th>
            <th style="min-width:80px;">Location</th>
             <th class="no-sort">SampleType</th>
            <th class="no-sort">Parameter</th>
            <th style="min-width:120px;">Scheduled Date</th>
            <th style="min-width:120px;">Date&nbsp;Occurred</th>
            <th>Author</th>
            <th class="no-sort">Note Body</th>
        </tr>
   <% if(Model !=null)%>
   <%{     %>
   <% foreach (var item in Model) { %>
        <tr>
            <td class="tableActionLinks">
                <span class="tableActionDetails"><%= Html.ActionButton("Note", "Details", new {controller="Note", id = item.note_id })%></span> 
              <% if (item.GetCOCId().Count > 0) %>
                <%{ %>
                <span class="tableActionImageDetails"><%= Html.ActionButton("ChainofCustody", "Details", new { controller = "ChainofCustody", id = item.GetCOCId()[0] })%></span> 
                <%} %>
            </td>
            <td>
                <%= Html.ActionLink(item.Location.name, "Details", new { controller = "Location", id = item.Location.location_id })%>
            </td>
              <td><% if (item.SampleType != null) %>
                  <%{ %>
                <%= Html.Encode(item.SampleType.name) %>
                  <%}
                      else
                      { %>
                   <%= Html.Encode("") %>
                  <%} %>
            </td>
            <td>
               <% foreach(var i in item.FetchParameter()) { %><%= i.ToString()%> <% } %>

            </td>
            <td>
                <%if (item.GetScheduleDate().Count >0) %>
                <%{ %>
                   <%=Html.Encode(item.GetScheduleDate()[0].ToDateTimeFormat()) %>
                <%} %>
                <%else %>
                <%{ %>
                 <%=Html.Encode("")%>
                <%} %>
            </td>
            <td>
                <%= Html.Encode(item.DateOccurred)%>
            </td>
            <td>
                <%= Html.Encode(item.created_user)%>
            </td>
            <td>
                <%= Html.Encode(item.body)%>
            </td>
        </tr>
<% } }%>
    </table>

     <script type="text/javascript">
        $(document).ready(function () {
            $('#note_setting_id').change(function () {
              //  alert($('#note_setting_id').val());

                // var versionsProgress = $("#states-loading-progress");
                //  versionsProgress.show();
                $.ajax({
                    type: "GET",
                    url: "/AdvancedNoteSearch.aspx/GetDescription",
                    data: { "stateId": $('#note_setting_id').val() },
                    datatype: "json",
                    traditional: true,
                    success: function (data) {
                        var txtdescription = "";
                        var selectedId = $('#note_setting_id').val();
                        for (var i = 0; i < data.length; i++) {
                            if (selectedId == data[i].Value) {

                                txtdescription = data[i].Text
                            }
                        }

                        $('#setting_description').html(txtdescription);

                    }
                });
            });
                  $("#date_from").datepicker({ dateFormat: "yy-mm-dd" });

            $("#date_to").datepicker({ dateFormat: "yy-mm-dd" });

            $("#setting_description").resizable({});
            document.getElementById("setting_description").readOnly = "true";
        });
         </script>
</asp:Content>