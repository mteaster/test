<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: ViewData["Message"] %></h2>
    <div id="logindisplay"><% Html.RenderPartial("LogOnUserControl"); %></div> 
    <p>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sagittis tristique
        mauris id cursus. Donec lobortis posuere gravida. Vestibulum non augue consequat,
        gravida nunc vel, blandit magna. Etiam dignissim leo quis massa bibendum aliquet.
        Suspendisse et aliquet quam. Donec eget purus ac massa mattis lobortis. Donec mollis
        purus non neque suscipit, id congue magna scelerisque. Vestibulum id libero ac odio
        tempus tincidunt a sed purus. Nunc id molestie eros, eu tincidunt arcu. Class aptent
        taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vivamus
        varius vulputate magna, id rhoncus ante tincidunt vel. Cras faucibus hendrerit ornare.
        Donec vehicula eros vitae justo sollicitudin, a cursus nibh tincidunt. Nulla dignissim
        blandit dignissim. Duis interdum pretium libero, id ultrices sapien lacinia vel.
        In est tortor, semper ac feugiat vitae, vestibulum eget massa.
    </p>
</asp:Content>
