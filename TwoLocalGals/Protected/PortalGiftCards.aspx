<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true" CodeBehind="PortalGiftCards.aspx.cs" Inherits="TwoLocalGals.Protected.PortalGiftCards" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Gift Cards</title>
        <link href="/Styles/PortalGiftCardsM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalGiftCards.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

    function JsMoneyChanged(field) {
        var money = parseFloat(field.value.replace("$", ""));
        field.value = JsFormatMoney(money);
    }

    function JsUsePointsChecked() {
        if (event.target.checked) {
            document.getElementById('<%=CreditCardRowZero.ClientID%>').style.display = 'none';
            document.getElementById('<%=CreditCardRowOne.ClientID%>').style.display = 'none';
            document.getElementById('<%=CreditCardRowTwo.ClientID%>').style.display = 'none';
            document.getElementById('<%=CreditCardRowThree.ClientID%>').style.display = 'none';
            document.getElementById('<%=CreditCardRowFour.ClientID%>').style.display = 'none';
        }
        else {
            document.getElementById('<%=CreditCardRowZero.ClientID%>').style.display = 'table-row';
            document.getElementById('<%=CreditCardRowOne.ClientID%>').style.display = 'table-row';
            document.getElementById('<%=CreditCardRowTwo.ClientID%>').style.display = 'table-row';
            document.getElementById('<%=CreditCardRowThree.ClientID%>').style.display = 'table-row';
            document.getElementById('<%=CreditCardRowFour.ClientID%>').style.display = 'table-row';
        }
    }

    </script>
    <div class="ErrorDiv" style="margin-top: 20px;">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div class="SuccessDiv" style="margin-top: 20px;">
        <asp:Label ID="SuccessLabel" runat="server" />
    </div>
    <div class="ContentHolder">
        <div class="Title">
            Redeem Gift Card</div>
        <div class="Redeem">
            Enter E-Gift Card code:
            <asp:TextBox ID="Redeem" CssClass="GiftCardTextBox" style="text-align: center; width: 150px;" runat="server" />
            <asp:Button ID="RedeemButton" CssClass="GiftCardButton" OnClick="RedeemClick" Text="Redeem" runat="server" />
        </div>
        <div class="Title" style="margin-top: 40px;">
            Buy Gift Card</div>
        <table class="GiftCardTable">
            <tr>
                <td>
                    Your Name:
                </td>
                <td>
                    <asp:TextBox ID="GiverName" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Your Email:
                </td>
                <td>
                    <asp:TextBox ID="BillingEmail" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Recipient Name:
                </td>
                <td>
                    <asp:TextBox ID="RecipientName" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Recipient Email:
                </td>
                <td>
                    <asp:TextBox ID="RecipientEmail" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Attach Message:
                </td>
                <td>
                    <asp:TextBox ID="Message" CssClass="GiftCardTextBox" Rows="2" TextMode="multiline" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Gift Card Amount:
                </td>
                <td>
                    <asp:TextBox ID="Amount" CssClass="GiftCardTextBox" style="text-align: center;" Width="120px" onchange="JsMoneyChanged(this)" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center; font-weight: bold; padding-top: 15px; padding-bottom: 5px; font-size:110%;">Billing Information</td>
            </tr>
            <tr>
                <td>
                    Use Points:
                </td>
                <td>
                    <asp:CheckBox ID="UsePoints"  OnClick="JsUsePointsChecked()" runat="server" />
                </td>
            </tr>
            <tr id="CreditCardRowZero" runat="server">
                <td>
                </td>
                <td>
                    <img src="/2LG_CCLogo.jpg" alt="None" />
                </td>
            </tr>
            <tr id="CreditCardRowOne" runat="server">
                <td>
                    Credit Card:
                </td>
                <td>
                    <asp:TextBox ID="CardNumber" CssClass="GiftCardTextBox" style="text-align:center;" runat="server" />
                </td>
            </tr>
            <tr id="CreditCardRowTwo" runat="server">
               <td>
                    Expiration:
                </td>
                <td>
                    <asp:DropDownList ID="ExpirationMonth" CssClass="GiftCardInput" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList> / 
                    <asp:TextBox ID="ExpirationYear" runat="server" Width="85px" MaxLength="4" CssClass="GiftCardInput" />
                    CVV Code
                    <asp:TextBox ID="CCVCode" runat="server" Width="65px" MaxLength="3" CssClass="GiftCardInput" />
                </td>
            </tr>
            <tr id="CreditCardRowThree" runat="server">
                <td>
                    Street Address:
                </td>
                <td>
                    <asp:TextBox ID="Address" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr id="CreditCardRowFour" runat="server">
                <td>
                    Zip Code:
                </td>
                <td>
                    <asp:TextBox ID="ZipCode" CssClass="GiftCardTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="PurchaseButton" CssClass="GiftCardButton" OnClick="PurchaseClick" Text="Purchase" runat="server" />
                    <asp:Button ID="CancelButton" CssClass="GiftCardButton" OnClick="CancelClick" Text="Cancel" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
