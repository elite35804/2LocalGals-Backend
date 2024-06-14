namespace TwoLocalGals.Code
{
    public class WebQuoteModel
    {
        public int FranchiseID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string BestPhone { get; set; }
        public string AlternatePhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AccountType { get; set; }
        public bool NewBuilding { get; set; }
        public string Advertisement { get; set; }
        public string PreferredContact { get; set; }

        public bool Housekeeping { get; set; }
        public string NC_Details { get; set; }
        public string NC_Frequency { get; set; }
        public string NC_Bathrooms { get; set; }
        public string NC_Bedrooms { get; set; }
        public string NC_SquareFootage { get; set; }
        public bool NC_Vacuum { get; set; }
        public bool NC_DoDishes { get; set; }
        public bool NC_ChangeBed { get; set; }
        public string NC_Pets { get; set; }
        public bool NC_FlooringCarpet { get; set; }
        public bool NC_FlooringHardwood { get; set; }
        public bool NC_FlooringTile { get; set; }
        public bool NC_FlooringLinoleum { get; set; }
        public bool NC_FlooringSlate { get; set; }
        public bool NC_FlooringMarble { get; set; }
        public string NC_CleanRating { get; set; }
        public string NC_CleaningType { get; set; }
        public bool NC_RequestEcoCleaners { get; set; }

        public bool DC_Blinds { get; set; }
        public string DC_BlindsAmount { get; set; }
        public string DC_BlindsCondition { get; set; }
        public bool DC_Windows { get; set; }
        public string DC_WindowsAmount { get; set; }
        public bool DC_WindowsSills { get; set; }
        public bool DC_Walls { get; set; }
        public string DC_WallsDetail { get; set; }
        public bool DC_Baseboards { get; set; }
        public bool DC_DoorFrames { get; set; }
        public bool DC_LightSwitches { get; set; }
        public bool DC_VentCovers { get; set; }
        public bool DC_InsideVents { get; set; }
        public bool DC_Pantry { get; set; }
        public bool DC_LaundryRoom { get; set; }
        public bool DC_CeilingFans { get; set; }
        public string DC_CeilingFansAmount { get; set; }
        public bool DC_LightFixtures { get; set; }
        public bool DC_KitchenCuboards { get; set; }
        public string DC_KitchenCuboardsDetail { get; set; }
        public bool DC_BathroomCuboards { get; set; }
        public string DC_BathroomCuboardsDetail { get; set; }
        public bool DC_Oven { get; set; }
        public bool DC_Refrigerator { get; set; }
        public string DC_OtherOne { get; set; }
        public string DC_OtherTwo { get; set; }

        public bool CarpetCleaning { get; set; }
        public string CC_SquareFootage { get; set; }
        public string CC_RoomCountSmall { get; set; }
        public string CC_RoomCountLarge { get; set; }
        public bool CC_PetOdorAdditive { get; set; }
        public string CC_Details { get; set; }

        public bool WindowWashing { get; set; }
        public string WW_BuildingStyle { get; set; }
        public string WW_BuildingLevels { get; set; }
        public bool WW_VaultedCeilings { get; set; }
        public bool WW_PostConstruction { get; set; }
        public string WW_WindowCount { get; set; }
        public string WW_WindowType { get; set; }
        public string WW_InsidesOutsides { get; set; }
        public bool WW_Razor { get; set; }
        public string WW_RazorCount { get; set; }
        public bool WW_HardWater { get; set; }
        public string WW_HardWaterCount { get; set; }
        public bool WW_FrenchWindows { get; set; }
        public string WW_FrenchWindowCount { get; set; }
        public bool WW_StormWindows { get; set; }
        public string WW_StormWindowCount { get; set; }
        public bool WW_Screens { get; set; }
        public string WW_ScreensCount { get; set; }
        public bool WW_Tracks { get; set; }
        public string WW_TracksCount { get; set; }
        public bool WW_Wells { get; set; }
        public string WW_WellsCount { get; set; }
        public bool WW_Gutters { get; set; }
        public string WW_GuttersFeet { get; set; }
        public string WW_Details { get; set; }

        public bool Homewatch { get; set; }
        public string HW_Frequency { get; set; }
        public string HW_StartDate { get; set; }
        public string HW_EndDate { get; set; }
        public bool HW_GarbageCans { get; set; }
        public string HW_GarbageDay { get; set; }
        public bool HW_PlantsWatered { get; set; }
        public string HW_PlantsWateredFrequency { get; set; }
        public bool HW_Thermostat { get; set; }
        public string HW_ThermostatTemperature { get; set; }
        public bool HW_Breakers { get; set; }
        public string HW_BreakersLocation { get; set; }
        public bool HW_CleanBeforeReturn { get; set; }
        public string HW_Details { get; set; }
    }
}