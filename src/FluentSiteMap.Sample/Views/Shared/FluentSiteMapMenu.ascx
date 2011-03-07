﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<ul id="menu">
    <% foreach (var node in Model.Children) { %>
    
    <li><a href="<%: Url.Encode(node.Url) %>"><%: node.Title %></a></li>

    <% } %>
</ul>

