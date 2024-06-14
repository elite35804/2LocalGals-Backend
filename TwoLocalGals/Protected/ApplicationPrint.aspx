<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicationPrint.aspx.cs" Inherits="TwoLocalGals.Protected.ApplicationPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2LG Application Print</title>
    <link href="/Styles/ApplicationPrint.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
    <div class="Page">
        <div class="TitleInfo">Independent Contractor Application</div>
        <table class="Entry">
            <tr>
                <td class="EntrySection" colspan="2">
                    Personal Information
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    How did you find out about this job opportunity?
                </td>
                <td>
                    <asp:Label ID="FindUs" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Have you ever worked with this company before?
                </td>
                <td>
                    <asp:Label ID="WorkedBefore" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    If so, when:
                </td>
                <td>
                    <asp:Label ID="WorkedBeforeWhen" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    First Name:
                </td>
                <td>
                    <asp:Label ID="FirstName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Last Name:
                </td>
                <td>
                    <asp:Label ID="LastName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Address:
                </td>
                <td>
                    <asp:Label ID="Address" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    City:
                </td>
                <td>
                    <asp:Label ID="City" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    State:
                </td>
                <td>
                    <asp:Label ID="State" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Zip:
                </td>
                <td>
                    <asp:Label ID="Zip" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    How long at this address?
                </td>
                <td>
                    <asp:Label ID="HowLongAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Best Phone:
                </td>
                <td>
                    <asp:Label ID="BestPhone" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Alternate Phone:
                </td>
                <td>
                    <asp:Label ID="AlternatePhone" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Email:
                </td>
                <td>
                    <asp:Label ID="Email" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    SSN:
                </td>
                <td>
                    <asp:Label ID="SSN" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Date Of Birth:
                </td>
                <td>
                    <asp:Label ID="Birthday" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Drivers License Number and State:
                </td>
                <td>
                    <asp:Label ID="DriversLicense" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Drivers License Expiration Date:
                </td>
                <td>
                    <asp:Label ID="DriversLicenseExpire" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Do you have a registered, reliable car to drive?
                </td>
                <td>
                    <asp:Label ID="HaveCar" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Days & Times available:
                </td>
                <td>
                    <asp:Label ID="DaysAvailable" Width="250px" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    High School:
                </td>
                <td>
                    <asp:Label ID="HighSchool" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    College:
                </td>
                <td>
                    <asp:Label ID="College" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Did you receive a diploma from high school or GED?
                </td>
                <td>
                    <asp:Label ID="HighSchoolDiploma" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    HAVE YOU EVER BEEN CONVICTED OF A FELONY?
                </td>
                <td>
                    <asp:Label ID="Felony" runat="server" />
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
                    <asp:Label ID="FelonyDescription" Width="250px" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntrySection" colspan="2">
                    Referrals
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
                    <asp:Label ID="RefOneName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Position:
                </td>
                <td>
                    <asp:Label ID="RefOnePosition" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Company:
                </td>
                <td>
                    <asp:Label ID="RefOneCompany" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Adress, City, State:
                </td>
                <td>
                    <asp:Label ID="RefOneAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:Label ID="RefOnePhoneNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name:
                </td>
                <td>
                    <asp:Label ID="RefTwoName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Position:
                </td>
                <td>
                    <asp:Label ID="RefTwoPosition" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Company:
                </td>
                <td>
                    <asp:Label ID="RefTwoCompany" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Adress, City, State:
                </td>
                <td>
                    <asp:Label ID="RefTwoAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:Label ID="RefTwoPhoneNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntrySection" colspan="2">
                    Work Experience
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
                    <asp:Label ID="EmpOneName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Address, City, State:
                </td>
                <td>
                    <asp:Label ID="EmpOneAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:Label ID="EmpOnePhoneNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name of last supervisor:
                </td>
                <td>
                    <asp:Label ID="EmpOneSupervisor" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Start Date:
                </td>
                <td>
                    <asp:Label ID="EmpOneStartDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    End Date:
                </td>
                <td>
                    <asp:Label ID="EmpOneEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Your last job title:
                </td>
                <td>
                    <asp:Label ID="EmpOneJobTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Reason for leaving (be specific):
                </td>
                <td>
                    <asp:Label ID="EmpOneReasonLeave" Width="250px" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name of Employer:
                </td>
                <td>
                    <asp:Label ID="EmpTwoName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Address, City, State:
                </td>
                <td>
                    <asp:Label ID="EmpTwoAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:Label ID="EmpTwoPhoneNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name of last supervisor:
                </td>
                <td>
                    <asp:Label ID="EmpTwoSupervisor" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Start Date:
                </td>
                <td>
                    <asp:Label ID="EmpTwoStartDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    End Date:
                </td>
                <td>
                    <asp:Label ID="EmpTwoEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Your last job title:
                </td>
                <td>
                    <asp:Label ID="EmpTwoJobTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Reason for leaving (be specific):
                </td>
                <td>
                    <asp:Label ID="EmpTwoReasonLeave" Width="250px" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name of Employer:
                </td>
                <td>
                    <asp:Label ID="EmpThreeName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Address, City, State:
                </td>
                <td>
                    <asp:Label ID="EmpThreeAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:Label ID="EmpThreePhoneNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Name of last supervisor:
                </td>
                <td>
                    <asp:Label ID="EmpThreeSupervisor" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Start Date:
                </td>
                <td>
                    <asp:Label ID="EmpThreeStartDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    End Date:
                </td>
                <td>
                    <asp:Label ID="EmpThreeEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Your last job title:
                </td>
                <td>
                    <asp:Label ID="EmpThreeJobTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Reason for leaving (be specific):
                </td>
                <td>
                    <asp:Label ID="EmpThreeReasonLeave" Width="250px" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    May we contact your present employer?
                </td>
                <td>
                    <asp:Label ID="ContactEmployer" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
