<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Unavailable.aspx.cs" Inherits="TwoLocalGals.Protected.Unavailable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Unavailable</title>
    <link href="/Styles/Unavailable.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

        window.onload = function () {
            JsTimeChanged();
        }

        function JsAllDayButton() {
            document.getElementById('<%=StartTime.ClientID %>').value = "6:00 AM";
            document.getElementById('<%=EndTime.ClientID %>').value = "9:00 PM";
            JsTimeChanged();
        }

        function JsTimeChanged() {

            var startTime = document.getElementById('<%=StartTime.ClientID %>').value;
            var endTime = document.getElementById('<%=EndTime.ClientID %>').value;
            document.getElementById('<%=SaveButton.ClientID %>').disabled = (endTime == startTime);
        }

    </script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="NewButton" OnClick="NewClick" Text="New" runat="server" />
        <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete" runat="server" />
        <asp:Button ID="DoneButton" OnClick="DoneClick" Text="Done" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitleApp">
        <asp:Label ID="TitleLabel" runat="server" />
    </div>
    <asp:Panel ID="AppointmentPanel" runat="server" DefaultButton="SaveButton">
        <table class="AppDateTime">
            <tr>
                <td class="AppDateTimeHeader">
                    Date:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="DateRequested" runat="server" CssClass="entryTextBoxCenter" Width="170px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderDateRequested" TargetControlID="DateRequested" runat="server">
                    </asp:CalendarExtender>
                </td>
                <td class="AppDateTimeHeader">
                    Start Time:
                </td>
                <td class="AppDateTime">
                    <asp:DropDownList ID="StartTime" CssClass="entryDropDownList" Width="110px" runat="server" onchange="JsTimeChanged()">
                        <asp:ListItem>6:00 AM</asp:ListItem>
                        <asp:ListItem>6:15 AM</asp:ListItem>
                        <asp:ListItem>6:30 AM</asp:ListItem>
                        <asp:ListItem>6:45 AM</asp:ListItem>
                        <asp:ListItem>7:00 AM</asp:ListItem>
                        <asp:ListItem>7:15 AM</asp:ListItem>
                        <asp:ListItem>7:30 AM</asp:ListItem>
                        <asp:ListItem>7:45 AM</asp:ListItem>
                        <asp:ListItem>8:00 AM</asp:ListItem>
                        <asp:ListItem>8:15 AM</asp:ListItem>
                        <asp:ListItem>8:30 AM</asp:ListItem>
                        <asp:ListItem>8:45 AM</asp:ListItem>
                        <asp:ListItem>9:00 AM</asp:ListItem>
                        <asp:ListItem>9:15 AM</asp:ListItem>
                        <asp:ListItem>9:30 AM</asp:ListItem>
                        <asp:ListItem>9:45 AM</asp:ListItem>
                        <asp:ListItem>10:00 AM</asp:ListItem>
                        <asp:ListItem>10:15 AM</asp:ListItem>
                        <asp:ListItem>10:30 AM</asp:ListItem>
                        <asp:ListItem>10:45 AM</asp:ListItem>
                        <asp:ListItem>11:00 AM</asp:ListItem>
                        <asp:ListItem>11:15 AM</asp:ListItem>
                        <asp:ListItem>11:30 AM</asp:ListItem>
                        <asp:ListItem>11:45 AM</asp:ListItem>
                        <asp:ListItem>12:00 PM</asp:ListItem>
                        <asp:ListItem>12:15 PM</asp:ListItem>
                        <asp:ListItem>12:30 PM</asp:ListItem>
                        <asp:ListItem>12:45 PM</asp:ListItem>
                        <asp:ListItem>1:00 PM</asp:ListItem>
                        <asp:ListItem>1:15 PM</asp:ListItem>
                        <asp:ListItem>1:30 PM</asp:ListItem>
                        <asp:ListItem>1:45 PM</asp:ListItem>
                        <asp:ListItem>2:00 PM</asp:ListItem>
                        <asp:ListItem>2:15 PM</asp:ListItem>
                        <asp:ListItem>2:30 PM</asp:ListItem>
                        <asp:ListItem>2:45 PM</asp:ListItem>
                        <asp:ListItem>3:00 PM</asp:ListItem>
                        <asp:ListItem>3:15 PM</asp:ListItem>
                        <asp:ListItem>3:30 PM</asp:ListItem>
                        <asp:ListItem>3:45 PM</asp:ListItem>
                        <asp:ListItem>4:00 PM</asp:ListItem>
                        <asp:ListItem>4:15 PM</asp:ListItem>
                        <asp:ListItem>4:30 PM</asp:ListItem>
                        <asp:ListItem>4:45 PM</asp:ListItem>
                        <asp:ListItem>5:00 PM</asp:ListItem>
                        <asp:ListItem>5:15 PM</asp:ListItem>
                        <asp:ListItem>5:30 PM</asp:ListItem>
                        <asp:ListItem>5:45 PM</asp:ListItem>
                        <asp:ListItem>6:00 PM</asp:ListItem>
                        <asp:ListItem>6:15 PM</asp:ListItem>
                        <asp:ListItem>6:30 PM</asp:ListItem>
                        <asp:ListItem>6:45 PM</asp:ListItem>
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>7:15 PM</asp:ListItem>
                        <asp:ListItem>7:30 PM</asp:ListItem>
                        <asp:ListItem>7:45 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                        <asp:ListItem>8:15 PM</asp:ListItem>
                        <asp:ListItem>8:30 PM</asp:ListItem>
                        <asp:ListItem>8:45 PM</asp:ListItem>
                        <asp:ListItem>9:00 PM</asp:ListItem>
                        <asp:ListItem>9:15 PM</asp:ListItem>
                        <asp:ListItem>9:30 PM</asp:ListItem>
                        <asp:ListItem>9:45 PM</asp:ListItem>
                        <asp:ListItem>10:00 PM</asp:ListItem>
                        <asp:ListItem>10:15 PM</asp:ListItem>
                        <asp:ListItem>10:30 PM</asp:ListItem>
                        <asp:ListItem>10:45 PM</asp:ListItem>
                        <asp:ListItem>11:00 PM</asp:ListItem>
                        <asp:ListItem>11:15 PM</asp:ListItem>
                        <asp:ListItem>11:30 PM</asp:ListItem>
                        <asp:ListItem>11:45 PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="AppDateTimeHeader">
                    End Time:
                </td>
                <td class="AppDateTime">
                    <asp:DropDownList ID="EndTime" CssClass="entryDropDownList" Width="110px" runat="server" onchange="JsTimeChanged()">
                        <asp:ListItem>6:00 AM</asp:ListItem>
                        <asp:ListItem>6:15 AM</asp:ListItem>
                        <asp:ListItem>6:30 AM</asp:ListItem>
                        <asp:ListItem>6:45 AM</asp:ListItem>
                        <asp:ListItem>7:00 AM</asp:ListItem>
                        <asp:ListItem>7:15 AM</asp:ListItem>
                        <asp:ListItem>7:30 AM</asp:ListItem>
                        <asp:ListItem>7:45 AM</asp:ListItem>
                        <asp:ListItem>8:00 AM</asp:ListItem>
                        <asp:ListItem>8:15 AM</asp:ListItem>
                        <asp:ListItem>8:30 AM</asp:ListItem>
                        <asp:ListItem>8:45 AM</asp:ListItem>
                        <asp:ListItem>9:00 AM</asp:ListItem>
                        <asp:ListItem>9:15 AM</asp:ListItem>
                        <asp:ListItem>9:30 AM</asp:ListItem>
                        <asp:ListItem>9:45 AM</asp:ListItem>
                        <asp:ListItem>10:00 AM</asp:ListItem>
                        <asp:ListItem>10:15 AM</asp:ListItem>
                        <asp:ListItem>10:30 AM</asp:ListItem>
                        <asp:ListItem>10:45 AM</asp:ListItem>
                        <asp:ListItem>11:00 AM</asp:ListItem>
                        <asp:ListItem>11:15 AM</asp:ListItem>
                        <asp:ListItem>11:30 AM</asp:ListItem>
                        <asp:ListItem>11:45 AM</asp:ListItem>
                        <asp:ListItem>12:00 PM</asp:ListItem>
                        <asp:ListItem>12:15 PM</asp:ListItem>
                        <asp:ListItem>12:30 PM</asp:ListItem>
                        <asp:ListItem>12:45 PM</asp:ListItem>
                        <asp:ListItem>1:00 PM</asp:ListItem>
                        <asp:ListItem>1:15 PM</asp:ListItem>
                        <asp:ListItem>1:30 PM</asp:ListItem>
                        <asp:ListItem>1:45 PM</asp:ListItem>
                        <asp:ListItem>2:00 PM</asp:ListItem>
                        <asp:ListItem>2:15 PM</asp:ListItem>
                        <asp:ListItem>2:30 PM</asp:ListItem>
                        <asp:ListItem>2:45 PM</asp:ListItem>
                        <asp:ListItem>3:00 PM</asp:ListItem>
                        <asp:ListItem>3:15 PM</asp:ListItem>
                        <asp:ListItem>3:30 PM</asp:ListItem>
                        <asp:ListItem>3:45 PM</asp:ListItem>
                        <asp:ListItem>4:00 PM</asp:ListItem>
                        <asp:ListItem>4:15 PM</asp:ListItem>
                        <asp:ListItem>4:30 PM</asp:ListItem>
                        <asp:ListItem>4:45 PM</asp:ListItem>
                        <asp:ListItem>5:00 PM</asp:ListItem>
                        <asp:ListItem>5:15 PM</asp:ListItem>
                        <asp:ListItem>5:30 PM</asp:ListItem>
                        <asp:ListItem>5:45 PM</asp:ListItem>
                        <asp:ListItem>6:00 PM</asp:ListItem>
                        <asp:ListItem>6:15 PM</asp:ListItem>
                        <asp:ListItem>6:30 PM</asp:ListItem>
                        <asp:ListItem>6:45 PM</asp:ListItem>
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>7:15 PM</asp:ListItem>
                        <asp:ListItem>7:30 PM</asp:ListItem>
                        <asp:ListItem>7:45 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                        <asp:ListItem>8:15 PM</asp:ListItem>
                        <asp:ListItem>8:30 PM</asp:ListItem>
                        <asp:ListItem>8:45 PM</asp:ListItem>
                        <asp:ListItem>9:00 PM</asp:ListItem>
                        <asp:ListItem>9:15 PM</asp:ListItem>
                        <asp:ListItem>9:30 PM</asp:ListItem>
                        <asp:ListItem>9:45 PM</asp:ListItem>
                        <asp:ListItem>10:00 PM</asp:ListItem>
                        <asp:ListItem>10:15 PM</asp:ListItem>
                        <asp:ListItem>10:30 PM</asp:ListItem>
                        <asp:ListItem>10:45 PM</asp:ListItem>
                        <asp:ListItem>11:00 PM</asp:ListItem>
                        <asp:ListItem>11:15 PM</asp:ListItem>
                        <asp:ListItem>11:30 PM</asp:ListItem>
                        <asp:ListItem>11:45 PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <button type="button" onclick="JsAllDayButton()">
                        All Day</button>
                </td>
                <td class="AppDateTimeHeader">
                    Recurrence:
                </td>
                <td class="AppDateTime">
                    <asp:DropDownList ID="RecurrenceType" CssClass="entryDropDownList" Width="90px" onchange="JsFormValueChanged()" runat="server">
                        <asp:ListItem>None</asp:ListItem>
                        <asp:ListItem>Daily</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                        <asp:ListItem>Bi-Weekly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="UnCalendarMenu">
        <asp:Button ID="PrevButton" OnClick="PrevButtonClick" Text="<<<" runat="server" />
        <asp:Label ID="UnCalendarCaption" CssClass="UnCalendarCaption" runat="server">January 2015</asp:Label>
        <asp:Button ID="NextButton" OnClick="NextButtonClick" Text=">>>" runat="server" />
    </div>
    <asp:Table ID="UnCalendar" CssClass="UnCalendar" runat="server">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Sunday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Monday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Tuesday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Wednesday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Thursday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Friday</asp:TableHeaderCell>
            <asp:TableHeaderCell>Saturday</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
