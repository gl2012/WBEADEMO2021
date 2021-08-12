<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.Sample>" %>
<%@ Import Namespace="WBEADMS.Views.NoteHelpers" %>
       
            <tr>
             <% if (ViewData["Preview"] == null || (bool)ViewData["Preview"] == false)
                 {%>
                   <td>
                    <%} else {%>
                   <td style="background-color:#fbfcfe;"> Preview
                   <%} %>
                    <%= Html.ActionLink(Model.wbea_id, "Details", new { controller = "Sample", id = Model.id }) %>
                </td>
           
                
                <td>
                    <%= HtmlHelperExtensions.ToStringOrDefaultTo(Model.PreparedBy, "Unknown")%>
                </td>
         
               
                <td>
                    <%= Model.date_received_from_lab%>
                </td>
           
				
				<td>
				    <%= Model.average_storage_temperature%> <%= Model.AverageStorageTemperatureUnit%>
				</td>
			
                
                <td>
                    <%= Model.media_serial_number%>
                </td>
          
                <td>
                    <%= Model.lab_sample_id%>
                </td>
           
                 <td>
                    <% var notebody="";%>
                     <%foreach (var n in Model.Notes)
                        { %>
                     <% 
                         notebody = n.body + "\r\n <br />" + notebody; %>
                     <%} %>
                     <%=notebody %>
                </td>
            <% if (ViewData["CoC"] != null) { %>
          
                <td class="noPrint" >
                    <%  
                        var coc = (ChainOfCustody)ViewData["CoC"];
                        string cocStatusName = coc.Status.name;
                        string cocId = coc.chain_of_custody_id;

                        if (cocStatusName == "Opened" || cocStatusName == "Preparing" || cocStatusName == "Prepared" || cocStatusName == "Deploying" || cocStatusName == "Deployed" || coc.Status.name == "Retrieving"
                            || coc.Status.name == "Retrieved" || coc.Status.name == "Shipping") {
                            if (ViewData["Preview"] == null || (bool)ViewData["Preview"] == false) {
                    %>
                        <%= Html.Hidden("sample_id", Model.sample_id)%>
                     <%= Html.Hidden("coc_id", coc.chain_of_custody_id)%>
                  
                        <input class="secondaryButton" type="submit" id="remove" value="Remove" /></td>

                     <td>  <div class="buttonList"><%= Html.ActionButton("Add Note", "Create1", 
				 new { controller = "Note", parent_id = Model.sample_id, parent_type = 5,cocid=coc.chain_of_custody_id })%></div>
                    <% 
                            } 
                        }
                   %>
                </td>
            
            <% } %>
        </tr>