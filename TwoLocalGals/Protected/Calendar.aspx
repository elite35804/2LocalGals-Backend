<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Calendar.aspx.cs" Inherits="Nexus.Protected.WebFormCalendar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <asp:Panel ID="MenuPanel" runat="server" DefaultButton="ApplyButton">
        <asp:Table ID="MenuTable" CssClass="CalendarMenu" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="PrevButton" OnClick="PrevButtonClick" Text="<<<" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="WeekDate" runat="server" CssClass="entryTextBoxCenter" Width="170px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderWeekDate" TargetControlID="WeekDate" runat="server">
                    </asp:CalendarExtender>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="NextButton" OnClick="NextButtonClick" Text=">>>" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ContractorType" Width="150px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server" />
                </asp:TableCell>
                <asp:TableCell ID="FranchiseCell" />
                <asp:TableCell>
                    <asp:Button ID="ApplyButton" OnClick="ApplyButtonClick" Text="Apply" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Table ID="OutsideTableOne" CssClass="CalendarOutside" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel ID="Panel0" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="MondayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel1" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="TuesdayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel2" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="WednesdayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel3" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="ThursdayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel4" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="FridayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel5" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="SaturdayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel6" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="SundayTableOne" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="OutsideTableTwo" CssClass="CalendarOutside" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel ID="Panel7" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="MondayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel8" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="TuesdayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel9" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="WednesdayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel10" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="ThursdayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel11" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="FridayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel12" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="SaturdayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel13" CssClass="CalendarScrollPanel" runat="server" ScrollBars="Vertical">
                    <asp:Table ID="SundayTableTwo" CssClass="CalendarInside" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    © 2015 2LocalGalsHouseKeeping

    <script language="javascript" type="text/javascript">
        $('*').click(function () {
            var yPos = window.pageYOffset || document.documentElement.scrollTop;
            $.cookie("CalendarScrollPos", yPos);
        });

        window.onload = function WindowLoad() {
            if (JsGetParameter("DoScroll") == 'Y') {
                var yPos = $.cookie("CalendarScrollPos");
                window.scrollTo(0, yPos);
            }
        }
    </script>
</asp:Content>
