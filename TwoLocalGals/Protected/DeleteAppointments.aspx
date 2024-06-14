<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DeleteAppointments.aspx.cs" Inherits="Nexus.Protected.WebFormDeleteAppointments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server"></asp:ScriptManager>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitleApp">
        <asp:Label ID="TitleLabel" runat="server" />
    </div>
    <table class="AppDateTime">
        <tr>
            <td class="AppDateTimeHeader">
                Begin Date:
            </td>
            <td class="AppDateTime">
                <asp:TextBox ID="StartDate" runat="server" CssClass="entryTextBoxCenter" Width="170px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderStartDate" TargetControlID="StartDate"
                    runat="server">
                </asp:CalendarExtender>
            </td>
            <td class="AppDateTimeHeader">
                End Date:
            </td>
            <td class="AppDateTime">
                <asp:TextBox ID="EndDate" runat="server" CssClass="entryTextBoxCenter" Width="170px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderEndDate" TargetControlID="EndDate" runat="server">
                </asp:CalendarExtender>
            </td>
            <td class="AppDateTime">
                <asp:Button ID="DeleteRangeButton" OnClick="DeleteRangeButtonClick" Text="Delete Range"
                    runat="server" />
            </td>
            <td class="AppDateTime">
                <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
