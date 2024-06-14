<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PartnersModify.aspx.cs" Inherits="TwoLocalGals.Protected.PartnersModify" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="TopMenuButtons">
        Partner:
        <asp:DropDownList ID="PartnerList" class="chzn-select" Width="200px" runat="server" OnSelectedIndexChanged="PartnerChanged" AutoPostBack="true" />
        <asp:Button ID="NewButton" OnClick="NewClick" Text="New Partner" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <fieldset class="Entry" style="width: 600px;">
        <legend>Partner Information</legend>
        <table class="Entry">
            <tr>
                <td class="EntryHeader">
                    Approved:
                </td>
                <td>
                    <asp:CheckBox ID="Approved" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Business Type:
                </td>
                <td>
                    <asp:DropDownList ID="BusinessType" class="chzn-select" Width="250px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Company Name:
                </td>
                <td>
                    <asp:TextBox ID="CompanyName" Width="250px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Franchise:
                </td>
                <td>
                    <asp:ListBox ID="FranchiseList" class="chzn-select" data-placeholder="Choose a Franchise..." Width="252px" runat="server" SelectionMode="Multiple" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Web Address:
                </td>
                <td>
                    <asp:TextBox ID="WebAddress" Width="250px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:TextBox ID="PhoneNumber" Width="250px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Business Description:
                </td>
                <td>
                    <asp:TextBox ID="Description" runat="server" Rows="8" TextMode="multiline" Width="250px" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
