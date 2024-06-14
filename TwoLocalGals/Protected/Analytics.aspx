<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Analytics.aspx.cs" Inherits="TwoLocalGals.Protected.Analytics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/jscharts.js"></script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Error Label -->
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
        Service:
        <asp:DropDownList ID="ContractorType" Width="150px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server" />
        <br />
        <asp:Button ID="ApplyButton" OnClick="ApplyButtonClick" Text="Apply" runat="server" />
    </asp:Panel>
    <div id="profitChartDiv">
        Javascript Chart Draw Error
    </div>
    <script type="text/javascript">
        var profitChart = new JSChart('profitChartDiv', 'bar');
        profitChart.setDataXML('ChartData.aspx?C=4');
        profitChart.setValuesFormat('.');
        profitChart.setBarColor('#99ccff');
        profitChart.setBarOpacity(0.8);
        profitChart.setAxisNameX('');
        profitChart.setAxisNameY('');
        profitChart.setSize(970, 500);
        profitChart.setTitle('Profit');
        profitChart.setTitleColor('#000');
        profitChart.setTitleFontSize(20);
        profitChart.setBarValuesDecimals(0);
        profitChart.setBarValuesPrefix("$");
        profitChart.draw();
    </script>
    <div id="activeChartDiv">
        Javascript Chart Draw Error
    </div>
    <script type="text/javascript">
        var activeChart = new JSChart('activeChartDiv', 'bar');
        activeChart.setDataXML('ChartData.aspx?C=1');
        activeChart.setBarColor('#99ccff');
        activeChart.setBarOpacity(0.8);
        activeChart.setAxisNameX('');
        activeChart.setAxisNameY('');
        activeChart.setSize(970, 300);
        activeChart.setTitle('Active Customers');
        activeChart.setTitleColor('#5555AA');
        activeChart.setTitleFontSize(20);
        activeChart.draw();
    </script>
    <div id="appChartDiv">
        Javascript Chart Draw Error
    </div>
    <script type="text/javascript">
        var appChart = new JSChart('appChartDiv', 'bar');
        appChart.setDataXML('ChartData.aspx?C=2');
        appChart.setBarColor('#99ccff');
        appChart.setBarOpacity(0.8);
        appChart.setAxisNameX('');
        appChart.setAxisNameY('');
        appChart.setSize(970, 300);
        appChart.setTitle('Appointment Hours');
        appChart.setTitleColor('#5555AA');
        appChart.setTitleFontSize(20);
        appChart.draw();
    </script>
    <div id="satisfactionChartDiv">
        Javascript Chart Draw Error
    </div>
    <script type="text/javascript">
        var satChart = new JSChart('satisfactionChartDiv', 'bar');
        satChart.setDataXML('ChartData.aspx?C=3');
        satChart.setBarColor('#99ccff');
        satChart.setBarOpacity(0.8);
        satChart.setAxisNameX('');
        satChart.setAxisNameY('');
        satChart.setSize(970, 300);
        satChart.setTitle('Scheduling Satisfaction');
        satChart.setTitleColor('#5555AA');
        satChart.setTitleFontSize(20);
        satChart.draw();
    </script>
</asp:Content>
