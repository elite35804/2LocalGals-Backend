<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Transaction.aspx.cs" Inherits="TwoLocalGals.Protected.Transaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        function JsFormValueChangedCalc(field) {
            JsFormValueChanged(field);
            var hours = parseFloat(document.getElementById('<%=HoursBilled.ClientID %>').value);
            var rate = parseFloat(document.getElementById('<%=HourlyRate.ClientID %>').value.replace("$", ""));
            var tips = parseFloat(document.getElementById('<%=Tips.ClientID %>').value.replace("$", ""));
            var serviceFee = parseFloat(document.getElementById('<%=ServiceFee.ClientID %>').value.replace("$", ""));
            var discountPercent = parseFloat(document.getElementById('<%=DiscountPercent.ClientID %>').value.replace("%", ""));
            var discountReferral = parseFloat(document.getElementById('<%=DiscountReferral.ClientID %>').value.replace("%", ""));
            var discountAmount = parseFloat(document.getElementById('<%=DiscountAmount.ClientID %>').value.replace("$", ""));
            var salesTax = parseFloat(document.getElementById('<%=SalesTax.ClientID %>').value.replace("%", ""));
            var appDate = Date.parse(document.getElementById('<%=DateApply.ClientID %>').value);

            var total = hours * rate;
            total += serviceFee;
            total -= discountAmount;
            total += tips;
            if (appDate < Date.parse("1/15/2021")) {
                total -= (((discountPercent + discountReferral) / 100) * ((hours * rate) + serviceFee));
            }
            else {
                total -= (((discountPercent + discountReferral) / 100) * (hours * rate));
            }
            total += ((salesTax / 100) * (total -= tips));

            document.getElementById('<%=TotalAmount.ClientID %>').value = isNaN(total) ? "" : "$" + total.toFixed(2);
        }

    </script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="PrintView" OnClick="PrintViewClick" Text="Print-Out" runat="server" />
        <asp:Button ID="SendEmailButton" OnClick="SendEmailClick" Text="Send Email" runat="server" />
        <asp:Button ID="VoidButton" OnClick="VoidClick" Text="Void" runat="server" />
        <asp:Button ID="AuthButton" OnClick="AuthClick" Text="Authorize" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Submit" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitleApp">
        <asp:Label ID="TitleLabel" runat="server">Unknown Transaction</asp:Label>
    </div>
    <div id="CustomerServiceFee" class="AppServiceFee" runat="server">
        Customer Service Fee:
    </div>
    <!-- Created By -->
    <div style="font-size: 1.1em; text-align: center; color: Black;">
        <asp:Label ID="CreatedByLabel" runat="server" />
    </div>
    <div style="text-align: center; margin-top: 10px;">
        <div class="TransactionSet">
            <div class="TransactionItem">
                Transaction Type
                <asp:DropDownList ID="TransType" onchange="JsFormValueChanged(this)" runat="server">
                    <asp:ListItem>Sale</asp:ListItem>
                    <asp:ListItem>Return</asp:ListItem>
                    <asp:ListItem>Invoice</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="TransactionItem">
                Transaction Date
                <asp:TextBox ID="DateApply" runat="server" CssClass="entryTextBoxCenter" onchange="JsFormValueChangedCalc(this)"
                    Width="170px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderAppDate" TargetControlID="DateApply" runat="server">
                </asp:CalendarExtender>
            </div>
            <div class="TransactionItem">
                Payment
                <asp:DropDownList ID="PaymentType" onchange="JsFormValueChanged(this)" runat="server" />
            </div>
        </div>
        <div style="margin-top: 5px">
            <div class="TransactionItem">
                Customer Email
                <asp:TextBox ID="Email" runat="server" autocomplete="off" AutoCompleteType="Disabled" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)"
                    Width="250px"></asp:TextBox>
            </div>
        </div>
        <fieldset class="TransactionField">
            <legend>Transaction Information</legend>
            <table class="TransactionTable">
                <tr>
                    <td>
                        Hours Billed
                    </td>
                    <td>
                        <asp:TextBox ID="HoursBilled" CssClass="entryTextBoxCenter" Width="100px" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            runat="server"  />
                    </td>
                    <td>
                        Hourly Rate
                    </td>
                    <td>
                        <asp:TextBox ID="HourlyRate" CssClass="entryTextBoxCenter" Width="100px" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Fee
                    </td>
                    <td>
                        <asp:TextBox ID="ServiceFee" CssClass="entryTextBoxCenter" Width="100px" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            runat="server" />
                    </td>
                    <td>
                        Tips
                    </td>
                    <td>
                        <asp:TextBox ID="Tips" CssClass="entryTextBoxCenter" Width="100px" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Discount $
                    </td>
                    <td>
                        <asp:TextBox ID="DiscountAmount" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                    <td>
                        Discount %
                    </td>
                    <td>
                        <asp:TextBox ID="DiscountPercent" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Referral %
                    </td>
                    <td>
                        <asp:TextBox ID="DiscountReferral" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                    <td>
                        Sales Tax %
                    </td>
                    <td>
                        <asp:TextBox ID="SalesTax" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="TableRowHeaderCC" runat="server">
                        Carpet Cleaning $
                    </td>
                    <td id="TableRowCC" runat="server">
                        <asp:TextBox ID="SubContractorCC" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                    <td id="TableRowHeaderWW" runat="server">
                        Window Washing $
                    </td>
                    <td id="TableRowWW" runat="server">
                        <asp:TextBox ID="SubContractorWW" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="TableRowHeaderHW" runat="server">
                        Home Guard $
                    </td>
                    <td id="TableRowHW" runat="server">
                        <asp:TextBox ID="SubContractorHW" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChangedCalc(this)"
                            Width="100px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:center; padding-top:10px;">
                        <b>Total</b>
                        <asp:TextBox ID="TotalAmount" CssClass="entryTextBoxCenter" ReadOnly="false" onchange="JsFormValueChanged(this)"
                            Width="100px" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="TransactionSet">
            Memo<span style="display: inline-block; vertical-align: middle; margin-left: 5px;">
                <asp:TextBox ID="Notes" runat="server" Rows="3" TextMode="multiline" onchange="JsFormValueChanged(this)"
                    Width="370px" />
            </span>
        </div>
    </div>
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
