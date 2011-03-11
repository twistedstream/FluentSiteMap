﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<ul id="menu">
    <% foreach (var node in Model.Children) { %>
    
    <li><% Html.RenderPartial(SiteMapHelper.NodeView, node); %></li>

    <% } %>
</ul>

