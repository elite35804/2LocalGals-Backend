<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Appointments.aspx.cs"
    Inherits="Nexus.Protected.WebFormAppointments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Styles/Appointments.css" rel="stylesheet" type="text/css" />
    <style>
        .galleryImage {
            max-width: 75px;
            width: 75px;
            padding: 5px 0px 0px 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script language="javascript" type="text/javascript">

        var oldStartTimeArray = [];

        window.onload = function () {
            var needsSplit = false;
            for (var i = 0; i < 10; i++) {
                oldStartTimeArray[i] = JsStringToTime(document.getElementById("ContractorStartTime_" + i).value);
                if (JsContractorChanged(i))
                    needsSplit = true;
                JsContractorRecurrenceTypeChanged(i);
            }

            if (needsSplit) {
                JsCustomerServiceFeeChanged(null);
                JsCustomerTipsChanged(null);
                JsContractorHoursChanged(null);
            }

            JsCalcTotal();
        }

        function JsAppDateChanged(field) {
            JsFormValueChanged(field);
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

        function JsCalcPercentNoRoundChanged(field) {
            JsFormPercentNoRoundChanged(field);
            JsCalcTotal();
        }

        function JsContractorCount() {
            var contractorCount = 0;
            for (var i = 0; i < 10; i++) {
                if (document.getElementById("ContractorRow_" + i).style.display == "table-row") {
                    contractorCount++;
                }
            }
            return contractorCount;
        }

        function JsHosekeepingContractorCount() {
            var contractorCount = 0;
            for (var i = 0; i < 10; i++) {
                if (document.getElementById("ContractorRow_" + i).style.display == "table-row") {
                    if (document.getElementById("ContractorType_" + i).value == "1") {
                        contractorCount++;
                    }
                }
            }
            return contractorCount;
        }

        function JsContractorChanged(i) {
            var ret = false;

            var newConType = parseInt(document.getElementById("Contractor_" + i).value.split("|")[1]);
            var curConType = parseInt(document.getElementById("ContractorType_" + i).value);
            var appID = parseInt(document.getElementById("ContractorAppID_" + i).value);
            if (isNaN(newConType)) newConType = 1;

            //window.alert("Curr: " + curConType + " New: " + newConType);

            document.getElementById("ContractorType_" + i).value = newConType.toString();

            if (newConType > 1 && curConType <= 1) {
                //Other Contractor Type
                document.getElementById("ContractorHours_" + i).value = "0.00";
                document.getElementById("ContractorHours_" + i).style.display = "none";

                if (curConType == 1) {
                    document.getElementById("ContractorServiceFee_" + i).value = "$0.00";
                    JsFormValueChanged(document.getElementById("ContractorServiceFee_" + i));
                    document.getElementById("ContractorTips_" + i).value = "$0.00";
                    JsFormValueChanged(document.getElementById("ContractorTips_" + i));

                    JsCustomerServiceFeeChanged(null);
                    JsCustomerTipsChanged(null);
                    JsContractorHoursChanged(null);
                    JsCalcTotal();
                    ret = true;
                }

                document.getElementById("ContractorRow_" + i).style.backgroundColor = "#FFB870";
                document.getElementById("ContractorRowExtra_" + i).style.backgroundColor = "#FFB870";
            }
            if (newConType == 1 && (curConType > 1 || appID == 0)) {
                //Housekeeping Contractor Type
                document.getElementById("ContractorHours_" + i).style.display = "inline";

                JsCustomerServiceFeeChanged(null);
                JsCustomerTipsChanged(null);
                JsContractorTimeChanged(i);
                ret = true;

                if ((i % 2) == 0) {
                    document.getElementById("ContractorRow_" + i).style.backgroundColor = "#B2E6FF";
                    document.getElementById("ContractorRowExtra_" + i).style.backgroundColor = "#B2E6FF";
                } else {
                    document.getElementById("ContractorRow_" + i).style.backgroundColor = "#E8F7FF";
                    document.getElementById("ContractorRowExtra_" + i).style.backgroundColor = "#E8F7FF";
                }
            }

            return ret;
        }

        function JsContractorTimeChanged(i) {
            var startTime = JsStringToTime(document.getElementById("ContractorStartTime_" + i).value);
            var endTime = JsStringToTime(document.getElementById("ContractorEndTime_" + i).value);
            var oldTime = oldStartTimeArray[i];

            //Keep Start and End Times in Sync
            if (oldTime.getTime() != startTime.getTime()) {
                endTime = new Date(startTime.getTime() + (endTime - oldTime));
                document.getElementById("ContractorEndTime_" + i).value = JsTimeToString(endTime);
                oldStartTimeArray[i] = startTime;
            }

            if (document.getElementById("ContractorType_" + i).value == "1") {
                //Calculate Hours Worked and Billed
                var hoursDiff = (endTime - startTime) / 1000 / 60 / 60;
                if (hoursDiff < 0) hoursDiff = 0;
                document.getElementById("ContractorHours_" + i).value = hoursDiff;
                JsContractorHoursChanged(document.getElementById("ContractorHours_" + i));
            }

            JsFormValueChanged(document.getElementById("ContractorStartTime_" + i));
            JsFormValueChanged(document.getElementById("ContractorEndTime_" + i));
        }

        function JsContractorHoursChanged(field) {
            var hoursBilled = 0;
            var contractorCount = JsContractorCount();

            for (var i = 0; i < contractorCount; i++) {
                if (document.getElementById("ContractorType_" + i).value == "1") {
                    hoursBilled += parseFloat(document.getElementById("ContractorHours_" + i).value);
                }
            }

            if (field != null) {
                JsFormHoursChanged(field);
            }
            document.getElementById('<%=HoursBilled.ClientID%>').value = hoursBilled.toFixed(2);
            JsCalcHoursChanged(document.getElementById('<%=HoursBilled.ClientID%>'));
        }

        function JsCustomerServiceFeeChanged(field) {
            var serviceFee = parseFloat(document.getElementById('<%=ServiceFee.ClientID%>').value.replace("$", ""));
            var contractorCount = JsContractorCount();
            var housekeepingCount = JsHosekeepingContractorCount();

            //window.alert("serviceFee: " + serviceFee + " housekeepingCount: " + housekeepingCount);

            if (housekeepingCount == 0) {
                document.getElementById('<%=ServiceFee.ClientID%>').value = "$0.00";
            }
            else {
                for (var i = 0; i < contractorCount; i++) {
                    if (document.getElementById("ContractorType_" + i).value == "1") {
                        document.getElementById("ContractorServiceFee_" + i).value = JsFormatMoney(serviceFee / housekeepingCount);
                        JsFormValueChanged(document.getElementById("ContractorServiceFee_" + i));
                    }
                }
            }

            if (field != null) {
                JsCalcMoneyChanged(field);
            }
        }

        function JsContractorServiceFeeChanged(i) {
            if (document.getElementById("ContractorType_" + i).value == "1") {
                var serviceFee = 0;
                var contractorCount = JsContractorCount();
                for (var j = 0; j < contractorCount; j++) {
                    if (document.getElementById("ContractorType_" + j).value == "1") {
                        serviceFee += parseFloat(document.getElementById("ContractorServiceFee_" + j).value.replace("$", ""));
                    }
                }
                document.getElementById('<%=ServiceFee.ClientID%>').value = JsFormatMoney(serviceFee);
                JsFormValueChanged(document.getElementById('<%=ServiceFee.ClientID%>'));
            }
            JsCalcMoneyChanged(document.getElementById("ContractorServiceFee_" + i));
        }

        function JsCustomerTipsChanged(field) {
            var tips = parseFloat(document.getElementById('<%=Tips.ClientID%>').value.replace("$", ""));
            var contractorCount = JsContractorCount();
            var housekeeperCount = JsHosekeepingContractorCount();

            for (var i = 0; i < contractorCount; i++) {
                if (document.getElementById("ContractorType_" + i).value != "1") {
                    tips -= parseFloat(document.getElementById("ContractorTips_" + i).value.replace("$", ""));
                }
            }
            for (var i = 0; i < contractorCount; i++) {
                if (document.getElementById("ContractorType_" + i).value == "1") {
                    document.getElementById("ContractorTips_" + i).value = JsFormatMoney(tips / housekeeperCount);
                    JsFormValueChanged(document.getElementById("ContractorTips_" + i));
                }
            }
            if (field != null) {
                JsCalcMoneyChanged(field);
            }
        }

        function JsContractorTipsChanged(field) {
            var tips = 0;
            var contractorCount = JsContractorCount();
            for (var i = 0; i < contractorCount; i++) {
                tips += parseFloat(document.getElementById("ContractorTips_" + i).value.replace("$", ""));
            }
            document.getElementById('<%=Tips.ClientID%>').value = JsFormatMoney(tips);
            JsFormValueChanged(document.getElementById('<%=Tips.ClientID%>'));
            JsCalcMoneyChanged(field);
        }

        function JsCalcTotal() {
            var hours = parseFloat(document.getElementById('<%=HoursBilled.ClientID %>').value);
            var rate = parseFloat(document.getElementById('<%=HourlyRate.ClientID %>').value.replace("$", ""));
            var tips = parseFloat(document.getElementById('<%=Tips.ClientID %>').value.replace("$", ""));
            var serviceFee = parseFloat(document.getElementById('<%=ServiceFee.ClientID %>').value.replace("$", ""));
            var discountPercent = parseFloat(document.getElementById('<%=DiscountPercent.ClientID %>').value.replace("%", ""));
            var discountReferral = parseFloat(document.getElementById('<%=DiscountReferral.ClientID %>').value.replace("%", ""));
            var discountAmount = parseFloat(document.getElementById('<%=DiscountAmount.ClientID %>').value.replace("$", ""));
            var salesTax = parseFloat(document.getElementById('<%=SalesTax.ClientID %>').value.replace("%", ""));
            var appDate = Date.parse(document.getElementById('<%=AppDate.ClientID %>').value);

            var contractorCount = JsContractorCount();
            var subContractors = 0;
            for (var i = 0; i < contractorCount; i++) {
                if (document.getElementById("ContractorType_" + i).value != "1") {
                    subContractors += parseFloat(document.getElementById("ContractorServiceFee_" + i).value.replace("$", ""));
                }
            }

            var total = hours * rate;
            total += serviceFee;
            total -= discountAmount;
            total += tips;
            total += subContractors;
            if (appDate < Date.parse("1/15/2021")) {
                total -= (((discountPercent + discountReferral) / 100) * ((hours * rate) + serviceFee));
            }
            else {
                total -= (((discountPercent + discountReferral) / 100) * (hours * rate));
            }

            total += ((salesTax / 100) * (total -= tips));

            document.getElementById('<%=SubContractors.ClientID %>').innerHTML = isNaN(total) ? "" : "$" + subContractors.toFixed(2);
            document.getElementById('<%=CustomerTotal.ClientID %>').innerHTML = isNaN(total) ? "" : "$" + total.toFixed(2);
        }

        function JsContractorRecurrenceTypeChanged(i) {
            var rec = document.getElementById("ContractorRecurrenceType_" + i).value;
            document.getElementById("ContractorWeekFreqeuncyLabel_" + i).style.display = (rec == "Weekly" ? "inline" : "none");
            document.getElementById("ContractorWeekFreqeuncy_" + i).style.display = (rec == "Weekly" ? "inline" : "none");
            document.getElementById("ContractorWeekOfMonthLabel_" + i).style.display = (rec == "Monthly" ? "inline" : "none");
            document.getElementById("ContractorWeekOfMonth_" + i).style.display = (rec == "Monthly" ? "inline" : "none");
            document.getElementById("ContractorDayOfWeekLabel_" + i).style.display = (rec == "Monthly" ? "inline" : "none");
            document.getElementById("ContractorDayOfWeek_" + i).style.display = (rec == "Monthly" ? "inline" : "none");
            document.getElementById("ContractorApplyToFutureLabel_" + i).style.display = (rec != "None" ? "inline" : "none");
            document.getElementById("ContractorApplyToFuture_" + i).style.display = (rec != "None" ? "inline" : "none");
        }

    </script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>


    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="FollowUpButton" OnClick="FollowUpClick" Text="Follow Up" runat="server" />
        <asp:Button ID="PaymentButton" OnClick="ViewPaymentClick" Text="View Payment" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitleApp">
        <asp:LinkButton ID="TitleLink" OnCommand="LinkSaveCommand" runat="server" />
        <asp:Label ID="TitleLabel" runat="server" />
    </div>
    <div id="CustomerServiceFee" class="AppServiceFee" runat="server">
        Customer Service Fee:
   
    </div>
    <asp:HiddenField ID="DiscountReferral" Value="0" runat="server" />
    <asp:Panel ID="AppointmentPanel" runat="server" DefaultButton="SaveButton">
        <table class="AppDateTime">
            <tr>
                <td class="AppDateTimeHeader">Date:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="AppDate" runat="server" CssClass="entryTextBoxCenter" Width="110px"
                        onchange="JsAppDateChanged(this)"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtenderAppDate" TargetControlID="AppDate" runat="server"></asp:CalendarExtender>
                </td>
                <td>Status:
                </td>
                <td>
                    <asp:DropDownList ID="AppStatus" ClientIDMode="Static" Width="105px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Active</asp:ListItem>
                        <asp:ListItem>Rescheduled</asp:ListItem>
                        <asp:ListItem>Canceled</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table class="AppDateTime" style="border: 1px solid #AAA;">
            <tr>
                <td class="AppDateTimeHeader">Hours Billed:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="HoursBilled" runat="server" onchange="JsCalcHoursChanged(this)"
                        CssClass="AppEntryCenter" />
                </td>
                <td class="AppDateTimeHeader">Hourly Rate:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="HourlyRate" runat="server" CssClass="AppEntryCenter" onchange="JsCalcMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="AppDateTimeHeader">Service Fee:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="ServiceFee" runat="server" CssClass="AppEntryCenter" onchange="JsCustomerServiceFeeChanged(this)" />
                </td>
                <td class="AppDateTimeHeader">Tips:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="Tips" runat="server" CssClass="AppEntryCenter" onchange="JsCustomerTipsChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="AppDateTimeHeader">Discount %:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="DiscountPercent" runat="server" CssClass="AppEntryCenter" onchange="JsCalcPercentChanged(this)" />
                </td>
                <td class="AppDateTimeHeader">Discount $:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="DiscountAmount" runat="server" CssClass="AppEntryCenter" onchange="JsCalcMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="AppDateTimeHeader">Sales Tax %:
                </td>
                <td class="AppDateTime">
                    <asp:TextBox ID="SalesTax" runat="server" CssClass="AppEntryCenter" onchange="JsCalcPercentNoRoundChanged(this)" />
                </td>
            </tr>
        </table>
        <table class="AppDateTime" style="margin: 15px auto 20px auto;">
            <tr>
                <td class="AppDateTimeHeader" style="width: auto; font-weight: bold;">Sub Contractors:
                </td>
                <td class="AppDateTime">
                    <asp:Label ID="SubContractors" runat="server" CssClass="AppEntryCenter" Width="70px">$0.0</asp:Label>
                </td>
                <td class="AppDateTimeHeader" style="width: auto; font-weight: bold;">Customer Total:
                </td>
                <td class="AppDateTime">
                    <asp:Label ID="CustomerTotal" runat="server" CssClass="AppEntryCenter" Width="70px">$0.0</asp:Label>
                </td>

            </tr>
        </table>
        <fieldset class="Entry" style="width: 800px;">
            <legend>Assigned Contractors</legend>
            <div class="AlignCenter">
                <asp:Button ID="AddContractorNewButton" OnClick="AddContractorNewClick" Text="Add Contractor" runat="server" />
                <asp:Button ID="ReplaceContractorButton" OnClick="ReplaceContractorClick" Text="Replace Top Contractor" runat="server" />
                <asp:Button ID="AddContractorScheduleButton" OnClick="AddContractorScheduleClick" Text="Add from Schedule" runat="server" />
            </div>
            <asp:Table ID="ContractorTable" CssClass="ContractorTable" runat="server">
                <asp:TableHeaderRow ID="ContractorHeaderRow" runat="server">
                    <asp:TableHeaderCell CssClass="AlignCenter"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200px">Contractor</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter">Start</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter">End</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter">Hours</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter">Service</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter">Tips</asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="AlignCenter" ColumnSpan="2">Adjustment</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow ID="ContractorRow_0" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:HiddenField ID="ContractorType_0" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_0" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_0" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(0)" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_0" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(0)" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_0" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(0)" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="ContractorHours_0" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_0" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(0)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_0" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_0" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_0" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_0" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_0" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(0)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_0" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_0" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_0" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_0" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_0" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_0" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_0" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_0" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_1" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_1" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_1" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_1" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_1" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(1)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_1" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(1)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_1" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(1)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_1" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_1" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(1)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_1" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_1" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_1" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_1" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_1" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(1)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_1" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_1" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_1" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_1" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_1" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_1" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_1" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_1" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_2" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_2" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_2" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_2" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_2" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(2)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_2" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(2)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_2" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(2)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_2" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_2" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(2)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_2" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_2" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_2" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_2" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_2" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(2)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_2" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_2" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_2" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_2" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_2" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_2" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_2" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_2" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_3" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_3" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_3" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_3" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_3" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(3)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_3" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(3)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_3" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(3)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_3" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_3" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(3)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_3" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_3" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_3" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_3" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_3" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(3)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_3" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_3" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_3" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_3" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_3" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_3" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_3" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_3" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_4" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_4" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_4" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_4" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_4" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(4)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_4" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(4)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_4" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(4)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_4" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_4" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(4)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_4" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_4" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_4" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_4" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_4" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(4)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_4" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_4" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_4" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_4" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_4" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_4" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_4" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_4" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_5" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_5" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_5" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_5" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_5" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(5)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_5" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(5)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_5" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(5)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_5" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_5" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(5)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_5" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_5" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_5" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_5" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_5" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(5)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_5" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_5" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_5" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_5" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_5" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_5" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_5" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_5" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_6" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_6" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_6" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_6" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_6" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(6)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_6" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(6)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_6" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(6)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_6" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_6" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(6)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_6" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_6" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_6" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_6" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_6" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(6)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_6" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_6" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_6" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_6" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_6" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_6" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_6" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_6" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_7" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_7" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_7" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_7" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_7" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(7)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_7" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(7)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_7" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(7)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_7" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_7" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(7)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_7" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_7" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_7" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_7" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_7" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(7)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_7" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_7" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_7" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_7" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_7" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_7" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_7" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_7" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_8" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_8" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_8" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_8" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_8" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(8)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_8" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(8)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_8" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(8)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_8" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_8" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(8)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_8" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_8" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_8" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_8" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #B2E6FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_8" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(8)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_8" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_8" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_8" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_8" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_8" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_8" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_8" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_8" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRow_9" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell>
                        <asp:Button ID="ContractorRemoveButton_9" OnClick="RemoveContractorClick" Text="X" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:HiddenField ID="ContractorType_9" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:HiddenField ID="ContractorAppID_9" ClientIDMode="Static" runat="server" Value="-1" />
                        <asp:DropDownList ID="Contractor_9" ClientIDMode="Static" class="chzn-select" Width="172px" onchange="JsContractorChanged(9)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorStartTime_9" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(9)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorEndTime_9" ClientIDMode="Static" Width="90px" onchange="JsContractorTimeChanged(9)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorHours_9" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorHoursChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorServiceFee_9" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorServiceFeeChanged(9)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorTips_9" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsContractorTipsChanged(this)" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ContractorAdjustmentType_9" ClientIDMode="Static" class="chzn-select" Width="130px" runat="server" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="ContractorAdjustment_9" ClientIDMode="Static" CssClass="ContractorEntry" onchange="JsFormMoneyChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ContractorRowExtra_9" ClientIDMode="Static" CssClass="ContractorRow" Style="background-color: #E8F7FF;" runat="server">
                    <asp:TableCell ColumnSpan="9" CssClass="ExtraCell">
                        Recurrence:
                       
                        <asp:DropDownList ID="ContractorRecurrenceType_9" ClientIDMode="Static" CssClass="entryDropDownList" Width="75px" onchange="JsContractorRecurrenceTypeChanged(9)"
                            runat="server">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorWeekFreqeuncyLabel_9" ClientIDMode="Static" runat="server"> Week Freqeuncy: </asp:Label>
                        <asp:TextBox ID="ContractorWeekFreqeuncy_9" ClientIDMode="Static" runat="server" CssClass="AppEntryCenter" Width="50px"
                            onchange="JsFormValueChanged(this)" />
                        <asp:Label ID="ContractorWeekOfMonthLabel_9" ClientIDMode="Static" runat="server"> Week: </asp:Label>
                        <asp:DropDownList ID="ContractorWeekOfMonth_9" ClientIDMode="Static" CssClass="entryDropDownList" Width="80px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>First</asp:ListItem>
                            <asp:ListItem>Second</asp:ListItem>
                            <asp:ListItem>Third</asp:ListItem>
                            <asp:ListItem>Fourth</asp:ListItem>
                            <asp:ListItem>Last</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorDayOfWeekLabel_9" ClientIDMode="Static" runat="server"> Day: </asp:Label>
                        <asp:DropDownList ID="ContractorDayOfWeek_9" ClientIDMode="Static" CssClass="entryDropDownList" Width="100px" onchange="JsFormValueChanged(this)"
                            runat="server">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="ContractorApplyToFutureLabel_9" ClientIDMode="Static" runat="server"> Re-Make Future: </asp:Label>
                        <asp:CheckBox ID="ContractorApplyToFuture_9" ClientIDMode="Static" OnClick="JsFormValueChanged(this)" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </fieldset>
    </asp:Panel>
    <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete Appointment" runat="server" />
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
