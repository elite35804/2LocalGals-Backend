<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Users.aspx.cs" Inherits="Nexus.Protected.Users" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        User:
        <asp:DropDownList ID="UserList" class="chzn-select" Width="200px" runat="server" OnSelectedIndexChanged="UserChanged" AutoPostBack="true" />
        <asp:Button ID="NewButton" OnClick="NewClick" Text="New User" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Fake Username Password to Stop Autocomplete -->
    <input style="display:none" type="text" name="fakeusernameremembered"/>
    <input style="display:none" type="password" name="fakepasswordremembered"/>
    <fieldset class="Entry" style="width: 600px;">
        <legend>User Information</legend>
        <table class="Entry">
            <tr>
                <td class="EntryHeader">
                    Username:
                </td>
                <td>
                    <asp:TextBox ID="WootEntry" autocomplete="off" AutoCompleteType="Disabled" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Password:
                </td>
                <td>
                    <asp:TextBox ID="BlahEntry" autocomplete="off" AutoCompleteType="Disabled" TextMode="password" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Attached Contractor:
                </td>
                <td>
                    <asp:DropDownList ID="Contractor" class="chzn-select" Width="252px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Access Level:
                </td>
                <td>
                    <asp:DropDownList ID="Access" class="chzn-select" Width="252px" runat="server">
                        <asp:ListItem Text="No Access (Disabled)" Value="0" />
                        <asp:ListItem Text="Contractor" Value="2" />
                        <asp:ListItem Text="Manager" Value="5" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">
                    Franchise:
                </td>
                <td>
                    <asp:ListBox ID="FranchiseList" class="chzn-select" data-placeholder="Choose a Franchise..." Width="252px" runat="server" SelectionMode="Multiple" />
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete User" runat="server" />
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
