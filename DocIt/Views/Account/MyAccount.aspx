<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	My Account
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 
    <h2 class="title-icon icon-account">My Account Details</h2>
	
    <div class="buttonList">
		<ul>
		<li><%= Html.ActionButton("Edit My Details", "EditAccount") %></li>
		<li><%= Html.ActionLink("Change Password", "ChangePassword") %></li>
		</ul>
	</div>    
	
    <fieldset>
        <legend>Account Details</legend>
        <table >
        <tr> <td>
       
		<table>
			<tr>
				<td class="label">User Name:</td>
				<td><%= Html.Encode(Model.user_name) %></td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td><%= Html.Encode(Model.first_name) %></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td><%= Html.Encode(Model.last_name) %></td>
			</tr>
			<tr>
				<td class="label">Email:</td>
				<td><%= Html.Encode(Model.email) %></td>
			</tr>
			<tr>
				<td class="label">Phone Number:</td>
				<td><%= Html.Encode(Model.phone_number) %></td>
			</tr>
			<tr>
				<td class="label">Role:</td>
				<td><%= Model.Role.name %></td>
			</tr>
			<tr>
				<td class="label">Date Created:</td>
				<td><%= Html.Encode(Model.date_created) %></td>
			</tr>
		</table>
            </td>
            <td>
             <table style="border:1px solid grey;"><tr><td style="vertical-align:text-top"><span class="labelFilter success"><b> &nbsp Filter Setting&nbsp </b></span> </td><td></td></tr>
                   <tr><td>
                <table >
                      
   
                    <tr><td>
                 SampleType Group</td></tr>
                <tr><td>
                  <div id="divPanel">  
                 <ul id="parameter_list_ul" class="noBullets" >

		          <%  foreach (var parameter in  (Dictionary<int, string>)ViewData["sampletypeList"]) { 
                          
                          %>
			            <li>
				            <input type="checkbox" name="parameter_list" disabled="disabled" id="parameter<%= parameter.Key %>" value="<%= parameter.Value %>" <%= (((List<string>)ViewData["usersampletypeList"]).Contains(parameter.Key.ToString())) ?
                                    "checked=\"checked\" " : "" %>/>
				            <label style="display:inline" for="parameter<%= parameter.Key %>"><%= parameter.Value%></label>
			            </li>
		            <%  } %>

		            </ul>
                 </div>
                    </td></tr></table>
            </td>
            <td>
                <table >
                      
   
                 <tr><td>
				    Location <span class="labelFilter1 danger">Inactive</span>
                 </td></tr>
                <tr><td>
                 <div id="divPanel1">
                 <ul id="location_list_ul" class="noBullets" >

		          <%  foreach (var p in  (Dictionary<int, string>)ViewData["locationtypeList"]) { 
                          
                          %>
			            <li>
				            <input type="checkbox" name="Location_list" disabled="disabled" id="parameter<%= p.Key %>" value="<%= p.Value %>" <%= (((List<string>)ViewData["userlocationtypeList"]).Contains(p.Key.ToString())) ?
                                    "checked=\"checked\" " : "" %>/>
                            <%if (((List<string>)ViewData["locationtypeListInactive"]).Contains(p.Key.ToString()))
                                { %>
				            <span class="labelFilter1 danger"><label style="display:inline" for="parameter<%= p.Key %>">   <%= p.Value%></label></span>
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
    </fieldset>
    
    <div class="buttonList">
		<%= Html.ActionButton("Edit My Details", "EditAccount") %>
		<%= Html.ActionLink("Change Password", "ChangePassword") %>
	</div>    
</asp:Content>