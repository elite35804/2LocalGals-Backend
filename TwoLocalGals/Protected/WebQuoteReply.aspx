<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="WebQuoteReply.aspx.cs" Inherits="TwoLocalGals.Protected.WebQuoteReply" EnableEventValidation="false" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">

        function JsTemplateChanged() {
            var t = document.getElementById('<%=Templates.ClientID%>');
            document.getElementById('<%=QuoteTemplateName.ClientID%>').value = t.options[t.selectedIndex].text;
        }

        function JsAddContents() {
            var text = atob(document.getElementById('<%=Templates.ClientID%>').value).replace(/\r?\n|\n|\0/g, "");
            document.getElementById('ReplyTextBox').value += text;
        }

        function JsInsertContents() {
            var text = atob(document.getElementById('<%=Templates.ClientID%>').value).replace(/\r?\n|\n|\0/g, "");
            insertAtCaret("ReplyTextBox", text);
        }

        function JsReplaceContents() {
            var text = atob(document.getElementById('<%=Templates.ClientID%>').value).replace(/\r?\n|\n|\0/g, "");
            document.getElementById('ReplyTextBox').value = text;
        }

        function insertAtCaret(areaId, text) {
            var txtarea = document.getElementById(areaId);
            var scrollPos = txtarea.scrollTop;
            var strPos = 0;
            var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ? "ff" : (document.selection ? "ie" : false));
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                strPos = range.text.length;
            }
            else if (br == "ff") strPos = txtarea.selectionStart;

            var front = (txtarea.value).substring(0, strPos);
            var back = (txtarea.value).substring(strPos, txtarea.value.length);
            txtarea.value = front + text + back;
            strPos = strPos + text.length;
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                range.moveStart('character', strPos);
                range.moveEnd('character', 0);
                range.select();
            }
            else if (br == "ff") {
                txtarea.selectionStart = strPos;
                txtarea.selectionEnd = strPos;
                txtarea.focus();
            }
            txtarea.scrollTop = scrollPos;
        }

    </script>
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="SendButton" Text="Send Quote" OnClick="SendClick" runat="server" />
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
        <legend>Quote Info</legend>
        <p>
            <span class="WebQuoteHeader">Best Phone: </span>
            <asp:Label ID="BestPhone" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Alternate Phone: </span>
            <asp:Label ID="AlternatePhone" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Address: </span>
            <asp:Label ID="Address" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                City: </span>
            <asp:Label ID="City" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                State: </span>
            <asp:Label ID="State" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Zip: </span>
            <asp:Label ID="Zip" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Account Type: </span><asp:Label ID="AccountType" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Newly Constructed: </span><asp:Label ID="NewBuilding" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">Square Footage: </span><asp:Label ID="SquareFootage" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">Advertisement: </span><asp:Label ID="Advertisement" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Preferred Contact: </span><asp:Label ID="PreferredContact" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">Frequency: </span><asp:Label ID="Frequency" class="WebQuoteValue" runat="server" />
        </p>
    </fieldset>
    <fieldset id="HousekeepingSection" class="Entry" style="width: 900px;" runat="server">
        <legend>Housekeeping</legend>
                <p>
            <span class="WebQuoteHeader">Clean Rating: </span>
            <asp:Label ID="CleanRating" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Bedrooms: </span>
            <asp:Label ID="Bedrooms" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Bathrooms: </span>
            <asp:Label ID="Bathrooms" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Pets: </span>
            <asp:Label ID="Pets" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Eco Cleaners Requested: </span>
            <asp:Label ID="RequestEcoCleaners" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Flooring: </span>
            <asp:Label ID="Flooring" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Do Dishes: </span>
            <asp:Label ID="DoDishes" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Change Bed Linens: </span>
            <asp:Label ID="ChangeBed" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Blinds: </span>
            <asp:Label ID="Blinds" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Windows: </span>
            <asp:Label ID="Windows" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Walls: </span>
            <asp:Label ID="Walls" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Fans: </span>
            <asp:Label ID="Fans" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Kitchen Cupboards: </span>
            <asp:Label ID="KitchenCupboards" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Bathroom Cupboards: </span>
            <asp:Label ID="BathroomCupboards" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Pantry: </span>
            <asp:Label ID="Pantry" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">Laundry Room: </span>
            <asp:Label ID="LaundryRoom" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Baseboards: </span>
            <asp:Label ID="Baseboards" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">
                Door Frames: </span>
            <asp:Label ID="DoorFrames" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">
                Vent Covers: </span>
            <asp:Label ID="VentCovers" class="WebQuoteValue" runat="server" />
            <span class="WebQuoteHeader">
                Inside Vents: </span>
            <asp:Label ID="InsideVents" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Light Switches: </span>
            <asp:Label ID="LightSwitches" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Light Fixtures: </span>
            <asp:Label ID="LightFixtures" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Refrigerator: </span>
            <asp:Label ID="Refrigerator" class="WebQuoteValue" runat="server" />, <span class="WebQuoteHeader">
                Oven: </span>
            <asp:Label ID="Oven" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Other One: </span><asp:Label ID="OtherOne" class="WebQuoteValue" runat="server" />, 
            <span class="WebQuoteHeader">Other Two: </span><asp:Label ID="OtherTwo" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Housekeeping (Details / Comments): </span>
            <asp:Label ID="NC_Details" class="WebQuoteValue" runat="server" />
        </p>
    </fieldset>
    <fieldset id="CarpetCleaningSection" class="Entry" style="width: 900px;" runat="server">
        <legend>Carpet Cleaning</legend>
        <p>
            <span class="WebQuoteHeader">Carpets Square Footage: </span><asp:Label ID="CC_SquareFootage" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Pet Odor Additive: </span><asp:Label ID="CC_PetOdorAdditive" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Areas up to 200 sq/ft each: </span><asp:Label ID="CC_RoomCountSmall" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Areas over 200 sq/ft each: </span><asp:Label ID="CC_RoomCountLarge" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Carpet Cleaning (Details / Comments): </span>
            <asp:Label ID="CC_Details" class="WebQuoteValue" runat="server" />
        </p>
    </fieldset>
    <fieldset id="WindowWashingSection" class="Entry" style="width: 900px;" runat="server">
        <legend>Window Washing</legend>
        <p>
            <span class="WebQuoteHeader">Building Style: </span><asp:Label ID="WW_BuildingStyle" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Building Levels: </span><asp:Label ID="WW_BuildingLevels" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Post Construction: </span><asp:Label ID="WW_PostConstruction" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Vaulted Ceilings: </span><asp:Label ID="WW_VaultedCeilings" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Window Count: </span><asp:Label ID="WW_WindowCount" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Window Type: </span><asp:Label ID="WW_WindowType" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Insides/Outsides: </span><asp:Label ID="WW_InsidesOutsides" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Need Razor: </span><asp:Label ID="WW_RazorCount" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Hard Water: </span><asp:Label ID="WW_HardWaterCount" class="WebQuoteValue" runat="server" />
            <span class="WebQuoteHeader">French Windows: </span><asp:Label ID="WW_FrenchWindows" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Storm Windows: </span><asp:Label ID="WW_StormWindows" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Window Screens: </span><asp:Label ID="WW_Screens" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Window Tracks: </span><asp:Label ID="WW_Tracks" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Window Wells: </span><asp:Label ID="WW_Wells" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Gutters: </span><asp:Label ID="WW_Gutters" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Window Washing (Details / Comments): </span>
            <asp:Label ID="WW_Details" class="WebQuoteValue" runat="server" />
        </p>
    </fieldset>
     <fieldset id="HomewatchSection" class="Entry" style="width: 900px;" runat="server">
        <legend>Home Guard</legend>
        <p>
            <span class="WebQuoteHeader">Frequency: </span><asp:Label ID="HW_Frequency" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Start Date: </span><asp:Label ID="HW_StartDate" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Finish Date: </span><asp:Label ID="HW_EndDate" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Clean Before Return: </span><asp:Label ID="HW_CleanBeforeReturn" class="WebQuoteValue" runat="server" />,
        </p>
        <p>
            <span class="WebQuoteHeader">Garbage Cans: </span><asp:Label ID="HW_GarbageCans" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Plants Watered: </span><asp:Label ID="HW_PlantsWatered" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Thermostat: </span><asp:Label ID="HW_Thermostat" class="WebQuoteValue" runat="server" />,
            <span class="WebQuoteHeader">Breakers: </span><asp:Label ID="HW_Breakers" class="WebQuoteValue" runat="server" />
        </p>
        <p>
            <span class="WebQuoteHeader">Home Guard (Details / Comments): </span>
            <asp:Label ID="HW_Details" class="WebQuoteValue" runat="server" />
        </p>
    </fieldset>
    <fieldset class="Entry" style="width: 900px;">
        <legend>Email Reply</legend>
        <p>You can use HTML and CSS to customize your promotional email. You may also include pre-defined tags that will populate with our company's and the customer's information when sent. The list of tags you can use is as follows [CompanyLogo] [LetterheadLogo] [CustomerFirstName] [CustomerLastName] [CustomerFullName] [FranchiseName] [FranchisePhone] [FranchiseWebsite] [FranchiseEmail] [FranchiseAddress] [FranchiseCity] [FranchiseState] [FranchiseZip].</p>
        <p>
            Select Template: <asp:DropDownList ID="Templates" class="chzn-select" Width="450px" onchange="JsTemplateChanged()" runat="server" />
            <button type="button" onclick="JsAddContents()">Add</button>
            <button type="button" onclick="JsInsertContents()">Insert</button>
        </p>
        <p>
            Save Template As: <asp:TextBox ID="QuoteTemplateName" Width="450px" runat="server" />
            <asp:Button ID="SaveTemplateButton" Text="Save Template" OnClick="SaveQuoteClick" runat="server" />
            <asp:Button ID="DeleteTempalteButton" Text="Delete Template" OnClick="DeleteQuoteClick" runat="server" />
        </p>
        <p>
            <asp:TextBox ID="ReplyTextBox" ClientIDMode="Static" style="overflow:scroll;" Rows="25" TextMode="multiline" Width="890px" runat="server" />
        </p>
    </fieldset>
    <asp:Button ID="DeleteButton" OnClick="DeleteClick" Text="Delete Web Quote" runat="server" />
    © 2015 2LocalGalsHouseKeeping
</asp:Content>
