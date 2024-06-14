<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Customers.aspx.cs" Inherits="Nexus.Protected.WebFormCustomers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script language="javascript" type="text/javascript">
        function JsShowHideBilling() {
            var billingChecked = document.getElementById('<%=BillingSame.ClientID %>').checked;
            document.getElementById('<%=BillingAddressPanel.ClientID %>').style.display = (billingChecked ? "none" : "inline");
        }

        function JsEstimatePrice() {
            var hours = parseFloat(document.getElementById('<%=EstimatedHours.ClientID %>').value);
            if (isNaN(hours) || document.getElementById('EstimatedDivHours').style.display == 'none') hours = 0;
            var rate = parseFloat(document.getElementById('<%=RatePerHour.ClientID %>').value.replace("$", ""));
            if (isNaN(rate)) rate = 0;
            var service = parseFloat(document.getElementById('<%=ServiceFee.ClientID %>').value.replace("$", ""));
            if (isNaN(service) || document.getElementById('EstimatedDivServiceFee').style.display == 'none') service = 0;
            var cc = parseFloat(document.getElementById('<%=EstimatedCC.ClientID %>').value.replace("$", ""));
            if (isNaN(cc) || document.getElementById('EstimatedDivCC').style.display == 'none') cc = 0;
            var ww = parseFloat(document.getElementById('<%=EstimatedWW.ClientID %>').value.replace("$", ""));
            if (isNaN(ww) || document.getElementById('EstimatedDivWW').style.display == 'none') ww = 0;
            var hw = parseFloat(document.getElementById('<%=EstimatedHW.ClientID %>').value.replace("$", ""));
            if (isNaN(hw) || document.getElementById('EstimatedDivHW').style.display == 'none') hw = 0;
            var total = ((hours * rate) + service + cc + ww + hw);
            document.getElementById('<%=EstimatedPrice.ClientID %>').value = isNaN(total) ? "" : "$" + total.toFixed(2);
        }

        function JsHousekeepingChanged() {
            var checked = document.getElementById('<%=HousekeepingCheckbox.ClientID %>').checked;
            document.getElementById('<%=HousekeepingSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivRPH').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivHours').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivServiceFee').style.display = (checked ? 'block' : 'none');
        }

        function JsCarpetCleaningChanged() {
            var checked = false;
            var box = document.getElementById('<%=CarpetCleaningCheckbox.ClientID %>');
            if (box != null) checked = box.checked;
            document.getElementById('<%=CarpetCleaningSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivCC').style.display = (checked ? 'block' : 'none');
        }

        function JsWindowWashingChanged() {
            var checked = false;
            var box = document.getElementById('<%=WindowWashingCheckbox.ClientID %>');
            if (box != null) checked = box.checked;
            document.getElementById('<%=WindowWashingSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivWW').style.display = (checked ? 'block' : 'none');
        }

        function JsHomewatchChanged() {
            var checked = false;
            var box = document.getElementById('<%=HomewatchCheckbox.ClientID %>');
            if (box != null) checked = box.checked;
            document.getElementById('<%=HomewatchSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('EstimatedDivHW').style.display = (checked ? 'block' : 'none');
        }

        function JsCleanBeforeReturnChanged() {
            var checked = document.getElementById('<%=HW_CleanBeforeReturn.ClientID %>').checked;
            if (checked) {
                document.getElementById('<%=HousekeepingCheckbox.ClientID %>').checked = true;
                JsHousekeepingChanged();
            }
        }

    </script>
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoComplete.asmx" />
        </Services>
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="SearchButton">
            <asp:Button ID="SearchButton" OnClick="SearchClick" Text="Search" runat="server" />
            <asp:TextBox ID="SearchBox" runat="server" Width="800px" />
            <div id="divwidth">
            </div>
            <asp:AutoCompleteExtender ID="AutoCompleteSearch" runat="server" TargetControlID="SearchBox"
                ServicePath="AutoComplete.asmx" ServiceMethod="GetCustomerCompletionList" CompletionInterval="100"
                CompletionListElementID="divwidth" FirstRowSelected="true" MinimumPrefixLength="3" CompletionSetCount="20" />
        </asp:Panel>
        <div style="margin-top: 5px;">
            <asp:Button ID="PrevButton" OnClick="PrevClick" Text="<<<" runat="server" />
            <asp:DropDownList ID="AccountStatusFilter" CssClass="entryDropDownList" Width="100px"
                runat="server">
                <asp:ListItem>Quote</asp:ListItem>
                <asp:ListItem>Web Quote</asp:ListItem>
                <asp:ListItem>New</asp:ListItem>
                <asp:ListItem>One Time</asp:ListItem>
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>As Needed</asp:ListItem>
                <asp:ListItem>Follow Up</asp:ListItem>
                <asp:ListItem>Inactive</asp:ListItem>
                <asp:ListItem>Fired</asp:ListItem>
                <asp:ListItem>Ignored</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="NextButton" OnClick="NextClick" Text=">>>" runat="server" />
            <asp:Button ID="WebQuoteButton" OnClick="WebQuoteClick" Text="Web Quote" runat="server" />
            <asp:Button ID="NewBidButton" OnClick="BidClick" Text="Bid View" runat="server" />
            <asp:Button ID="NewCustomerButton" OnClick="NewClick" Text="New Customer" runat="server" />
            <asp:Button ID="CopyCustomerButton" OnClick="CopyCustomerClick" Text="Copy Customer" runat="server" />
            <asp:Button ID="CopyBilling" OnClick="CopyBillingClick" Text="Copy Billing" runat="server" />
            <asp:Button ID="ReferredButton" OnClick="ReferredClick" Text="Referred By" runat="server" />
            <asp:Button ID="CleaningPackButton" OnClick="CleaningPackClick" Text="Cleaning Pack" runat="server" />
            <asp:Button ID="GiftCardButton" OnClick="GiftCardClick" Text="Gift Card" runat="server" />
        </div>
        <div style="margin-top: 5px;">
            <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save" Style="font-size: 150%;" runat="server" />
            <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" Style="font-size: 150%;" runat="server" />
        </div>
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div id="CustomerTitle" class="CustomerTitle" runat="server">New Customer</div>
    <div id="CustomerTitleExtra" class="CustomerTitleExtra" runat="server">
        <asp:Label ID="CustomerTitleExtraLabel" runat="server" />
        <asp:LinkButton ID="ReferredByLink" Text="" CommandArgument="/Protected/Customers.aspx" OnCommand="LinkSaveCommand" runat="server" Visible="false" />
    </div>
    <asp:Panel DefaultButton="SaveButton" runat="server">
        <div class="customerPanelLeft">
            <fieldset class="EntryField">
                <legend>Account Information</legend>
                <div class="EntryLine">
                    <div class="EntryHeader" style="width: 400px;">
                        <asp:Button ID="SendWelcomeButton" Style="margin-top: 5px;" OnClick="SendWelcomeClick" Text="Send Welcome Letter"
                            runat="server" />
                        <asp:Button ID="SendLoginInfo" Style="margin-top: 5px;" OnClick="SendLoginInfoClick" Text="Send Login Info" Enabled="false"
                            runat="server" />
                        <asp:Button ID="EnableRewardsButton" Style="margin-top: 5px;" OnClick="EnableRewardsClick" Text="Enable Rewards"
                            runat="server" />
                        Send Promotions<asp:CheckBox ID="SendPromotions" runat="server" Checked="true" />
                        <br />
                        <br />
                        <asp:Button ID="ReviewUsButton" Style="margin-top: 5px;" OnClick="ReviewUsClick" Text="Review Us" Enabled="false" runat="server" />
                        <br />
                        <br />
                        <asp:TextBox ID="Redeem" CssClass="AlignCenter" runat="server" />
                        <asp:Button ID="RedeemButton" OnClick="RedeemClick" Text="Redeem Gift Card" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Franchise:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="FranchiseList" class="chzn-select" Width="250px" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Account Status:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="AccountStatus" class="chzn-select" Width="250px" runat="server">
                            <asp:ListItem>Quote</asp:ListItem>
                            <asp:ListItem>Web Quote</asp:ListItem>
                            <asp:ListItem>New</asp:ListItem>
                            <asp:ListItem>One Time</asp:ListItem>
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>As Needed</asp:ListItem>
                            <asp:ListItem>Follow Up</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                            <asp:ListItem>Fired</asp:ListItem>
                            <asp:ListItem>Ignored</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Account Type:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="AccountType" class="chzn-select" Width="250px" runat="server">
                            <asp:ListItem>Home</asp:ListItem>
                            <asp:ListItem>Business</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Account Rep:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="AccountRep" class="chzn-select" Width="250px" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Payment Type:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="PaymentType" class="chzn-select" Width="250px" runat="server" />
                    </div>
                </div>
                <div id="EstimatedDivRPH" class="EntryLine">
                    <div class="EntryHeader">
                        Rate Per Hour:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="RatePerHour" class="chzn-select" Width="250px" runat="server">
                            <asp:ListItem>$50.00</asp:ListItem>
                            <asp:ListItem>$49.00</asp:ListItem>
                            <asp:ListItem>$48.00</asp:ListItem>
                            <asp:ListItem>$47.00</asp:ListItem>
                            <asp:ListItem>$46.00</asp:ListItem>
                            <asp:ListItem>$45.00</asp:ListItem>
                            <asp:ListItem>$44.00</asp:ListItem>
                            <asp:ListItem>$43.00</asp:ListItem>
                            <asp:ListItem>$42.00</asp:ListItem>
                            <asp:ListItem>$41.00</asp:ListItem>
                            <asp:ListItem>$40.00</asp:ListItem>
                            <asp:ListItem>$39.00</asp:ListItem>
                            <asp:ListItem>$38.00</asp:ListItem>
                            <asp:ListItem>$37.00</asp:ListItem>
                            <asp:ListItem>$36.00</asp:ListItem>
                            <asp:ListItem>$35.00</asp:ListItem>
                            <asp:ListItem>$34.00</asp:ListItem>
                            <asp:ListItem>$33.00</asp:ListItem>
                            <asp:ListItem>$32.00</asp:ListItem>
                            <asp:ListItem>$31.00</asp:ListItem>
                            <asp:ListItem>$30.00</asp:ListItem>
                            <asp:ListItem>$29.50</asp:ListItem>
                            <asp:ListItem>$29.00</asp:ListItem>
                            <asp:ListItem>$28.50</asp:ListItem>
                            <asp:ListItem>$28.00</asp:ListItem>
                            <asp:ListItem>$27.50</asp:ListItem>
                            <asp:ListItem>$27.00</asp:ListItem>
                            <asp:ListItem>$26.50</asp:ListItem>
                            <asp:ListItem>$26.00</asp:ListItem>
                            <asp:ListItem>$25.50</asp:ListItem>
                            <asp:ListItem>$25.00</asp:ListItem>
                            <asp:ListItem>$24.50</asp:ListItem>
                            <asp:ListItem>$24.00</asp:ListItem>
                            <asp:ListItem>$23.50</asp:ListItem>
                            <asp:ListItem>$23.00</asp:ListItem>
                            <asp:ListItem>$22.50</asp:ListItem>
                            <asp:ListItem>$22.00</asp:ListItem>
                            <asp:ListItem>$21.50</asp:ListItem>
                            <asp:ListItem>$21.00</asp:ListItem>
                            <asp:ListItem>$20.50</asp:ListItem>
                            <asp:ListItem>$20.00</asp:ListItem>
                            <asp:ListItem>$19.50</asp:ListItem>
                            <asp:ListItem>$19.00</asp:ListItem>
                            <asp:ListItem>$18.50</asp:ListItem>
                            <asp:ListItem>$18.00</asp:ListItem>
                            <asp:ListItem>$17.50</asp:ListItem>
                            <asp:ListItem>$17.00</asp:ListItem>
                            <asp:ListItem>$16.50</asp:ListItem>
                            <asp:ListItem>$16.00</asp:ListItem>
                            <asp:ListItem>$15.50</asp:ListItem>
                            <asp:ListItem>$15.00</asp:ListItem>
                            <asp:ListItem>$14.50</asp:ListItem>
                            <asp:ListItem>$14.00</asp:ListItem>
                            <asp:ListItem>$13.50</asp:ListItem>
                            <asp:ListItem>$13.00</asp:ListItem>
                            <asp:ListItem>$12.50</asp:ListItem>
                            <asp:ListItem>$12.00</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <!-- Service Fee -->
                <div id="EstimatedDivServiceFee" class="EntryLine">
                    <div class="EntryHeader">
                        Service Fee:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="ServiceFee" class="chzn-select" Width="250px" runat="server">
                            <asp:ListItem>$0.00</asp:ListItem>
                            <asp:ListItem>$5.00</asp:ListItem>
                            <asp:ListItem>$10.00</asp:ListItem>
                            <asp:ListItem>$15.00</asp:ListItem>
                            <asp:ListItem>$20.00</asp:ListItem>
                            <asp:ListItem>$25.00</asp:ListItem>
                            <asp:ListItem>$30.00</asp:ListItem>
                            <asp:ListItem>$35.00</asp:ListItem>
                            <asp:ListItem>$40.00</asp:ListItem>
                            <asp:ListItem>$45.00</asp:ListItem>
                            <asp:ListItem>$50.00</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="EstimatedDivHours" class="EntryLine">
                    <div class="EntryHeader">
                        Estimated Hours:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="EstimatedHours" runat="server" CssClass="entryTextBox" onchange="JsFormHoursChanged(this)" />
                    </div>
                </div>
                <div id="EstimatedDivCC" class="EntryLine">
                    <div class="EntryHeader">
                        Carpet Cleaning:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="EstimatedCC" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                    </div>
                </div>
                <div id="EstimatedDivWW" class="EntryLine">
                    <div class="EntryHeader">
                        Window Washing:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="EstimatedWW" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                    </div>
                </div>
                <div id="EstimatedDivHW" class="EntryLine">
                    <div class="EntryHeader">
                        Home Guard:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="EstimatedHW" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Estimated Price:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="EstimatedPrice" runat="server" CssClass="entryTextBox" Width="170px" onchange="JsFormMoneyChanged(this)" />
                        <button type="button" onclick="JsEstimatePrice()">Calculate</button>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Advertisement:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="Advertisement" class="chzn-select" Width="250px" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Preferred Contact:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="PreferredContact" class="chzn-select" Width="250px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Phone</asp:ListItem>
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Text Message</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>
            <fieldset class="EntryField">
                <legend>Customer Information</legend>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Custom Note:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CustomNote" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Business Name:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="BusinessName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Company Contact:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CompanyContact" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        First Name:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="FirstName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Last Name:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="LastName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Spouse's Name:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="SpouseName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Address:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="LocationAddress" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        City:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="LocationCity" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        State:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="LocationState" runat="server" Width="100px" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Zip Code:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="LocationZipCode" runat="server" Width="75px" MaxLength="5" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Best Phone:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="BestPhone" runat="server" CssClass="entryTextBox" Width="200px" onchange="JsFormValueChanged(this)" />
                        Cell<asp:CheckBox ID="BestPhoneCell" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Alternate Phone 1:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="AlternatePhoneOne" runat="server" CssClass="entryTextBox" Width="200px" onchange="JsFormValueChanged(this)" />
                        Cell<asp:CheckBox ID="AlternatePhoneOneCell" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Alternate Phone 2:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="AlternatePhoneTwo" runat="server" CssClass="entryTextBox" Width="200px" onchange="JsFormValueChanged(this)" />
                        Cell<asp:CheckBox ID="AlternatePhoneTwoCell" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Email:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="Email" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
            </fieldset>
            <fieldset class="EntryField">
                <legend>Billing Information</legend>
                <!-- Credit Card Number -->
                <div class="EntryLine">
                    <div class="EntryHeader">
                        Credit Card Number:
                   
                    </div>
                    <div class="EntryValue">
                        <div id="CreditCardPanel">
                            <asp:TextBox ID="CreditCardNumber" runat="server" CssClass="entryTextBox" Style="text-align: center;" />
                        </div>
                    </div>
                </div>
                <!-- Experation Date and CVV Code -->
                <div class="EntryLine">
                    <div class="EntryValue">
                        Expiration: Month
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="ExpirationMonth" CssClass="entryDropDownList"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Year
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="ExpirationYear" runat="server" Width="60px" MaxLength="4" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        CVV Code:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CCVCode" runat="server" Width="45px" MaxLength="3" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <!-- Same as Customer Information -->
                <div class="EntryLine">
                    <div class="EntryValue">
                        Billing Address same as Customer:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="BillingSame" Checked="true" OnClick="JsShowHideBilling()" runat="server" />
                    </div>
                </div>
                <asp:Panel ID="BillingAddressPanel" runat="server">
                    <!-- Billing Name -->
                    <div class="EntryLine">
                        <div class="EntryHeader">
                            Billing Name:
                       
                        </div>
                        <div class="EntryValue">
                            <asp:TextBox ID="BillingName" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                        </div>
                    </div>
                    <!-- Billing Address -->
                    <div class="EntryLine">
                        <div class="EntryHeader">
                            Address:
                       
                        </div>
                        <div class="EntryValue">
                            <asp:TextBox ID="BillingAddress" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                        </div>
                    </div>
                    <!-- Billing City -->
                    <div class="EntryLine">
                        <div class="EntryHeader">
                            City:
                       
                        </div>
                        <div class="EntryValue">
                            <asp:TextBox ID="BillingCity" runat="server" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                        </div>
                    </div>
                    <!-- Billing State and Zip Code -->
                    <div class="EntryLine">
                        <div class="EntryHeader">
                            State:
                       
                        </div>
                        <div class="EntryValue">
                            <asp:TextBox ID="BillingState" runat="server" Width="100px" CssClass="entryTextBoxCenter"
                                onchange="JsFormValueChanged(this)" />
                        </div>
                        <div class="EntryValue">
                            Zip Code:
                       
                        </div>
                        <div class="EntryValue">
                            <asp:TextBox ID="BillingZip" runat="server" Width="75px" MaxLength="5" CssClass="entryTextBoxCenter"
                                onchange="JsFormValueChanged(this)" />
                        </div>
                    </div>
                </asp:Panel>
            </fieldset>
            <fieldset class="EntryField">
                <!-- Transactions -->
                <legend>Transaction Information</legend>
                <asp:Button ID="PaymentButton" OnClick="PaymentClick" Text="New Payment" runat="server" />
                <asp:Button ID="ReturnButton" OnClick="ReturnClick" Text="New Return" runat="server" />
                <asp:Button ID="InvoiceButton" OnClick="InvoiceClick" Text="New Invoice" runat="server" />
                <br />
                From:
               
                <asp:TextBox ID="InvoiceStartDate" runat="server" CssClass="entryTextBoxCenter" Width="110px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderInvoiceStartDate" TargetControlID="InvoiceStartDate" runat="server"></asp:CalendarExtender>
                To:
               
                <asp:TextBox ID="InvoiceEndDate" runat="server" CssClass="entryTextBoxCenter" Width="110px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderInvoiceEndDate" TargetControlID="InvoiceEndDate" runat="server"></asp:CalendarExtender>
                <asp:Button ID="EmailInvoiceButton" OnClick="EmailInvoiceClick" Text="Email Invoice Range" runat="server" />
                <br />
                Email Memo:
               
                <asp:TextBox ID="EmailInvoiceMemo" CssClass="entryTextBoxCenter" Width="320px" runat="server" />
                <asp:Panel runat="server" Height="300px" ScrollBars="Vertical">
                    <div class="AppTable">
                        <asp:Table ID="TransactionTable" CssClass="AppTable" runat="server" />
                    </div>
                </asp:Panel>
            </fieldset>
            <fieldset id="ReferralsFieldset" class="EntryField" visible="false" runat="server">
                <legend>Referrals</legend>
                <asp:Panel ID="Panel1" runat="server" Height="120px" ScrollBars="Vertical">
                    <asp:Table ID="ReferralTable" CssClass="AppTable" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100px">Account Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="300px">Custoemr Name</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
            </fieldset>
        </div>
        <div class="customerPanelRight">
            <fieldset class="EntryField">
                <legend>Scheduling</legend>
                <!-- Notes -->
                <div class="EntryLine">
                    <div class="EntryValue">
                        Notes:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_Notes" runat="server" Rows="6" TextMode="multiline" Width="370px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <!-- Frequency, Day -->
                <div class="EntryLine">
                    <div class="EntryValue">
                        Frequency:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_Frequency" CssClass="entryDropDownList" Width="147px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Onetime</asp:ListItem>
                            <asp:ListItem>As Needed</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Bi-weekly</asp:ListItem>
                            <asp:ListItem>Every 3 Weeks</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Day:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_DayOfWeek" CssClass="entryDropDownList" Width="147px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Flexible</asp:ListItem>
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <!-- Time of Day -->
                <div class="EntryLine">
                    <div class="EntryValue">
                        Preferred Time:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_TimeOfDayPrefix" CssClass="entryDropDownList" Width="147px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Flexible</asp:ListItem>
                            <asp:ListItem>After</asp:ListItem>
                            <asp:ListItem>Before</asp:ListItem>
                            <asp:ListItem>Done By</asp:ListItem>
                            <asp:ListItem>Must Be</asp:ListItem>
                            <asp:ListItem>A.M.</asp:ListItem>
                            <asp:ListItem>P.M.</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_TimeOfDaySuffix" CssClass="entryDropDownList" Width="147px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Any Time</asp:ListItem>
                            <asp:ListItem>9:00 AM</asp:ListItem>
                            <asp:ListItem>9:30 AM</asp:ListItem>
                            <asp:ListItem>10:00 AM</asp:ListItem>
                            <asp:ListItem>10:30 AM</asp:ListItem>
                            <asp:ListItem>11:00 AM</asp:ListItem>
                            <asp:ListItem>11:30 AM</asp:ListItem>
                            <asp:ListItem>12:00 PM</asp:ListItem>
                            <asp:ListItem>12:30 PM</asp:ListItem>
                            <asp:ListItem>1:00 PM</asp:ListItem>
                            <asp:ListItem>1:30 PM</asp:ListItem>
                            <asp:ListItem>2:00 PM</asp:ListItem>
                            <asp:ListItem>2:30 PM</asp:ListItem>
                            <asp:ListItem>3:00 PM</asp:ListItem>
                            <asp:ListItem>3:30 PM</asp:ListItem>
                            <asp:ListItem>4:00 PM</asp:ListItem>
                            <asp:ListItem>4:30 PM</asp:ListItem>
                            <asp:ListItem>5:00 PM</asp:ListItem>
                            <asp:ListItem>5:30 PM</asp:ListItem>
                            <asp:ListItem>6:00 PM</asp:ListItem>
                            <asp:ListItem>6:30 PM</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Special:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_SpecialNotes" runat="server" Rows="3" TextMode="multiline" Width="360px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
            </fieldset>
            <fieldset class="EntryField">
                <legend>Scheduled Appointments</legend>
                <asp:Button ID="NewAppointment" OnClick="NewAppointmentClick" Text="New Appointment"
                    runat="server" />
                <asp:Button ID="Schedule" OnClick="ScheduleClick" Text="Schedule" runat="server" />
                <asp:Button ID="DeleteAppointmentRange" OnClick="DeleteAppointmentRangeClick" Text="Delete Range"
                    runat="server" />
                <asp:Panel ID="AppPanel" runat="server" Height="300px" ScrollBars="Vertical">
                    <div class="AppTable">
                        <asp:Table ID="AppTable" CssClass="AppTable" runat="server" />
                    </div>
                </asp:Panel>
            </fieldset>
            <fieldset class="EntryField">
                <legend>Property Information</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Square Footage:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_SquareFootage" runat="server" Width="100px" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Pets:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_Pets" CssClass="entryDropDownList" Width="100px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>1-2</asp:ListItem>
                            <asp:ListItem>3+</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Clean Rating:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_CleanRating" CssClass="entryDropDownList" Width="100px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>N/A</asp:ListItem>
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Newly Constructed:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NewBuilding" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Keys:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_RequiresKeys" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        How are we getting in?
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_EnterHome" CssClass="entryDropDownList" Width="210px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Will Be Home</asp:ListItem>
                            <asp:ListItem>Key Under Mat</asp:ListItem>
                            <asp:ListItem>Garage Code</asp:ListItem>
                            <asp:ListItem>Gate Code</asp:ListItem>
                            <asp:ListItem>Door Code</asp:ListItem>
                            <asp:ListItem>Key Code</asp:ListItem>
                            <asp:ListItem>Door Open</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Gate Code:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_GateCode" runat="server" Width="40px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Garage Code:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_GarageCode" runat="server" Width="40px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Door Code:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_DoorCode" runat="server" Width="40px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Take Before/After Pics:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="TakePic" runat="server" />

                    </div>



                </div>
            </fieldset>
            <fieldset class="EntryField" runat="server">
                <legend>Services:</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        <label id="HousekeepingLabel" runat="server">
                            <asp:CheckBox ID="HousekeepingCheckbox" OnClick="JsHousekeepingChanged()" runat="server" />Housekeeping</label>
                        <label id="CarpetCleaningLabel" runat="server">
                            <asp:CheckBox ID="CarpetCleaningCheckbox" OnClick="JsCarpetCleaningChanged()" runat="server" />Carpets</label>
                        <label id="WindowWashingLabel" runat="server">
                            <asp:CheckBox ID="WindowWashingCheckbox" OnClick="JsWindowWashingChanged()" runat="server" />Windows</label>
                        <br />
                        <label id="HomewatchLabel" runat="server">
                            <asp:CheckBox ID="HomewatchCheckbox" OnClick="JsHomewatchChanged()" runat="server" />Home Guard</label>
                    </div>
                </div>
            </fieldset>
            <fieldset id="HomewatchSection" class="EntryField" runat="server">
                <legend>Home Guard</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Frequency:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="HW_Frequency" CssClass="entryDropDownList" Width="150px" runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Once</asp:ListItem>
                            <asp:ListItem>Daily</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Bi-weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Clean Property:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="HW_CleanBeforeReturn" OnClick="JsCleanBeforeReturnChanged()" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Start:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_StartDate" runat="server" Width="120px" CssClass="entryTextBoxCenter"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_StartDate" TargetControlID="HW_StartDate" runat="server"></asp:CalendarExtender>
                    </div>
                    <div class="EntryValue">
                        Finish:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_EndDate" runat="server" Width="120px" CssClass="entryTextBoxCenter"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_EndDate" TargetControlID="HW_EndDate" runat="server"></asp:CalendarExtender>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 140px;">
                        Garbage Cans:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="HW_GarbageCans" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Garbage Day:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="HW_GarbageDay" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 140px;">
                        Plants Watered:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="HW_PlantsWatered" runat="server" />
                    </div>
                    <div class="EntryValue">
                        How Often:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_PlantsWateredFrequency" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 140px;">
                        Thermostat:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="HW_Thermostat" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Temperature:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_ThermostatTemperature" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 140px;">
                        Check Breakers:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="HW_Breakers" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Location:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_BreakersLocation" runat="server" Width="120px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Details:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="HW_Details" runat="server" Rows="5" TextMode="multiline" Width="350px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        <asp:Button ID="SendHWContract" Style="margin-top: 5px;" OnClick="SendHWContractClick" Text="Send Contract" runat="server" />
                    </div>
                    <div class="EntryValue">
                        <asp:Button ID="ViewHWContract" Enabled="false" Style="margin-top: 5px;" OnClick="ViewHWContractClick" Text="View Contract" runat="server" />
                    </div>
                </div>
            </fieldset>
            <fieldset id="HousekeepingSection" class="EntryField" runat="server">
                <legend>Housekeeping</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Bedrooms:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_Bedrooms" CssClass="entryDropDownList" Width="100px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Bathrooms:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_Bathrooms" CssClass="entryDropDownList" Width="100px" runat="server"
                            onchange="JsFormValueChanged(this)">
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Flooring Type:
                   
                    </div>
                    <div class="EntryValue" style="width: 80px; text-align: right">
                        Carpet
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringCarpet" runat="server" />
                    </div>
                    <div class="EntryValue">
                        , Hardwood
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringHardwood" runat="server" />
                    </div>
                    <div class="EntryValue">
                        , Tile
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringTile" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 198px; text-align: right">
                        Linoleum
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringLinoleum" runat="server" />
                    </div>
                    <div class="EntryValue">
                        , Slate
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringSlate" runat="server" />
                    </div>
                    <div class="EntryValue">
                        , Marble
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_FlooringMarble" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Do Dishes:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_DoDishes" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Change Bed Linens:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_ChangeBed" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Eco Cleaners Requested:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_RequestEcoCleaners" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Bring Vacuum:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_BringVacuum" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Cleaning Type:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="NC_CleaningType" CssClass="entryDropDownList" Width="150px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>General Clean</asp:ListItem>
                            <asp:ListItem>General Clean Plus</asp:ListItem>
                            <asp:ListItem>Deep Clean</asp:ListItem>
                            <asp:ListItem>Construction Clean</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="EntryValue">
                        Organize:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="NC_Organize" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 90px;">
                        Blinds:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Blinds" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="DC_BlindsAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Condition:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="DC_BlindsCondition" CssClass="entryDropDownList" Width="120px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Dusty</asp:ListItem>
                            <asp:ListItem>Greasy</asp:ListItem>
                            <asp:ListItem>Dusty/Greasy</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 90px;">
                        Windows:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Windows" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="DC_WindowsAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Tracks & Sills:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_WindowsSills" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 90px;">
                        Walls:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Walls" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Detail:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="DC_WallsDetail" CssClass="entryDropDownList" Width="150px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Spot Clean Only</asp:ListItem>
                            <asp:ListItem>All of Walls</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 90px;">
                        Ceiling Fans:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_CeilingFans" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="DC_CeilingFansAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Baseboards:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Baseboards" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Doors/Door Frames:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_DoorFrames" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Light Fixtures:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_LightFixtures" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Light Switches:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_LightSwitches" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Vent Covers:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_VentCovers" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Inside Vents:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_InsideVents" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Clean Pantry
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Pantry" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Clean Laundry Room
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_LaundryRoom" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 150px;">
                        Kitchen Cupboards:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_KitchenCuboards" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Detail:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="DC_KitchenCuboardsDetail" CssClass="entryDropDownList" Width="150px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Insides Only</asp:ListItem>
                            <asp:ListItem>Outsides Only</asp:ListItem>
                            <asp:ListItem>Insides and Outsides</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 150px;">
                        Bathroom Cupboards:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_BathroomCuboards" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Detail:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="DC_BathroomCuboardsDetail" CssClass="entryDropDownList" Width="150px"
                            runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem>Insides Only</asp:ListItem>
                            <asp:ListItem>Outsides Only</asp:ListItem>
                            <asp:ListItem>Insides and Outsides</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Oven:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Oven" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Refrigerator:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="DC_Refrigerator" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Other:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="DC_OtherOne" runat="server" Width="150px" CssClass="entryTextBox"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Other:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="DC_OtherTwo" runat="server" Width="150px" CssClass="entryTextBox"
                            onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Details:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="NC_Details" runat="server" Rows="5" TextMode="multiline" Width="350px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
            </fieldset>
            <fieldset id="CarpetCleaningSection" class="EntryField" runat="server">
                <legend>Carpet Cleaning</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Square Footage:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CC_SquareFootage" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Pet Odor Additive:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="CC_PetOdorAdditive" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Areas up to 200 sq/ft:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CC_RoomCountSmall" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        over 200 sq/ft:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CC_RoomCountLarge" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Details:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="CC_Details" runat="server" Rows="5" TextMode="multiline" Width="350px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
            </fieldset>
            <fieldset id="WindowWashingSection" class="EntryField" runat="server">
                <legend>Window Washing</legend>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Building Style:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_BuildingStyle" runat="server" Width="120px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Levels:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="WW_BuildingLevels" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Window Count:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_WindowCount" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                    <div class="EntryValue">
                        Window Type:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="WW_WindowType" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Wood</asp:ListItem>
                            <asp:ListItem>Vinyl</asp:ListItem>
                            <asp:ListItem>Aluminum</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Post Construction:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_PostConstruction" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Vaulted Ceilings:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_VaultedCeilings" runat="server" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Insides / Outsides:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:DropDownList ID="WW_InsidesOutsides" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Insides</asp:ListItem>
                            <asp:ListItem>Outsides</asp:ListItem>
                            <asp:ListItem>Both</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Razor Needed:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_Razor" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_RazorCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Hard Water:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_HardWater" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_HardWaterCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        French Windows:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_FrenchWindows" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_FrenchWindowCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        Storm Windows:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_StormWindows" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_StormWindowCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        Screens Washed / Dusted:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_Screens" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_ScreensCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        Tracks Cleaned:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_Tracks" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_TracksCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        Window Wells Cleaned:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_Wells" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Amount:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_WellsCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue" style="width: 180px;">
                        Gutters Cleaned:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:CheckBox ID="WW_Gutters" runat="server" />
                    </div>
                    <div class="EntryValue">
                        Linear Feet:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_GuttersFeet" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
                <div class="EntryLine">
                    <div class="EntryValue">
                        Details:
                   
                    </div>
                    <div class="EntryValue">
                        <asp:TextBox ID="WW_Details" runat="server" Rows="5" TextMode="multiline" Width="350px"
                            CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                    </div>
                </div>
            </fieldset>
        </div>

    </asp:Panel>
    <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete Customer" runat="server" />
    © 2015 2LocalGalsHouseKeeping
   
    <script language="javascript" type="text/javascript">
        $('*').click(function () {
            var yPos = window.pageYOffset || document.documentElement.scrollTop;
            $.cookie("CustomerScrollPos", yPos);
        });

        window.onload = function WindowLoad() {
            if (JsGetParameter("DoScroll") == 'Y') {
                var yPos = $.cookie("CustomerScrollPos");
                window.scrollTo(0, yPos);
            }
            JsShowHideBilling();
            JsHousekeepingChanged();
            JsCarpetCleaningChanged();
            JsWindowWashingChanged();
            JsHomewatchChanged();
        }
    </script>
</asp:Content>
