<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<% if (Model.Url == null) { %>
    <%: Model.Title%>
<% } else { %>
    <a href="<%: Model.Url %>"<% if (Model.Target != null) { %> target="<%: Model.Target %>"<% } %>><%: Model.Title%></a>
<% } %>
