<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Payments.aspx.cs" Inherits="TwoLocalGals.Protected.Payments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript" src="/DataTables/js/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="/DataTables/js/jquery.dataTables.js"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#InvoceTable').dataTable({
                "bStateSave": true,
                "aoColumns": [{ "sType": "date" }, null, null, { "sType": "currency" }, { "sType": "currency" }, null, { "sType": "currency" }, null, { "sType": "currency" }, { "sType": "currency" }, { "sType": "currency" }, null, null]
            });

            if (JsGetCookie('ClearSearch') == 'Clear') {
                $('.dataTables_filter input').val('').keyup();
                JsSetCookie('ClearSearch', 'No', null);
            }

            if (JsGetParameter("Search") != '')
                $('.dataTables_filter input').val(JsGetParameter("Search")).keyup();
        });
    </script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <asp:Panel ID="MenuPanel" runat="server" DefaultButton="ApplyButton">
        &nbspStart Date:
        <asp:TextBox ID="StartDate" runat="server" CssClass="entryTextBoxCenter" Width="85px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtenderStartDate" TargetControlID="StartDate"
            runat="server">
        </asp:CalendarExtender>
        End Date:
        <asp:TextBox ID="EndDate" runat="server" CssClass="entryTextBoxCenter" Width="85px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtenderEndDate" TargetControlID="EndDate" runat="server">
        </asp:CalendarExtender>
        Quick Dates:
        <asp:Button ID="TodayButton" OnClick="TodayClick" Text="Today" runat="server" />
        <asp:Button ID="TomorrowButton" OnClick="TomorrowClick" Text="Tomorrow" runat="server" />
        <asp:Button ID="YesterdayButton" OnClick="YesterdayClick" Text="Yesterday" runat="server" />
        <asp:Button ID="ThisWeekButton" OnClick="ThisWeekClick" Text="This Week" runat="server" />
        <asp:Button ID="LastWeekButton" OnClick="LastWeekClick" Text="Last Week" runat="server" />
        <asp:Button ID="Last30DaysButton" OnClick="Last30DaysClick" Text="Last 30 Days" runat="server" />
        Options:
        <asp:Button ID="UnpaidButton" OnClick="UnpaidClick" Text="Unpaid Only" runat="server" />
        <asp:Button ID="CashCheckButton" OnClick="CashCheckClick" Text="Cash/Checks Only" runat="server" />
        <asp:Button ID="CreditCardButton" OnClick="CreditCardClick" Text="Credit Card Only" runat="server" />
        <br />
        <asp:Button ID="ApplyButton" OnClick="ApplyButtonClick" Text="Apply" runat="server" />
    </asp:Panel>
    <div class="Invoces">
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="InvoceTable">
            <thead>
                <tr>
                    <th>
                        Date
                    </th>
                    <th>
                        Customer
                    </th>
                    <th>
                        Item
                    </th>
                    <th>
                        Service Fee
                    </th>
                    <th>
                        Sub Con
                    </th>
                    <th>
                        Hours(Rate)
                    </th>
                    <th>
                        Tips
                    </th>
                    <th>
                        Disc %
                    </th>
                    <th>
                        Disc $
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Balance
                    </th>
                    <th>
                        Payment Type
                    </th>
                    <th>
                        Paid
                    </th>
                </tr>
            </thead>
            <tbody>
                <% GetPaymentsTableHTML(); %>
            </tbody>
        </table>
        <div style="clear: both; text-align: left; padding-top: 20px;">
            © 2015 2LocalGalsHouseKeeping
        </div>
    </div>
</asp:Content>
