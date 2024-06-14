<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ApplicationEdit.aspx.cs" Inherits="TwoLocalGals.Protected.ApplicationEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG - Application</title>
    <link href="/Styles/ApplicationEdit.css" rel="stylesheet" type="text/css" />
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
    <div class="TitleInfo">
                Independent Contractor Application
    </div>
    <table class="Entry">
        <tr>
            <td class="EntryHeader">
                How did you find out about this job opportunity?
            </td>
            <td>
                <asp:TextBox ID="FindUs" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Have you ever worked with this company before?
            </td>
            <td>
                <asp:DropDownList ID="WorkedBefore" runat="server">
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                If so, when:
            </td>
            <td>
                <asp:TextBox ID="WorkedBeforeWhen" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                First Name:
            </td>
            <td>
                <asp:TextBox ID="FirstName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Last Name:
            </td>
            <td>
                <asp:TextBox ID="LastName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Address:
            </td>
            <td>
                <asp:TextBox ID="Address" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                City:
            </td>
            <td>
                <asp:TextBox ID="City" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                State:
            </td>
            <td>
                <asp:TextBox ID="State" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Zip:
            </td>
            <td>
                <asp:TextBox ID="Zip" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                How long at this address?
            </td>
            <td>
                <asp:TextBox ID="HowLongAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Best Phone:
            </td>
            <td>
                <asp:TextBox ID="BestPhone" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Alternate Phone:
            </td>
            <td>
                <asp:TextBox ID="AlternatePhone" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Email:
            </td>
            <td>
                <asp:TextBox ID="Email" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                SSN:
            </td>
            <td>
                <asp:TextBox ID="SSN" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Date Of Birth:
            </td>
            <td>
                <asp:TextBox ID="Birthday" runat="server" CssClass="entryTextBox"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderBirthday" TargetControlID="Birthday" runat="server">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Drivers License Number and State:
            </td>
            <td>
                <asp:TextBox ID="DriversLicense" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Drivers License Expiration Date:
            </td>
            <td>
                <asp:TextBox ID="DriversLicenseExpire" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Do you have a registered, reliable car to drive?
            </td>
            <td>
                <asp:DropDownList ID="HaveCar" runat="server">
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Days & Times available:
            </td>
            <td>
                <asp:TextBox ID="DaysAvailable" Width="250px" Rows="4" TextMode="multiline" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                High School:
            </td>
            <td>
                <asp:TextBox ID="HighSchool" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                College:
            </td>
            <td>
                <asp:TextBox ID="College" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Did you receive a diploma from high school or GED?
            </td>
            <td>
                <asp:DropDownList ID="HighSchoolDiploma" runat="server">
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>GED</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                HAVE YOU EVER BEEN CONVICTED OF A FELONY?
            </td>
            <td>
                <asp:DropDownList ID="Felony" runat="server">
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                If yes, explain number of conviction(s),<br />
                nature of offense(s) leading to conviction(s),<br />
                and how recently such offense(s)<br />
                was/were committed. On paper? Off paper?<br />
                PO Officer name and number:
            </td>
            <td>
                <asp:TextBox ID="FelonyDescription" Width="250px" Rows="8" TextMode="multiline" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="EntryTitle" colspan="2">
                Please list two references other than relatives
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name:
            </td>
            <td>
                <asp:TextBox ID="RefOneName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Position:
            </td>
            <td>
                <asp:TextBox ID="RefOnePosition" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Company:
            </td>
            <td>
                <asp:TextBox ID="RefOneCompany" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Adress, City, State:
            </td>
            <td>
                <asp:TextBox ID="RefOneAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="RefOnePhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name:
            </td>
            <td>
                <asp:TextBox ID="RefTwoName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Position:
            </td>
            <td>
                <asp:TextBox ID="RefTwoPosition" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Company:
            </td>
            <td>
                <asp:TextBox ID="RefTwoCompany" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Adress, City, State:
            </td>
            <td>
                <asp:TextBox ID="RefTwoAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="RefTwoPhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryTitle" colspan="2">
                Please list two relatives to contact in an emergency
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name:
            </td>
            <td>
                <asp:TextBox ID="RelOneName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Relationship:
            </td>
            <td>
                <asp:TextBox ID="RelOneRelation" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Adress, City, State:
            </td>
            <td>
                <asp:TextBox ID="RelOneAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="RelOnePhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name:
            </td>
            <td>
                <asp:TextBox ID="RelTwoName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Relationship:
            </td>
            <td>
                <asp:TextBox ID="RelTwoRelation" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Adress, City, State:
            </td>
            <td>
                <asp:TextBox ID="RelTwoAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="RelTwoPhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryTitle" colspan="2">
                **SKIP WORK EXPERIENCE SECTION IF RESUME IS ATTACHED**<br />
                Please list your work experience for the past three jobs<br />
                beginning with your most recent job.<br />
                If you were self-employed, give business name.
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of Employer:
            </td>
            <td>
                <asp:TextBox ID="EmpOneName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Address, City, State:
            </td>
            <td>
                <asp:TextBox ID="EmpOneAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="EmpOnePhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of last supervisor:
            </td>
            <td>
                <asp:TextBox ID="EmpOneSupervisor" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Start Date:
            </td>
            <td>
                <asp:TextBox ID="EmpOneStartDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                End Date:
            </td>
            <td>
                <asp:TextBox ID="EmpOneEndDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Your last job title:
            </td>
            <td>
                <asp:TextBox ID="EmpOneJobTitle" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Reason for leaving (be specific):
            </td>
            <td>
                <asp:TextBox ID="EmpOneReasonLeave" Width="250px" Rows="4" TextMode="multiline" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of Employer:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Address, City, State:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoPhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of last supervisor:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoSupervisor" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Start Date:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoStartDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                End Date:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoEndDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Your last job title:
            </td>
            <td>
                <asp:TextBox ID="EmpTwoJobTitle" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Reason for leaving (be specific):
            </td>
            <td>
                <asp:TextBox ID="EmpTwoReasonLeave" Width="250px" Rows="4" TextMode="multiline" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of Employer:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeName" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Address, City, State:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeAddress" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Phone Number:
            </td>
            <td>
                <asp:TextBox ID="EmpThreePhoneNumber" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Name of last supervisor:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeSupervisor" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Start Date:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeStartDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                End Date:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeEndDate" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Your last job title:
            </td>
            <td>
                <asp:TextBox ID="EmpThreeJobTitle" runat="server" CssClass="entryTextBox" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                Reason for leaving (be specific):
            </td>
            <td>
                <asp:TextBox ID="EmpThreeReasonLeave" Width="250px" Rows="4" TextMode="multiline" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="EntryHeader">
                May we contact your present employer?
            </td>
            <td>
                <asp:DropDownList ID="ContactEmployer" runat="server">
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
