using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals
{
    public partial class WebQuote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (!string.IsNullOrEmpty(Request["title"])) FranchiseLegend.InnerText = Request["title"];
                else FranchiseLegend.InnerText = "Salt Lake - Herriman Web Quote";

                Advertisement.Items.Clear();
                Advertisement.Items.Add(new ListItem());
                foreach (string i in Database.GetFranchiseDropDown(-1, "advertisementList"))
                    Advertisement.Items.Add(i);

                int franchiseID = Globals.SafeIntParse(Request["franID"]);
                int serviceMask = Database.GetFranchiseServiceMask(Globals.IDToMask(franchiseID));
                if ((serviceMask & 2) == 0) CarpetCleaningLabel.Visible = false;
                if ((serviceMask & 4) == 0) WindowWashingLabel.Visible = false;
                if ((serviceMask & 8) == 0) HomewatchLabel.Visible = false;
                if (serviceMask <= 1)
                {
                    ServicesSpan.Visible = false;
                    HousekeepingCheckbox.Checked = true;
                    HousekeepingSection.Style["display"] = "block";
                    HousekeepingLabel.Visible = false;
                }

                SubmitButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(SubmitButton, null) + ";");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void SubmitClick(object sender, EventArgs e)
        {
            try
            {
                int franchiseID = Globals.SafeIntParse(Request["franID"]);

                CustomerStruct customer = new CustomerStruct();
                customer.accountStatus = "Web Quote";
                customer.franchiseMask = Globals.IDToMask(franchiseID);
                customer.firstName = FirstName.Text;
                customer.lastName = LastName.Text;
                customer.email = Email.Text;
                customer.bestPhone = Globals.FormatPhone(BestPhone.Text);
                customer.alternatePhoneOne = Globals.FormatPhone(AlternatePhone.Text);
                customer.locationAddress = Address.Text;
                customer.locationCity = City.Text;
                customer.locationState = State.Text;
                customer.locationZip = Zip.Text;
                customer.accountType = AccountType.Text;
                customer.newBuilding = NewBuilding.Checked;
                customer.advertisement = Advertisement.Text;
                customer.preferredContact = PreferredContact.Text;
                customer.NC_Frequency = NC_Frequency.Text;

                customer.NC_CleanRating = NC_CleanRating.Text;
                customer.NC_Bedrooms = NC_Bedrooms.Text;
                customer.NC_Bathrooms = NC_Bathrooms.Text;
                customer.NC_SquareFootage = NC_SquareFootage.Text;
                customer.NC_Pets = NC_Pets.Text;
                customer.NC_FlooringCarpet = NC_FlooringCarpet.Checked;
                customer.NC_FlooringHardwood = NC_FlooringHardwood.Checked;
                customer.NC_FlooringTile = NC_FlooringTile.Checked;
                customer.NC_FlooringLinoleum = NC_FlooringLinoleum.Checked;
                customer.NC_FlooringSlate = NC_FlooringSlate.Checked;
                customer.NC_FlooringMarble = NC_FlooringMarble.Checked;
                customer.NC_Details = NC_Details.Text;
                customer.NC_DoDishes = NC_DoDishes.Checked;
                customer.NC_ChangeBed = NC_ChangeBed.Checked;
                customer.NC_RequestEcoCleaners = NC_RequestEcoCleaners.Checked;

                customer.DC_Blinds = DC_Blinds.Checked;
                customer.DC_BlindsAmount = DC_BlindsAmount.Text;
                customer.DC_BlindsCondition = DC_BlindsCondition.Text;
                customer.DC_Windows = DC_Windows.Checked;
                customer.DC_WindowsAmount = DC_WindowsAmount.Text;
                customer.DC_WindowsSills = DC_WindowsSills.Checked;
                customer.DC_Walls = DC_Walls.Checked;
                customer.DC_WallsDetail = DC_WallsDetail.Text;
                customer.DC_CeilingFans = DC_CeilingFans.Checked;
                customer.DC_CeilingFansAmount = DC_CeilingFansAmount.Text;
                customer.DC_KitchenCuboards = DC_KitchenCuboards.Checked;
                customer.DC_KitchenCuboardsDetail = DC_KitchenCuboardsDetail.Text;
                customer.DC_BathroomCuboards = DC_BathroomCuboards.Checked;
                customer.DC_BathroomCuboardsDetail = DC_BathroomCuboardsDetail.Text;
                customer.DC_Baseboards = DC_Baseboards.Checked;
                customer.DC_DoorFrames = DC_DoorFrames.Checked;
                customer.DC_VentCovers = DC_VentCovers.Checked;
                customer.DC_InsideVents = DC_InsideVents.Checked;
                customer.DC_Pantry = DC_Pantry.Checked;
                customer.DC_LaundryRoom = DC_LaundryRoom.Checked;
                customer.DC_LightSwitches = DC_LightSwitches.Checked;
                customer.DC_LightFixtures = DC_LightFixtures.Checked;
                customer.DC_Refrigerator = DC_Refrigerator.Checked;
                customer.DC_Oven = DC_Oven.Checked;
                customer.DC_OtherOne = DC_OtherOne.Text;
                customer.DC_OtherTwo = DC_OtherTwo.Text;

                customer.CC_SquareFootage = CC_SquareFootage.Text;
                customer.CC_RoomCountSmall = CC_RoomCountSmall.Text;
                customer.CC_RoomCountLarge = CC_RoomCountLarge.Text;
                customer.CC_PetOdorAdditive = CC_PetOdorAdditive.Checked;
                customer.CC_Details = CC_Details.Text;
                customer.WW_BuildingStyle = WW_BuildingStyle.Text;
                customer.WW_BuildingLevels = WW_BuildingLevels.Text;
                customer.WW_VaultedCeilings = WW_VaultedCeilings.Checked;
                customer.WW_WindowCount = WW_WindowCount.Text;
                customer.WW_WindowType = WW_WindowType.Text;
                customer.WW_InsidesOutsides = WW_InsidesOutsides.Text;
                customer.WW_Razor = WW_Razor.Checked;
                customer.WW_RazorCount = WW_RazorCount.Text;
                customer.WW_HardWater = WW_HardWater.Checked;
                customer.WW_HardWaterCount = WW_HardWaterCount.Text;
                customer.WW_FrenchWindows = WW_FrenchWindows.Checked;
                customer.WW_FrenchWindowCount = WW_FrenchWindowCount.Text;
                customer.WW_StormWindows = WW_StormWindows.Checked;
                customer.WW_StormWindowCount = WW_StormWindowCount.Text;
                customer.WW_Screens = WW_Screens.Checked;
                customer.WW_ScreensCount = WW_ScreensCount.Text;
                customer.WW_Tracks = WW_Tracks.Checked;
                customer.WW_TracksCount = WW_TracksCount.Text;
                customer.WW_Wells = WW_Wells.Checked;
                customer.WW_WellsCount = WW_WellsCount.Text;
                customer.WW_Gutters = WW_Gutters.Checked;
                customer.WW_GuttersFeet = WW_GuttersFeet.Text;
                customer.WW_Details = WW_Details.Text;
                customer.HW_Frequency = HW_Frequency.Text;
                customer.HW_StartDate = HW_StartDate.Text;
                customer.HW_EndDate = HW_EndDate.Text;
                customer.HW_GarbageCans = HW_GarbageCans.Checked;
                customer.HW_GarbageDay = HW_GarbageDay.Text;
                customer.HW_PlantsWatered = HW_PlantsWatered.Checked;
                customer.HW_PlantsWateredFrequency = HW_PlantsWateredFrequency.Text;
                customer.HW_Thermostat = HW_Thermostat.Checked;
                customer.HW_ThermostatTemperature = HW_ThermostatTemperature.Text;
                customer.HW_Breakers = HW_Breakers.Checked;
                customer.HW_BreakersLocation = HW_BreakersLocation.Text;
                customer.HW_CleanBeforeReturn = HW_CleanBeforeReturn.Checked;
                customer.HW_Details = HW_Details.Text;

                customer.sectionMask = 0;
                if (HousekeepingCheckbox.Checked) customer.sectionMask |= 1;
                if (CarpetCleaningCheckbox.Checked) customer.sectionMask |= 2;
                if (WindowWashingCheckbox.Checked) customer.sectionMask |= 4;
                if (HomewatchCheckbox.Checked) customer.sectionMask |= 8;

                if (string.IsNullOrEmpty(customer.firstName))
                {
                    ErrorLabel.Text = "Required field 'First Name' is blank";
                    return;
                }

                if (string.IsNullOrEmpty(customer.bestPhone))
                {
                    ErrorLabel.Text = "Required field 'Best Phone' is blank";
                    return;
                }

                if (string.IsNullOrEmpty(customer.email))
                {
                    ErrorLabel.Text = "Required field 'Email' is blank";
                    return;
                }

                if (string.IsNullOrEmpty(customer.locationCity))
                {
                    ErrorLabel.Text = "Required field 'City' is blank";
                    return;
                }

                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                {
                    if (fran.franchiseID == franchiseID)
                    {
                        if (fran.defaultRatePerHour > customer.ratePerHour) customer.ratePerHour = fran.defaultRatePerHour;
                        if (fran.defaultServiceFee > customer.serviceFee) customer.serviceFee = fran.defaultServiceFee;
                        if (!string.IsNullOrEmpty(fran.state)) customer.locationState = fran.state;
                        break;
                    }
                }

                string error = Database.SetCustomer(ref customer);
                if (error != null)
                {
                    ErrorLabel.Text = error;
                    return;
                }

                Response.Redirect(@"https://www.2localgals.com/thankyou.html");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SubmitClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(@"https://www.2localgals.com");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
            
        }
    }
}