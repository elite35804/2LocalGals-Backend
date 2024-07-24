<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Franchises.aspx.cs" Inherits="Nexus.Protected.Franchises" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

        function JsSmtpServer(field) {
            if (field.value == "Gmail") {
                document.getElementById('<%=EmailSmtp.ClientID %>').value = 'smtp.gmail.com';
                document.getElementById('<%=EmailPort.ClientID %>').value = '587';
                document.getElementById('<%=EmailSecure.ClientID %>').checked = true;
            }
            if (field.value == "Yahoo") {
                document.getElementById('<%=EmailSmtp.ClientID %>').value = 'smtp.bizmail.yahoo.com';
                document.getElementById('<%=EmailPort.ClientID %>').value = '465';
                document.getElementById('<%=EmailSecure.ClientID %>').checked = true;
            }
            if (field.value == "Hotmail") {
                document.getElementById('<%=EmailSmtp.ClientID %>').value = 'smtp.live.com';
                document.getElementById('<%=EmailPort.ClientID %>').value = '465';
                document.getElementById('<%=EmailSecure.ClientID %>').checked = true;
            }
            if (field.value == "GoDaddy") {
                document.getElementById('<%=EmailSmtp.ClientID %>').value = 'smtpout.secureserver.net';
                document.getElementById('<%=EmailPort.ClientID %>').value = '25';
                document.getElementById('<%=EmailSecure.ClientID %>').checked = false;
            }
            if (field.value == "Hostway") {
                document.getElementById('<%=EmailSmtp.ClientID %>').value = '172.16.145.71';
                document.getElementById('<%=EmailPort.ClientID %>').value = '25';
                document.getElementById('<%=EmailSecure.ClientID %>').checked = false;
            }
        };

    </script>
    <!-- Fake Username Password to Stop Autocomplete -->
    <input style="display: none" type="text" name="fakeemailremembered" />
    <input style="display: none" type="password" name="fakepasswordremembered" />
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        Franchise:
       
        <asp:DropDownList ID="FranchiseList" class="chzn-select" Width="220px" runat="server"
            OnSelectedIndexChanged="FranchiseChanged" AutoPostBack="true" />
        <asp:Button ID="NewButton" OnClick="NewClick" Text="New Franchise" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <fieldset class="Entry" style="width: 600px;">
        <legend>Franchise Information</legend>
        <table class="Entry">
            <tr>
                <td class="EntryHeader">Franchise Name:
                </td>
                <td>
                    <asp:TextBox ID="FranchiseName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Email Address:
                </td>
                <td>
                    <asp:TextBox ID="EM" autocomplete="off" AutoCompleteType="Disabled" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Email Password:
                </td>
                <td>
                    <asp:TextBox ID="EP" autocomplete="off" AutoCompleteType="Disabled" TextMode="Password" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Common SMPT Settings:
                </td>
                <td>
                    <asp:DropDownList ID="CommonSmtp" CssClass="entryDropDownList" Width="252px" onchange="JsSmtpServer(this)"
                        runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Gmail</asp:ListItem>
                        <asp:ListItem>Yahoo</asp:ListItem>
                        <asp:ListItem>Hotmail</asp:ListItem>
                        <asp:ListItem>GoDaddy</asp:ListItem>
                        <asp:ListItem>Hostway</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Email Smtp Server:
                </td>
                <td>
                    <asp:TextBox ID="EmailSmtp" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Emal Smtp Port:
                </td>
                <td>
                    <asp:TextBox ID="EmailPort" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Email SSL Enabled:
                </td>
                <td>
                    <asp:CheckBox ID="EmailSecure" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Phone Number:
                </td>
                <td>
                    <asp:TextBox ID="Phone" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Address:
                </td>
                <td>
                    <asp:TextBox ID="Address" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">City:
                </td>
                <td>
                    <asp:TextBox ID="City" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">State:
                </td>
                <td>
                    <asp:TextBox ID="State" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Zip:
                </td>
                <td>
                    <asp:TextBox ID="Zip" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Web Link:
                </td>
                <td>
                    <asp:TextBox ID="WebLink" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Review Us Link:
                </td>
                <td>
                    <asp:TextBox ID="ReviewUsLink" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Default Rate Per Hour:
                </td>
                <td>
                    <asp:TextBox ID="DefaultRatePerHour" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Default Service Fee:
                </td>
                <td>
                    <asp:TextBox ID="DefaultServiceFee" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Default Scheduling Fee:
                </td>
                <td>
                    <asp:TextBox ID="DefaultScheduleFee" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Customer Rewards (%):
                </td>
                <td>
                    <asp:TextBox ID="RewardsPercentage" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Cleaning Equipment and Supplies (%):
                </td>
                <td>
                    <asp:TextBox ID="SuppliesPercentage" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Fuel and Car Maintenance (%):
                </td>
                <td>
                    <asp:TextBox ID="CarPercentage" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">State Sales Tax (%):
                </td>
                <td>
                    <asp:TextBox ID="SalesTax" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">SMS Username:
                </td>
                <td>
                    <asp:TextBox ID="SMSUsername" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">SMS Password:
                </td>
                <td>
                    <asp:TextBox ID="SMSPassword" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">EPN Account:
                </td>
                <td>
                    <asp:TextBox ID="EPNAccount" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Restrict Key:
                </td>
                <td>
                    <asp:TextBox ID="RestrictKey" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Batch Time:
                </td>
                <td>
                    <asp:DropDownList ID="BatchTime" class="chzn-select" Width="147px" runat="server">
                        <asp:ListItem>3:00 PM</asp:ListItem>
                        <asp:ListItem>4:00 PM</asp:ListItem>
                        <asp:ListItem>5:00 PM</asp:ListItem>
                        <asp:ListItem>6:00 PM</asp:ListItem>
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                        <asp:ListItem>9:00 PM</asp:ListItem>
                        <asp:ListItem>10:00 PM</asp:ListItem>
                        <asp:ListItem>11:00 PM</asp:ListItem>
                        <asp:ListItem>12:00 AM</asp:ListItem>
                        <asp:ListItem>1:00 AM</asp:ListItem>
                        <asp:ListItem>2:00 AM</asp:ListItem>
                        <asp:ListItem>3:00 AM</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Send Schedules:
                </td>
                <td>
                    <asp:DropDownList ID="SendSchedules" class="chzn-select" Width="147px" runat="server">
                        <asp:ListItem>3:00 PM</asp:ListItem>
                        <asp:ListItem>4:00 PM</asp:ListItem>
                        <asp:ListItem>5:00 PM</asp:ListItem>
                        <asp:ListItem>6:00 PM</asp:ListItem>
                        <asp:ListItem>7:00 PM</asp:ListItem>
                        <asp:ListItem>8:00 PM</asp:ListItem>
                        <asp:ListItem>9:00 PM</asp:ListItem>
                        <asp:ListItem>10:00 PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Notes Enabled:
                </td>
                <td>
                    <asp:CheckBox ID="NotesEnabled" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Advertisement Dropdown:
                </td>
                <td>
                    <asp:TextBox ID="AdvertisementList" runat="server" Rows="6" TextMode="multiline"
                        Width="248px" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Adjustment Dropdown:
                </td>
                <td>
                    <asp:TextBox ID="AdjustmentList" runat="server" Rows="6" TextMode="multiline" Width="248px"
                        CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Payment Dropdown:
                </td>
                <td>
                    <asp:TextBox ID="PaymentList" runat="server" Rows="6" TextMode="multiline" Width="248px"
                        CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Partner Category Dropdown:
                </td>
                <td>
                    <asp:TextBox ID="PartnerCategoryList" runat="server" Rows="6" TextMode="multiline" Width="248px"
                        CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="EntryHeader">Upload Default Picture:
                </td>
                <td>
                    <asp:Image ID="DefaultPic" runat="server" Height="75px" Width="130px" />
                    <br />
                    <br />
                    <asp:FileUpload ID="FranchiseImg" runat="server" />
                </td>
            </tr>

        </table>
    </fieldset>
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
