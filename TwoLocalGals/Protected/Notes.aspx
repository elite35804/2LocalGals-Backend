<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Notes.aspx.cs" Inherits="TwoLocalGals.Protected.Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>

    <asp:Panel ID="NotesPanel" runat="server" />
</asp:Content>
