<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Note>>" %>
<%@ Import Namespace="WBEADMS" %>
<%@ Import Namespace="WBEADMS.Views" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Notes Overview
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-notes">Notes Overview</h2>
	<!-- Search Area -->

	<div class="searchArea forcePrint">
		<form method="Post">		
			<div class="searchField">
				<label for="is_starred">Starred:</label>
				<%= Html.DropDownList("note_setting_id", "")%>
			</div>
			<div class="searchField">
				<label for="search">Date Created:</label>
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
            <span class="textArea"><%=Html.TextArea("setting_description", ViewData["setting_Description"])%></span>
        </p>
	<!-- Action Area + Top Paging-->
	<div id="actionArea">
		
	</div>
	
	<!-- Data table -->
    <table id="sortable-index">
        <tr>
            <th style="width:65px;" class="no-sort">Actions</th>
            <th style="min-width:80px;">Location</th>
             <th style="min-width:80px;">SampleType</th>
            <th class="no-sort">Parameter</th>
            <th style="min-width:120px;">Date&nbsp;Created</th>
            <th style="min-width:120px;">Date&nbsp;Occurred</th>
            <th>Author</th>
            <th class="no-sort">Note Body</th>
        </tr>
 <%if (Model != null) %>
   <%{ %>

     <% foreach (var item in Model)
         { %>
        <tr>
            <td class="tableActionLinks">
                <span><%= Html.NoteDetailsLink(item) %></span>
                <span><%= Html.NoteStarLink(item)%></span>
            </td>
            <td>
                <%= Html.ActionLink(item.Location.name, "Details", new { controller = "Location", id = item.Location.location_id })%>
            </td>
            <td><%=Html.Encode(item.SampleType) %> </td>
            <td>
                <% foreach (var parameter in item.parameters)
         { %><%=parameter.name%> <% } %>
            </td>
            <td>
                <%= Html.Encode(item.DateCreated)%>
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
<% }
         } %>
    </table>
    
    <br class="clear" />

     <script type="text/javascript">
        $(document).ready(function () {
            $('#note_setting_id').change(function () {
                alert($('#note_setting_id').val());

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
               

        });

         </script>

</asp:Content>