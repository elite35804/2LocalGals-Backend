<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ContractorInfo.aspx.cs" Inherits="TwoLocalGals.Protected.ContractorInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <fieldset class="Entry" style="width: 537px; height: 560px;">
        <legend>Contractor Information</legend>
        <table class="Entry">
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
                    Birthday:
                </td>
                <td>
                    <asp:TextBox ID="Birthday" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderBirthday" TargetControlID="Birthday" runat="server">
                    </asp:CalendarExtender>
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
                    Upload Picture:
                </td>
                <td>
        <asp:FileUpload ID="UploadPic" runat="server" text="Upload Image" />

                </td>
            </tr>
        </table>
    </fieldset>
    © 2015 2LocalGalsHouseKeeping
</asp:Content>

