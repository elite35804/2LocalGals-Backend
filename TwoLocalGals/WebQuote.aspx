<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebQuote.aspx.cs" Inherits="TwoLocalGals.WebQuote" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2LG Web Quote</title>
    <link href="~/Styles/WebQuote.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
</head>
<body>
    <script language="javascript" type="text/javascript">

        window.onload = function WindowLoad() {
            JsHousekeepingChanged();
            JsCarpetCleaningChanged();
            JsWindowWashingChanged();
            JsHomewatchChanged();
        }

        function JsHousekeepingChanged() {
            var checked = document.getElementById('<%=HousekeepingCheckbox.ClientID %>').checked;
            document.getElementById('HousekeepingSection').style.display = (checked ? 'block' : 'none');
        }

        function JsCarpetCleaningChanged() {
            var checked = document.getElementById('<%=CarpetCleaningCheckbox.ClientID %>').checked;
            document.getElementById('CarpetCleaningSection').style.display = (checked ? 'block' : 'none');
        }

        function JsWindowWashingChanged() {
            var checked = document.getElementById('<%=WindowWashingCheckbox.ClientID %>').checked;
            document.getElementById('WindowWashingSection').style.display = (checked ? 'block' : 'none');
        }

        function JsHomewatchChanged() {
            var checked = document.getElementById('<%=HomewatchCheckbox.ClientID %>').checked;
            document.getElementById('HomewatchSection').style.display = (checked ? 'block' : 'none');
        }

        function JsCleanBeforeReturnChanged() {
            var checked = document.getElementById('<%=HW_CleanBeforeReturn.ClientID %>').checked;
            if (checked) {
                document.getElementById('<%=HousekeepingCheckbox.ClientID %>').checked = true;
                JsHousekeepingChanged();
            }
        }

    </script>
    <form id="MainForm" runat="server">
    <asp:ScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ScriptManager>
    <div class="Page">
        <img src="/2LG_Letterhead.png" alt="None" />
        <div class="ErrorMessage">
            <asp:Label ID="ErrorLabel" runat="server" CssClass="ErrorLabel" />
        </div>
        <fieldset style="text-align: left; margin: 30px; padding: 30px;">
            <legend id="FranchiseLegend" runat="server">2 Local Gals Web Quote</legend>
            <div class="ContactIcon">
                <img src="2LG_Contact.jpg" alt="none" />
            </div>
            <div class="FormData">
                <p>
                    First Name:
                    <asp:TextBox ID="FirstName" Width="150px" runat="server"></asp:TextBox>
                    <span class="Required">(Required)</span></p>
                <p>
                    Last Name:
                    <asp:TextBox ID="LastName" Width="150px" runat="server"></asp:TextBox>
                </p>
                <p>
                    Email:
                    <asp:TextBox ID="Email" Width="250px" runat="server"></asp:TextBox>
                    <span class="Required">(Required)</span>
                </p>
                <p>
                    Best Phone:
                    <asp:TextBox ID="BestPhone" Width="150px" runat="server"></asp:TextBox>
                    <span class="Required">(Required)</span>
                </p>
                <p>
                    Alternate Phone:
                    <asp:TextBox ID="AlternatePhone" Width="150px" runat="server"></asp:TextBox>
                </p>
                <p>
                    Best way to contact you:
                    <asp:DropDownList ID="PreferredContact" Width="147px" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Phone</asp:ListItem>
                        <asp:ListItem>Email</asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
                    Address:
                    <asp:TextBox ID="Address" Width="250px" runat="server"></asp:TextBox>
                </p>
                <p>
                    City:
                    <asp:TextBox ID="City" Width="150px" runat="server"></asp:TextBox>
                    <span class="Required">(Required)</span>
                </p>
                <p>
                    State:
                    <asp:TextBox ID="State" Width="150px" runat="server"></asp:TextBox>
                    Zip:
                    <asp:TextBox ID="Zip" CssClass="Center" Width="70px" MaxLength="5" runat="server"></asp:TextBox>
                </p>
                <p>
                    Location Type:
                    <asp:DropDownList ID="AccountType" Width="100px" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Home</asp:ListItem>
                        <asp:ListItem>Business</asp:ListItem>
                    </asp:DropDownList>
                    <label>Newly Constructed:<asp:CheckBox ID="NewBuilding" runat="server" /></label>
                </p>
                <p>
                    Approximate Square Footage:
                    <asp:TextBox ID="NC_SquareFootage" Width="70px" CssClass="Center" runat="server"></asp:TextBox>
                </p>
                <p>
                    Frequency:
                    <asp:DropDownList ID="NC_Frequency" Width="147px" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Onetime</asp:ListItem>
                        <asp:ListItem>As Needed</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                        <asp:ListItem>Bi-weekly</asp:ListItem>
                        <asp:ListItem>Every 3 Weeks</asp:ListItem>
                        <asp:ListItem>Monthly</asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
                    How did you hear about us:
                    <asp:DropDownList ID="Advertisement" Width="250px" runat="server" />
                </p>
                <p>
                    <span id="ServicesSpan" class="EntryCaption" runat="server">What services are you interested in?</span>
                </p>
                <p>
                    <label id="HomewatchLabel" runat="server"><asp:CheckBox ID="HomewatchCheckbox" OnClick="JsHomewatchChanged()" runat="server" />Home Guard</label>
                    <label id="HousekeepingLabel" runat="server"><asp:CheckBox ID="HousekeepingCheckbox" OnClick="JsHousekeepingChanged()" runat="server" />Housekeeping</label>
                    <label id="CarpetCleaningLabel" runat="server"><asp:CheckBox ID="CarpetCleaningCheckbox" OnClick="JsCarpetCleaningChanged()" runat="server" />Carpet Cleaning</label>
                    <label id="WindowWashingLabel" runat="server"><asp:CheckBox ID="WindowWashingCheckbox" OnClick="JsWindowWashingChanged()" runat="server" />Window Washing</label>
                </p>
                <div id="HomewatchSection" style="display:none;">
                    <p>
                        <span class="EntryCaption">Home Guard Questions</span>
                    </p>
                    <p>
                        How often do you need us to come?
                        <asp:DropDownList ID="HW_Frequency" Width="147px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Once</asp:ListItem>
                            <asp:ListItem>Daily</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Bi-weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        When did you need us to start?
                        <asp:TextBox ID="HW_StartDate" runat="server" CssClass="entryTextBox"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_StartDate" TargetControlID="HW_StartDate" runat="server">
                        </asp:CalendarExtender>
                    </p>
                    <p>
                        When did you need us to finish?
                        <asp:TextBox ID="HW_EndDate" runat="server" CssClass="entryTextBox"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtenderHW_EndDate" TargetControlID="HW_EndDate" runat="server">
                        </asp:CalendarExtender>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="HW_GarbageCans" runat="server" />Garbage Can(s) In/Out from Curb</label>
                        : Garbage Day
                        <asp:DropDownList ID="HW_GarbageDay" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="HW_PlantsWatered" runat="server" />Plants Watered</label>
                        : How Often
                        <asp:TextBox ID="HW_PlantsWateredFrequency" runat="server" Width="120px" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="HW_Thermostat" runat="server" />Check Thermostat</label>
                        : Temperature
                        <asp:TextBox ID="HW_ThermostatTemperature" runat="server" Width="60px" CssClass="Center" />
                    </p>
                        <p>
                        <label><asp:CheckBox ID="HW_Breakers" runat="server" />Check for Tripped Breakers</label>
                        : Location
                        <asp:TextBox ID="HW_BreakersLocation" runat="server" Width="120px" CssClass="Center" />
                    </p>
                    <p>
                        <label>Clean property before you return (If yes, fill out housekeeping form):<asp:CheckBox ID="HW_CleanBeforeReturn" OnClick="JsCleanBeforeReturnChanged()" runat="server" /></label>
                    </p>
                    <p>
                        Home Guard (Details / Comments)
                        <asp:TextBox ID="HW_Details" runat="server" Rows="8" TextMode="multiline" Width="450px" />
                    </p>
                </div>
                <div id="HousekeepingSection" style="display:none;" runat="server">
                    <p>
                        <span class="EntryCaption">Housekeeping General Cleaning Questions</span>
                    </p>
                    <p>
                        How clean is the location: (1 = Very Clean, 10 = Very Dirty)
                        <asp:DropDownList ID="NC_CleanRating" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>N/A</asp:ListItem>
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
                    </p>
                    <p>
                        <label>Eco Cleaners Requested:<asp:CheckBox ID="NC_RequestEcoCleaners" runat="server" /></label>
                    </p>
                    <p>
                        Bedrooms:
                        <asp:DropDownList ID="NC_Bedrooms" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
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
                        , Bathrooms:
                        <asp:DropDownList ID="NC_Bathrooms" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
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
                    </p>
                    <p>
                        Pets (Dogs / Cats):
                        <asp:DropDownList ID="NC_Pets" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem>1-2</asp:ListItem>
                            <asp:ListItem>3+</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        Flooring:
                        <label><asp:CheckBox ID="NC_FlooringCarpet" runat="server" />Carpet ,</label>
                        <label><asp:CheckBox ID="NC_FlooringHardwood" runat="server" />Hardwood ,</label>
                        <label><asp:CheckBox ID="NC_FlooringTile" runat="server" />Tile ,</label>
                        <label><asp:CheckBox ID="NC_FlooringLinoleum" runat="server" />Linoleum ,</label>
                        <label><asp:CheckBox ID="NC_FlooringSlate" runat="server" />Slate ,</label>
                        <label><asp:CheckBox ID="NC_FlooringMarble" runat="server" />Marble</label>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="NC_DoDishes" runat="server" />Do Dishes,</label>
                        <label><asp:CheckBox ID="NC_ChangeBed" runat="server" />Change Bed Linens</label>
                    </p>
                    <p>
                        <span class="EntryCaption">Housekeeping Deep Cleaning Questions</span>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Blinds" runat="server" />Blinds</label>
                        : Count
                        <asp:TextBox ID="DC_BlindsAmount" runat="server" Width="60px" CssClass="Center" />
                        , Condition
                        <asp:DropDownList ID="DC_BlindsCondition" Width="120px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Dusty</asp:ListItem>
                            <asp:ListItem>Greasy</asp:ListItem>
                            <asp:ListItem>Dusty/Greasy</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Windows" runat="server" />Windows</label>
                        : Count
                        <asp:TextBox ID="DC_WindowsAmount" runat="server" CssClass="Center" Width="60px" />
                        ,
                        <label><asp:CheckBox ID="DC_WindowsSills" runat="server" />Tracks & Sills</label>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Walls" runat="server" />Walls</label>
                        : Detail
                        <asp:DropDownList ID="DC_WallsDetail" Width="150px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Spot Clean Only</asp:ListItem>
                            <asp:ListItem>All of Walls</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_CeilingFans" runat="server" />Fans</label>
                        : Count
                        <asp:TextBox ID="DC_CeilingFansAmount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_KitchenCuboards" runat="server" />Kitchen Cupboards and Drawers</label>
                        : Detail
                        <asp:DropDownList ID="DC_KitchenCuboardsDetail" Width="150px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Insides Only</asp:ListItem>
                            <asp:ListItem>Outsides Only</asp:ListItem>
                            <asp:ListItem>Insides and Outsides</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_BathroomCuboards" runat="server" />Bathroom Cupboards and Drawers</label>
                        : Detail
                        <asp:DropDownList ID="DC_BathroomCuboardsDetail" Width="150px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Insides Only</asp:ListItem>
                            <asp:ListItem>Outsides Only</asp:ListItem>
                            <asp:ListItem>Insides and Outsides</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Pantry" runat="server" />Clean Pantry,</label>
                        <label><asp:CheckBox ID="DC_LaundryRoom" runat="server" />Clean Laundry Room</label>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Baseboards" runat="server" />Baseboards,</label>
                        <label><asp:CheckBox ID="DC_DoorFrames" runat="server" />Doors/Door Frames,</label>
                        <label><asp:CheckBox ID="DC_VentCovers" runat="server" />Vent Covers</label>
                        <label><asp:CheckBox ID="DC_InsideVents" runat="server" />Inside Vents</label>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_LightSwitches" runat="server" />Light Switches,</label>
                        <label><asp:CheckBox ID="DC_LightFixtures" runat="server" />Light Fixtures</label>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="DC_Refrigerator" runat="server" />Refrigerator,</label>
                        <label><asp:CheckBox ID="DC_Oven" runat="server" />Oven</label>
                    </p>
                    <p>
                        Other:
                        <asp:TextBox ID="DC_OtherOne" Width="350px" runat="server" />
                    </p>
                    <p>
                        Other:
                        <asp:TextBox ID="DC_OtherTwo" Width="350px" runat="server" />
                    </p>
                    <p>
                        Housekeeping (Details / Comments):
                        <asp:TextBox ID="NC_Details" runat="server" Rows="8" TextMode="multiline" Width="450px" />
                    </p>
                </div>
                <div id="CarpetCleaningSection" style="display:none;">
                    <p>
                        <span class="EntryCaption">Carpet Cleaning Questions</span>
                    </p>
                    <p>
                        Approximate Square Footage of Carpet:
                        <asp:TextBox ID="CC_SquareFootage" Width="70px" CssClass="Center" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        How many areas need to be cleaned, up to 200 sq/ft each? 
                        <asp:DropDownList ID="CC_RoomCountSmall" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
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
                    </p>
                    <p>
                        How many areas need to be cleaned, over 200 sq/ft each?
                        <asp:DropDownList ID="CC_RoomCountLarge" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
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
                    </p>
                    <p>
                        <label>Pet odor additive used to help eliminate pet odors:<asp:CheckBox ID="CC_PetOdorAdditive" runat="server" /></label>
                    </p>
                    <p>
                        Carpet Cleaning (Details / Comments):
                        <asp:TextBox ID="CC_Details" runat="server" Rows="8" TextMode="multiline" Width="450px" />
                    </p>
                </div>
                    <div id="WindowWashingSection" style="display:none;">
                    <p>
                        <span class="EntryCaption">Window Washing Questions</span>
                    </p>
                    <p>
                        Building Style (Ranch, Rambler, Office, Etc.): 
                        <asp:TextBox ID="WW_BuildingStyle" Width="150px" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        Building Levels:
                        <asp:DropDownList ID="WW_BuildingLevels" Width="100px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label>Vaulted Ceilings:<asp:CheckBox ID="WW_VaultedCeilings" runat="server" /></label>
                    </p>
                    <p>
                        <label>Is this a post construction cleaning?<asp:CheckBox ID="WW_PostConstruction" runat="server" /></label>
                    </p>
                    <p>
                        (Window cleanings are based off of windows per side.<br /> 
                        Anything with a frame around it is considered a window)
                    </p>
                    <p>
                        How many windows do you need cleaned? 
                        <asp:TextBox ID="WW_WindowCount" Width="70px" CssClass="Center" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        What kind of windows? (Vinyl, Metal, Etc.)
                        <asp:DropDownList ID="WW_WindowType" Width="150px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Wood</asp:ListItem>
                            <asp:ListItem>Vinyl</asp:ListItem>
                            <asp:ListItem>Aluminum</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        Insides / Outsides
                        <asp:DropDownList ID="WW_InsidesOutsides" Width="150px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Insides</asp:ListItem>
                            <asp:ListItem>Outsides</asp:ListItem>
                            <asp:ListItem>Both</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_Razor" runat="server" />Do windows need a razor to remove paint, stucco, or tape?</label>
                        : Count
                        <asp:TextBox ID="WW_RazorCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_HardWater" runat="server" />Does hard water need to be removed from any windows?</label>
                        : Count
                        <asp:TextBox ID="WW_HardWaterCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_FrenchWindows" runat="server" />French Windows</label>
                        : Count
                        <asp:TextBox ID="WW_FrenchWindowCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_StormWindows" runat="server" />Storm Windows</label>
                        : Count
                        <asp:TextBox ID="WW_StormWindowCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_Screens" runat="server" />Screens Washed or Dusted</label>
                        : Count
                        <asp:TextBox ID="WW_ScreensCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_Tracks" runat="server" />Tracks Cleaned and Detailed</label>
                        : Count
                        <asp:TextBox ID="WW_TracksCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_Wells" runat="server" />Window Wells Cleaned</label>
                        : Count
                        <asp:TextBox ID="WW_WellsCount" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        <label><asp:CheckBox ID="WW_Gutters" runat="server" />Gutters Cleaned</label>
                        : Linear Feet
                        <asp:TextBox ID="WW_GuttersFeet" runat="server" Width="60px" CssClass="Center" />
                    </p>
                    <p>
                        Window Washing (Details / Comments):
                        <asp:TextBox ID="WW_Details" runat="server" Rows="8" TextMode="multiline" Width="450px" />
                    </p>
                </div>
                <asp:Button ID="SubmitButton" Text="Submit" OnClick="SubmitClick" CssClass="SubmitButton"
                    runat="server" />
                <asp:Button ID="CancelButton" Text="Cancel" OnClick="CancelClick" CssClass="SubmitButton"
                    runat="server" />
            </div>
        </fieldset>
    </div>
    </form>
    <!-- Google Tag Manager -->
    <noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-TQLF5BS"
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
    new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
    j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
    '//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
    })(window,document,'script','dataLayer','GTM-TQLF5BS');</script>
    <!-- End Google Tag Manager -->
</body>
</html>
