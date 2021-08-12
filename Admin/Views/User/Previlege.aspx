<%@ Page  Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WBEADMS.Models.User>" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Edit Item <%= Model %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
       <h2 class="title-icon icon-items-edit">Edit <%= Model %></h2>
	
    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

    <% Html.RenderPartial("Form"); %>

    <p>
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>
