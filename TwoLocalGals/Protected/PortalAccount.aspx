<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true"
    CodeBehind="PortalAccount.aspx.cs" Inherits="TwoLocalGals.Protected.PortalAccount" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Information</title>
     <link href="/Styles/PortalAccountM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalAccount.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

        function JsBillingSameAsLocation() {
            if (document.getElementById("EditButton").style.display == 'none') {
                if (event.target.checked) {
                    document.getElementById('<%=BillingAddressTable.ClientID%>').style.display = 'none';
                }
                else {
                    document.getElementById('<%=BillingAddressTable.ClientID%>').style.display = 'table';
                }
            }
            else {
                event.target.checked = (!event.target.checked);
            }
        }

        function JsEnableEdit() {
            document.getElementById("EditButton").style.display = 'none';
            document.getElementById('<%=CancelButton.ClientID%>').style.display = 'inline';
            document.getElementById('<%=SaveButton.ClientID%>').style.display = 'inline';

            document.getElementById('<%=BestPhone.ClientID%>').disabled = false;
            document.getElementById('<%=BestPhone.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BestPhone.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=AlternatePhoneOne.ClientID%>').disabled = false;
            document.getElementById('<%=AlternatePhoneOne.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=AlternatePhoneOne.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=AlternatePhoneTwo.ClientID%>').disabled = false;
            document.getElementById('<%=AlternatePhoneTwo.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=AlternatePhoneTwo.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=Email.ClientID%>').disabled = false;
            document.getElementById('<%=Email.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=Email.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=LocationAddress.ClientID%>').disabled = false;
            document.getElementById('<%=LocationAddress.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=LocationAddress.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=LocationCity.ClientID%>').disabled = false;
            document.getElementById('<%=LocationCity.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=LocationCity.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=LocationState.ClientID%>').disabled = false;
            document.getElementById('<%=LocationState.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=LocationState.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=LocationZipCode.ClientID%>').disabled = false;
            document.getElementById('<%=LocationZipCode.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=LocationZipCode.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=CardNumber.ClientID%>').disabled = false;

            document.getElementById('<%=ExpirationMonth.ClientID%>').disabled = false;
            document.getElementById('<%=ExpirationYear.ClientID%>').disabled = false;

            document.getElementById('<%=CCVCode.ClientID%>').disabled = false;

            document.getElementById('<%=BillingName.ClientID%>').disabled = false;
            document.getElementById('<%=BillingName.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BillingName.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=BillingAddress.ClientID%>').disabled = false;
            document.getElementById('<%=BillingAddress.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BillingAddress.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=BillingCity.ClientID%>').disabled = false;
            document.getElementById('<%=BillingCity.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BillingCity.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=BillingState.ClientID%>').disabled = false;
            document.getElementById('<%=BillingState.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BillingState.ClientID%>').style.padding = '2px 1px 2px 1px';

            document.getElementById('<%=BillingZipCode.ClientID%>').disabled = false;
            document.getElementById('<%=BillingZipCode.ClientID%>').style.border = '1px solid #AAA';
            document.getElementById('<%=BillingZipCode.ClientID%>').style.padding = '2px 1px 2px 1px';
        }

    </script>
    <div class="TopMenuButtons">
        <input id="EditButton" class="AccountButtons" type="button" value="Edit Information" onclick="JsEnableEdit()" />
        <asp:Button ID="CancelButton" CssClass="AccountButtons" OnClick="CancelClick" Text="Cancel" runat="server" style="display: none;" />
        <asp:Button ID="SaveButton" CssClass="AccountButtons" OnClick="SaveClick" Text="Save Changes" runat="server" style="display: none;" />
    </div>
    <div class="ErrorDiv">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div class="AccountContainer">
        <table class="AccountTable">
            <tr>
                <th colspan="2">Contact Information</th>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Business Name:
                </td>
                <td >
                    <asp:TextBox ID="BusinessName" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Company Contact:
                </td>
                <td >
                    <asp:TextBox ID="CompanyContact" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
             <tr>
                <td class="AccountEntryHeader">
                    First Name:
                </td>
                <td >
                    <asp:TextBox ID="FirstName" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
             <tr>
                <td class="AccountEntryHeader">
                    Last Name:
                </td>
                <td >
                    <asp:TextBox ID="LastName" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
             <tr>
                <td class="AccountEntryHeader">
                    Spouse Name:
                </td>
                <td >
                    <asp:TextBox ID="SpouseName" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Best Phone:
                </td>
                <td >
                    <asp:TextBox ID="BestPhone" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Alternate Phone 1:
                </td>
                <td >
                    <asp:TextBox ID="AlternatePhoneOne" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Alternate Phone 2:
                </td>
                <td >
                    <asp:TextBox ID="AlternatePhoneTwo" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Email:
                </td>
                <td >
                    <asp:TextBox ID="Email" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
        </table>
        <table class="AccountTable">
            <tr>
                <th colspan="2">Property Location</th>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Street Address:
                </td>
                <td >
                    <asp:TextBox ID="LocationAddress" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    City:
                </td>
                <td >
                    <asp:TextBox ID="LocationCity" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    State:
                </td>
                <td >
                    <asp:TextBox ID="LocationState" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Zip Code:
                </td>
                <td >
                    <asp:TextBox ID="LocationZipCode" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
        </table>
    </div>
    <div class="AccountContainer">
        <table class="AccountTable">
            <tr>
                <th colspan="2">Billing Information</th>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <img src="/2LG_CCLogo.jpg" alt="None" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Credit Card:
                </td>
                <td >
                    <asp:TextBox ID="CardNumber" runat="server" CssClass="AccountTextBox" disabled="true" style="text-align:center; color:Black; background-color:White; border: 1px solid #AAA;" />
                </td>
            </tr>
            <tr>
               <td class="AccountEntryHeader">
                    Expiration:
                </td>
                <td>
                    <asp:DropDownList ID="ExpirationMonth" CssClass="AccountInput" runat="server" onchange="JsFormValueChanged(this)" disabled="true" style="color:Black;">
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
                    <asp:TextBox ID="ExpirationYear" runat="server" Width="75px" MaxLength="4" CssClass="AccountInput" onchange="JsFormValueChanged(this)" disabled="true" style="color:Black; background-color:White; border: 1px solid #AAA;" />
                    CVV Code
                    <asp:TextBox ID="CCVCode" runat="server" Width="55px" MaxLength="3" CssClass="AccountInput" onchange="JsFormValueChanged(this)" disabled="true" style="color:Black; background-color:White; border: 1px solid #AAA;" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    Billing Address same as Location:
                    <asp:CheckBox ID="BillingSame" OnClick="JsBillingSameAsLocation()" runat="server"/>
                </td>
            </tr>
        </table>
        <table id="BillingAddressTable" class="AccountTable" runat="server">
            <tr>
                <td class="AccountEntryHeader">
                    Billing Name:
                </td>
                <td >
                    <asp:TextBox ID="BillingName" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Street Address:
                </td>
                <td >
                    <asp:TextBox ID="BillingAddress" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    City:
                </td>
                <td >
                    <asp:TextBox ID="BillingCity" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    State:
                </td>
                <td >
                    <asp:TextBox ID="BillingState" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
            <tr>
                <td class="AccountEntryHeader">
                    Zip Code:
                </td>
                <td >
                    <asp:TextBox ID="BillingZipCode" CssClass="AccountTextBox" onchange="JsFormValueChanged(this)" runat="server" disabled="true" style="border: 0; color:Black; background-color:White;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
