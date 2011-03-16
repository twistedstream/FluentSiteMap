<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<ul>
    <% Html.RenderPartial(SiteMapHelper.SiteMapNodeView, SiteMapHelper.RootNode); %>
</ul>
