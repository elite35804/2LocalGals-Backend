using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalAccount : System.Web.UI.Page
    {
        int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);
                ((CustomerPortal)this.Page.Master).SetActiveMenuItem(1);

                if (!Page.IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                    LoadCustomer();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PortalAccount.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        protected void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    CancelClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        private void LoadCustomer()
        {
            CustomerStruct customer;
            string error = Database.GetCustomerByID(-1, customerID, out customer);
            if (error != null)
            {
                ErrorLabel.Text = "Error Loading Customer(" + customerID + "): " + error;
            }
            else
            {
                // Contact Information
                BusinessName.Text = customer.businessName;
                CompanyContact.Text = customer.companyContact;
                FirstName.Text = customer.firstName;
                LastName.Text = customer.lastName;
                SpouseName.Text = customer.spouseName;
                BestPhone.Text = Globals.FormatPhone(customer.bestPhone);
                AlternatePhoneOne.Text = Globals.FormatPhone(customer.alternatePhoneOne);
                AlternatePhoneTwo.Text = Globals.FormatPhone(customer.alternatePhoneTwo);
                Email.Text = customer.email;

                //Property Location
                LocationAddress.Text = customer.locationAddress;
                LocationCity.Text = customer.locationCity;
                LocationState.Text = customer.locationState;
                LocationZipCode.Text = customer.locationZip;

                //Billing Information
                CardNumber.Text = Globals.FormatCard(customer.creditCardNumber, true);
                ExpirationMonth.Text = Globals.SafeIntParse(customer.creditCardExpMonth).ToString();
                ExpirationYear.Text = Globals.FormatCardExpYear(customer.creditCardExpYear);
                CCVCode.Text = customer.creditCardCCV;
                BillingSame.Checked = customer.billingSame;
                BillingName.Text = customer.billingName;
                BillingAddress.Text = customer.billingAddress;
                BillingCity.Text = customer.billingCity;
                BillingState.Text = customer.billingState;
                BillingZipCode.Text = customer.billingZip;

                if (BillingSame.Checked)
                    BillingAddressTable.Style["display"] = "none";
            }
        }

        public bool SaveChanges()
        {
            try
            {
                if (string.IsNullOrEmpty(LocationAddress.Text)) //Fix Non-Edit Mode
                    return true;

                ErrorLabel.Text = "";
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0)
                {
                    ErrorLabel.Text = "Error saving changes, bad customer ID";
                }
                else
                {
                    CustomerStruct customer;
                    string error = Database.GetCustomerByID(-1, customerID, out customer);
                    if (error != null)
                    {
                        ErrorLabel.Text = error;
                    }
                    else
                    {
                        // Customer Information
                        customer.bestPhone = Globals.FormatPhone(BestPhone.Text);
                        customer.alternatePhoneOne = Globals.FormatPhone(AlternatePhoneOne.Text);
                        customer.alternatePhoneTwo = Globals.FormatPhone(AlternatePhoneTwo.Text);
                        customer.email = Email.Text.Trim();

                        //Property Location
                        customer.locationAddress = LocationAddress.Text;
                        customer.locationCity = LocationCity.Text;
                        customer.locationState = LocationState.Text;
                        customer.locationZip = LocationZipCode.Text;

                        // Billing Information
                        string creditCardNumber = Globals.OnlyNumbers(CardNumber.Text);
                        if (creditCardNumber.Length != 4)
                        {
                            if (creditCardNumber.Length != 15 && creditCardNumber.Length != 16 && creditCardNumber.Length != 0)
                            {
                                ErrorLabel.Text = "Invalid Credit Card Number";
                                return false;
                            }
                            else
                            {
                                customer.creditCardNumber = creditCardNumber;
                            }
                        }
                        customer.creditCardExpMonth = ExpirationMonth.Text;
                        customer.creditCardExpYear = ExpirationYear.Text;
                        customer.creditCardCCV = Globals.OnlyNumbers(CCVCode.Text);
                        customer.billingSame = BillingSame.Checked;
                        customer.billingName = BillingName.Text;
                        customer.billingAddress = BillingAddress.Text;
                        customer.billingCity = BillingCity.Text;
                        customer.billingState = BillingState.Text;
                        customer.billingZip = BillingZipCode.Text;

                        error = Database.SetCustomer(ref customer);
                        if (error != null)
                        {
                            ErrorLabel.Text = error;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
            }
            return false;
        }
    }
}