<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true"
    CodeBehind="PortalAddTip.aspx.cs" Inherits="TwoLocalGals.Protected.PortalAddTip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Add Tip</title>
    <link href="/Styles/PortalAddTipM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalAddTip.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ErrorDiv" style="margin-top: 20px;">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div style="margin: 20px auto 0px auto; font-size: 110%;">
        <asp:Label ID="AppointmentLabel" CssClass="AppointmentLabel" runat="server" />
        <div>
            <asp:TextBox ID="Tip" CssClass="TipTextBox" onchange="JsFormMoneyChanged(this)"
                runat="server" />
        </div>
        <div>
            <asp:Button ID="AddTipButton" CssClass="TipButton" OnClick="AddTipClick" Text="Add Tip"
                runat="server" />
            <asp:Button ID="CancelButton" CssClass="TipButton" OnClick="CancelClick" Text="Cancel"
                runat="server" />
        </div>
    </div>
</asp:Content>
