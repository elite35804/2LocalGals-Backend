using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Drawing;

namespace TwoLocalGals.Protected
{
    public partial class Transaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5) 
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Transaction";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetNoStore();

                SaveButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(SaveButton, null) + ";");

                if (!IsPostBack)
                {
                    int serviceMask = Database.GetFranchiseServiceMask(Globals.GetFranchiseMask());
                    if ((serviceMask & 2) == 0) TableRowHeaderCC.Visible = TableRowCC.Visible = false;
                    if ((serviceMask & 4) == 0) TableRowHeaderWW.Visible = TableRowWW.Visible = false;
                    if ((serviceMask & 8) == 0) TableRowHeaderHW.Visible = TableRowHW.Visible = false;

                    if (Request.UrlReferrer != null)
                        ViewState["PreviousPageUrl"] = Request.UrlReferrer.ToString();

                    AuthButton.Visible = false;
                    PaymentType.Items.Clear();
                    foreach (string i in Database.GetFranchiseDropDown(Globals.GetFranchiseMask(), "paymentList"))
                    {
                        if (i == "Auth") AuthButton.Visible = true;
                        PaymentType.Items.Add(i);
                    }

                    int transID = Globals.SafeIntParse(Request["transID"]);
                    if (transID >= 5000)
                    {
                        TransactionStruct trans = Database.GetTransactionByID(Globals.GetFranchiseMask(), transID);

                        Session["trans_custID"] = trans.customerID.ToString();
                        Session["trans_type"] = trans.transType;
                        Session["trans_date"] = trans.dateApply.ToString("d");
                        Session["trans_payment"] = trans.paymentType;
                        Session["trans_email"] = trans.email;
                        Session["trans_hours"] = Globals.FormatHours(trans.hoursBilled);
                        Session["trans_rate"] = Globals.FormatMoney(trans.hourlyRate);
                        Session["trans_fee"] = Globals.FormatMoney(trans.serviceFee);
                        Session["trans_subConCC"] = Globals.FormatMoney(trans.subContractorCC);
                        Session["trans_subConWW"] = Globals.FormatMoney(trans.subContractorWW);
                        Session["trans_subConHW"] = Globals.FormatMoney(trans.subContractorHW);
                        Session["trans_subConCL"] = Globals.FormatMoney(trans.subContractorCL);
                        Session["trans_tips"] = Globals.FormatMoney(trans.tips);
                        Session["trans_salesTax"] = Globals.FormatPercent(trans.salesTax, false);
                        Session["trans_discountA"] = Globals.FormatMoney(trans.discountAmount);
                        Session["trans_discountP"] = Globals.FormatPercent(trans.discountPercent);
                        Session["trans_discountR"] = Globals.FormatPercent(trans.discountReferral);
                        Session["trans_total"] = Globals.FormatMoney(trans.total);
                        Session["trans_notes"] = trans.notes;

                        AuthButton.Visible = false;
                        SaveButton.Enabled = false;
                        SaveButton.Text = "Submitted";

                        if (trans.emailSent) SendEmailButton.ForeColor = Color.Green;
                        if (trans.isVoid)
                        {
                            VoidButton.Text = "Transaction Voided";
                            VoidButton.Enabled = false;
                        }
                        else
                        {
                            VoidButton.Visible = !string.IsNullOrEmpty(trans.paymentID) && !trans.isVoid;
                        }
                        if (trans.auth == 3) VoidButton.Enabled = false;
                    }
                    else
                    {
                        if (Session["trans_custID"] == null)
                        {
                            Session["trans_custID"] = Request["custID"];
                            Session["trans_type"] = Request["transType"];
                            Session["trans_date"] = Request["transDate"];
                            Session["trans_payment"] = null;
                            Session["trans_email"] = null;
                            Session["trans_hours"] = Globals.FormatHours(Globals.FormatHours(Request["hoursBilled"]));
                            Session["trans_rate"] = Globals.FormatMoney(Globals.FormatMoney(Request["hourlyRate"]));
                            if (Request["hourlyRate"] == "E") Session["trans_rate"] = "Error";
                            Session["trans_fee"] = Globals.FormatMoney(Globals.FormatMoney(Request["serviceFee"]));
                            Session["trans_subConCC"] = Globals.FormatMoney(Globals.FormatMoney(Request["subCC"]));
                            Session["trans_subConWW"] = Globals.FormatMoney(Globals.FormatMoney(Request["subWW"]));
                            Session["trans_subConHW"] = Globals.FormatMoney(Globals.FormatMoney(Request["subHW"]));
                            Session["trans_subConCL"] = Globals.FormatMoney(Globals.FormatMoney(Request["subCL"]));
                            Session["trans_tips"] = Globals.FormatMoney(Globals.FormatMoney(Request["tips"]));
                            Session["trans_salesTax"] = Globals.FormatPercent(Globals.FormatPercent(Request["salesTax"], false), false);
                            if (Request["salesTax"] == "E") Session["trans_salesTax"] = "Error";
                            Session["trans_discountA"] = Globals.FormatMoney(Globals.FormatMoney(Request["discountAmount"]));
                            Session["trans_discountP"] = Globals.FormatPercent(Globals.FormatPercent(Request["discountPercent"]));
                            if (Request["discountPercent"] == "E") Session["trans_discountP"] = "Error";
                            Session["trans_discountR"] = Globals.FormatPercent(Globals.FormatPercent(Request["discountReferral"]));
                            if (Request["discountReferral"] == "E") Session["trans_discountR"] = "Error";
                            Session["trans_total"] = Globals.FormatMoney(Globals.FormatMoney(Request["total"]));
                            Session["trans_notes"] = null;
                        }

                        VoidButton.Visible = false;
                        SendEmailButton.Visible = false;
                    }

                    if (Session["trans_custID"] != null)
                    {
                        CustomerStruct customer;
                        int customerID = Globals.SafeIntParse((string)Session["trans_custID"]);
                        string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                        if (error == null)
                        {
                            TitleLabel.Text = @"<a href=""Customers.aspx?custID=" + customerID + @""">" + customer.customerTitle + @"</a>";
                            CustomerServiceFee.InnerHtml = "<b>Customer Service Fee: </b>" + Globals.FormatMoney(customer.serviceFee) + @" <b>Points: </b>" + Globals.FormatPoints(customer.points, customer.ratePerHour) + " (" + Globals.FormatMoney(customer.points) + ")";
                            TransType.Text = (string)Session["trans_type"];
                            DateApply.Text = (string)Session["trans_date"];
                            string payment = Session["trans_payment"] != null ? (string)Session["trans_payment"] : customer.paymentType;
                            Globals.SetDropDownList(ref PaymentType, payment);
                            Email.Text = Session["trans_email"] != null ? (string)Session["trans_email"] : customer.email;
                            HoursBilled.Text = (string)Session["trans_hours"];
                            HourlyRate.Text = (string)Session["trans_rate"];
                            ServiceFee.Text = (string)Session["trans_fee"];
                            SubContractorCC.Text = (string)Session["trans_subConCC"];
                            SubContractorWW.Text = (string)Session["trans_subConWW"];
                            SubContractorHW.Text = (string)Session["trans_subConHW"];
                            Tips.Text = (string)Session["trans_tips"];
                            SalesTax.Text = (string)Session["trans_salesTax"];
                            DiscountAmount.Text = (string)Session["trans_discountA"];
                            DiscountPercent.Text = (string)Session["trans_discountP"];
                            DiscountReferral.Text = (string)Session["trans_discountR"];
                            TotalAmount.Text = (string)Session["trans_total"];
                            Notes.Text = (string)Session["trans_notes"];
                        }
                        else ErrorLabel.Text = "GetCustomerByID: " + error;
                    }
                    else ErrorLabel.Text = "Transaction Session Not Set";
                }
                if (Globals.FormatPercent(SalesTax.Text, true) == 0) SalesTax.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void AuthClick(object sender, EventArgs e)
        {
            if (!Globals.IsPaymentCreditCard(PaymentType.Text))
            {
                ErrorLabel.Text = "Payment type must be a credit card.";
            }
            else
            {
                PaymentType.Text = "Auth";
                SaveClick(sender, e);
            }
        }

        private TransactionStruct GetOpenAuthTransaction(int franchiseMask, int customerID, DateTime date)
        {
            foreach(var trans in Database.GetTransactions(franchiseMask, customerID, date, date, "T.dateCreated"))
            {
                if (trans.auth == 1 && !trans.isVoid && trans.dateCreated > (DateTime.Now - TimeSpan.FromDays(10)))
                {
                    return trans;
                }
            }
            return new TransactionStruct();
        }

        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
                string error = null;
                int custID = Globals.SafeIntParse(Request["custID"]);
                CustomerStruct customer;

                if (null == (error = Database.GetCustomerByID(Globals.GetFranchiseMask(), custID, out customer)))
                {
                    FranchiseStruct franchise = Globals.GetFranchiseByMask(customer.franchiseMask);

                    TransactionStruct trans = new TransactionStruct();
                    trans.transType = TransType.Text;
                    trans.paymentType = PaymentType.Text;
                    trans.customerID = custID;
                    trans.dateCreated = Globals.UtcToMst(DateTime.UtcNow);
                    trans.dateApply = Globals.SafeDateParse(DateApply.Text);
                    trans.total = Globals.FormatMoney(TotalAmount.Text);
                    trans.hoursBilled = Globals.FormatHours(HoursBilled.Text);
                    trans.hourlyRate = Globals.FormatMoney(HourlyRate.Text);
                    trans.serviceFee = Globals.FormatMoney(ServiceFee.Text);
                    trans.subContractorCC = Globals.FormatMoney(SubContractorCC.Text);
                    trans.subContractorWW = Globals.FormatMoney(SubContractorWW.Text);
                    trans.subContractorHW = Globals.FormatMoney(SubContractorHW.Text);
                    trans.discountAmount = Globals.FormatMoney(DiscountAmount.Text);
                    trans.discountPercent = Globals.FormatPercent(DiscountPercent.Text);
                    trans.discountReferral = Globals.FormatPercent(DiscountReferral.Text);
                    trans.tips = Globals.FormatMoney(Tips.Text);
                    trans.salesTax = Globals.FormatPercent(SalesTax.Text, false);
                    trans.email = Email.Text;
                    trans.notes = Notes.Text;
                    trans.username = Globals.GetUsername();

                    if (trans.paymentType == "Trade" || (trans.total > 0 && trans.total < 10000))
                    {
                        if (trans.transType == "Sale" || trans.transType == "Return")
                        {
                            if (Globals.IsPaymentCreditCard(trans.paymentType))
                            {
                                //Credit Card
                                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                                if (!string.IsNullOrEmpty(fran.ePNAccount) && !string.IsNullOrEmpty(fran.restrictKey))
                                {
                                    string invoce;
                                    string address = customer.billingSame || string.IsNullOrEmpty(customer.billingAddress) ? customer.locationAddress : customer.billingAddress;
                                    string zip = customer.billingSame || string.IsNullOrEmpty(customer.billingZip) ? customer.locationZip : customer.billingZip;
                                    trans.lastFourCard = Globals.FormatCardLastFour(customer.creditCardNumber);

                                    if (trans.transType == "Return")
                                    {
                                        error = CreditCard.Refund(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, trans.total, out invoce, out trans.paymentID);
                                    }
                                    else
                                    {
                                        TransactionStruct openAuth = GetOpenAuthTransaction(customer.franchiseMask, customer.customerID, trans.dateApply);
                                        if (trans.paymentType.ToLower() == "auth")
                                        {
                                            if (!string.IsNullOrEmpty(openAuth.paymentID))
                                            {
                                                error = "Existing open authorization.";
                                            }
                                            else
                                            {
                                                trans.auth = 1;
                                                error = CreditCard.Authorize(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, trans.total, out invoce, out trans.paymentID);
                                            }
                                        }
                                        else if (trans.paymentType.ToLower() == "capture" || !string.IsNullOrEmpty(openAuth.paymentID))
                                        {
                                            
                                            if (string.IsNullOrEmpty(openAuth.paymentID))
                                            {
                                                error = "No open authorizations.";
                                            }
                                            else
                                            {
                                                if (trans.total > openAuth.total)
                                                {
                                                    string voidID;
                                                    error = CreditCard.Void(fran.ePNAccount, fran.restrictKey, openAuth.paymentID, out voidID);
                                                    if (error != null)
                                                    {
                                                        error = "Amount greater than auth, cannot void existing auth. " + error;
                                                    }
                                                    else
                                                    {
                                                        error = Database.VoidTransaction(openAuth.transID);
                                                        if (error != null)
                                                        {
                                                            error = "Amount greater than auth, Error setting void on existing auth. " + error;
                                                        }
                                                        else
                                                        {
                                                            error = CreditCard.Charge(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, trans.total, out invoce, out trans.paymentID);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    trans.auth = 2;
                                                    error = CreditCard.Capture(fran.ePNAccount, fran.restrictKey, openAuth.paymentID, trans.total, out trans.paymentID);
                                                    if (error == null)
                                                    {
                                                        Database.CloseAuthTransaction(openAuth.transID);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            error = CreditCard.Charge(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, trans.total, out invoce, out trans.paymentID);
                                        }
                                    }
                                }
                                else ErrorLabel.Text = "E-Credit Card Error: Franchise ePNAccount is not setup.";

                                if (error != null)
                                {
                                    ErrorLabel.Text = "E-Credit Card Error: " + error;
                                    return;
                                }
                            }

                            if (trans.paymentType.ToLower() == "points")
                            {
                                //User Points
                                if (trans.transType == "Sale" && customer.points < trans.total)
                                {
                                    ErrorLabel.Text = "Insufficient Points";
                                    return;
                                }
                                error = Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, trans.transType == "Sale" ? -trans.total : trans.total);
                                if (error != null)
                                {
                                    ErrorLabel.Text = "Add Points Error: " + error;
                                    return;
                                }
                            }
                            else if (franchise.rewardsPercentage > 0 && customer.rewardsEnabled && !trans.paymentType.ToLower().Contains("gift") && !trans.IsAuth() && !trans.paymentType.ToLower().Contains("trade"))
                            {
                                //Earn Points
                                decimal total = trans.total - (trans.subContractorCC + trans.subContractorWW + trans.subContractorHW + trans.tips + trans.serviceFee);
                                decimal pointsEarned = (trans.transType == "Sale" ? total : -total) * (franchise.rewardsPercentage / 100);
                                Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, pointsEarned);
                            }
                        }

                        if (null == (error = Database.InsertTransaction(ref trans)))
                        {
                            if (trans.auth != 1) SendTransEmail(trans.transID);
                            CancelClick(sender, e);
                        }
                        else ErrorLabel.Text = "Error Inserting Transaction: " + error;
                    }
                    else ErrorLabel.Text = "Invalid Transaction Amount";
                }
                else ErrorLabel.Text = "GetCustomerByID: " + error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public void VoidClick(object sender, EventArgs e)
        {
            try
            {
                string error = null;
                int transID = Globals.SafeIntParse(Request["transID"]);
                if (transID >= 5000)
                {
                    TransactionStruct trans = Database.GetTransactionByID(Globals.GetFranchiseMask(), transID);
                    if (!trans.isVoid)
                    {
                        CustomerStruct customer;
                        if (null == (error = Database.GetCustomerByID(Globals.GetFranchiseMask(), trans.customerID, out customer)))
                        {
                            FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                            if (!string.IsNullOrEmpty(fran.ePNAccount) && !string.IsNullOrEmpty(fran.restrictKey))
                            {
                                string voidID;
                                if (null == (error = CreditCard.Void(fran.ePNAccount, fran.restrictKey, trans.paymentID, out voidID)))
                                {
                                    error = Database.VoidTransaction(transID);

                                    if (fran.rewardsPercentage > 0 && customer.rewardsEnabled)
                                    {
                                        decimal points = -(trans.total - (trans.serviceFee + trans.tips)) * (fran.rewardsPercentage / 100);
                                        Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, points);
                                    }

                                    if (error == null)
                                    {
                                        if (trans.auth != 1) SendTransEmail(transID);
                                        CancelClick(sender, e);
                                    }
                                    else ErrorLabel.Text = "Error Database.Void: " + error;
                                }
                                else ErrorLabel.Text = "Error ECreditCard.Void: " + error;
                            }
                            else ErrorLabel.Text = "Error Voiding: Invalid Franchise Credit Card Credentials";
                        }
                        else ErrorLabel.Text = "Error Voiding (GetCustomer): " + error;
                    }
                    else ErrorLabel.Text = "Error Voiding: Transaction Already Voided";
                }
                else ErrorLabel.Text = "Error Voiding: Invalid Transaction ID";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "VoidClick EX: " + ex.Message;
            }
        }

        public void PrintViewClick(object sender, EventArgs e)
        {
            try
            {
                string url = @"TransactionPrintout.aspx";
                if (!string.IsNullOrEmpty(Request["transID"]))
                {
                    url = Globals.BuildQueryString(url, "transID", Request["transID"]);
                }
                else
                {
                    Session["trans_type"] = TransType.Text;
                    Session["trans_date"] = DateApply.Text;
                    Session["trans_payment"] = PaymentType.Text;
                    Session["trans_email"] = Email.Text;
                    Session["trans_hours"] = HoursBilled.Text;
                    Session["trans_rate"] = HourlyRate.Text;
                    Session["trans_fee"] = ServiceFee.Text;
                    Session["trans_subConCC"] = SubContractorCC.Text;
                    Session["trans_subConWW"] = SubContractorWW.Text;
                    Session["trans_subConHW"] = SubContractorHW.Text;
                    Session["trans_tips"] = Tips.Text;
                    Session["trans_salesTax"] = SalesTax.Text;
                    Session["trans_discountA"] = DiscountAmount.Text;
                    Session["trans_discountP"] = DiscountPercent.Text;
                    Session["trans_discountR"] = DiscountReferral.Text;
                    Session["trans_total"] = TotalAmount.Text;
                    Session["trans_notes"] = Notes.Text;
                }
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PrintViewClick EX: " + ex.Message;
            }
        }

        public string SendTransEmail(int transID)
        {
            if (Globals.ValidEmail(Email.Text))
            {
                string error = SendEmail.SendTransaction(Email.Text, transID);
                if (error == null)
                {
                    if (null == (error = Database.TransactionEmailSent(transID)))
                    {
                        return null;
                    }
                    else return "Error Setting Database Email Confirm Bit: " + error;
                }
                else return "Error Sending Email: " + error;
            }
            else return "Invalid Email Address";
        }

        public void SendEmailClick(object sender, EventArgs e)
        {
            try
            {
                int transID = Globals.SafeIntParse(Request["transID"]);
                string error = SendTransEmail(transID);
                if (error == null) CancelClick(sender, e);
                else ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PrintViewClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["PreviousPageUrl"] != null)
                    Response.Redirect(ViewState["PreviousPageUrl"].ToString());
                else
                    Response.Redirect("Schedule.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }
    }
}