<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Account Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-account-edit">Edit Account Details</h2>
	
    <p>
        <%=Html.ActionLink("Back to My Account", "MyAccount") %>
    </p>

    <%= Html.BoxedValidationSummary("Edit was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Account Details</legend>
            <table><tr><td>
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td><label for="user_name">User Name:</label></td>
					<td>
						<%= Html.TextBox("user_name", Model.user_name, new { disabled = "disabled" })%>
						<%= Html.ValidationMessage("user_name", "*") %>
					</td>
				</tr>
				<tr>
					<td><label for="first_name">First Name:</label></td>
					<td>
						<%= Html.TextBox("first_name", Model.first_name) %>
						<%= Html.ValidationMessage("first_name", "*") %>					
					</td>
				</tr>
				<tr>
					<td><label for="last_name">Last Name:</label></td>
					<td>
						<%= Html.TextBox("last_name", Model.last_name) %>
						<%= Html.ValidationMessage("last_name", "*") %>					
					</td>
				</tr>
				<tr>
					<td><label for="email">Email:</label></td>
					<td>
						<%= Html.TextBox("email", Model.email) %>
						<%= Html.ValidationMessage("email", "*") %>					
					</td>
				</tr>
				<tr>
					<td><label for="phone_number">Phone Number:</label></td>
					<td>
                <%= Html.TextBox("phone_number", Model.phone_number) %>
                <%= Html.ValidationMessage("phone_number", "*") %>					
					</td>
				</tr>
			</table>
                </td><td>
               <table style="border:1px solid grey"><tr><td><span class="labelFilter success"><b> &nbsp Filter Setting&nbsp </b></span> </td></tr>
                   <tr><td>
                    
                 <table >
                    <tr><td>
                 SampleType Groups</td></tr>
                     <tr><td><input type="checkbox" id="select_all" onclick="checkboxFunc(1)"/>  Select All </td>
                         
                <tr><td colspan="2">
                 <div id="divPanel">
                 <ul id="parameter_list_ul" class="noBullets">

		          <%  foreach (var parameter in  (Dictionary<string, string>)ViewData["sampletypeList"]) { 
                          
                          %>
			            <li>
				            <input type="checkbox" class="checkbox" name="parameter_list"  id="parameter<%= parameter.Key %>" value="<%= parameter.Key.ToString() %>" <%= (((List<string>)ViewData["usersampletypeList"]).Contains(parameter.Key.ToString())) ?
                                    "checked=\"checked\" " : "" %>/>
				            <label style="display:inline" for="parameter<%= parameter.Key %>"><%= parameter.Value%></label>
			            </li>
		            <%  } %>

		            </ul>
                   </div> 
            </td>
            </table>
                  
                    </td>
                <td>
              <table >
                    <tr><td>
                 Locations <span class="labelFilter1 danger">Inactive</span> </td></tr>
                    <tr><td><input type="checkbox" id="select_all1" onclick="checkboxFunc(2)"/>  Select All </td>
                        
                <tr><td colspan="2">
               <div id="divPanel1">
                 <ul id="location_list_ul" class="noBullets">

		          <%  foreach (var p in  (Dictionary<int, string>)ViewData["locationTypeList"]) { 
                          
                          %>
			            <li>
				            <input type="checkbox" name="location_list"  id="parameter<%= p.Key %>" value="<%= p.Key.ToString() %>" <%= (((List<string>)ViewData["userLocationtypeList"]).Contains(p.Key.ToString())) ?
                                    "checked=\"checked\" " : "" %>/>
				            <%if (((List<string>)ViewData["locationtypeListInactive"]).Contains(p.Key.ToString()))
                                { %>
				            <label style="display:inline" class="labelFilter1 danger" for="parameter<%= p.Key %>"> <%= p.Value%></label>
                            <%}else{ %>
                             <label style="display:inline" for="parameter<%= p.Key %>"><%= p.Value%></label>
                           
                            
                            <%} %>
			            </li>
		            <%  } %>

		            </ul>
                  </div>  
            </td>
            </table>
                  
                    </td>


                   </tr></table></td></tr></table>
			<br /><input type="submit" value="Save" />
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to My Account", "MyAccount") %>
    </div>
    

</asp:Content>