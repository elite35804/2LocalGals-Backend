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
    public partial class CleaningPacks : System.Web.UI.Page
    {
        private DBRow pack = null;
        private CustomerStruct customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);
                PurchaseButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(PurchaseButton, null) + ";");

                int cleaningPackID = Globals.SafeIntParse(Request["packID"]);
                if (cleaningPackID > 0) pack = Database.GetCleaningPackByID(cleaningPackID);
                int customerID = pack == null ? Globals.SafeIntParse(Request["custID"]) : pack.GetInt("customerID");
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
                if (error != null)
                {
                    ErrorLabel.Text = "Error Loading Customer";
                    return;
                }

                if (!this.IsPostBack)
                {
                    PaymentType.Items.Clear();
                    foreach (string i in Database.GetFranchiseDropDown(Globals.GetFranchiseMask(), "paymentList"))
                    {
                        if (!i.ToLower().Contains("points") && !i.ToLower().Contains("trade") && !i.ToLower().Contains("gift") && !i.ToLower().Contains("invoice"))
                            PaymentType.Items.Add(i);
                    }

                    Globals.SetPreviousPage(this, null);
                    LoadCleaningPack();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        protected void PrintClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TransactionPrintout.aspx?packID=" + pack.GetInt("cleaningPackID"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error PrintClick EX: " + ex.Message;
            }
        }

        protected void EmailClick(object sender, EventArgs e)
        {
            try
            {
                EmailButton.ForeColor = Color.Red;
                pack.SetValue("email", Email.Text);
                string error = SendEmail.SendTransDoc(TransDoc.GetCleaningPackDoc(customer.franchiseMask, pack));
                if (error != null)
                {
                    ErrorLabel.Text = "Error Sending Email: " + error;
                }
                else
                {
                    EmailButton.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error EmailClick EX: " + ex.Message;
            }
        }

        protected void VoidClick(object sender, EventArgs e)
        {
            try
            {
                if (customer.customerID > 0 && pack != null)
                {
                    if (!string.IsNullOrEmpty(Email.Text) && !Globals.ValidEmail(Email.Text))
                    {
                        ErrorLabel.Text = "Invalid Email";
                        return;
                    }

                    FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                    int cleaningPackID = pack.GetInt("cleaningPackID");
                    string paymentID = pack.GetString("paymentID");
                    string voidID;

                    if (cleaningPackID <= 0)
                    {
                        ErrorLabel.Text = "Invalid Cleaning Pack ID.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(fran.ePNAccount) || string.IsNullOrEmpty(fran.restrictKey))
                        {
                            ErrorLabel.Text = "E-Credit Card Error: Franchise ePNAccount is not setup.";
                        }
                        else
                        {
                            string error = CreditCard.Void(fran.ePNAccount, fran.restrictKey, paymentID, out voidID);
                            if (error != null)
                            {
                                ErrorLabel.Text = "E-Credit Card Error: " + error;
                            }
                            else
                            {
                                error = Database.AddCustomerPoints(Globals.GetFranchiseMask(), customer.customerID, -pack.GetDecimal("points"));
                                if (error != null)
                                {
                                    ErrorLabel.Text = "Error Removing Points: " + error;
                                }
                                else
                                {
                                    DBRow voidRow = new DBRow();
                                    voidRow.SetValue("isVoid", true);

                                    error = Database.DynamicSetWithKeyInt("CleaningPacks", "cleaningPackID", ref cleaningPackID, voidRow);
                                    if (error != null)
                                    {
                                        ErrorLabel.Text = "Error Setting Cleaning Pack: " + error;
                                    }
                                    else
                                    {
                                        //SEND EMAIL
                                        pack.SetValue("isVoid", true);
                                        pack.SetValue("email", Email.Text);
                                        SendEmail.SendTransDoc(TransDoc.GetCleaningPackDoc(customer.franchiseMask, pack));
                                        Response.Redirect("CleaningPacks.aspx?packID=" + cleaningPackID);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error VoidClick EX: " + ex.Message;
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "Schedule.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }


        protected void PurchaseClick(object sender, EventArgs e)
        {
            if (customer.customerID > 0)
            {
                string paymentID = null;
                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                try
                {
                    string error = null;

                    int visits = Globals.SafeIntParse(CleaningPack.SelectedValue);
                    decimal hoursPerVisit = Globals.FormatHours(HoursPerVisit.Text);
                    decimal serviceFeePerVisit = Globals.FormatMoney(ServiceFee.Text);
                    decimal ratePerHour = Globals.FormatMoney(HourlyRate.Text);
                    decimal points = Globals.FormatPoints(Points.Text);
                    decimal amount = Globals.FormatMoney(Total.Text);

                    if (string.IsNullOrEmpty(TransType.Text))
                    {
                        ErrorLabel.Text = "You Must First Select a Transaction Type";
                        return;
                    }

                    if (points <= 0)
                    {
                        ErrorLabel.Text = "Invalid Field Points";
                        return;
                    }

                    if (amount <= 0 || amount > 20000)
                    {
                        ErrorLabel.Text = "Invalid Field Total";
                        return;
                    }

                    if (!string.IsNullOrEmpty(Email.Text) && !Globals.ValidEmail(Email.Text))
                    {
                        ErrorLabel.Text = "Invalid Email";
                        return;
                    }

                    pack = new DBRow();
                    pack.SetValue("customerID", customer.customerID);
                    pack.SetValue("transType", TransType.Text);
                    pack.SetValue("paymentType", PaymentType.Text);
                    pack.SetValue("visits", visits);
                    pack.SetValue("hoursPerVisit", hoursPerVisit);
                    pack.SetValue("serviceFeePerVisit", serviceFeePerVisit);
                    pack.SetValue("ratePerHour", ratePerHour);
                    pack.SetValue("points", points);
                    pack.SetValue("amount", amount);
                    pack.SetValue("email", Email.Text);
                    pack.SetValue("memo", Memo.Text);
                    pack.SetValue("username", Globals.GetUsername());

                    if (Globals.IsPaymentCreditCard(PaymentType.Text))
                    {
                        //Credit Card
                        if (string.IsNullOrEmpty(fran.ePNAccount) || string.IsNullOrEmpty(fran.restrictKey))
                        {
                            ErrorLabel.Text = "E-Credit Card Error: Franchise ePNAccount is not setup.";
                            return;
                        }
                        else
                        {
                            string invoce;
                            string address = customer.billingSame || string.IsNullOrEmpty(customer.billingAddress) ? customer.locationAddress : customer.billingAddress;
                            string zip = customer.billingSame || string.IsNullOrEmpty(customer.billingZip) ? customer.locationZip : customer.billingZip;
                            if (TransType.Text == "Return")
                            {
                                if (PayTrace.IsPayTraceAccount(fran.ePNAccount)) 
                                    error = CreditCard.Refund(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, amount, out invoce, out paymentID);
                            }
                            else
                            {
                                error = CreditCard.Charge(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, amount, out invoce, out paymentID);
                            }

                            if (error != null)
                            {
                                ErrorLabel.Text = "E-Credit Card Error: " + error;
                                return;
                            }

                            pack.SetValue("lastFourCard", Globals.FormatCardLastFour(customer.creditCardNumber));
                            pack.SetValue("paymentID", paymentID);
                        }
                    }

                    error = Database.AddCustomerPoints(Globals.GetFranchiseMask(), customer.customerID, TransType.Text == "Return" ? -points : points);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Adding Customer Points: " + error;
                    }
                    else
                    {
                        int cleaningPackID = 0;
                        error = Database.DynamicSetWithKeyInt("CleaningPacks", "cleaningPackID", ref cleaningPackID, pack);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Saving Transaction: " + error;
                        }
                        else
                        {
                            paymentID = null;
                            pack.SetValue("cleaningPackID", cleaningPackID, true);
                            pack.SetValue("dateCreated", DateTime.UtcNow);
                            SendEmail.SendTransDoc(TransDoc.GetCleaningPackDoc(customer.franchiseMask, pack));
                            Response.Redirect("CleaningPacks.aspx?packID=" + cleaningPackID);
                            return;
                        }
                        Database.AddCustomerPoints(Globals.GetFranchiseMask(), customer.customerID, TransType.Text == "Return" ? points : -points);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = "Error PurchaseClick EX: " + ex.Message;
                }
                finally
                {
                    if (paymentID != null)
                    {
                        string voidID;
                        CreditCard.Void(fran.ePNAccount, fran.restrictKey, paymentID, out voidID);
                    }
                }
            }
        }

        private void LoadCleaningPack()
        {
            try
            {
                if (customer.customerID > 0)
                {
                    if (pack == null)
                    {
                        TitleLabel.InnerHtml = @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a> - Purchase Cleaning Pack";
                        CleaningPackSubTitle.Visible = false;
                        PurchaseButton.Enabled = true;
                        Globals.SetDropDownList(ref PaymentType, customer.paymentType);
                        Email.Text = customer.email;
                        TransType.Text = "Sale";

                        HourlyRate.Text = Globals.FormatMoney(customer.ratePerHour);
                        ServiceFee.Text = Globals.FormatMoney(customer.serviceFee);
                        decimal hoursPerVisit = Database.GetCustomerAverageHoursPerDay(customer.customerID);
                        if (hoursPerVisit <= 0) hoursPerVisit = 2;
                        HoursPerVisit.Text = Globals.FormatHours(hoursPerVisit);
                        CleaningPack.SelectedIndex = 0;
                    }
                    else
                    {
                        bool isVoid = pack.GetBool("isVoid");
                        string paymentType = pack.GetString("paymentType");
                        TitleLabel.InnerHtml = @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a> - Cleaning Pack " + pack.GetString("transType") + " Receipt";
                        CleaningPackSubTitle.InnerHtml = "<b>Invoice: </b>" + pack.GetInt("cleaningPackID") + " <b>Date: </b>" + Globals.UtcToMst(pack.GetDate("dateCreated")).ToString("d") + @", <b>Payment Type: </b>" + paymentType;
                        if (pack.GetString("lastFourCard") != null) CleaningPackSubTitle.InnerHtml += " (xxxx " + pack.GetString("lastFourCard") + ")";
                        if (isVoid) CleaningPackSubTitle.InnerHtml += " <b>(VOID)</b>";
                        VoidButton.Enabled = !isVoid && Globals.IsPaymentCreditCard(paymentType);
                        PurchaseButton.Enabled = true;
                        EmailButton.Enabled = true;
                        PrintButton.Enabled = true;
                        Globals.SetDropDownList(ref PaymentType, paymentType);
                        Email.Text = pack.GetString("email");
                        Memo.Text = pack.GetString("memo");

                        int visits = pack.GetInt("visits");
                        decimal hoursPerVisit = pack.GetDecimal("hoursPerVisit");
                        decimal serviceFeePerVisit = pack.GetDecimal("serviceFeePerVisit");
                        decimal ratePerHour = pack.GetDecimal("ratePerHour");
                        decimal amount = pack.GetDecimal("amount");
                        decimal points = pack.GetDecimal("points");

                        CleaningPack.SelectedValue = visits.ToString();
                        HoursPerVisit.Text = Globals.FormatHours(hoursPerVisit);
                        ServiceFee.Text = Globals.FormatMoney(serviceFeePerVisit);
                        HourlyRate.Text = Globals.FormatMoney(ratePerHour);
                        Total.Text = Globals.FormatMoney(amount);
                        Points.Text = Globals.FormatPoints(points);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error LoadCleaningPack EX: " + ex.Message;
            }
        }
    }
}