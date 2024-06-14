using Newtonsoft.Json;
using Nexus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TwoLocalGals.Code;

namespace TwoLocalGals
{
    public class WebQuoteService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 400;
            try
            {
                if (context.Request.ServerVariables["REQUEST_METHOD"] != "POST")
                {
                    context.Response.Write("Bad Request");
                    return;
                }

                string json = null;
                using (StreamReader streamReader = new StreamReader(context.Request.GetBufferedInputStream()))
                {
                    json = streamReader.ReadToEnd();
                }

                WebQuoteModel quote = JsonConvert.DeserializeObject<WebQuoteModel>(json);

                if (quote == null)
                {
                    context.Response.Write("Invalid JSON");
                    return;
                }

                CustomerStruct customer = new CustomerStruct();
                customer.accountStatus = "Web Quote";
                customer.franchiseMask = Globals.IDToMask(quote.FranchiseID);
                customer.firstName = quote.FirstName;
                customer.lastName = quote.LastName;
                customer.email = quote.Email;
                customer.bestPhone = Globals.FormatPhone(quote.BestPhone);
                customer.alternatePhoneOne = Globals.FormatPhone(quote.AlternatePhone);
                customer.locationAddress = quote.Address;
                customer.locationCity = quote.City;
                customer.locationState = quote.State;
                customer.locationZip = quote.Zip;
                customer.accountType = quote.AccountType;
                customer.newBuilding = quote.NewBuilding;
                customer.advertisement = quote.Advertisement;
                customer.preferredContact = quote.PreferredContact;
                customer.NC_Frequency = quote.NC_Frequency;

                customer.NC_CleanRating = quote.NC_CleanRating;
                customer.NC_Bedrooms = quote.NC_Bedrooms;
                customer.NC_Bathrooms = quote.NC_Bathrooms;
                customer.NC_SquareFootage = quote.NC_SquareFootage;
                customer.NC_Pets = quote.NC_Pets;
                customer.NC_FlooringCarpet = quote.NC_FlooringCarpet;
                customer.NC_FlooringHardwood = quote.NC_FlooringHardwood;
                customer.NC_FlooringTile = quote.NC_FlooringTile;
                customer.NC_FlooringLinoleum = quote.NC_FlooringLinoleum;
                customer.NC_FlooringSlate = quote.NC_FlooringSlate;
                customer.NC_FlooringMarble = quote.NC_FlooringMarble;
                customer.NC_Details = quote.NC_Details;
                customer.NC_DoDishes = quote.NC_DoDishes;
                customer.NC_ChangeBed = quote.NC_ChangeBed;
                customer.NC_RequestEcoCleaners = quote.NC_RequestEcoCleaners;

                customer.DC_Blinds = quote.DC_Blinds;
                customer.DC_BlindsAmount = quote.DC_BlindsAmount;
                customer.DC_BlindsCondition = quote.DC_BlindsCondition;
                customer.DC_Windows = quote.DC_Windows;
                customer.DC_WindowsAmount = quote.DC_WindowsAmount;
                customer.DC_WindowsSills = quote.DC_WindowsSills;
                customer.DC_Walls = quote.DC_Walls;
                customer.DC_WallsDetail = quote.DC_WallsDetail;
                customer.DC_CeilingFans = quote.DC_CeilingFans;
                customer.DC_CeilingFansAmount = quote.DC_CeilingFansAmount;
                customer.DC_KitchenCuboards = quote.DC_KitchenCuboards;
                customer.DC_KitchenCuboardsDetail = quote.DC_KitchenCuboardsDetail;
                customer.DC_BathroomCuboards = quote.DC_BathroomCuboards;
                customer.DC_BathroomCuboardsDetail = quote.DC_BathroomCuboardsDetail;
                customer.DC_Baseboards = quote.DC_Baseboards;
                customer.DC_DoorFrames = quote.DC_DoorFrames;
                customer.DC_VentCovers = quote.DC_VentCovers;
                customer.DC_InsideVents = quote.DC_InsideVents;
                customer.DC_Pantry = quote.DC_Pantry;
                customer.DC_LaundryRoom = quote.DC_LaundryRoom;
                customer.DC_LightSwitches = quote.DC_LightSwitches;
                customer.DC_LightFixtures = quote.DC_LightFixtures;
                customer.DC_Refrigerator = quote.DC_Refrigerator;
                customer.DC_Oven = quote.DC_Oven;
                customer.DC_OtherOne = quote.DC_OtherOne;
                customer.DC_OtherTwo = quote.DC_OtherTwo;

                customer.CC_SquareFootage = quote.CC_SquareFootage;
                customer.CC_RoomCountSmall = quote.CC_RoomCountSmall;
                customer.CC_RoomCountLarge = quote.CC_RoomCountLarge;
                customer.CC_PetOdorAdditive = quote.CC_PetOdorAdditive;
                customer.CC_Details = quote.CC_Details;
                customer.WW_BuildingStyle = quote.WW_BuildingStyle;
                customer.WW_BuildingLevels = quote.WW_BuildingLevels;
                customer.WW_VaultedCeilings = quote.WW_VaultedCeilings;
                customer.WW_WindowCount = quote.WW_WindowCount;
                customer.WW_WindowType = quote.WW_WindowType;
                customer.WW_InsidesOutsides = quote.WW_InsidesOutsides;
                customer.WW_Razor = quote.WW_Razor;
                customer.WW_RazorCount = quote.WW_RazorCount;
                customer.WW_HardWater = quote.WW_HardWater;
                customer.WW_HardWaterCount = quote.WW_HardWaterCount;
                customer.WW_FrenchWindows = quote.WW_FrenchWindows;
                customer.WW_FrenchWindowCount = quote.WW_FrenchWindowCount;
                customer.WW_StormWindows = quote.WW_StormWindows;
                customer.WW_StormWindowCount = quote.WW_StormWindowCount;
                customer.WW_Screens = quote.WW_Screens;
                customer.WW_ScreensCount = quote.WW_ScreensCount;
                customer.WW_Tracks = quote.WW_Tracks;
                customer.WW_TracksCount = quote.WW_TracksCount;
                customer.WW_Wells = quote.WW_Wells;
                customer.WW_WellsCount = quote.WW_WellsCount;
                customer.WW_Gutters = quote.WW_Gutters;
                customer.WW_GuttersFeet = quote.WW_GuttersFeet;
                customer.WW_Details = quote.WW_Details;
                customer.HW_Frequency = quote.HW_Frequency;
                customer.HW_StartDate = quote.HW_StartDate;
                customer.HW_EndDate = quote.HW_EndDate;
                customer.HW_GarbageCans = quote.HW_GarbageCans;
                customer.HW_GarbageDay = quote.HW_GarbageDay;
                customer.HW_PlantsWatered = quote.HW_PlantsWatered;
                customer.HW_PlantsWateredFrequency = quote.HW_PlantsWateredFrequency;
                customer.HW_Thermostat = quote.HW_Thermostat;
                customer.HW_ThermostatTemperature = quote.HW_ThermostatTemperature;
                customer.HW_Breakers = quote.HW_Breakers;
                customer.HW_BreakersLocation = quote.HW_BreakersLocation;
                customer.HW_CleanBeforeReturn = quote.HW_CleanBeforeReturn;
                customer.HW_Details = quote.HW_Details;

                customer.sectionMask = 0;
                if (quote.Housekeeping) customer.sectionMask |= 1;
                if (quote.CarpetCleaning) customer.sectionMask |= 2;
                if (quote.WindowWashing) customer.sectionMask |= 4;
                if (quote.Homewatch) customer.sectionMask |= 8;

                if (string.IsNullOrEmpty(customer.firstName))
                {

                    context.Response.Write("Required field 'First Name' is blank");
                    return;
                }

                if (string.IsNullOrEmpty(customer.bestPhone))
                {
                    context.Response.Write("Required field 'Best Phone' is blank");
                    return;
                }

                if (string.IsNullOrEmpty(customer.email))
                {
                    context.Response.Write("Required field 'Email' is blank");
                    return;
                }

                if (string.IsNullOrEmpty(customer.locationCity))
                {
                    context.Response.Write("Required field 'City' is blank");
                    return;
                }

                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                {
                    if (fran.franchiseID == quote.FranchiseID)
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
                    Database.LogThis("WebQutoeService.SetCustomer: " + error, null);
                    context.Response.Write("Database Error");
                    return;
                }
                else
                {
                    context.Response.StatusCode = 200;
                    context.Response.Write("Success");
                }
            }
            catch (Exception ex)
            {
                Database.LogThis("WebQutoeService", ex);
                context.Response.Write("Exception Caught");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}