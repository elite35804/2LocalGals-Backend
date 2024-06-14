<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerPortal.Master" AutoEventWireup="true"
    CodeBehind="PortalAppointments.aspx.cs" Inherits="TwoLocalGals.Protected.PortalAppointments" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <title>2LG Appointments</title>
    <link href="/Styles/PortalAppointmentsM.css" media="screen and (max-width: 800px)"
        rel="stylesheet" type="text/css" />
    <link href="/Styles/PortalAppointments.css" media="screen and (min-width: 800px)"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="DataTable">
        <div class="DataTableTitle">
            Appointment Details</div>
        <div id="Frequency" class="FrequencyLabel" runat="server">
        </div>
        <asp:Table ID="AppointmentTable" CssClass="DataTable" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Appointment</asp:TableHeaderCell>
                <asp:TableHeaderCell>Hours</asp:TableHeaderCell>
                <asp:TableHeaderCell>Contractors</asp:TableHeaderCell>
                <asp:TableHeaderCell>Tips</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <div class="DataTable">
        <div class="DataTableTitle">
            Appointment Transactions</div>
        <asp:Table ID="TransactionTable" CssClass="DataTable" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Appointment</asp:TableHeaderCell>
                <asp:TableHeaderCell>Type</asp:TableHeaderCell>
                <asp:TableHeaderCell>Amount</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>
