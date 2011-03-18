﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<ul id="menu">
    <% foreach (var child in Model.Children.Where(n => !n.HiddenInMenu)) { %>
    
    <li><% Html.FluentSiteMap().Node(child); %></li>

    <% } %>
</ul>

