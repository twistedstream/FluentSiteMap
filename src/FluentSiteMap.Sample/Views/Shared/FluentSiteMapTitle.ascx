<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<%: Model == null ? "{missing sitemap node}" : Model.Description %>

