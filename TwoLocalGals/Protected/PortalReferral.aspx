<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true" CodeBehind="PortalReferral.aspx.cs" Inherits="TwoLocalGals.Protected.PortalReferral" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Referral</title>
    <link href="/Styles/PortalReferralM.css" media="screen and (max-width: 800px)" rel="stylesheet"
        type="text/css" />
    <link href="/Styles/PortalReferral.css" media="screen and (min-width: 800px)" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="ErrorDiv">
        <asp:Label ID="ErrorLabel" runat="server" />
    </div>
    <div id="SuccessDiv" class="Success" runat="server">Your referral email has been sent. Thank You!</div>
    <div id="NormalDiv" class="Normal" runat="server">
        <div class="Title">Referral Program</div>
        <p>If you think we are a great company and want to spread the word, refer your friends and family today, and we will thank you by saving you up to 30% on each of your upcoming visits that your referrals are an active customer of ours.</p>
        <ul>
            <li>1st Referral= 10% Off</li>
            <li>2nd Referral= 15% Off</li>
            <li>3rd Referral= 20% Off</li>
            <li>4th Referral= 25% Off</li>
            <li>5th Referral= 30% Off</li>
        </ul>
        <div class="ReferralMenu">
            Name <asp:TextBox ID="Name" CssClass="ReferralTextBox" runat="server" />
        </div>
        <div class="ReferralMenu">
            Email <asp:TextBox ID="Email" CssClass="ReferralTextBox" runat="server" />
        </div>
        <div class="ReferralMenu">
            <asp:Button ID="SendButton" CssClass="ReferralButton" OnClick="SendClick" Text="Send Referral Email" runat="server" />
            <asp:Button ID="CancelButton" CssClass="ReferralButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        </div>
    </div>
</asp:Content>
