<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% foreach (var node in SiteMapHelper.CurrentNode.Children) { %>

        <p><% Html.RenderPartial(SiteMapHelper.NodeView, node); %></p>

    <% } %>
</asp:Content>
