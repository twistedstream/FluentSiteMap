<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% foreach (var child in SiteMapHelper.CurrentNode.Children) { %>

        <p><% Html.FluentSiteMap().Node(child); %></p>

    <% } %>
</asp:Content>
