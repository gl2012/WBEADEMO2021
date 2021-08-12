<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Note>>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Daily System Check
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-dsc">Daily System Check</h2>
    <p class="noPrint">
        Would you like to <strong id="performDSC"><%= Html.ActionLink("Perform A Daily Systems Check", "SelectDSCLocation") %></strong>?
    </p>
         
<% Html.RenderPartial("../Note/NoteTable", Model.ToArray<WBEADMS.Models.Note>()); %><%-- "../Note/NoteTable" is like a hack to access /Views/Note/NoteTable.ascx --%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>