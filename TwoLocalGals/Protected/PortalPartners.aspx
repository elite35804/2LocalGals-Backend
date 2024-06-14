<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true" CodeBehind="PortalPartners.aspx.cs" Inherits="TwoLocalGals.Protected.PortalPartners" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Partners</title>
    <link href="/Styles/PortalPartnersM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalPartners.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="ErrorDiv" style="margin-top: 20px;">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div class="SuccessDiv" style="margin-top: 20px;">
        <asp:Label ID="SuccessLabel" runat="server" />
    </div>
    <div class="ContentHolder">
        <div class="Title">
            Our Partners
        </div>
        <div class="PartnerDiv">
            <a id="PartnersLink" href="/Partners.aspx" runat="server">Click here to view our partner refferals</a>
        </div>
        <div class="Title">
            Become a Partner
        </div>
        <p>Submit your company inforamation here and once approved we will add you to our list of partners.</p>
        <table class="PartnerTable">
            <tr>
                <td class="PartnerEntryHeader">
                    Company Name:
                </td>
                <td>
                    <asp:TextBox ID="CompanyName" CssClass="PartnerTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="PartnerEntryHeader">
                    Web Address:
                </td>
                <td>
                    <asp:TextBox ID="WebAddress" CssClass="PartnerTextBox" runat="server" />
                </td>
            </tr>
            <tr>
               <td class="PartnerEntryHeader">
                    Phone Number:
                </td>
                <td>
                    <asp:TextBox ID="PhoneNumber" CssClass="PartnerTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="PartnerEntryHeader">
                    Business Type:
                </td>
                <td>
                    <asp:DropDownList ID="BusinessType" CssClass="PartnerTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="PartnerEntryHeader">
                   Business Description:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="Description" runat="server" Rows="4" TextMode="multiline" CssClass="PartnerDescription" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="SubmitButton" CssClass="PartnerButton" OnClick="SubmitClick" Text="Sumbit" runat="server" />
                    <asp:Button ID="CancelButton" CssClass="PartnerButton" OnClick="CancelClick" Text="Cancel" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
