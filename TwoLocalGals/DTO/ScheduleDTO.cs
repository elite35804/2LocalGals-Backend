using System;
using System.Collections.Generic;

namespace TwoLocalGals.DTO
{
    public class ScheduleDTO
    {

        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public decimal Hours { get; set; }
        public string Miles { get; set; }
        public double Minutes { get; set; }
        public decimal AproxPay { get; set; }
        public bool Keys { get; set; }
        public int CustomerId { get; set; }
        public string bestPhone { get; set; }
        public bool bestPhoneCell { get; set; }
        public string alternatePhoneOne { get; set; }
        public bool alternatePhoneOneCell { get; set; }
        public string alternatePhoneTwo { get; set; }
        public bool alternatePhoneTwoCell { get; set; }
        public string locationAddress { get; set; }
        public string locationCity { get; set; }
        public string locationState { get; set; }
        public string locationZip { get; set; }
        public string paymentType { get; set; }

        public bool NewlyConstructed { get; set; }

        public string Bathrooms { get; set; }
        public string Bedrooms { get; set; }
        public string SquareFootage { get; set; }
        public bool NC_Vacuum { get; set; }
        public bool NC_DoDishes { get; set; }
        public bool NC_ChangeBed { get; set; }
        public string PetsCount { get; set; }
        public bool NC_FlooringCarpet { get; set; }
        public bool NC_FlooringHardwood { get; set; }
        public bool NC_FlooringTile { get; set; }
        public bool NC_FlooringLinoleum { get; set; }
        public bool NC_FlooringSlate { get; set; }
        public bool NC_FlooringMarble { get; set; }
        public string HowtoEnterHome { get; set; }
        public bool NC_RequiresKeys { get; set; }
        public bool NC_Organize { get; set; }
        public string CleanRating { get; set; }
        public string NC_CleaningType { get; set; }
        public string GateCode { get; set; }
        public string GarageCode { get; set; }
        public string NC_DoorCode { get; set; }
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

        public string Details { get; set; }

        public bool TakePic { get; set; }

        public bool JobCompleted { get; set; }

        public List<PartnerDTO> Partners { get; set; }
    }

    public class PartnerDTO
    {
        public string Name { get; set; }
        public string category { get; set; }
        public string phoneNumber { get; set; }
        public string webAddress { get; set; }
        public string description { get; set; }
        public bool approved { get; set; }
        


    }
}