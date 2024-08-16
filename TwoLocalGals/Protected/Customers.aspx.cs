using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Nexus.Protected
{
    public partial class WebFormCustomers : System.Web.UI.Page
    {
        private CustomerStruct customer;
        private int referredBy = -1;
        private int copyID = 0;
        private bool copyOnlyBilling = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);

                this.Page.Title = "2LG Customers";
                ErrorLabel.Text = "";

                MaintainScrollPositionOnPostBack = true;

                int customerID = Globals.SafeIntParse(Request["custID"]);
                DeleteAppointmentRange.Enabled = customerID > 0;
                DeleteButton.Visible = Globals.GetUserAccess(this) >= 7;

                Session["trans_custID"] = null;

                if (!IsPostBack)
                {
                    int serviceMask = Database.GetFranchiseServiceMask(Globals.GetFranchiseMask());
                    if ((serviceMask & 2) == 0) CarpetCleaningLabel.Visible = false;
                    if ((serviceMask & 4) == 0) WindowWashingLabel.Visible = false;
                    if ((serviceMask & 8) == 0) HomewatchLabel.Visible = false;

                    if (Request.UrlReferrer != null && customerID > 0)
                        ViewState["PreviousPageUrl"] = Request.UrlReferrer.ToString();

                    AccountStatusFilter.Text = Globals.GetCookieValue("AccountStatusFilter");
                    if (null != (ErrorLabel.Text = LoadCustomer(customerID)))
                    {
                        SaveButton.Enabled = false;
                        DeleteButton.Enabled = false;
                        return;
                    }
                }

                LoadAppointments(customerID);
                LoadTransactionHistory(customerID);
                LoadReferrals(customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void LinkSaveCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                if (SaveChanges()) Response.Redirect(e.CommandArgument.ToString());
            }
        }

        public void PrevClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    Response.Cookies.Add(new HttpCookie("AccountStatusFilter", AccountStatusFilter.Text));
                    int customerID = Database.GetNextCustomer(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), AccountStatusFilter.Text, true);
                    Response.Redirect("Customers.aspx?custID=" + customerID);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PrevClick EX: " + ex.Message;
            }
        }

        public void NextClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    Response.Cookies.Add(new HttpCookie("AccountStatusFilter", AccountStatusFilter.Text));
                    int customerID = Database.GetNextCustomer(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), AccountStatusFilter.Text, false);
                    Response.Redirect("Customers.aspx?custID=" + customerID);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NextClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = Database.DeleteCustomer(Globals.SafeIntParse(Request["custID"]));
                if (error == null) Response.Redirect("Customers.aspx");
                else ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }

        public void WebQuoteClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect("WebQuoteReply.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "WebQuoteClick EX: " + ex.Message;
            }
        }

        public void BidClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect("BidSheet.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "BidClick EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect("Customers.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NewClick EX: " + ex.Message;
            }
        }

        public void CopyCustomerClick(object sender, EventArgs e)
        {
            try
            {
                copyID = 0;
                copyOnlyBilling = false;
                if (SearchBox.Text.Contains("ID="))
                {
                    copyID = Globals.SafeIntParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3));
                    if (SaveChanges()) Response.Redirect("Customers.aspx?custID=" + customer.customerID);
                }
                else
                {
                    ErrorLabel.Text = "You must first select a source customer in the Search Box.";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CopyCustomerClick EX: " + ex.Message;
            }
        }

        public void CopyBillingClick(object sender, EventArgs e)
        {
            try
            {
                copyID = 0;
                copyOnlyBilling = true;
                if (SearchBox.Text.Contains("ID="))
                {
                    copyID = Globals.SafeIntParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3));
                    if (SaveChanges()) Response.Redirect("Customers.aspx?custID=" + customer.customerID);
                }
                else
                {
                    ErrorLabel.Text = "You must first select a source customer in the Search Box.";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CopyBillingClick EX: " + ex.Message;
            }
        }

        public void ReferredClick(object sender, EventArgs e)
        {
            try
            {
                referredBy = 0;
                if (SearchBox.Text.Contains("ID=")) referredBy = Globals.SafeIntParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3));
                if (SaveChanges()) Response.Redirect("Customers.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ReferredClick EX: " + ex.Message;
            }
        }

        public void CleaningPackClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges()) Response.Redirect("CleaningPacks.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ReferredClick EX: " + ex.Message;
            }
        }

        public void GiftCardClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges()) Response.Redirect("GiftCards.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "GiftCardClick EX: " + ex.Message;
            }
        }

        public void SearchClick(object sender, EventArgs e)
        {
            try
            {
                if (SearchBox.Text != "")
                {
                    int customerID = 0;
                    if (SearchBox.Text.Contains("ID="))
                        int.TryParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3), out customerID);

                    if (SaveChanges()) Response.Redirect("Customers.aspx?custID=" + customerID);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SearchClick EX: " + ex.Message;
            }
        }

        public void EnableRewardsClick(object sender, EventArgs e)
        {
            try
            {
                Database.GetCustomerByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), out customer);
                if (customer.customerID > 0)
                {
                    DBRow row = new DBRow();
                    row.SetValue("rewardsEnabled", true);
                    string error = Database.DynamicSetWithKeyInt("Customers", "customerID", ref customer.customerID, row);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Enabling Reqards: " + error;
                    }
                    else
                    {
                        if (Globals.ValidEmail(customer.email))
                            error = SendEmail.SendCustomerRewardsEnabled(customer);
                        if (error != null)
                        {
                            ErrorLabel.Text = error;
                        }
                        else
                        {
                            EnableRewardsButton.Text = "Rewards Enabled";
                            EnableRewardsButton.Enabled = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "EnableRewardsClick EX: " + ex.Message;
            }
        }

        public void ReviewUsClick(object sender, EventArgs e)
        {
            try
            {
                int customerID = Globals.SafeIntParse(Request["custID"]);
                if (string.IsNullOrWhiteSpace(FirstName.Text))
                {
                    ErrorLabel.Text = "Customer first name cannot be blank.";
                }
                else
                {
                    string error = Texting.SendReviewUsText(customerID);
                    if (error != null)
                    {
                        ErrorLabel.Text = "SMS Error: " + error;
                    }
                    else
                    {
                        if (Globals.ValidEmail(Email.Text))
                        {
                            error = SendEmail.SendReviewUsEmail(customerID);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Email Error: " + error;
                                return;
                            }
                        }

                        DBRow row = new DBRow();
                        row.SetValue("reviewUsDate", DateTime.Now);
                        error = Database.DynamicSetWithKeyInt("Customers", "customerID", ref customerID, row);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Setting Customer Review Date: " + error;
                        }
                        else
                        {
                            ReviewUsButton.Text = "Review Us (Sent: " + DateTime.Now.ToString("d") + ")";
                            ReviewUsButton.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendWelcomeClick EX: " + ex.Message;
            }
        }

        public void SendWelcomeClick(object sender, EventArgs e)
        {
            try
            {
                if (Globals.ValidEmail(Email.Text))
                {
                    if (!string.IsNullOrEmpty(FirstName.Text))
                    {
                        if (!string.IsNullOrEmpty(AccountRep.SelectedItem.Text))
                        {
                            int franchiseID = Globals.SafeIntParse(FranchiseList.SelectedValue);
                            if (franchiseID != 0)
                            {
                                foreach (FranchiseStruct franchise in Database.GetFranchiseList())
                                {
                                    if (franchise.franchiseID == franchiseID)
                                    {
                                        string error = SendEmail.SendWelcomeLetter(Email.Text, FirstName.Text, AccountRep.SelectedItem.Text, franchise);
                                        if (error == null)
                                        {
                                            error = Database.SetCustomerWelcomeLetter(Globals.SafeIntParse(Request["custID"]));
                                            if (error == null)
                                            {
                                                SendWelcomeButton.Text = "Welcome Letter Sent";
                                                SendWelcomeButton.Enabled = false;
                                            }
                                            else ErrorLabel.Text = error;
                                        }
                                        else ErrorLabel.Text = error;
                                        return;
                                    }
                                }
                                ErrorLabel.Text = "You must first select a valid franchise.";
                            }
                            else ErrorLabel.Text = "You must first select a franchsie.";
                        }
                        else ErrorLabel.Text = "You must first select an account representative.";
                    }
                    else ErrorLabel.Text = "Customer first name cannot be blank.";
                }
                else ErrorLabel.Text = "Invalid customer email.";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendWelcomeClick EX: " + ex.Message;
            }
        }

        public void SendLoginInfoClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    int customerID = Globals.SafeIntParse(Request["custID"]);
                    if (customerID <= 0)
                    {
                        ErrorLabel.Text = "Error: Invalid Customer";
                    }
                    else
                    {
                        string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error: Invalid Customer";
                        }
                        else
                        {
                            error = SendEmail.SendCustomerLoginInfo(customer);
                            if (error != null)
                            {
                                ErrorLabel.Text = error;
                            }
                            else
                            {
                                DBRow row = new DBRow();
                                row.SetValue("loginInfoSent", true);
                                error = Database.DynamicSetWithKeyInt("Customers", "customerID", ref customer.customerID, row);
                                if (error != null)
                                {
                                    ErrorLabel.Text = "Error Setting Customer Login Info Sent: " + error;
                                }
                                else
                                {
                                    SendLoginInfo.ForeColor = Color.DarkGreen;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendLoginInfoClick EX: " + ex.Message;
            }
        }

        public void RedeemClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    if (customer.customerID <= 0)
                    {
                        ErrorLabel.Text = "Error: Invalid Customer";
                    }
                    else
                    {
                        int giftCardID = (int)Globals.DecodeZBase32(Redeem.Text);
                        if (giftCardID == 0)
                        {
                            ErrorLabel.Text = "Invalid E-Gift Card Code";
                        }
                        else
                        {
                            GiftCardStruct giftCard = Database.GetGiftCardByID(-1, giftCardID);
                            if (giftCard.giftCardID == 0)
                            {
                                ErrorLabel.Text = "Invalid E-Gift Card Code";
                            }
                            else
                            {
                                if (giftCard.isVoid)
                                {
                                    ErrorLabel.Text = "This E-Gift Card has been voided.";
                                }
                                else
                                {
                                    if (giftCard.redeemed != 0)
                                    {
                                        ErrorLabel.Text = "This E-Gift Card has already been redeemed.";
                                    }
                                    else
                                    {
                                        giftCard.redeemed = customer.customerID;

                                        DBRow row = new DBRow();
                                        row.SetValue("redeemed", customer.customerID);
                                        string error = Database.DynamicSetWithKeyInt("GiftCards", "giftCardID", ref giftCard.giftCardID, row);
                                        if (error != null)
                                        {
                                            ErrorLabel.Text = "Error Redeeming E-Gift Card: " + error;
                                        }
                                        else
                                        {
                                            error = Database.AddCustomerPoints(-1, customer.customerID, giftCard.amount);
                                            if (error != null)
                                            {
                                                ErrorLabel.Text = "Error converting E-Gift Card Point: " + error;
                                            }
                                            else
                                            {
                                                Response.Redirect("Customers.aspx?custID=" + customer.customerID);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendLoginInfoClick EX: " + ex.Message;
            }
        }

        public void SendHWContractClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    int customerID = Globals.SafeIntParse(Request["custID"]);
                    if (customerID <= 0)
                    {
                        ErrorLabel.Text = "Error: Invalid Customer";
                    }
                    else
                    {
                        string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error: Invalid Customer";
                        }
                        else
                        {
                            error = SendEmail.SendHomeGuardActivation(customer);
                            if (error != null)
                            {
                                ErrorLabel.Text = error;
                            }
                            else
                            {
                                DBRow row = new DBRow();
                                row.SetValue("dateSent", DateTime.Now.ToString("g"));
                                row.SetValue("dateSigned", "");
                                Database.DynamicSetWithKeyInt("HomeGuardContract", "customerID", ref customerID, row);

                                ViewHWContract.Enabled = false;
                                SendHWContract.ForeColor = Color.Red;
                                SendHWContract.Text = "Contract Sent (" + DateTime.Now.ToString("g") + ")";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendHWContractClick EX: " + ex.Message;
            }
        }

        public void ViewHWContractClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    int customerID = Globals.SafeIntParse(Request["custID"]);
                    if (customerID <= 0)
                    {
                        ErrorLabel.Text = "Error: Invalid Customer";
                    }
                    else
                    {
                        Response.Redirect(Globals.BuildQueryString("/HomeGuardContract.aspx", "View", Globals.Encrypt(customerID.ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendHWContractClick EX: " + ex.Message;
            }
        }

        public void NewAppointmentClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    if (customer.customerID == 0) ErrorLabel.Text = "You must first provide a valid customer name before scheduling an appointment";
                    else Response.Redirect("Appointments.aspx?custID=" + customer.customerID);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NewAppointmentClick EX: " + ex.Message;
            }
        }

        public void ScheduleClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges()) Response.Redirect("Schedule.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ScheduleClick EX: " + ex.Message;
            }
        }

        public void DeleteAppointmentRangeClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges()) Response.Redirect("DeleteAppointments.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteAppointmentRangeClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["PreviousPageUrl"] != null && string.IsNullOrEmpty(ErrorLabel.Text))
                {
                    string prevUrl = ViewState["PreviousPageUrl"].ToString();
                    if (prevUrl.Contains("ViewReport.aspx") || prevUrl.Contains("Schedule.aspx") || prevUrl.Contains("Calendar.aspx"))
                        Response.Redirect(Globals.BuildQueryString(prevUrl, "DoScroll", "Y"));
                }
                Response.Redirect("Customers.aspx?custID=" + customer.customerID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }

        public void SaveClick(object sender, EventArgs e)
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

        public void RedirectTransaction(string transType)
        {
            try
            {
                string query = Globals.BuildQueryString("Transaction.aspx", "transType", transType);
                query = Globals.BuildQueryString(query, "custID", Globals.SafeIntParse(Request["custID"]));
                query = Globals.BuildQueryString(query, "transDate", Globals.UtcToMst(DateTime.UtcNow).ToString("d"));
                query = Globals.BuildQueryString(query, "hourlyRate", Globals.FormatMoney(RatePerHour.Text));
                query = Globals.BuildQueryString(query, "serviceFee", Globals.FormatMoney(ServiceFee.Text));
                query = Globals.BuildQueryString(query, "total", Globals.FormatMoney(ServiceFee.Text));
                Response.Redirect(query);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "RedirectTransaction EX: " + ex.Message;
            }
        }

        public void PaymentClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    RedirectTransaction("Sale");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PaymentClick EX: " + ex.Message;
            }
        }

        public void ReturnClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    RedirectTransaction("Return");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ReturnClick EX: " + ex.Message;
            }
        }

        public void InvoiceClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    RedirectTransaction("Invoice");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "InvoiceClick EX: " + ex.Message;
            }
        }

        public void EmailInvoiceClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    ErrorLabel.Text = "";
                    EmailInvoiceButton.ForeColor = Color.Red;
                    int customerID = Globals.SafeIntParse(Request["custID"]);
                    DateTime startDate = Globals.DateTimeParse(InvoiceStartDate.Text);
                    DateTime endDate = Globals.DateTimeParse(InvoiceEndDate.Text);

                    if (customerID <= 0)
                    {
                        ErrorLabel.Text = "Invalid Customer for Email Invoices";
                    }
                    else
                    {
                        if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                        {
                            ErrorLabel.Text = "Invalid Date Range for Email Invoices";
                        }
                        else
                        {
                            TransDoc doc = new TransDoc(Globals.GetFranchiseMask(), customerID);
                            doc.emailSubject = "2 Local Gals Invoice (" + startDate.ToString("d") + " to " + endDate.ToString("d") + ")";

                            var appList = Database.GetAppsByCustomerID(customerID);
                            appList.Reverse();

                            foreach (AppStruct app in appList)
                            {
                                if (app.appointmentDate.Date >= startDate.Date && app.appointmentDate.Date <= endDate.Date)
                                {
                                    TransDocService service;
                                    if (doc.serviceList.Count == 0 || doc.serviceList[doc.serviceList.Count - 1].date.Date != app.appointmentDate)
                                    {
                                        service = new TransDocService();
                                        service.date = app.appointmentDate;
                                        service.description = "Cleaning Service";
                                        doc.serviceList.Add(service);
                                    }

                                    service = doc.serviceList[doc.serviceList.Count - 1];
                                    if (app.appType == 1) service.hours += app.customerHours;
                                    if (app.customerRate > service.rate) service.rate = app.customerRate;
                                    service.serviceFee += app.customerServiceFee;
                                    service.discount += app.customerDiscountAmount + Globals.CalculateDiscountPercent(app.appType == 1 ? app.customerHours : 0, app.customerRate, app.customerServiceFee, app.customerDiscountPercent + app.customerDiscountReferral, app.appointmentDate);
                                    service.amount += Globals.CalculateAppointmentTotal(app);
                                    if (!string.IsNullOrEmpty(EmailInvoiceMemo.Text)) doc.memo = EmailInvoiceMemo.Text;
                                    doc.serviceList[doc.serviceList.Count - 1] = service;
                                }
                            }

                            DBRow row = new DBRow();
                            row.SetValue("customerID", customerID);
                            row.SetValue("startDate", startDate);
                            row.SetValue("endDate", endDate);
                            row.SetValue("email", Email.Text);
                            row.SetValue("memo", EmailInvoiceMemo.Text ?? "");

                            int invoiceID = 0;
                            string error = Database.DynamicSetWithKeyInt("InvoiceRange", "invoiceID", ref invoiceID, row);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Error Send Invoice Email (DB): " + error; ;
                            }
                            else
                            {
                                doc.transNumber = invoiceID.ToString();
                                error = SendEmail.SendTransDoc(doc);
                                if (error != null)
                                {
                                    Database.DynamicDeleteWithKey("InvoiceRange", "invoiceID", invoiceID);
                                    ErrorLabel.Text = "Error Send Invoice Email: " + error;
                                }
                                else
                                {
                                    EmailInvoiceButton.ForeColor = Color.Green;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "InvoiceClick EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                ErrorLabel.Text = "";
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

                int customerID = Globals.SafeIntParse(Request["custID"]);
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                if (error != null)
                {
                    ErrorLabel.Text = error;
                }
                else
                {
                    customer.customerID = customerID;

                    if (referredBy >= 0)
                        customer.referredBy = referredBy;

                    // Account Info
                    customer.franchiseMask = Globals.IDToMask(Globals.SafeIntParse(FranchiseList.SelectedValue));
                    customer.accountStatus = AccountStatus.Text;
                    customer.accountType = AccountType.Text;
                    customer.newBuilding = NewBuilding.Checked;
                    customer.bookedBy = Globals.SafeIntParse(AccountRep.SelectedValue);
                    customer.paymentType = PaymentType.Text;
                    customer.ratePerHour = Globals.FormatMoney(RatePerHour.Text);
                    customer.serviceFee = Globals.FormatMoney(ServiceFee.Text);
                    customer.estimatedHours = EstimatedHours.Text;
                    customer.estimatedCC = EstimatedCC.Text;
                    customer.estimatedWW = EstimatedWW.Text;
                    customer.estimatedHW = EstimatedHW.Text;
                    customer.estimatedPrice = EstimatedPrice.Text;
                    customer.advertisement = Advertisement.Text;
                    customer.preferredContact = PreferredContact.Text;
                    customer.sendPromotions = SendPromotions.Checked;

                    // Customer Info
                    customer.customNote = CustomNote.Text;
                    customer.businessName = BusinessName.Text;
                    customer.companyContact = CompanyContact.Text;
                    customer.firstName = FirstName.Text;
                    customer.lastName = LastName.Text;
                    customer.spouseName = SpouseName.Text;
                    customer.locationAddress = LocationAddress.Text;
                    customer.locationCity = LocationCity.Text;
                    customer.locationState = LocationState.Text;
                    customer.locationZip = LocationZipCode.Text;
                    customer.bestPhone = Globals.FormatPhone(BestPhone.Text);
                    customer.bestPhoneCell = BestPhoneCell.Checked;
                    customer.alternatePhoneOne = Globals.FormatPhone(AlternatePhoneOne.Text);
                    customer.alternatePhoneOneCell = AlternatePhoneOneCell.Checked;
                    customer.alternatePhoneTwo = Globals.FormatPhone(AlternatePhoneTwo.Text);
                    customer.alternatePhoneTwoCell = AlternatePhoneTwoCell.Checked;
                    customer.email = Email.Text.Replace(" ", "");
                    customer.NC_Notes = NC_Notes.Text;
                    customer.TakePic = TakePic.Checked;


                    // Scheduling
                    customer.NC_Frequency = NC_Frequency.Text;
                    customer.NC_DayOfWeek = NC_DayOfWeek.Text;
                    customer.NC_TimeOfDayPrefix = NC_TimeOfDayPrefix.Text;
                    customer.NC_TimeOfDay = NC_TimeOfDaySuffix.Text;
                    customer.NC_Special = NC_SpecialNotes.Text;

                    // Billing Info
                    string creditCardNumber = Globals.OnlyNumbers(CreditCardNumber.Text);
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
                    customer.billingZip = BillingZip.Text;

                    // Normal Clean
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
                    customer.NC_CleanRating = NC_CleanRating.Text;
                    customer.NC_RequiresKeys = NC_RequiresKeys.Checked;
                    customer.NC_Vacuum = NC_BringVacuum.Checked;
                    customer.NC_DoDishes = NC_DoDishes.Checked;
                    customer.NC_ChangeBed = NC_ChangeBed.Checked;
                    customer.NC_EnterHome = NC_EnterHome.Text;
                    customer.NC_CleaningType = NC_CleaningType.Text;
                    customer.NC_Organize = NC_Organize.Checked;
                    customer.NC_Details = NC_Details.Text;
                    customer.NC_GateCode = NC_GateCode.Text;
                    customer.NC_GarageCode = NC_GarageCode.Text;
                    customer.NC_DoorCode = NC_DoorCode.Text;
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

                    //Auto Notes
                    string autoNotes = "";
                    CustomerStruct oldCustomer;
                    if (null != Database.GetCustomerByID(Globals.GetFranchiseMask(), customer.customerID, out oldCustomer))
                        oldCustomer = new CustomerStruct();
                    if (oldCustomer.customerID != 0)
                    {
                        if (oldCustomer.bestPhone != customer.bestPhone && !string.IsNullOrEmpty(oldCustomer.bestPhone))
                            autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated phone from (" + oldCustomer.bestPhone + ") to (" + customer.bestPhone + ")";
                        if (oldCustomer.alternatePhoneOne != customer.alternatePhoneOne && !string.IsNullOrEmpty(oldCustomer.alternatePhoneOne))
                            autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated alt phone 1 from (" + oldCustomer.alternatePhoneOne + ") to (" + customer.alternatePhoneOne + ")";
                        if (oldCustomer.alternatePhoneTwo != customer.alternatePhoneTwo && !string.IsNullOrEmpty(oldCustomer.alternatePhoneTwo))
                            autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated alt phone 2 from (" + oldCustomer.alternatePhoneTwo + ") to (" + customer.alternatePhoneTwo + ")";
                        if (oldCustomer.email != customer.email && !string.IsNullOrEmpty(oldCustomer.email))
                            autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated email from (" + oldCustomer.email + ") to (" + customer.email + ")";
                        if (oldCustomer.creditCardNumber != customer.creditCardNumber && !string.IsNullOrEmpty(oldCustomer.creditCardNumber))
                            autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated card from (xxxx " + Globals.FormatCardLastFour(oldCustomer.creditCardNumber) + ") to (xxxx " + Globals.FormatCardLastFour(customer.creditCardNumber) + ")";
                        if (!string.IsNullOrEmpty(oldCustomer.creditCardExpYear) && !string.IsNullOrEmpty(oldCustomer.creditCardExpMonth) && !string.IsNullOrEmpty(oldCustomer.creditCardCCV))
                        {
                            if (oldCustomer.creditCardExpYear != customer.creditCardExpYear || oldCustomer.creditCardExpMonth != customer.creditCardExpMonth || oldCustomer.creditCardCCV != customer.creditCardCCV)
                                autoNotes += "\n" + Globals.GetUsername() + " " + mst.ToString("d") + ": Updated (Yr " + oldCustomer.creditCardExpYear + ", Mo " + oldCustomer.creditCardExpMonth + ", CVV " + oldCustomer.creditCardCCV + ") to (Yr " + customer.creditCardExpYear + ", Mo " + customer.creditCardExpMonth + ", CVV " + customer.creditCardCCV + ")";
                        }
                    }
                    if (autoNotes != "") customer.NC_Notes += autoNotes;

                    if (copyID > 0)
                    {
                        CustomerStruct copyCustomer;
                        if (null == Database.GetCustomerByID(Globals.GetFranchiseMask(), copyID, out copyCustomer))
                        {
                            customer.creditCardNumber = copyCustomer.creditCardNumber;
                            customer.creditCardExpMonth = copyCustomer.creditCardExpMonth;
                            customer.creditCardExpYear = copyCustomer.creditCardExpYear;
                            customer.creditCardCCV = copyCustomer.creditCardCCV;
                            customer.billingSame = copyCustomer.billingSame;
                            customer.billingName = copyCustomer.billingName;
                            customer.billingAddress = copyCustomer.billingAddress;
                            customer.billingCity = copyCustomer.billingCity;
                            customer.billingState = copyCustomer.billingState;
                            customer.billingZip = copyCustomer.billingZip;

                            if (!copyOnlyBilling)
                            {
                                customer.accountType = copyCustomer.accountType;
                                customer.paymentType = copyCustomer.paymentType;
                                customer.ratePerHour = copyCustomer.ratePerHour;
                                customer.serviceFee = copyCustomer.serviceFee;
                                customer.firstName = copyCustomer.firstName;
                                customer.lastName = copyCustomer.lastName;
                                customer.businessName = copyCustomer.businessName;
                                customer.bestPhone = copyCustomer.bestPhone;
                                customer.bestPhoneCell = copyCustomer.bestPhoneCell;
                                customer.alternatePhoneOne = copyCustomer.alternatePhoneOne;
                                customer.alternatePhoneOneCell = copyCustomer.alternatePhoneOneCell;
                                customer.alternatePhoneTwo = copyCustomer.alternatePhoneTwo;
                                customer.alternatePhoneTwoCell = copyCustomer.alternatePhoneTwoCell;
                                customer.email = copyCustomer.email;
                                customer.NC_Notes = copyCustomer.NC_Notes;
                                customer.NC_Special = copyCustomer.NC_Special;
                                customer.NC_Details = copyCustomer.NC_Details;
                                customer.CC_Details = copyCustomer.CC_Details;
                                customer.WW_Details = copyCustomer.WW_Details;
                                customer.HW_Details = copyCustomer.HW_Details;
                            }
                        }
                    }

                    //Check for blank new customer
                    if (customer.customerID == 0 && string.IsNullOrEmpty(customer.firstName) && string.IsNullOrEmpty(customer.lastName) && string.IsNullOrEmpty(customer.businessName)) return true;

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
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
            }
            return false;
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

                PaymentType.Items.Clear();
                foreach (string i in Database.GetFranchiseDropDown(Globals.GetFranchiseMask(), "paymentList"))
                    PaymentType.Items.Add(i);

                customer = new CustomerStruct();
                if ((error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer)) != null)
                    return error;

                FranchiseStruct fran = Globals.GetFranchiseByMask(Globals.GetFranchiseMask());

                if (customerID == 0)
                {
                    CustomerTitle.InnerText = "New Customer";
                    CustomerTitleExtra.InnerText = "";
                    if (!string.IsNullOrEmpty(fran.state)) defaultState = fran.state;
                    if (fran.defaultRatePerHour > defaultRatePerHour) defaultRatePerHour = fran.defaultRatePerHour;
                    if (fran.defaultServiceFee > defaultServiceFee) defaultServiceFee = fran.defaultServiceFee;
                    customer.sendPromotions = true;
                    customer.sectionMask = 1;
                }
                else
                {
                    float followUpScore = 0;
                    string followUpColor = "black";
                    List<DBRow> scoreList = Database.GetFollowUpScoresByCustomerID(customerID);
                    string followUpURL = Globals.BuildQueryString("ViewReport.aspx", "report", "15");
                    followUpURL = Globals.BuildQueryString(followUpURL, "startDate", (DateTime.Now - TimeSpan.FromDays(90)).ToString("d"));
                    followUpURL = Globals.BuildQueryString(followUpURL, "endDate", DateTime.Now.ToString("d"));
                    followUpURL = Globals.BuildQueryString(followUpURL, "mask", customer.franchiseMask);
                    followUpURL = Globals.BuildQueryString(followUpURL, "custID", customer.customerID);
                    if (scoreList.Count > 0)
                    {
                        foreach (DBRow score in scoreList)
                        {
                            followUpScore += score.GetInt("timeManagement");
                            followUpScore += score.GetInt("professionalism");
                            followUpScore += score.GetInt("cleaningQuality");
                        }
                        followUpScore /= (3 * scoreList.Count);

                        if (followUpScore > 0f) followUpColor = "red";
                        if (followUpScore >= 4f) followUpColor = "orange";
                        if (followUpScore >= 4.5f) followUpColor = "green";
                    }

                    CustomerTitle.InnerText = customer.customerTitle;
                    CustomerTitleExtraLabel.Text = @"<b>Satisfaction: </b><a href=""" + followUpURL + @"""><b style=""color:" + followUpColor + @";"">" + followUpScore.ToString("N2") + "</b></a> <b>Points: </b>" + Globals.FormatPoints(customer.points, customer.ratePerHour);
                    CustomerTitleExtraLabel.Text += customer.bookedDate > new DateTime(1980, 1, 1) ? " <b>Created: </b>" + customer.bookedDate.ToString("d") : "";
                    CustomerStruct referredCustomer;
                    Database.GetCustomerByID(-1, customer.referredBy, out referredCustomer);
                    if (referredCustomer.customerID > 0)
                    {
                        CustomerTitleExtraLabel.Text += " <b>Referred By: </b>";
                        ReferredByLink.CommandArgument = @"Customers.aspx?custID=" + referredCustomer.customerID;
                        ReferredByLink.Text = referredCustomer.customerTitle;
                        ReferredByLink.Visible = true;
                    }

                    SendLoginInfo.Text = "Send Login Info (" + Globals.CustomerIDToPassphrase(customerID) + ")";
                    SendLoginInfo.Enabled = true;

                    if (customer.reviewUsDate > new DateTime(1990, 1, 1))
                    {
                        ReviewUsButton.Text = "Review Us (Sent: " + customer.reviewUsDate.ToString("d") + ")";
                        ReviewUsButton.ForeColor = (customer.reviewUsDate < (DateTime.Now - TimeSpan.FromDays(180))) ? Color.Red : Color.Green;
                    }
                    ReviewUsButton.Enabled = true;

                    DBRow hwContractRow = Database.GetHomeGuardContract(customerID);
                    if (hwContractRow != null)
                    {
                        if (string.IsNullOrEmpty(hwContractRow.GetString("dateSigned")))
                        {
                            SendHWContract.ForeColor = Color.Red;
                            SendHWContract.Text = "Contract Sent (" + hwContractRow.GetString("dateSent") + ")";
                        }
                        else
                        {
                            SendHWContract.ForeColor = Color.Green;
                            SendHWContract.Text = "Contract Verified";
                            ViewHWContract.Enabled = true;
                        }
                    }
                }

                //Web Quote
                WebQuoteButton.Visible = customer.customerID > 0;
                if (customer.quoteReply) WebQuoteButton.Style["color"] = "Green";

                // Account Info
                FranchiseList.Items.Clear();
                FranchiseList.Items.Add(new ListItem("", "0"));
                FranchiseList.Items.AddRange(Globals.GetFranchiseList(Globals.GetFranchiseMask(), customer.franchiseMask));
                if (FranchiseList.Items.Count == 2) FranchiseList.SelectedIndex = 1;
                Globals.SetDropDownList(ref AccountStatus, customer.accountStatus);
                if (string.IsNullOrEmpty(AccountStatus.Text)) AccountStatus.Text = "Quote";
                AccountStatusFilter.Text = customer.accountStatus;
                Globals.SetDropDownList(ref AccountType, customer.accountType);
                if (string.IsNullOrEmpty(AccountType.Text)) AccountType.Text = "Home";
                NewBuilding.Checked = customer.newBuilding;
                ContractorStruct contractor;
                AccountRep.Items.Clear();
                AccountRep.Items.Add(new ListItem("", "0"));
                int accountRepID = customer.bookedBy == 0 ? Globals.GetUserContractorID(this) : customer.bookedBy;
                AccountRep.Items.AddRange(Globals.GetContractorList(Globals.GetFranchiseMask(), accountRepID, 0, out contractor, true, true));
                Globals.SetDropDownList(ref PaymentType, customer.paymentType);
                RatePerHour.Text = defaultRatePerHour > 0 ? Globals.FormatMoney(defaultRatePerHour) : Globals.FormatMoney(customer.ratePerHour);
                ServiceFee.Text = defaultServiceFee > 0 ? Globals.FormatMoney(defaultServiceFee) : Globals.FormatMoney(customer.serviceFee);
                EstimatedHours.Text = customer.estimatedHours;
                EstimatedCC.Text = customer.estimatedCC;
                EstimatedWW.Text = customer.estimatedWW;
                EstimatedHW.Text = customer.estimatedHW;
                EstimatedPrice.Text = customer.estimatedPrice;
                Globals.SetDropDownList(ref Advertisement, customer.advertisement);
                Globals.SetDropDownList(ref PreferredContact, customer.preferredContact);
                SendPromotions.Checked = customer.sendPromotions;

                // Customer Info
                CustomNote.Text = customer.customNote;
                BusinessName.Text = customer.businessName;
                CompanyContact.Text = customer.companyContact;
                FirstName.Text = customer.firstName;
                LastName.Text = customer.lastName;
                SpouseName.Text = customer.spouseName;
                LocationAddress.Text = customer.locationAddress;
                LocationCity.Text = customer.locationCity;
                LocationState.Text = defaultState != null ? defaultState : customer.locationState;
                LocationZipCode.Text = customer.locationZip;
                BestPhone.Text = Globals.FormatPhone(customer.bestPhone);
                BestPhoneCell.Checked = customer.bestPhoneCell;
                AlternatePhoneOne.Text = Globals.FormatPhone(customer.alternatePhoneOne);
                AlternatePhoneOneCell.Checked = customer.alternatePhoneOneCell;
                AlternatePhoneTwo.Text = Globals.FormatPhone(customer.alternatePhoneTwo);
                AlternatePhoneTwoCell.Checked = customer.alternatePhoneTwoCell;
                Email.Text = customer.email;
                NC_Notes.Text = customer.NC_Notes;
                TakePic.Checked = customer.TakePic;
                if (customer.welcomeLetter)
                {
                    SendWelcomeButton.Text = "Welcome Letter Sent";
                    SendWelcomeButton.Enabled = false;
                }

                if (customer.loginInfoSent)
                {
                    SendLoginInfo.ForeColor = Color.DarkGreen;
                }

                if (customer.customerID == 0 || fran.rewardsPercentage <= 0)
                {
                    EnableRewardsButton.Visible = false;
                }
                else if (customer.rewardsEnabled)
                {
                    EnableRewardsButton.Text = "Rewards Enabled";
                    EnableRewardsButton.Enabled = false;
                }

                // Scheduling
                Globals.SetDropDownList(ref NC_Frequency, customer.NC_Frequency);
                Globals.SetDropDownList(ref NC_DayOfWeek, customer.NC_DayOfWeek);
                Globals.SetDropDownList(ref NC_TimeOfDayPrefix, customer.NC_TimeOfDayPrefix);
                Globals.SetDropDownList(ref NC_TimeOfDaySuffix, customer.NC_TimeOfDay);
                NC_SpecialNotes.Text = customer.NC_Special;

                // Billing Info
                CreditCardNumber.Text = Globals.FormatCard(customer.creditCardNumber, true);
                ExpirationMonth.Text = Globals.SafeIntParse(customer.creditCardExpMonth).ToString();
                ExpirationYear.Text = Globals.FormatCardExpYear(customer.creditCardExpYear);
                CCVCode.Text = customer.creditCardCCV;
                BillingSame.Checked = customer.billingSame;
                BillingName.Text = customer.billingName;
                BillingAddress.Text = customer.billingAddress;
                BillingCity.Text = customer.billingCity;
                BillingState.Text = customer.billingState;
                BillingZip.Text = customer.billingZip;

                // Normal Clean
                Globals.SetDropDownList(ref NC_Bedrooms, customer.NC_Bedrooms);
                Globals.SetDropDownList(ref NC_Bathrooms, customer.NC_Bathrooms);
                NC_SquareFootage.Text = customer.NC_SquareFootage;
                Globals.SetDropDownList(ref NC_Pets, customer.NC_Pets);
                NC_FlooringCarpet.Checked = customer.NC_FlooringCarpet;
                NC_FlooringHardwood.Checked = customer.NC_FlooringHardwood;
                NC_FlooringTile.Checked = customer.NC_FlooringTile;
                NC_FlooringLinoleum.Checked = customer.NC_FlooringLinoleum;
                NC_FlooringSlate.Checked = customer.NC_FlooringSlate;
                NC_FlooringMarble.Checked = customer.NC_FlooringMarble;
                Globals.SetDropDownList(ref NC_CleanRating, customer.NC_CleanRating);
                NC_RequiresKeys.Checked = customer.NC_RequiresKeys;
                NC_BringVacuum.Checked = customer.NC_Vacuum;
                NC_DoDishes.Checked = customer.NC_DoDishes;
                NC_ChangeBed.Checked = customer.NC_ChangeBed;
                Globals.SetDropDownList(ref NC_EnterHome, customer.NC_EnterHome);
                Globals.SetDropDownList(ref NC_CleaningType, customer.NC_CleaningType);
                NC_Organize.Checked = customer.NC_Organize;
                NC_Details.Text = customer.NC_Details;
                NC_GateCode.Text = customer.NC_GateCode;
                NC_GarageCode.Text = customer.NC_GarageCode;
                NC_DoorCode.Text = customer.NC_DoorCode;
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

                return null;
            }
            catch (Exception ex)
            {
                return "LoadCustomer EX: " + ex.Message;
            }
        }

        public void LoadAppointments(int customerID)
        {
            try
            {
                if (AppTable.Rows.Count > 0) return;

                TableHeaderCell headerCell = null;
                TableHeaderRow headerRow = new TableHeaderRow();

                headerCell = new TableHeaderCell();
                headerCell.Text = "Date";
                headerCell.Width = 100;
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Start Time";
                headerCell.Width = 80;
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "End Time";
                headerCell.Width = 80;
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Contractor";
                headerCell.Width = 150;
                headerRow.Cells.Add(headerCell);

                int currentDateIndex = 1;
                DateTime currDate = Globals.UtcToMst(DateTime.UtcNow);
                Dictionary<int, int> rowColors = new Dictionary<int, int>();

                AppTable.Rows.Add(headerRow);
                foreach (AppStruct app in Database.GetAppsByCustomerID(customerID))
                {
                    TableCell cell = null;
                    TableRow row = new TableRow();

                    if (app.recurrenceID != 0)
                    {
                        if (!rowColors.ContainsKey(app.recurrenceID)) rowColors.Add(app.recurrenceID, rowColors.Keys.Count);
                        switch (rowColors[app.recurrenceID])
                        {
                            case 0: row.Style["background"] = "#00CCFF"; break;
                            case 1: row.Style["background"] = "#66FFFF"; break;
                            case 2: row.Style["background"] = "#0066FF"; break;
                            case 3: row.Style["background"] = "#0099CC"; break;
                            default: row.Style["background"] = "#CCCCCC"; break;
                        }
                    }

                    if (app.appType > 1)
                    {
                        row.Style["background"] = "#ffb84d";
                    }

                    string status = "";
                    if (app.appStatus == 1) status = "<br>Rescheduled";
                    if (app.appStatus == 2) status = "<br>Canceled";

                    cell = new TableCell();
                    LinkButton dateLink = new LinkButton();
                    dateLink.Text = app.appointmentDate.ToString("ddd MM/dd/yy") + status;
                    dateLink.Command += new CommandEventHandler(LinkSaveCommand);
                    dateLink.CommandArgument = @"Appointments.aspx?appID=" + app.appointmentID;
                    cell.Controls.Add(dateLink);
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = app.startTime.ToString("t");
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = app.endTime.ToString("t");
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    LinkButton conLink = new LinkButton();
                    conLink.Text = app.contractorTitle;
                    conLink.Command += new CommandEventHandler(LinkSaveCommand);
                    conLink.CommandArgument = @"Appointments.aspx?appID=" + app.appointmentID;
                    cell.Controls.Add(conLink);
                    row.Cells.Add(cell);

                    
                    AppTable.Rows.Add(row);

                    if (app.appointmentDate.Date >= currDate.Date)
                        currentDateIndex = AppTable.Rows.Count;
                }

                TableRow currDateRow = new TableRow();
                currDateRow.Style["color"] = "White";
                currDateRow.Style["background"] = "Black";
                currDateRow.Style["text-align"] = "center";
                TableCell currDateCell = new TableCell();
                currDateCell.ColumnSpan = 4;
                currDateCell.Text = "Today: " + currDate.ToString("dddd MM/dd/yy");
                currDateRow.Cells.Add(currDateCell);
                AppTable.Rows.AddAt(currentDateIndex, currDateRow);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadAppointments EX: " + ex.Message;
            }
        }

        public void LoadTransactionHistory(int customerID)
        {
            try
            {
                if (customerID > 0)
                {
                    int franMask = Globals.GetFranchiseMask();
                    List<TransactionStruct> transList = Database.GetTransactions(franMask, customerID, new DateTime(1800, 1, 1), new DateTime(9000, 1, 1), "T.dateCreated DESC");
                    List<GiftCardStruct> giftCardList = Database.GetGiftCardsByCustomerID(franMask, customerID);
                    List<DBRow> cleaningPackList = Database.GetCleaningPackByCustomerID(customerID);
                    List<DBRow> invoiceRangeList = Database.GetInvoiceRangeByCustomerID(customerID);

                    while (transList.Count > 0 || giftCardList.Count > 0 || cleaningPackList.Count > 0 || invoiceRangeList.Count > 0)
                    {
                        string text = "";

                        TransactionStruct trans = transList.Count > 0 ? transList[0] : new TransactionStruct();
                        GiftCardStruct giftCard = giftCardList.Count > 0 ? giftCardList[0] : new GiftCardStruct();
                        DBRow pack = cleaningPackList.Count > 0 ? cleaningPackList[0] : new DBRow();
                        DBRow invoice = invoiceRangeList.Count > 0 ? invoiceRangeList[0] : new DBRow();


                        if (trans.dateCreated >= giftCard.dateCreated && trans.dateCreated >= pack.GetDate("dateCreated") && trans.dateCreated >= invoice.GetDate("dateCreated"))
                        {
                            string transType = trans.transType;
                            if (trans.auth == 1) transType = "Open Auth";
                            if (trans.auth == 3) transType = "Captured Auth";
                            if (trans.isVoid) transType = "Voided";
     

                            text = "[" + trans.dateApply.ToString("MM/dd/yy") + "]";
                            text += " " + transType + " (" + trans.transID + ")";
                            text += " in the amount of " + Globals.FormatMoney(trans.total);
                            if (!string.IsNullOrEmpty(trans.email) && trans.emailSent) text += " sent to " + trans.email.Replace(',', ' ');
                            if (trans.transType != "Invoice")
                            {
                                text += " using " + trans.paymentType;
                                if (!string.IsNullOrEmpty(trans.lastFourCard)) text += " xxxx" + trans.lastFourCard;
                            }
                            if (!string.IsNullOrEmpty(trans.username)) text += " by " + trans.username;

                            text = @"<a href=""Transaction.aspx?transID=" + trans.transID + @""">" + text + @"</a>";

                            transList.RemoveAt(0);
                        }
                        else if (giftCard.dateCreated >= pack.GetDate("dateCreated") && giftCard.dateCreated >= invoice.GetDate("dateCreated"))
                        {
                            text = @"[" + Globals.UtcToMst(giftCard.dateCreated).ToString("d") + @"]" + (giftCard.isVoid ? " (VOID)" : "") + @" E-Gift Card in the amount of " + Globals.FormatMoney(giftCard.amount) + @". Code: " + Globals.EncodeZBase32((uint)giftCard.giftCardID) + @".";
                            text += @" using " + giftCard.paymentType;
                            if (!string.IsNullOrEmpty(giftCard.lastFourCard)) text += " xxxx" + giftCard.lastFourCard;
                            if (giftCard.redeemed > 0) text += @" Redeemed by " + giftCard.redeemedTitle + ".";
                            else text += @" Not redeemed.";
                            text += " Sent to " + giftCard.recipientName + " (" + giftCard.recipientEmail.Replace(',', ' ') + "). Billing email (" + giftCard.billingEmail.Replace(',', ' ') + ").";
                            if (!string.IsNullOrEmpty(giftCard.username)) text += @" Phone-in 2LG username " + giftCard.username;
                            text = @"<a href=""GiftCards.aspx?giftCardID=" + giftCard.giftCardID + @""">" + text + @"</a>";
                            giftCardList.RemoveAt(0);
                        }
                        else if (pack.GetDate("dateCreated") >= invoice.GetDate("dateCreated"))
                        {
                            text = @"[" + Globals.UtcToMst(pack.GetDate("dateCreated")).ToString("d") + @"]" + (pack.GetBool("isVoid") ? " (VOID)" : "") + @" Cleaning Pack " + pack.GetString("transType") + @" in the amount of " + Globals.FormatMoney(pack.GetDecimal("amount"));

                            if (!string.IsNullOrEmpty(pack.GetString("email"))) text += " sent to " + pack.GetString("email").Replace(',', ' ');
                            text += @" using " + pack.GetString("paymentType");
                            if (!string.IsNullOrEmpty(pack.GetString("lastFourCard"))) text += " xxxx" + pack.GetString("lastFourCard");
                            text += @" by " + pack.GetString("username");

                            text = @"<a href=""CleaningPacks.aspx?packID=" + pack.GetInt("cleaningPackID") + @""">" + text + @"</a>";
                            cleaningPackList.RemoveAt(0);
                        }
                        else
                        {
                            text = @"[" + Globals.UtcToMst(invoice.GetDate("dateCreated")).ToString("d") + @"] Email Invoice Range, ID: " + invoice.GetInt("invoiceID") + ", from " + invoice.GetDate("startDate").ToString("d") + " to " + invoice.GetDate("endDate").ToString("d") + ", sent to " + invoice.GetString("email");
                            if (!string.IsNullOrEmpty(invoice.GetString("memo"))) text += ", memo: " + invoice.GetString("memo");
                            invoiceRangeList.RemoveAt(0);
                        }
                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(text));
                        TransactionTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadTransactionHistory EX: " + ex.Message;
            }
        }

        public void LoadReferrals(int customerID)
        {
            try
            {
                if (customerID > 0 && ReferralTable.Rows.Count == 1)
                {
                    List<CustomerStruct> referralsList = Database.GetCustomerReferrals(customerID);
                    ReferralsFieldset.Visible = referralsList.Count > 0;
                    foreach (CustomerStruct referral in referralsList)
                    {
                        TableCell cell = null;
                        TableRow row = new TableRow();

                        cell = new TableCell();
                        cell.Text = referral.accountStatus;
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        LinkButton dateLink = new LinkButton();
                        dateLink.Text = referral.customerTitle;
                        dateLink.Command += new CommandEventHandler(LinkSaveCommand);
                        dateLink.CommandArgument = @"Customers.aspx?custID=" + referral.customerID;
                        cell.Controls.Add(dateLink);
                        row.Cells.Add(cell);

                        ReferralTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadReferrals EX: " + ex.Message;
            }
        }
    }
}