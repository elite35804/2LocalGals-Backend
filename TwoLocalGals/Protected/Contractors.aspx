<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Contractors.aspx.cs" Inherits="Nexus.Protected.Contractors" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        Contractor:
        <asp:DropDownList ID="ContractorList" class="chzn-select" Width="250px"
            runat="server" OnSelectedIndexChanged="ContractorChanged" AutoPostBack="true" />
        Only Active
        <asp:CheckBox ID="OnlyActive" Checked="true" runat="server" OnCheckedChanged="OnlyActiveChanged"
            AutoPostBack="true" />
        <asp:Button ID="NewButton" OnClick="NewClick" Text="New Contractor" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="ApplicationButton" OnClick="ApplicationClick" Text="View Application" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <fieldset class="Entry" style="width: 600px;">
        <legend>Contractor Information</legend>
        <table class="Entry">
            <tr>
                <td class="EntryHeader">
                    Franchise:
                </td>
                <td>
                    <asp:ListBox ID="FranchiseList" Width="252px" runat="server" class="chzn-select" SelectionMode="Multiple" data-placeholder="Choose a Franchise..." />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Contractor Type:
                </td>
                <td>
                    <asp:ListBox ID="ContractorType" Width="252px" runat="server" class="chzn-select" SelectionMode="Multiple" data-placeholder="Choose a Type..." />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    First Name:
                </td>
                <td>
                    <asp:TextBox ID="FirstName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Last Name:
                </td>
                <td>
                    <asp:TextBox ID="LastName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Business Name:
                </td>
                <td>
                    <asp:TextBox ID="BusinessName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Team Name:
                </td>
                <td>
                    <asp:TextBox ID="TeamName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Address:
                </td>
                <td>
                    <asp:TextBox ID="Address" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    City:
                </td>
                <td>
                    <asp:TextBox ID="City" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    State:
                </td>
                <td>
                    <asp:TextBox ID="State" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Zip:
                </td>
                <td>
                    <asp:TextBox ID="Zip" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Best Phone:
                </td>
                <td>
                    <asp:TextBox ID="BestPhone" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Alternate Phone:
                </td>
                <td>
                    <asp:TextBox ID="AlternatePhone" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Email:
                </td>
                <td>
                    <asp:TextBox ID="Email" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    SSN:
                </td>
                <td>
                    <asp:TextBox ID="SSN" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Start Time:
                </td>
                <td>
                    <asp:DropDownList ID="StartDay" CssClass="entryDropDownList" Width="252px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>6:00 AM</asp:ListItem>
                        <asp:ListItem>6:30 AM</asp:ListItem>
                        <asp:ListItem>7:00 AM</asp:ListItem>
                        <asp:ListItem>7:30 AM</asp:ListItem>
                        <asp:ListItem>8:00 AM</asp:ListItem>
                        <asp:ListItem>8:30 AM</asp:ListItem>
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
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>7:30 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    End Time:
                </td>
                <td>
                    <asp:DropDownList ID="EndDay" CssClass="entryDropDownList" Width="252px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>6:00 AM</asp:ListItem>
                        <asp:ListItem>6:30 AM</asp:ListItem>
                        <asp:ListItem>7:00 AM</asp:ListItem>
                        <asp:ListItem>7:30 AM</asp:ListItem>
                        <asp:ListItem>8:00 AM</asp:ListItem>
                        <asp:ListItem>8:30 AM</asp:ListItem>
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
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>7:30 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Hourly Rate:
                </td>
                <td>
                    <asp:TextBox ID="HourlyRate" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Service Split:
                </td>
                <td>
                    <asp:TextBox ID="ServiceSplit" runat="server" CssClass="entryTextBox" onchange="JsFormPercentChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Payment Type:
                </td>
                <td>
                    <asp:DropDownList ID="PaymentType" CssClass="entryDropDownList" Width="252px" runat="server"
                        onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Direct Deposit</asp:ListItem>
                        <asp:ListItem>Check</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Payment Day:
                </td>
                <td>
                    <asp:DropDownList ID="PaymentDay" CssClass="entryDropDownList" Width="252px" runat="server"
                        onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Monday</asp:ListItem>
                        <asp:ListItem>Tuesday</asp:ListItem>
                        <asp:ListItem>Wednesday</asp:ListItem>
                        <asp:ListItem>Thursday</asp:ListItem>
                        <asp:ListItem>Friday</asp:ListItem>
                        <asp:ListItem>Saturday</asp:ListItem>
                        <asp:ListItem>Sunday</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Hire Date:
                </td>
                <td>
                    <asp:TextBox ID="HireDate" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderHireDate" TargetControlID="HireDate" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Date Of Birth:
                </td>
                <td>
                    <asp:TextBox ID="Birthday" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderBirthday" TargetControlID="Birthday" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Waiver Date:
                </td>
                <td>
                    <asp:TextBox ID="WaiverDate" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderWaiverDate" TargetControlID="WaiverDate" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Waiver Updated:
                </td>
                <td>
                    <asp:TextBox ID="WaiverUpdateDate" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderWaiverUpdateDate" TargetControlID="WaiverUpdateDate" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
                        <tr>
                <td class="EntryHeader">
                    Insurance Date:
                </td>
                <td>
                    <asp:TextBox ID="InsuranceDate" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderInsuranceDate" TargetControlID="InsuranceDate" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Insurance Updated:
                </td>
                <td>
                    <asp:TextBox ID="InsuranceUpdateDate" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderInsuranceUpdateDate" TargetControlID="InsuranceUpdateDate" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Background Check:
                </td>
                <td>
                    <asp:TextBox ID="BackgroundCheck" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderBackgroundCheck" TargetControlID="BackgroundCheck" runat="server">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Notes:
                </td>
                <td>
                    <asp:TextBox ID="Notes" runat="server" Rows="6" TextMode="multiline" Width="248px"
                        CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Account Representative:
                </td>
                <td>
                    <asp:CheckBox ID="AccountRep" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Active:
                </td>
                <td>
                    <asp:CheckBox ID="Active" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Scheduled:
                </td>
                <td>
                    <asp:CheckBox ID="Scheduled" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Email Schedules:
                </td>
                <td>
                    <asp:CheckBox ID="SendSchedules" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Email Payroll:
                </td>
                <td>
                    <asp:CheckBox ID="SendPayroll" runat="server" />
                </td>
            </tr>
            <tr id="ApplicantRow" runat="server">
                <td class="EntryHeader">
                    Applicant:
                </td>
                <td>
                    <asp:CheckBox ID="Applicant" runat="server" />
                </td>
            </tr>

            
            <tr>
                <td class="EntryHeader">
                    Send Schedules By Email:
                </td>
                <td>
                    <asp:CheckBox ID="SendScheduleByEmail" runat="server" />

                </td>
            </tr>
        </table>
    </fieldset>
    <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete Contractor" runat="server" />
    © 2015 2LocalGalsHouseKeeping
    <script language="javascript" type="text/javascript">
        window.onload = function WindowLoad() {
            JsContractorTypeChanged();
        }
    </script>
</asp:Content>
