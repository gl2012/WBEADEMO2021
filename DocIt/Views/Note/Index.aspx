<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WBEADMS.Models.Note>>" %>
<%@ Import Namespace="WBEADMS.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Notes Overview
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title-icon icon-notes">Notes Overview</h2>
	<p class="noPrint">Would you like to <strong><%= Html.ActionButton("Create A New Note", "Create") %></strong>?</p>
    <% Html.RenderPartial("NoteTable", Model.ToArray<WBEADMS.Models.Note>()); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
</asp:Content>