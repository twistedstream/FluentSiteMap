<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<ul id="menu">
    <% foreach (var node in Model.Children.Where(n => !n.HiddenInMenu)) { %>
    
    <li><% Html.RenderPartial(SiteMapHelper.NodeView, node); %></li>

    <% } %>
</ul>

