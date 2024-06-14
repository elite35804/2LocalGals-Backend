using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Diagnostics;

namespace TwoLocalGals
{
    public partial class HomeGuardContract : System.Web.UI.Page
    {
        public string abbreviatedCompanyName = "R&J";
        public string fullCompanyName = "R&J Housekeeping, LLC, D.B.A. Two Local Gals Wasatch (“R&J”)";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                    if (!string.IsNullOrEmpty(Request["Sign"]))
                    {
                        int customerID = Globals.SafeIntParse(Globals.Decrypt(Request["Sign"]));
                        if (customerID <= 0)
                        {
                            Response.Redirect("https://www.2localgals.com/");
                        }
                        else
                        {
                            PopulateCompanyName(customerID);
                            CustomerStruct customer;
                            string error = Database.GetCustomerByID(-1, customerID, out customer);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Error looking up customer information.";
                            }
                            else
                            {
                                CustomerName.Text = customer.firstName + " " + customer.lastName;
                                CustomerAddress.Text = customer.locationAddress + ", " + customer.locationCity + " " + customer.locationState + ", " + customer.locationZip;
                                CustomerPhoneOne.Text = Globals.FormatPhone(customer.bestPhone);
                                CustomerPhoneTwo.Text = Globals.FormatPhone(customer.alternatePhoneOne);
                                CustomerEmail.Text = customer.email;

                                ContractCustomerTitle.InnerText = CustomerName.Text;
                                ContractCustomerName.Text = CustomerName.Text;
                                ContractCustomerAddress.Text = CustomerAddress.Text;
                                ContractPhoneOne.Text = CustomerPhoneOne.Text;
                                ContractPhoneTwo.Text = CustomerPhoneTwo.Text;
                                ContractEmail.Text = CustomerEmail.Text;

                                OtherDetails.InnerText = "";
                                if (!string.IsNullOrEmpty(customer.HW_Frequency)) OtherDetails.InnerText += "Inspect the property at a frequency of " + customer.HW_Frequency + ". ";
                                if (customer.HW_GarbageCans) OtherDetails.InnerText += "Garbage can(s) shall be taken in/out from curb on " + customer.HW_GarbageDay + ". ";
                                if (customer.HW_PlantsWatered) OtherDetails.InnerText += "Indoor plants shall be watered at a frequency of " + customer.HW_PlantsWateredFrequency + ". ";
                                if (customer.HW_Thermostat) OtherDetails.InnerText += "The thermostat shall be checked and set to a temperature of " + customer.HW_ThermostatTemperature + ". ";
                                if (customer.HW_Breakers) OtherDetails.InnerText += "Check breaker box for any tripped breakers located at " + customer.HW_BreakersLocation + ". ";
                                if (customer.HW_CleanBeforeReturn) OtherDetails.InnerText += "The property shall be cleaned before the client returns. ";
                                if (!string.IsNullOrEmpty(customer.HW_Details)) OtherDetails.InnerText += customer.HW_Details;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Request["View"]))
                    {
                        int customerID = Globals.SafeIntParse(Globals.Decrypt(Request["View"]));
                        if (customerID <= 0)
                        {
                            Response.Redirect("https://www.2localgals.com/");
                        }
                        else
                        {
                            PopulateCompanyName(customerID);
                            DBRow row = Database.GetHomeGuardContract(customerID);
                            if (row == null || string.IsNullOrEmpty(row.GetString("dateSigned")))
                            {
                                ErrorLabel.Text = "The contract has not yet been signed by the customer.";
                            }
                            else
                            {
                                CustomerName.Text = row.GetString("customerName");
                                CustomerAddress.Text = row.GetString("customerAddress");
                                CustomerPhoneOne.Text = row.GetString("customerPhoneOne");
                                CustomerPhoneTwo.Text = row.GetString("customerPhoneTwo");
                                CustomerEmail.Text = row.GetString("customerEmail");

                                ContractCustomerTitle.InnerText = CustomerName.Text;
                                ContractCustomerName.Text = CustomerName.Text;
                                ContractCustomerAddress.Text = CustomerAddress.Text;
                                ContractPhoneOne.Text = CustomerPhoneOne.Text;
                                ContractPhoneTwo.Text = CustomerPhoneTwo.Text;
                                ContractEmail.Text = CustomerEmail.Text;

                                OtherDetails.InnerText = "";
                                if (!string.IsNullOrEmpty(row.GetString("HW_Frequency"))) OtherDetails.InnerText += "Inspect the property at a frequency of " + row.GetString("HW_Frequency") + ". ";
                                if (row.GetBool("HW_GarbageCans")) OtherDetails.InnerText += "Garbage can(s) shall be taken in/out from curb on " + row.GetString("HW_GarbageDay") + ". ";
                                if (row.GetBool("HW_PlantsWatered")) OtherDetails.InnerText += "Indoor plants shall be watered at a frequency of " + row.GetString("HW_PlantsWateredFrequency") + ". ";
                                if (row.GetBool("HW_Thermostat")) OtherDetails.InnerText += "The thermostat shall be checked and set to a temperature of " + row.GetString("HW_ThermostatTemperature") + ". ";
                                if (row.GetBool("HW_Breakers")) OtherDetails.InnerText += "Check breaker box for any tripped breakers located at " +  row.GetString("HW_BreakersLocation") + ". ";
                                if (row.GetBool("HW_CleanBeforeReturn")) OtherDetails.InnerText += "The property shall be cleaned before the client returns. ";
                                if (!string.IsNullOrEmpty(row.GetString("HW_Details"))) OtherDetails.InnerText += row.GetString("HW_Details");

                                AgreeCheckbox.Checked = true;
                                AgreeCheckbox.Enabled = false;
                                SubmitButton.Visible = false;
                                AgreementSignedLabel.Text = "<b>Sent: </b>" + row.GetString("dateSent") + "<b>, Submitted: </b>" + row.GetString("dateSigned") + "<b>, IP: </b>" + row.GetString("ipAddress");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            try
            {
                if (!string.IsNullOrEmpty(Request["Sign"]))
                {
                    int customerID = Globals.SafeIntParse(Globals.Decrypt(Request["Sign"]));
                    if (customerID <= 0)
                    {
                        ErrorLabel.Text = "Error parsing customer ID.";
                    }
                    else
                    {
                        CustomerStruct customer;
                        string error = Database.GetCustomerByID(-1, customerID, out customer);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error looking up customer information.";
                        }
                        else
                        {
                            DBRow row = new DBRow();
                            row.SetValue("customerID", customerID);
                            row.SetValue("dateSigned", DateTime.Now.ToString("g"));
                            row.SetValue("customerName", customer.firstName + " " + customer.lastName);
                            row.SetValue("customerAddress", customer.locationAddress + ", " + customer.locationCity + " " + customer.locationState + ", " + customer.locationZip);
                            row.SetValue("customerPhoneOne", Globals.FormatPhone(customer.bestPhone));
                            row.SetValue("customerPhoneTwo", Globals.FormatPhone(customer.alternatePhoneOne));
                            row.SetValue("customerEmail",customer.email);
                            row.SetValue("ipAddress", Request.ServerVariables.Get("REMOTE_ADDR"));
                            row.SetValue("HW_Frequency", customer.HW_Frequency);
                            row.SetValue("HW_GarbageCans", customer.HW_GarbageCans);
                            row.SetValue("HW_GarbageDay", customer.HW_GarbageDay);
                            row.SetValue("HW_PlantsWatered", customer.HW_PlantsWatered);
                            row.SetValue("HW_PlantsWateredFrequency", customer.HW_PlantsWateredFrequency);
                            row.SetValue("HW_Thermostat", customer.HW_Thermostat);
                            row.SetValue("HW_ThermostatTemperature", customer.HW_ThermostatTemperature);
                            row.SetValue("HW_Breakers", customer.HW_Breakers);
                            row.SetValue("HW_BreakersLocation", customer.HW_BreakersLocation);
                            row.SetValue("HW_CleanBeforeReturn", customer.HW_CleanBeforeReturn);
                            row.SetValue("HW_Details", customer.HW_Details);

                            error = Database.DynamicSetWithKeyInt("HomeGuardContract", "customerID", ref customerID, row);

                            if (error != null)
                            {
                                ErrorLabel.Text = "Submit Error: " + error;
                            }
                            else
                            {
                                Response.Redirect(Globals.BuildQueryString("ThankYou.aspx", "Msg", "Thank you for your submission. Your information is now verified. If you have any questions, feel free to call or email us."));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SubmitButton_Click EX: " + ex.Message;
            }
        }

        private void PopulateCompanyName(int customerID)
        {
            CustomerStruct customer;
            Database.GetCustomerByID(-1, customerID, out customer);
            FranchiseStruct franchise = Globals.GetFranchiseByMask(customer.franchiseMask);

            switch (franchise.franchiseID)
            {
                default:
                    abbreviatedCompanyName = "R&J";
                    fullCompanyName = "R&J Housekeeping, LLC, D.B.A. Two Local Gals Wasatch (“R&J”)";
                    break;
                case 8:
                    abbreviatedCompanyName = "Lili's Housekeeping";
                    fullCompanyName = "Lili's Housekeeping, DBA 2 Local Gals St.George";
                    break;
                case 16:
                    abbreviatedCompanyName = "2 Local Gals Texas";
                    fullCompanyName = "2 Local Gals Housekeeping, DBA 2 Local Gals Texas";
                    break;
            }
        }

    }
}