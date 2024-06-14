using System;

namespace Nexus
{
    public class Texting
    {
        public static string SendCustomerConfirmationText(int appID, bool oneWeek = false)
        {
            AppStruct app;
            string error = Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app);
            if (error != null)
            {
                return "Error GetApointmentpByID: " + error;
            }
            else
            {
                CustomerStruct cust;
                error = Database.GetCustomerByID(Globals.GetFranchiseMask(), app.customerID, out cust);
                if (error != null)
                {
                    return "Error GetCustomerByID: " + error;
                }
                else
                {
                    FranchiseStruct franchise = Globals.GetFranchiseByMask(cust.franchiseMask);
                    string sms = "2 Local Gals, reminding you of your appointment on " + app.appointmentDate.ToString("d") + " around " + app.startTime.ToString("t") + ". Call " + franchise.phone + " for any questions. See you then!";
                    if (oneWeek) sms = "2 Local Gals, reminder of your upcoming cleaning appointment on " + app.appointmentDate.ToString("d") + ". We will confirm an exact time the day before. Thank you!";

                    error = null;
                    if (cust.alternatePhoneTwoCell)
                        error = SendText(cust.alternatePhoneTwo, sms, franchise);
                    if (cust.alternatePhoneOneCell)
                        error = SendText(cust.alternatePhoneOne, sms, franchise);
                    if (cust.bestPhoneCell)
                        error = SendText(cust.bestPhone, sms, franchise);
                    return error;
                }
            }
        }

        public static string SendReviewUsText(int customerID)
        {
            CustomerStruct cust;
            string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out cust);
            if (error != null)
            {
                return "Error GetCustomerByID: " + error;
            }
            else
            {
                FranchiseStruct franchise = Globals.GetFranchiseByMask(cust.franchiseMask);
                string sms = "Hey " + cust.firstName + ", 2 Local Gals thanks you. Let us know how we've made a difference at " + franchise.reviewUsLink;

                error = null;
                if (cust.alternatePhoneTwoCell)
                    error = SendText(cust.alternatePhoneTwo, sms, franchise);
                if (cust.alternatePhoneOneCell)
                    error = SendText(cust.alternatePhoneOne, sms, franchise);
                if (cust.bestPhoneCell)
                    error = SendText(cust.bestPhone, sms, franchise);
                return error;
            }
        }

        public static string SendText(string phoneNumber, string message, FranchiseStruct franchise)
        {
            try
            {
                phoneNumber = Globals.OnlyNumbers(phoneNumber);

                if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10)
                    return "Invalid Phone Number";

                if (string.IsNullOrWhiteSpace(message))
                    return "Invalid Message";

                if (string.IsNullOrWhiteSpace(franchise.smsUsername) || string.IsNullOrWhiteSpace(franchise.smsPassword))
                    return "SMS texting is not setup on this franchise";

                if (franchise.smsUsername.Split(',').Length == 6)
                {
                    var asyncCall = RingTexting.SendSMS(phoneNumber, message, franchise);
                    asyncCall.Wait();
                    return asyncCall.Result;
                }
                if (franchise.smsUsername.Split(',').Length == 3)
                {
                    var asyncCall = DialPadTexting.SendSMS(phoneNumber, message, franchise);
                    asyncCall.Wait();
                    return asyncCall.Result;
                }
                //fall through case
                return EzTexting.SendText(phoneNumber, message, franchise);
            }
            catch (Exception ex)
            {
                Database.LogThis("Texting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.smsUsername + ")", ex);
                return ex.Message;
            }
        }
    }
}