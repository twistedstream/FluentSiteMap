<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul>
    <% Html.FluentSiteMap().SiteMapNode(SiteMapHelper.RootNode); %>
</ul>
