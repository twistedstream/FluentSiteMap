<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% foreach (var child in FluentSiteMap.SiteMap.CurrentNode.Children) { %>

        <p>
            <% Html.FluentSiteMap().Node(child); %>
            <% if (child.Metadata.ContainsKey("Price")) { %>
                ($<%: child.Metadata["Price"] %>)
            <% } %>
        </p>

    <% } %>
</asp:Content>
