using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nexus.Protected
{
    public partial class BidSheet : System.Web.UI.Page
    {
        CustomerStruct customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG BidSheet";
                MaintainScrollPositionOnPostBack = true;

                if (!IsPostBack)
                {
                    int serviceMask = Database.GetFranchiseServiceMask(Globals.GetFranchiseMask());
                    if ((serviceMask & 2) == 0) CarpetCleaningLabel.Visible = false;
                    if ((serviceMask & 4) == 0) WindowWashingLabel.Visible = false;
                    if ((serviceMask & 8) == 0) HomewatchLabel.Visible = false;

                    if (null != (ErrorLabel.Text = LoadCustomer(Globals.SafeIntParse(Request["custID"]))))
                    {
                        SaveButton.Enabled = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect("Customers.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Customers.aspx?custID=" + Globals.SafeIntParse(Request["custID"]));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }

        public string LoadCustomer(int customerID)
        {
            try
            {
                string error = null;
                string defaultState = null;
                decimal defaultRatePerHour = 0;
                decimal defaultServiceFee = 0;

                Advertisement.Items.Clear();
                foreach (string i in Database.GetFranchiseDropDown(Globals.GetFranchiseMask(), "advertisementList"))
                    Advertisement.Items.Add(i);

                CustomerStruct customer;
                if ((error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer)) != null)
                    return error;

                if (customerID == 0)
                {
                    CustomerTitleLabel.Text = "New Bid";
                    customer.sectionMask = 1;

                    foreach (FranchiseStruct fran in Database.GetFranchiseList())
                    {
                        if ((fran.franchiseMask & Globals.GetFranchiseMask()) != 0)
                        {
                            if (fran.defaultRatePerHour > defaultRatePerHour) defaultRatePerHour = fran.defaultRatePerHour;
                            if (fran.defaultServiceFee > defaultServiceFee) defaultServiceFee = fran.defaultServiceFee;
                            if (!string.IsNullOrEmpty(fran.state)) defaultState = fran.state;
                        }
                    }
                }
                else
                {
                    CustomerTitleLabel.Text = customer.customerTitle;
                }

                //General Questions
                FranchiseList.Items.Clear();
                FranchiseList.Items.Add(new ListItem("", "0"));
                FranchiseList.Items.AddRange(Globals.GetFranchiseList(Globals.GetFranchiseMask(), customer.franchiseMask));
                if (FranchiseList.Items.Count == 2) FranchiseList.SelectedIndex = 1;
                Globals.SetDropDownList(ref AccountType, customer.accountType);
                ContractorStruct contractor;
                AccountRep.Items.Clear();
                AccountRep.Items.Add(new ListItem("", "0"));
                int accountRepID = customer.bookedBy == 0 ? Globals.GetUserContractorID(this) : customer.bookedBy;
                AccountRep.Items.AddRange(Globals.GetContractorList(Globals.GetFranchiseMask(), accountRepID, 0,  out contractor, true, true));
                Globals.SetDropDownList(ref Advertisement, customer.advertisement);
                FirstName.Text = customer.firstName;
                LastName.Text = customer.lastName;
                LocationState.Text = defaultState ?? customer.locationState;
                NewBuilding.Checked = customer.newBuilding;
                Globals.SetDropDownList(ref NC_Bedrooms, customer.NC_Bedrooms);
                Globals.SetDropDownList(ref NC_Bathrooms, customer.NC_Bathrooms);
                NC_SquareFootage.Text = customer.NC_SquareFootage;
                Globals.SetDropDownList(ref NC_Pets, customer.NC_Pets);
                NC_DoDishes.Checked = customer.NC_DoDishes;
                NC_ChangeBed.Checked = customer.NC_ChangeBed;
                NC_FlooringCarpet.Checked = customer.NC_FlooringCarpet;
                NC_FlooringHardwood.Checked = customer.NC_FlooringHardwood;
                NC_FlooringTile.Checked = customer.NC_FlooringTile;
                NC_FlooringLinoleum.Checked = customer.NC_FlooringLinoleum;
                NC_FlooringSlate.Checked = customer.NC_FlooringSlate;
                NC_FlooringMarble.Checked = customer.NC_FlooringMarble;
                NC_CleanRating.Text = customer.NC_CleanRating;
                NC_CleaningType.Text = customer.NC_CleaningType;
                NC_Organize.Checked = customer.NC_Organize;
                NC_Details.Text = customer.NC_Details;
                NC_RequestEcoCleaners.Checked = customer.NC_RequestEcoCleaners;

                //Deep Clean
                DC_Blinds.Checked = customer.DC_Blinds;
                DC_BlindsAmount.Text = customer.DC_BlindsAmount;
                Globals.SetDropDownList(ref DC_BlindsCondition, customer.DC_BlindsCondition);
                DC_Windows.Checked = customer.DC_Windows;
                DC_WindowsAmount.Text = customer.DC_WindowsAmount;
                DC_WindowsSills.Checked = customer.DC_WindowsSills;
                DC_Walls.Checked = customer.DC_Walls;
                Globals.SetDropDownList(ref DC_WallsDetail, customer.DC_WallsDetail);
                DC_CeilingFans.Checked = customer.DC_CeilingFans;
                DC_CeilingFansAmount.Text = customer.DC_CeilingFansAmount;
                DC_Baseboards.Checked = customer.DC_Baseboards;
                DC_DoorFrames.Checked = customer.DC_DoorFrames;
                DC_LightFixtures.Checked = customer.DC_LightFixtures;
                DC_LightSwitches.Checked = customer.DC_LightSwitches;
                DC_VentCovers.Checked = customer.DC_VentCovers;
                DC_InsideVents.Checked = customer.DC_InsideVents;
                DC_Pantry.Checked = customer.DC_Pantry;
                DC_LaundryRoom.Checked = customer.DC_LaundryRoom;
                DC_KitchenCuboards.Checked = customer.DC_KitchenCuboards;
                Globals.SetDropDownList(ref DC_KitchenCuboardsDetail, customer.DC_KitchenCuboardsDetail);
                DC_BathroomCuboards.Checked = customer.DC_BathroomCuboards;
                Globals.SetDropDownList(ref DC_BathroomCuboardsDetail, customer.DC_BathroomCuboardsDetail);
                DC_Oven.Checked = customer.DC_Oven;
                DC_Refrigerator.Checked = customer.DC_Refrigerator;
                DC_OtherOne.Text = customer.DC_OtherOne;
                DC_OtherTwo.Text = customer.DC_OtherTwo;

                CC_SquareFootage.Text = customer.CC_SquareFootage;
                CC_RoomCountSmall.Text = customer.CC_RoomCountSmall;
                CC_RoomCountLarge.Text = customer.CC_RoomCountLarge;
                CC_PetOdorAdditive.Checked = customer.CC_PetOdorAdditive;
                CC_Details.Text = customer.CC_Details;
                WW_BuildingStyle.Text = customer.WW_BuildingStyle;
                WW_BuildingLevels.Text = customer.WW_BuildingLevels;
                WW_VaultedCeilings.Checked = customer.WW_VaultedCeilings;
                WW_PostConstruction.Checked = customer.WW_PostConstruction;
                WW_WindowCount.Text = customer.WW_WindowCount;
                WW_WindowType.Text = customer.WW_WindowType;
                WW_InsidesOutsides.Text = customer.WW_InsidesOutsides;
                WW_Razor.Checked = customer.WW_Razor;
                WW_RazorCount.Text = customer.WW_RazorCount;
                WW_HardWater.Checked = customer.WW_HardWater;
                WW_HardWaterCount.Text = customer.WW_HardWaterCount;
                WW_FrenchWindows.Checked = customer.WW_FrenchWindows;
                WW_FrenchWindowCount.Text = customer.WW_FrenchWindowCount;
                WW_StormWindows.Checked = customer.WW_StormWindows;
                WW_StormWindowCount.Text = customer.WW_StormWindowCount;
                WW_Screens.Checked = customer.WW_Screens;
                WW_ScreensCount.Text = customer.WW_ScreensCount;
                WW_Tracks.Checked = customer.WW_Tracks;
                WW_TracksCount.Text = customer.WW_TracksCount;
                WW_Wells.Checked = customer.WW_Wells;
                WW_WellsCount.Text = customer.WW_WellsCount;
                WW_Gutters.Checked = customer.WW_Gutters;
                WW_GuttersFeet.Text = customer.WW_GuttersFeet;
                WW_Details.Text = customer.WW_Details;
                HW_Frequency.Text = customer.HW_Frequency;
                HW_StartDate.Text = customer.HW_StartDate;
                HW_EndDate.Text = customer.HW_EndDate;
                HW_GarbageCans.Checked = customer.HW_GarbageCans;
                HW_GarbageDay.Text = customer.HW_GarbageDay;
                HW_PlantsWatered.Checked = customer.HW_PlantsWatered;
                HW_PlantsWateredFrequency.Text = customer.HW_PlantsWateredFrequency;
                HW_Thermostat.Checked = customer.HW_Thermostat;
                HW_ThermostatTemperature.Text = customer.HW_ThermostatTemperature;
                HW_Breakers.Checked = customer.HW_Breakers;
                HW_BreakersLocation.Text = customer.HW_BreakersLocation;
                HW_CleanBeforeReturn.Checked = customer.HW_CleanBeforeReturn;
                HW_Details.Text = customer.HW_Details;

                HousekeepingCheckbox.Checked = ((customer.sectionMask & 1) != 0);
                CarpetCleaningCheckbox.Checked = ((customer.sectionMask & 2) != 0);
                WindowWashingCheckbox.Checked = ((customer.sectionMask & 4) != 0);
                HomewatchCheckbox.Checked = ((customer.sectionMask & 8) != 0);

                //Estimated Price
                RatePerHour.Text = defaultRatePerHour > 0 ? Globals.FormatMoney(defaultRatePerHour) : Globals.FormatMoney(customer.ratePerHour);
                ServiceFee.Text = defaultServiceFee > 0 ? Globals.FormatMoney(defaultServiceFee) : Globals.FormatMoney(customer.serviceFee);
                EstimatedHours.Text = customer.estimatedHours;
                EstimatedCC.Text = customer.estimatedCC;
                EstimatedWW.Text = customer.estimatedWW;
                EstimatedHW.Text = customer.estimatedHW;
                EstimatedPrice.Text = customer.estimatedPrice;

                return null;
            }
            catch (Exception ex)
            {
                return "LoadCustomer EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                int customerID =  Globals.SafeIntParse(Request["custID"]);
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                if (error == null)
                {
                    //General Questions
                    customer.franchiseMask = Globals.IDToMask(Globals.SafeIntParse(FranchiseList.SelectedValue));
                    customer.accountType = AccountType.Text;
                    customer.bookedBy = Globals.SafeIntParse(AccountRep.SelectedValue);
                    customer.advertisement = Advertisement.Text;
                    customer.firstName = FirstName.Text;
                    customer.lastName = LastName.Text;
                    customer.locationState = LocationState.Text;
                    customer.newBuilding = NewBuilding.Checked;
                    customer.NC_Bedrooms = NC_Bedrooms.Text;
                    customer.NC_Bathrooms = NC_Bathrooms.Text;
                    customer.NC_SquareFootage = NC_SquareFootage.Text;
                    customer.NC_Pets = NC_Pets.Text;
                    customer.NC_DoDishes = NC_DoDishes.Checked;
                    customer.NC_ChangeBed = NC_ChangeBed.Checked;
                    customer.NC_FlooringCarpet = NC_FlooringCarpet.Checked;
                    customer.NC_FlooringHardwood = NC_FlooringHardwood.Checked;
                    customer.NC_FlooringTile = NC_FlooringTile.Checked;
                    customer.NC_FlooringLinoleum = NC_FlooringLinoleum.Checked;
                    customer.NC_FlooringSlate = NC_FlooringSlate.Checked;
                    customer.NC_FlooringMarble = NC_FlooringMarble.Checked;
                    customer.NC_CleanRating = NC_CleanRating.Text;
                    customer.NC_CleaningType = NC_CleaningType.Text;
                    customer.NC_Organize = NC_Organize.Checked;
                    customer.NC_Details = NC_Details.Text;
                    customer.NC_RequestEcoCleaners = NC_RequestEcoCleaners.Checked;

                    //Deep Clean.
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
                    customer.DC_Baseboards = DC_Baseboards.Checked;
                    customer.DC_DoorFrames = DC_DoorFrames.Checked;
                    customer.DC_LightFixtures = DC_LightFixtures.Checked;
                    customer.DC_LightSwitches = DC_LightSwitches.Checked;
                    customer.DC_VentCovers = DC_VentCovers.Checked;
                    customer.DC_InsideVents = DC_InsideVents.Checked;
                    customer.DC_Pantry = DC_Pantry.Checked;
                    customer.DC_LaundryRoom = DC_LaundryRoom.Checked;
                    customer.DC_KitchenCuboards = DC_KitchenCuboards.Checked;
                    customer.DC_KitchenCuboardsDetail = DC_KitchenCuboardsDetail.Text;
                    customer.DC_BathroomCuboards = DC_BathroomCuboards.Checked;
                    customer.DC_BathroomCuboardsDetail = DC_BathroomCuboardsDetail.Text;
                    customer.DC_Oven = DC_Oven.Checked;
                    customer.DC_Refrigerator = DC_Refrigerator.Checked;
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
                    customer.WW_PostConstruction = WW_PostConstruction.Checked;
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

                    //Estimated Price
                    customer.ratePerHour = Globals.FormatMoney(RatePerHour.Text);
                    customer.serviceFee = Globals.FormatMoney(ServiceFee.Text);
                    customer.estimatedHours = EstimatedHours.Text;
                    customer.estimatedCC = EstimatedCC.Text;
                    customer.estimatedWW = EstimatedWW.Text;
                    customer.estimatedHW = EstimatedHW.Text;
                    customer.estimatedPrice = EstimatedPrice.Text;

                    if (customer.customerID == 0 && string.IsNullOrEmpty(customer.firstName) && string.IsNullOrEmpty(customer.lastName)) return true;
                    if (customer.accountStatus == null) customer.accountStatus = "Quote";

                    error = Database.SetCustomer(ref customer);
                }

                if (error != null) ErrorLabel.Text = error;
                return error == null;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
    }
}