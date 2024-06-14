<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs" Inherits="Nexus.Protected.Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Error Label --> 
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <fieldset class="Entry" style="width: 600px;">
        <legend>Reports</legend>
        <asp:Table ID="FranchiseTable" CssClass="Entry" runat="server">
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader" ID="FranchiseTitleCell">
                    Franchise:
                </asp:TableCell>
                <asp:TableCell ID="FranchiseCell" />
            </asp:TableRow>
            <asp:TableRow ID="ContractorTypeRow" runat="server" style="display: none;">
                <asp:TableCell CssClass="EntryHeader" style="padding-top:12px; padding-bottom:12px;" ID="ContractorTypeTitleCell">
                    Services:
                </asp:TableCell>
                <asp:TableCell ID="ContractorTypeCell" style="padding-top:12px; padding-bottom:12px;">
                    <label id="HousekeepingLabel" runat="server"><asp:CheckBox ID="HousekeepingCheckbox" runat="server" />Housekeeping</label>
                    <label id="CarpetCleaningLabel" runat="server"><asp:CheckBox ID="CarpetCleaningCheckbox" runat="server" />Carpet Cleaning</label>
                    <label id="WindowWashingLabel" runat="server"><asp:CheckBox ID="WindowWashingCheckbox" runat="server" />Window Washing</label>
                    <label id="HomewatchLabel" runat="server"><asp:CheckBox ID="HomewatchCheckbox" runat="server" />Home Guard</label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="DatePickerTable" CssClass="Entry" runat="server">
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader">
                    Quick Dates:
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="QuickDates" class="chzn-select" Width="252px" runat="server"
                        OnSelectedIndexChanged="QuickDatesChanged" AutoPostBack="true">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Today</asp:ListItem>
                        <asp:ListItem>Tomorrow</asp:ListItem>
                        <asp:ListItem>Yesterday</asp:ListItem>
                        <asp:ListItem>Last Week</asp:ListItem>
                        <asp:ListItem>2 Weeks Ago</asp:ListItem>
                        <asp:ListItem>This Week</asp:ListItem>
                        <asp:ListItem>Next Week</asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader">
                    Start Date:
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="StartDate" runat="server" CssClass="entryTextBoxCenter" Width="250px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderStartDate" TargetControlID="StartDate"
                        runat="server">
                    </asp:CalendarExtender>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader">
                    End Date:
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="EndDate" runat="server" CssClass="entryTextBoxCenter" Width="250px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderEndDate" TargetControlID="EndDate" runat="server">
                    </asp:CalendarExtender>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="ReportTable" CssClass="Entry" Style="margin-top: 30px;" runat="server">
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader">
                    Report:
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ReportsList" class="chzn-select" Width="320px" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" CssClass="EntryCenter">
                    <asp:Button ID="ViewReport" Width="130px" OnClick="ViewReportClick" Text="View Report"
                        runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="ContractorsTable" CssClass="Entry" Style="margin-top: 30px;" runat="server">
            <asp:TableRow>
                <asp:TableCell CssClass="EntryHeader">
                    Contractors:
                </asp:TableCell>
                <asp:TableCell>
                    <asp:ListBox ID="ContractorList" class="chzn-select" data-placeholder="Select Some Contractors" Width="350px" runat="server" SelectionMode="Multiple" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" CssClass="EntryCenter">
                    <asp:Button ID="SendSchedules" Width="130px" OnClick="SendSchedulesClick" Text="Send Schedules" runat="server" />
                    <asp:Button ID="SendPayroll" Width="130px" OnClick="SendPayrollClick" Text="Send Payroll" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </fieldset>
    <% if (Nexus.Globals.GetUserAccess(this) > 2)
       { %>
    <fieldset class="Entry" style="width: 600px;">
        <legend>Other</legend>
        <asp:Button ID="PromotionsButton" Width="130px" Text="Bulk Emails" OnClick="SendPromotionsClick" runat="server" />
        <asp:Button ID="PartnersButton" Width="130px" Text="Edit Partners" OnClick="PartnersClick" runat="server" />
        <asp:Button ID="ExportCustomersButton" Width="130px" Text="Export Customers" OnClick="ExportCustomersClick" runat="server" />
        <br />
        <br />
        Test SMS: <asp:TextBox ID="TestSMSTextBox" runat="server" CssClass="entryTextBoxCenter" Width="250px" />
         <asp:Button ID="TestSMSButton" Width="130px" Text="Send" OnClick="TestSMSClick" runat="server" />
    </fieldset>
    <% } %>
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
