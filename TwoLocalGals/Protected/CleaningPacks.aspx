<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CleaningPacks.aspx.cs" Inherits="TwoLocalGals.Protected.CleaningPacks" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Cleaning Packs</title>
    <link href="/Styles/CleaningPacks.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

        window.onload = function () {
            JsCalcTotal();
        }

        function JsCalcMoneyChanged(field) {
            JsFormMoneyChanged(field);
            JsCalcTotal();
        }

        function JsCalcHoursChanged(field) {
            JsFormHoursChanged(field);
            JsCalcTotal();
        }

        function JsCalcPercentChanged(field) {
            JsFormPercentChanged(field);
            JsCalcTotal();
        }

        function JsCleaningPackChanged(field) {
            JsFormValueChanged(field);
            JsCalcTotal();
        }

        function JsCalcTotal() {
            var hours = parseFloat(document.getElementById('<%=HoursPerVisit.ClientID %>').value);
            var rate = parseFloat(document.getElementById('<%=HourlyRate.ClientID %>').value.replace("$", ""));
            var serviceFee = parseFloat(document.getElementById('<%=ServiceFee.ClientID %>').value.replace("$", ""));
            var visits = parseFloat(document.getElementById('<%=CleaningPack.ClientID %>').value);

            var discountPercent = 0;
            if (visits == 12) discountPercent = 16.666667;
            if (visits == 25) discountPercent = 20.0;
            if (visits == 38) discountPercent = 21.052631;
            if (visits == 52) discountPercent = 23.076923;
            if (visits == 67) discountPercent = 25.37313;

            var total = hours * rate;
            total += serviceFee;
            total *= visits;
            var savings = ((discountPercent / 100) * total);


            document.getElementById('<%=Info.ClientID %>').innerHTML = "<b>Original Price: </b>$" + total.toFixed(2) + ", <b>(%) Off: </b>" + discountPercent.toFixed(2) + ", <b>Savings: </b>$" + savings.toFixed(2) + ", <b>Points: </b>" + (total * 100).toFixed(0);
            document.getElementById('<%=Points.ClientID %>').value = isNaN(total) ? "" : (total * 100).toFixed(0);
            total -= savings;
            document.getElementById('<%=Total.ClientID %>').value = isNaN(total) ? "" : "$" + total.toFixed(2);
        }
    
    </script>

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
        Customer Cleaning Packs
    </div>
    <div id="CleaningPackSubTitle" class="CleaningPackSubTitle" runat="server"> 
        Purchase New Cleaning Pack
    </div>
    <div style="text-align:center; margin-top: 10px;">
        <span style="margin-right: 10px;">
            Transaction Type
            <asp:DropDownList ID="TransType" onchange="JsFormValueChanged(this)" runat="server">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Sale</asp:ListItem>
                <asp:ListItem>Return</asp:ListItem>
            </asp:DropDownList>
        </span>
        <span style="margin-right: 10px;">
            Payment
            <asp:DropDownList ID="PaymentType" onchange="JsFormValueChanged(this)" runat="server" />
        </span>
        <span style="margin-right: 10px;">
            Customer Email
            <asp:TextBox ID="Email" runat="server" autocomplete="off" AutoCompleteType="Disabled" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" Width="250px"></asp:TextBox>
        </span>
    </div>
    <div>
        <table class="CleaningPacks">
            <tr>
                <td>
                    Hourly Rate:
                </td>
                <td>
                    <asp:TextBox ID="HourlyRate" CssClass="CleaningPacksTextBox" runat= "server" onchange="JsCalcMoneyChanged(this)" />
                </td>
                <td>
                    Service Fee:
                </td>
                <td>
                    <asp:TextBox ID="ServiceFee" CssClass="CleaningPacksTextBox" runat= "server" onchange="JsCalcMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td>
                    Hours Per Visit:
                </td>
                <td>
                    <asp:TextBox ID="HoursPerVisit" CssClass="CleaningPacksTextBox" runat= "server" onchange="JsCalcHoursChanged(this)" />
                </td>
                <td>
                    Cleaning Pack:
                </td>
                <td>
                    <asp:DropDownList ID="CleaningPack" CssClass="CleaningPacksDropDown" runat="server" onchange="JsCleaningPackChanged(this)">
                        <asp:ListItem Value="12">10 Pack (2 Bonus)</asp:ListItem>
                        <asp:ListItem Value="25">20 Pack (5 Bonus)</asp:ListItem>
                        <asp:ListItem Value="38">30 Pack (8 Bonus)</asp:ListItem>
                        <asp:ListItem Value="52">40 Pack (12 Bonus)</asp:ListItem>
                        <asp:ListItem Value="67">50 Pack (17 Bonus)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Points:
                </td>
                <td>
                    <asp:TextBox ID="Points" CssClass="CleaningPacksTextBox" runat= "server" onchange="JsFormValueChanged(this)" />
                </td>
                <td>
                    Total:
                </td>
                <td>
                    <asp:TextBox ID="Total" CssClass="CleaningPacksTextBox" runat= "server" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center; color: Black;">
                    <asp:Label ID="Info" runat="server" />
                </td>
            </tr>
        </table>
        <div style="margin: 10px auto 20px auto; text-align: center;">
            Memo: <asp:TextBox CssClass="CleaningPacksTextBox" style="text-align: left;" Width="450px" ID="Memo" runat="server" />
        </div>
    </div>
</asp:Content>
