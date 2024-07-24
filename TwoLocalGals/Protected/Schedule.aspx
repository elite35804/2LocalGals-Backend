<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="TwoLocalGals.Protected.Schedule" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Schedule</title>
    <link href="/Styles/Schedule.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <asp:Panel ID="MenuPanel" runat="server" Style="margin: 3px;" DefaultButton="ApplyButton">
        <asp:Button ID="CompactModeButton" OnClick="CompactModeClick" Text="Compact Mode" runat="server" />
        <asp:Button ID="PrevButton" OnClick="PrevButtonClick" Text="<<<" runat="server" />
        <asp:TextBox ID="WeekDate" runat="server" CssClass="entryTextBoxCenter" Width="170px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtenderWeekDate" TargetControlID="WeekDate" runat="server"></asp:CalendarExtender>
        <asp:Button ID="NextButton" OnClick="NextButtonClick" Text=">>>" runat="server" />



        <asp:DropDownList ID="SortBy" Width="150px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server">
            <asp:ListItem>Sort by Frequency</asp:ListItem>
            <asp:ListItem>Sort by Team Name</asp:ListItem>
            <asp:ListItem>Sort by Score</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="ContractorType" Width="150px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server" />
        <asp:Button ID="ApplyButton" OnClick="ApplyButtonClick" Text="Apply" Style="margin-left: 5px;" runat="server" />
    </asp:Panel>
    <asp:Panel ID="SearchPanel" runat="server" Style="margin: 5px;" DefaultButton="SearchButton">
        <asp:Button ID="SearchButton" OnClick="SearchClick" Text="Search" runat="server" />
        <asp:TextBox ID="SearchBox" runat="server" Width="1000px" />
        <div id="divwidth">
        </div>
        <asp:AutoCompleteExtender ID="AutoCompleteSearch" runat="server" TargetControlID="SearchBox" ServicePath="AutoComplete.asmx" ServiceMethod="GetCustomerCompletionList" CompletionInterval="100" CompletionListElementID="divwidth" FirstRowSelected="true" MinimumPrefixLength="3" CompletionSetCount="20" />
    </asp:Panel>
    <div id="TitleDiv" runat="server" class="PageTitle">Schedule Title</div>
    <asp:Panel ID="LookupPanel" runat="server" Visible="false" Style="margin: 5px 0px 5px 20px;">
        Contractors
       
        <asp:DropDownList ID="LookupContractorCount" Width="80px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
        </asp:DropDownList>
        estimated hours
       
        <asp:DropDownList ID="LookupContractorHours" Width="80px" OnSelectedIndexChanged="ApplyButtonClick" AutoPostBack="true" runat="server">
            <asp:ListItem>0.50</asp:ListItem>
            <asp:ListItem>0.75</asp:ListItem>
            <asp:ListItem>1.00</asp:ListItem>
            <asp:ListItem>1.25</asp:ListItem>
            <asp:ListItem>1.50</asp:ListItem>
            <asp:ListItem>1.75</asp:ListItem>
            <asp:ListItem>2.00</asp:ListItem>
            <asp:ListItem>2.25</asp:ListItem>
            <asp:ListItem>2.50</asp:ListItem>
            <asp:ListItem>2.75</asp:ListItem>
            <asp:ListItem>3.00</asp:ListItem>
            <asp:ListItem>3.25</asp:ListItem>
            <asp:ListItem>3.50</asp:ListItem>
            <asp:ListItem>3.75</asp:ListItem>
            <asp:ListItem>4.00</asp:ListItem>
            <asp:ListItem>4.25</asp:ListItem>
            <asp:ListItem>4.50</asp:ListItem>
            <asp:ListItem>4.75</asp:ListItem>
            <asp:ListItem>5.00</asp:ListItem>
            <asp:ListItem>5.25</asp:ListItem>
            <asp:ListItem>5.50</asp:ListItem>
            <asp:ListItem>5.75</asp:ListItem>
            <asp:ListItem>6.00</asp:ListItem>
            <asp:ListItem>6.25</asp:ListItem>
            <asp:ListItem>6.50</asp:ListItem>
            <asp:ListItem>6.75</asp:ListItem>
            <asp:ListItem>7.00</asp:ListItem>
            <asp:ListItem>7.25</asp:ListItem>
            <asp:ListItem>7.50</asp:ListItem>
            <asp:ListItem>7.75</asp:ListItem>
            <asp:ListItem>8.00</asp:ListItem>
            <asp:ListItem>8.25</asp:ListItem>
            <asp:ListItem>8.50</asp:ListItem>
            <asp:ListItem>8.75</asp:ListItem>
            <asp:ListItem>9.00</asp:ListItem>
            <asp:ListItem>9.25</asp:ListItem>
            <asp:ListItem>9.50</asp:ListItem>
            <asp:ListItem>9.75</asp:ListItem>
            <asp:ListItem>10.00</asp:ListItem>
            <asp:ListItem>10.25</asp:ListItem>
            <asp:ListItem>10.50</asp:ListItem>
            <asp:ListItem>10.75</asp:ListItem>
            <asp:ListItem>11.00</asp:ListItem>
            <asp:ListItem>11.25</asp:ListItem>
            <asp:ListItem>11.50</asp:ListItem>
            <asp:ListItem>11.75</asp:ListItem>
            <asp:ListItem>12.00</asp:ListItem>
            <asp:ListItem>12.25</asp:ListItem>
            <asp:ListItem>12.50</asp:ListItem>
            <asp:ListItem>12.75</asp:ListItem>
            <asp:ListItem>13.00</asp:ListItem>
            <asp:ListItem>13.25</asp:ListItem>
            <asp:ListItem>13.50</asp:ListItem>
            <asp:ListItem>13.75</asp:ListItem>
            <asp:ListItem>14.00</asp:ListItem>
            <asp:ListItem>14.25</asp:ListItem>
            <asp:ListItem>14.50</asp:ListItem>
            <asp:ListItem>14.75</asp:ListItem>
            <asp:ListItem>15.00</asp:ListItem>
            <asp:ListItem>15.25</asp:ListItem>
            <asp:ListItem>15.50</asp:ListItem>
            <asp:ListItem>15.75</asp:ListItem>
            <asp:ListItem>16.00</asp:ListItem>
            <asp:ListItem>16.25</asp:ListItem>
            <asp:ListItem>16.50</asp:ListItem>
            <asp:ListItem>16.75</asp:ListItem>
            <asp:ListItem>17.00</asp:ListItem>
            <asp:ListItem>17.25</asp:ListItem>
            <asp:ListItem>17.50</asp:ListItem>
            <asp:ListItem>17.75</asp:ListItem>
            <asp:ListItem>18.00</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="LookupLabel" runat="server" />
    </asp:Panel>
    <div id="WeekTotal" class="WeekTotal" runat="server" />

    <asp:Table ID="ScheduleTable" CssClass="Schedule" runat="server" />

    <script language="javascript" type="text/javascript">
        //$("a").bind('click', function (e) {
                  $('*').click(function () {
                      var yPos = window.pageYOffset || document.documentElement.scrollTop;
                      $.cookie("ScheduleScrollPos", yPos);
                  });

                  window.onload = function WindowLoad() {
                      if (JsGetParameter("DoScroll") == 'Y') {
                          var yPos = $.cookie("ScheduleScrollPos");
                          window.scrollTo(0, yPos);
                      }
                  }
    </script >
</asp:Content>
