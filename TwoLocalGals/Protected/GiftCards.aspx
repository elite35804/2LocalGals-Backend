<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GiftCards.aspx.cs" Inherits="TwoLocalGals.Protected.GiftCards" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Gift Cards</title>
    <link href="/Styles/GiftCards.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="PrintButton" OnClick="PrintClick" Text="Print-Out" Enabled="false" runat="server" />
        <asp:Button ID="EmailButton" OnClick="EmailClick" Text="Email" Enabled="false" runat="server" />
        <asp:Button ID="VoidButton" OnClick="VoidClick" Text="Void" Enabled="false" runat="server" />
        <asp:Button ID="PurchaseButton" OnClick="PurchaseClick" Text="Purchase" Enabled="false" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitleApp" ID="TitleLabel" runat="server">
        Customer Gift Cards
    </div>
    <!-- Page SubTitle -->
    <div id="GiftCardSubTitle" class="GiftCardSubTitle" runat="server"> 
        Purchase New Gift Card
    </div>
    <div>
        <table class="GiftCards">
            <tr>
                <td>
                    Customer Name:
                </td>
                <td>
                    <asp:TextBox ID="CustomerName" CssClass="GiftCardsTextBox" runat= "server" onchange="JsFormValueChanged(this)" />
                </td>
                <td>
                    Customer Email:
                </td>
                <td>
                    <asp:TextBox ID="CustomerEmail" autocomplete="off" AutoCompleteType="Disabled" CssClass="GiftCardsTextBox" runat="server" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td>
                    Recipient Name:
                </td>
                <td>
                    <asp:TextBox ID="RecipientName" CssClass="GiftCardsTextBox" runat= "server" onchange="JsFormValueChanged(this)" />
                </td>
                <td>
                    Recipient Email:
                </td>
                <td>
                    <asp:TextBox ID="RecipientEmail" CssClass="GiftCardsTextBox" runat= "server" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td>
                    Payment:
                </td>
                <td>
                    <asp:DropDownList ID="PaymentType" CssClass="GiftCardsDropDown" onchange="JsFormValueChanged(this)" runat="server" />
                </td>
                <td>
                    Amount:
                </td>
                <td>
                    <asp:TextBox ID="Amount" CssClass="GiftCardsTextBox" runat= "server" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
        </table>
        <div style="margin: 10px auto 20px auto; text-align: center;">
            Attach Message: <asp:TextBox CssClass="GiftCardsTextBox" style="text-align: left;" Width="450px" ID="Message" runat="server" />
        </div>
    </div>
</asp:Content>
