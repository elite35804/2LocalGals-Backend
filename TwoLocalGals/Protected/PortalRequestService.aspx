<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true" CodeBehind="PortalRequestService.aspx.cs" Inherits="TwoLocalGals.Protected.PortalRequestService" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Request Service</title>
        <link href="/Styles/PortalRequestServiceM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalRequestService.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server" />
    <div class="ErrorDiv">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div class="ContentHolder">
        <div class="Title">
            Request Cleaning Service
        </div>
        <table class="ServiceMenuTable">
            <tr>
                <td>
                    Date:
                </td>
                <td style="padding: 0px;">
                    <asp:TextBox ID="RequestDate" runat="server" CssClass="ServiceTextBox" />
                    <asp:CalendarExtender ID="CalendarExtenderRequestDate" TargetControlID="RequestDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Notes:
                </td>
                <td>
                    <asp:TextBox ID="Notes" CssClass="ServiceNotes" runat="server" Rows="2" TextMode="multiline" />
                </td>
            </tr>
            <tr>
                <td>
                    Time:
                </td>
                <td>
                    <asp:DropDownList CssClass="ServiceDropDown" ID="TimePrefix" runat="server">
                        <asp:ListItem>Flexible</asp:ListItem>
                        <asp:ListItem>After</asp:ListItem>
                        <asp:ListItem>Before</asp:ListItem>
                        <asp:ListItem>Done By</asp:ListItem>
                        <asp:ListItem>Must Be</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="ServiceDropDown" ID="TimeSuffix" runat="server">
                        <asp:ListItem>Any Time</asp:ListItem>
                        <asp:ListItem>9:00 AM</asp:ListItem>
                        <asp:ListItem>9:30 AM</asp:ListItem>
                        <asp:ListItem>10:00 AM</asp:ListItem>
                        <asp:ListItem>10:30 AM</asp:ListItem>
                        <asp:ListItem>11:00 AM</asp:ListItem>
                        <asp:ListItem>11:30 AM</asp:ListItem>
                        <asp:ListItem>12:00 PM</asp:ListItem>
                        <asp:ListItem>12:30 PM</asp:ListItem>
                        <asp:ListItem>1:00 PM</asp:ListItem>
                        <asp:ListItem>1:30 PM</asp:ListItem>
                        <asp:ListItem>2:00 PM</asp:ListItem>
                        <asp:ListItem>2:30 PM</asp:ListItem>
                        <asp:ListItem>3:00 PM</asp:ListItem>
                        <asp:ListItem>3:30 PM</asp:ListItem>
                        <asp:ListItem>4:00 PM</asp:ListItem>
                        <asp:ListItem>4:30 PM</asp:ListItem>
                        <asp:ListItem>5:00 PM</asp:ListItem>
                        <asp:ListItem>5:30 PM</asp:ListItem>
                        <asp:ListItem>6:00 PM</asp:ListItem>
                        <asp:ListItem>6:30 PM</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="SendRequestButton" CssClass="ServiceButton" OnClick="SendRequestClick" Text="Send Request" runat="server" />
                </td>
            </tr>
        </table>
        <div>
            <asp:Table ID="RequestTable" CssClass="DataTable" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100px">Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200px">Time</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Notes</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80px"></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>
