<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ShippingCompany>" %>

        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <label for="name">
                        Name:</label>
                </td>
                <td>
                    <%= Html.TextBox("name", Model.name, new { maxlength = "100" })%>
                    <%= Html.ValidationMessage("name", "*") %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="active">
                        Active:</label>
                </td>
                <td>
                    <%= Html.CheckBox("active") %>
                    <%= Html.ValidationMessage("active", "*") %>
                </td>
            </tr>
        </table>