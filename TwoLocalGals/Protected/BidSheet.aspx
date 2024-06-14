<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="BidSheet.aspx.cs" Inherits="Nexus.Protected.BidSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script language="javascript" type="text/javascript">
        function JsEstimatePrice() {
            var hours = parseFloat(document.getElementById('<%=EstimatedHours.ClientID %>').value);
            if (isNaN(hours) || document.getElementById('TableRowHours').style.display == 'none') hours = 0;
            var rate = parseFloat(document.getElementById('<%=RatePerHour.ClientID %>').value.replace("$", ""));
            if (isNaN(rate)) rate = 0;
            var service = parseFloat(document.getElementById('<%=ServiceFee.ClientID %>').value.replace("$", ""));
            if (isNaN(service) || document.getElementById('TableRowServiceFee').style.display == 'none') service = 0;
            var cc = parseFloat(document.getElementById('<%=EstimatedCC.ClientID %>').value.replace("$", ""));
            if (isNaN(cc) || document.getElementById('TableRowCC').style.display == 'none') cc = 0;
            var ww = parseFloat(document.getElementById('<%=EstimatedWW.ClientID %>').value.replace("$", ""));
            if (isNaN(ww) || document.getElementById('TableRowWW').style.display == 'none') ww = 0;
            var hw = parseFloat(document.getElementById('<%=EstimatedHW.ClientID %>').value.replace("$", ""));
            if (isNaN(hw) || document.getElementById('TableRowHW').style.display == 'none') hw = 0;
            var total = ((hours * rate) + service + cc + ww + hw);
            document.getElementById('<%=EstimatedPrice.ClientID %>').value = isNaN(total) ? "" : "$" + total.toFixed(2);
        }

        function JsHousekeepingChanged() {
            var checked = document.getElementById('<%=HousekeepingCheckbox.ClientID %>').checked;
            document.getElementById('<%=HousekeepingSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('TableRowRPH').style.display = (checked ? 'table-row' : 'none');
            document.getElementById('TableRowServiceFee').style.display = (checked ? 'table-row' : 'none');
            document.getElementById('TableRowHours').style.display = (checked ? 'table-row' : 'none');
        }

        function JsCarpetCleaningChanged() {
            var checked = document.getElementById('<%=CarpetCleaningCheckbox.ClientID %>').checked;
            document.getElementById('<%=CarpetCleaningSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('TableRowCC').style.display = (checked ? 'table-row' : 'none');
        }

        function JsWindowWashingChanged() {
            var checked = document.getElementById('<%=WindowWashingCheckbox.ClientID %>').checked;
            document.getElementById('<%=WindowWashingSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('TableRowWW').style.display = (checked ? 'table-row' : 'none');
        }

        function JsHomewatchChanged() {
            var checked = document.getElementById('<%=HomewatchCheckbox.ClientID %>').checked;
            document.getElementById('<%=HomewatchSection.ClientID %>').style.display = (checked ? 'block' : 'none');
            document.getElementById('TableRowHW').style.display = (checked ? 'table-row' : 'none');
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
    </asp:ScriptManager>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="SaveButton" OnClick="SaveClick" Text="Save Changes" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitle">
        <asp:Label ID="CustomerTitleLabel" runat="server" />
    </div>
    <fieldset class="Entry" style="width: 900px;">
        <legend>General Questions</legend>
        <table class="BidSheet">
            <tr>
                <td class="BidSheetHeader">
                    Account Representative:
                </td>
                <td>
                    <asp:DropDownList ID="AccountRep" CssClass="entryDropDownList" Width="200px" runat="server"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What area are you in?
                </td>
                <td>
                    <asp:DropDownList ID="FranchiseList" CssClass="entryDropDownList" Width="200px" runat="server"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What is your first name?
                </td>
                <td>
                    <asp:TextBox ID="FirstName" runat="server" CssClass="entryTextBox" Width="200px"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What is your last name?
                </td>
                <td>
                    <asp:TextBox ID="LastName" runat="server" CssClass="entryTextBox" Width="200px"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How did you hear about us?
                </td>
                <td>
                    <asp:DropDownList ID="Advertisement" CssClass="entryDropDownList" Width="200px" runat="server"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Is this a home or a business?
                </td>
                <td>
                    <asp:DropDownList ID="AccountType" CssClass="entryDropDownList" Width="200px" runat="server"
                        onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Home</asp:ListItem>
                        <asp:ListItem>Business</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Is this building newly constructed?
                </td>
                <td>
                    <asp:CheckBox ID="NewBuilding" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What is the square footage?
                </td>
                <td>
                    <asp:TextBox ID="NC_SquareFootage" runat="server" Width="200px" CssClass="entryTextBoxCenter"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you have any pets? How Many?
                </td>
                <td>
                    <asp:DropDownList ID="NC_Pets" CssClass="entryDropDownList" Width="200px" runat="server"
                        onchange="JsFormValueChanged(this)">
                        <asp:ListItem>None</asp:ListItem>
                        <asp:ListItem>1-2</asp:ListItem>
                        <asp:ListItem>3+</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    On a scale of 1 to 10, 1 being really clean and 10 being really dirty?
                </td>
                <td>
                    <asp:DropDownList ID="NC_CleanRating" CssClass="entryDropDownList" Width="200px"
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
                </td>
            </tr>
        </table>
    </fieldset>
     <fieldset class="Entry" style="width: 900px;">
        <legend>Services</legend>
        <label id="HousekeepingLabel" runat="server"><asp:CheckBox ID="HousekeepingCheckbox" OnClick="JsHousekeepingChanged()" runat="server" />Housekeeping</label>
        <label id="CarpetCleaningLabel" runat="server"><asp:CheckBox ID="CarpetCleaningCheckbox" OnClick="JsCarpetCleaningChanged()" runat="server" />Carpets</label>
        <label id="WindowWashingLabel" runat="server"><asp:CheckBox ID="WindowWashingCheckbox" OnClick="JsWindowWashingChanged()" runat="server" />Windows</label>
        <label id="HomewatchLabel" runat="server"><asp:CheckBox ID="HomewatchCheckbox" OnClick="JsHomewatchChanged()" runat="server" />Home Guard</label>
    </fieldset>
        <fieldset class="Entry" style="width: 900px;">
        <legend>Estimated Price</legend>
        <table class="BidSheet">
            <tr id="TableRowRPH">
                <td class="BidSheetHeader">
                    Housekeeping Rate Per Hour:
                </td>
                <td>
                    <asp:DropDownList ID="RatePerHour" CssClass="entryDropDownList" Width="250px" runat="server"
                        onchange="JsFormValueChanged(this)">
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
                </td>
            </tr>
            <tr id="TableRowServiceFee">
                <td class="BidSheetHeader">
                    Housekeeping Service Fee:
                </td>
                <td>
                    <asp:DropDownList ID="ServiceFee" CssClass="entryDropDownList" Width="250px" runat="server"
                        onchange="JsFormValueChanged(this)">
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
                </td>
            </tr>
            <tr id="TableRowHours">
                <td class="BidSheetHeader">
                    Housekeeping Hours:
                </td>
                <td>
                    <asp:TextBox ID="EstimatedHours" runat="server" CssClass="entryTextBox" onchange="JsFormHoursChanged(this)" />
                </td>
            </tr>
            <tr id="TableRowCC">
                <td class="BidSheetHeader">
                    Carpet Cleaning:
                </td>
                <td>
                    <asp:TextBox ID="EstimatedCC" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
            <tr id="TableRowWW">
                <td class="BidSheetHeader">
                    Window Washing:
                </td>
                <td>
                    <asp:TextBox ID="EstimatedWW" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
            <tr id="TableRowHW">
                <td class="BidSheetHeader">
                    Home Guard:
                </td>
                <td>
                    <asp:TextBox ID="EstimatedHW" runat="server" CssClass="entryTextBox" onchange="JsFormMoneyChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Estimated Price:
                </td>
                <td>
                    <asp:TextBox ID="EstimatedPrice" runat="server" CssClass="entryTextBox" Width="170px" onchange="JsFormMoneyChanged(this)" />
                    <button type="button" onclick="JsEstimatePrice()">
                        Calculate</button>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="HomewatchSection" class="Entry" style="width: 900px; display:none;" runat="server">
        <legend>Home Guard</legend>
        <table class="BidSheet">
            <tr>
                <td class="BidSheetHeader">
                    How often do you need us to come?
                </td>
                <td>
                    <asp:DropDownList ID="HW_Frequency" CssClass="entryDropDownList" Width="150px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Once</asp:ListItem>
                        <asp:ListItem>Daily</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                        <asp:ListItem>Bi-weekly</asp:ListItem>
                        <asp:ListItem>Monthly</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    When did you need us to start?
                </td>
                <td>
                    <asp:TextBox ID="HW_StartDate" runat="server" Width="120px" CssClass="entryTextBoxCenter"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_StartDate" TargetControlID="HW_StartDate" runat="server">
                        </asp:CalendarExtender>
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader">
                    When did you need us to finish?
                </td>
                <td>
                    <asp:TextBox ID="HW_EndDate" runat="server" Width="120px" CssClass="entryTextBoxCenter"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_EndDate" TargetControlID="HW_EndDate" runat="server">
                        </asp:CalendarExtender>
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader">
                    Garbage Can(s) In/Out from Curb?
                </td>
                <td>
                    <asp:CheckBox ID="HW_GarbageCans" runat="server" />
                    Garbage Day:
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
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader">
                    Do you want your plants watered?
                </td>
                <td>
                    <asp:CheckBox ID="HW_PlantsWatered" runat="server" />
                    How Often:
                    <asp:TextBox ID="HW_PlantsWateredFrequency" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Check thermostat to ensure it is set for the correct temperature?
                </td>
                <td>
                    <asp:CheckBox ID="HW_Thermostat" runat="server" />
                    Temperature:
                    <asp:TextBox ID="HW_ThermostatTemperature" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you want us to check breaker box for any tripped breakers?
                </td>
                <td>
                    <asp:CheckBox ID="HW_Breakers" runat="server" />
                    Location:
                    <asp:TextBox ID="HW_BreakersLocation" runat="server" Width="120px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean property before you return?
                </td>
                <td>
                    <asp:CheckBox ID="HW_CleanBeforeReturn" OnClick="JsCleanBeforeReturnChanged()" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    Details:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="HW_Details" runat="server" Rows="8" TextMode="multiline" Width="890px" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="HousekeepingSection" class="Entry" style="width: 900px; display:none;" runat="server">
        <legend>Housekeeping</legend>
        <table class="BidSheet">
            <tr>
                <td class="BidSheetHeader">
                    Do we need to organize or just clean?
                </td>
                <td>
                    Organize<asp:CheckBox ID="NC_Organize" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many bedrooms?
                </td>
                <td>
                    <asp:DropDownList ID="NC_Bedrooms" CssClass="entryDropDownList" Width="200px" runat="server"
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
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many bathrooms?
                </td>
                <td>
                    <asp:DropDownList ID="NC_Bathrooms" CssClass="entryDropDownList" Width="200px" runat="server"
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
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you need us to do your dishes?
                </td>
                <td>
                    <asp:CheckBox ID="NC_DoDishes" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Should we change your bed linens?
                </td>
                <td>
                    <asp:CheckBox ID="NC_ChangeBed" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What kind of flooring is in the home?
                </td>
                <td>
                    Carpet
                    <asp:CheckBox ID="NC_FlooringCarpet" runat="server" />
                    , Hardwood
                    <asp:CheckBox ID="NC_FlooringHardwood" runat="server" />
                    , Tile
                    <asp:CheckBox ID="NC_FlooringTile" runat="server" />
                    <br />
                    Linoleum
                    <asp:CheckBox ID="NC_FlooringLinoleum" runat="server" />
                    , Slate
                    <asp:CheckBox ID="NC_FlooringSlate" runat="server" />
                    , Marble
                    <asp:CheckBox ID="NC_FlooringMarble" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    General clean or deep clean?
                </td>
                <td>
                    <asp:DropDownList ID="NC_CleaningType" CssClass="entryDropDownList" Width="200px"
                        runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>General Clean</asp:ListItem>
                        <asp:ListItem>General Clean Plus</asp:ListItem>
                        <asp:ListItem>Deep Clean</asp:ListItem>
                        <asp:ListItem>Construction Clean</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Blinds?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Blinds" runat="server" />
                    Amount:
                    <asp:TextBox ID="DC_BlindsAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                        onchange="JsFormValueChanged(this)" />
                    Condition:
                    <asp:DropDownList ID="DC_BlindsCondition" CssClass="entryDropDownList" Width="120px"
                        runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Dusty</asp:ListItem>
                        <asp:ListItem>Greasy</asp:ListItem>
                        <asp:ListItem>Dusty/Greasy</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Windows?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Windows" runat="server" />
                    Amount:
                    <asp:TextBox ID="DC_WindowsAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                        onchange="JsFormValueChanged(this)" />
                    Tracks & Sills:
                    <asp:CheckBox ID="DC_WindowsSills" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Walls?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Walls" runat="server" />
                    Detail:
                    <asp:DropDownList ID="DC_WallsDetail" CssClass="entryDropDownList" Width="150px"
                        runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Spot Clean Only</asp:ListItem>
                        <asp:ListItem>All of Walls</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Ceiling Fans?
                </td>
                <td>
                    <asp:CheckBox ID="DC_CeilingFans" runat="server" />
                    Amount:
                    <asp:TextBox ID="DC_CeilingFansAmount" runat="server" Width="60px" CssClass="entryTextBoxCenter"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Baseboards?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Baseboards" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Doors/Door Frames?
                </td>
                <td>
                    <asp:CheckBox ID="DC_DoorFrames" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Light Fixtures?
                </td>
                <td>
                    <asp:CheckBox ID="DC_LightFixtures" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Light Switches?
                </td>
                <td>
                    <asp:CheckBox ID="DC_LightSwitches" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Vent Covers?
                </td>
                <td>
                    <asp:CheckBox ID="DC_VentCovers" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Inside Vents?
                </td>
                <td>
                    <asp:CheckBox ID="DC_InsideVents" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Pantry?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Pantry" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Laundry Room?
                </td>
                <td>
                    <asp:CheckBox ID="DC_LaundryRoom" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Kitchen Cupboards?
                </td>
                <td>
                    <asp:CheckBox ID="DC_KitchenCuboards" runat="server" />
                    Detail:
                    <asp:DropDownList ID="DC_KitchenCuboardsDetail" CssClass="entryDropDownList" Width="150px"
                        runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Insides Only</asp:ListItem>
                        <asp:ListItem>Outsides Only</asp:ListItem>
                        <asp:ListItem>Insides and Outsides</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Bathroom Cupboards?
                </td>
                <td>
                    <asp:CheckBox ID="DC_BathroomCuboards" runat="server" />
                    Detail:
                    <asp:DropDownList ID="DC_BathroomCuboardsDetail" CssClass="entryDropDownList" Width="150px"
                        runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem>Insides Only</asp:ListItem>
                        <asp:ListItem>Outsides Only</asp:ListItem>
                        <asp:ListItem>Insides and Outsides</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Oven?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Oven" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Clean Refrigerator?
                </td>
                <td>
                    <asp:CheckBox ID="DC_Refrigerator" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Would you like us to use Eco Cleaners?
                </td>
                <td>
                    Eco Cleaners Requested<asp:CheckBox ID="NC_RequestEcoCleaners" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Other:
                </td>
                <td>
                    <asp:TextBox ID="DC_OtherOne" runat="server" Width="250px" CssClass="entryTextBox"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Other:
                </td>
                <td>
                    <asp:TextBox ID="DC_OtherTwo" runat="server" Width="250px" CssClass="entryTextBox"
                        onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader" colspan="2">
                    Details:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="NC_Details" runat="server" Rows="8" TextMode="multiline" Width="890px" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
        </table>
        <asp:TextBox ID="LocationState" Visible="false" runat="server" />
    </fieldset>
    <fieldset id="CarpetCleaningSection" class="Entry" style="width: 900px; display:none;" runat="server">
        <legend>Carpet Cleaning</legend>
        <table class="BidSheet">
            <tr>
                <td class="BidSheetHeader">
                    What is the square footage of the carpet needing to be cleaned?
                </td>
                <td>
                   <asp:TextBox ID="CC_SquareFootage" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many areas need to be cleaned, up to 200 sq/ft each?
                </td>
                <td>
                   <asp:TextBox ID="CC_RoomCountSmall" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many areas need to be cleaned, over 200 sq/ft each? 
                </td>
                <td>
                   <asp:TextBox ID="CC_RoomCountLarge" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you need a pet odor additive used to help eliminate pet odors?
                </td>
                <td>
                   <asp:CheckBox ID="CC_PetOdorAdditive" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    Details:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="CC_Details" runat="server" Rows="8" TextMode="multiline" Width="890px" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="WindowWashingSection" class="Entry" style="width: 900px; display:none;" runat="server">
        <legend>Window Washing</legend>
        <table class="BidSheet">
            <tr>
                <td class="BidSheetHeader">
                    What is the style of the building? (Ranch, Rambler, Office, Etc.)
                </td>
                <td>
                    <asp:TextBox ID="WW_BuildingStyle" runat="server" Width="120px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many levels is the building?
                </td>
                <td>
                    <asp:DropDownList ID="WW_BuildingLevels" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    How many windows do you need cleaned? (Window cleanings are based off of windows per side. Anything with a frame around it is considered a window)
                </td>
                <td>
                    <asp:TextBox ID="WW_WindowCount" runat="server" Width="80px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    What kind of windows?
                </td>
                <td>
                    <asp:DropDownList ID="WW_WindowType" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Wood</asp:ListItem>
                        <asp:ListItem>Vinyl</asp:ListItem>
                        <asp:ListItem>Aluminum</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Insides/Outsides?
                </td>
                <td>
                    <asp:DropDownList ID="WW_InsidesOutsides" CssClass="entryDropDownList" Width="100px" runat="server" onchange="JsFormValueChanged(this)">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Insides</asp:ListItem>
                        <asp:ListItem>Outsides</asp:ListItem>
                        <asp:ListItem>Both</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Are there any vaulted ceilings?
                </td>
                <td>
                    <asp:CheckBox ID="WW_VaultedCeilings" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Is this a post construction cleaning?
                </td>
                <td>
                    <asp:CheckBox ID="WW_PostConstruction" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do windows need a razor to remove paint, stucco, concrete or tape?
                </td>
                <td>
                    <asp:CheckBox ID="WW_Razor" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_RazorCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Does hard water need to be removed from any windows?
                </td>
                <td>
                    <asp:CheckBox ID="WW_HardWater" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_HardWaterCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Are there any French Windows? (Cut up windows)
                </td>
                <td>
                    <asp:CheckBox ID="WW_FrenchWindows" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_FrenchWindowCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Are there any storm windows?
                </td>
                <td>
                    <asp:CheckBox ID="WW_StormWindows" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_StormWindowCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you want your screens washed or dusted?
                </td>
                <td>
                    <asp:CheckBox ID="WW_Screens" runat="server" />
                    Amount
                    <asp:TextBox ID="WW_ScreensCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you want your tracks cleaned and detailed?
                </td>
                <td>
                    <asp:CheckBox ID="WW_Tracks" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_TracksCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you need any window wells cleaned out?
                </td>
                <td>
                    <asp:CheckBox ID="WW_Wells" runat="server" />
                    Amount:
                    <asp:TextBox ID="WW_WellsCount" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Do you need any gutters cleaned out?
                </td>
                <td>
                    <asp:CheckBox ID="WW_Gutters" runat="server" />
                    Linear Feet:
                    <asp:TextBox ID="WW_GuttersFeet" runat="server" Width="60px" CssClass="entryTextBoxCenter" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    Details:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="WW_Details" runat="server" Rows="8" TextMode="multiline" Width="890px" CssClass="entryTextBox" onchange="JsFormValueChanged(this)" />
                </td>
            </tr>
        </table>
    </fieldset>
    © 2015 2LocalGalsHouseKeeping
    <script language="javascript" type="text/javascript">

        window.onload = function WindowLoad() {
            JsHousekeepingChanged();
            JsCarpetCleaningChanged();
            JsWindowWashingChanged();
            JsHomewatchChanged();
        }

    </script>
</asp:Content>
