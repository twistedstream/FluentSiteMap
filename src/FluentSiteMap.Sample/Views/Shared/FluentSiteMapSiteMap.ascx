<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul>
    <% Html.FluentSiteMap().SiteMapNode(FluentSiteMap.SiteMap.RootNode); %>
</ul>
