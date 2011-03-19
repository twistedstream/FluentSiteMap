<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<FluentSiteMap.Sample.Models.Product>" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <p>
        Name: <strong><%: Model.Name %></strong>
    </p>
    <p>
        Description: <strong><%: Model.Description %></strong>
    </p>
    <p>
        Price: <strong>$<%: Model.Price %></strong>
    </p>
</asp:Content>