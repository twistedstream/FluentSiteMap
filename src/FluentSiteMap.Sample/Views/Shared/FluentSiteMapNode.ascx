<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<% if (Model.Url == null) { %>
    <%: Model.Title%>
<% } else { %>
    <a href="<%: Model.Url %>"><%: Model.Title%></a>
<% } %>
