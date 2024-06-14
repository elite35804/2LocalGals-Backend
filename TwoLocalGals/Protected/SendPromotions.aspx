<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SendPromotions.aspx.cs" Inherits="TwoLocalGals.Protected.SendPromotions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Send Promotions</title>
    <link href="/Styles/SendPromotions.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        Customer Status:
        <asp:DropDownList ID="AccountStatus" runat="server">
            <asp:ListItem>Quote</asp:ListItem>
            <asp:ListItem>Web Quote</asp:ListItem>
            <asp:ListItem>New</asp:ListItem>
            <asp:ListItem>One Time</asp:ListItem>
            <asp:ListItem>Active</asp:ListItem>
            <asp:ListItem>As Needed</asp:ListItem>
            <asp:ListItem>Follow Up</asp:ListItem>
            <asp:ListItem>Inactive</asp:ListItem>
            <asp:ListItem>Contractors</asp:ListItem>
        </asp:DropDownList>
        Services:
        <asp:DropDownList ID="ServiceType" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SendEmailButton" OnClick="SendEmailClick" Text="Batch Email" runat="server" />
        <asp:Panel ID="SearchPanel" runat="server" style="margin: 5px;" DefaultButton="SendPreviewButton">
            <asp:Button ID="SendPreviewButton" OnClick="SendPreviewClick" Text="Send Preview Email" runat="server" />
            <asp:TextBox ID="SearchBox" runat="server" Width="700px" />
            <div id="divwidth">
            </div>
            <asp:AutoCompleteExtender ID="AutoCompleteSearch" runat="server" TargetControlID="SearchBox" ServicePath="AutoComplete.asmx" ServiceMethod="GetCustomerCompletionList" CompletionInterval="100" CompletionListElementID="divwidth" FirstRowSelected="true" MinimumPrefixLength="3" CompletionSetCount="20" />
        </asp:Panel>
    </div>

    <asp:Table ID="FranchiseTable" CssClass="Entry" runat="server">
        <asp:TableRow>
            <asp:TableCell CssClass="EntryHeader" ID="FranchiseTitleCell">
                Franchise:
            </asp:TableCell>
            <asp:TableCell ID="FranchiseCell" />
        </asp:TableRow>
    </asp:Table>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!--img src="http://drive.google.com/uc?export=view&id=0BynTZTHQqF0LTE1HaXREUVlSa0U" /-->
    <div class="Promotions">
        <h1>Send Promotional Email</h1>
        <p>You can use HTML and CSS to customize your promotional email. You may also include pre-defined tags that will populate with our company's and the customer's information when sent. The list of tags you can use is as follows [CompanyLogo] [LetterheadLogo] [CustomerFirstName] [CustomerLastName] [CustomerFullName] [FranchiseName] [FranchisePhone] [FranchiseWebsite] [FranchiseEmail] [FranchiseAddress] [FranchiseCity] [FranchiseState] [FranchiseZip].</p>
        <p>You can also embed any Google Docs image into the email. Go to Google Docs and right click on the image you want to embed. Then click "Share". Then click "Copy link". Next go to your promotional email and copy and paste it into the "src" attribute of an html "img" tag.</p>
        <p>Example: <b>&ltimg src="PASTE_GOGLE_DOCS_IMG_URL" /&gt</b></p>
        <p>Subject: <asp:TextBox ID="SubjectTextBox" runat="server" Width="520px" /></p>
        <asp:TextBox ID="BodyTextBox" runat="server" Rows="25" TextMode="multiline" Width="100%" />
    </div>
</asp:Content>
