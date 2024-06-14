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
    public partial class GiftCards : System.Web.UI.Page
    {
        private GiftCardStruct giftCard;
        private CustomerStruct customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);
                PurchaseButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(PurchaseButton, null) + ";");

                int giftCardID = Globals.SafeIntParse(Request["giftCardID"]);
                if (giftCardID > 0) giftCard = Database.GetGiftCardByID(Globals.GetFranchiseMask(), giftCardID);
                int customerID = giftCard.customerID == 0 ? Globals.SafeIntParse(Request["custID"]) : giftCard.customerID;
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
                        if (!i.ToLower().Contains("trade") && !i.ToLower().Contains("gift") && !i.ToLower().Contains("invoice"))
                            PaymentType.Items.Add(i);
                    }

                    Globals.SetPreviousPage(this, null);
                    LoadGiftCard();
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
                Response.Redirect("TransactionPrintout.aspx?giftCardID=" + giftCard.giftCardID);
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
                giftCard.billingEmail = CustomerEmail.Text;
                string error = SendEmail.SendTransDoc(TransDoc.GetGiftCardDoc(customer.franchiseMask, giftCard));
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
                if (customer.customerID > 0 && giftCard.giftCardID > 0)
                {
                    if (!string.IsNullOrEmpty(CustomerEmail.Text) && !Globals.ValidEmail(CustomerEmail.Text))
                    {
                        ErrorLabel.Text = "Invalid Customer Email";
                        return;
                    }

                    if (giftCard.redeemed > 0)
                    {
                        ErrorLabel.Text = "Gift Card Already Redeemed";
                        return;
                    }

                    FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                    string voidID;

                    if (string.IsNullOrEmpty(fran.ePNAccount) || string.IsNullOrEmpty(fran.restrictKey))
                    {
                        ErrorLabel.Text = "E-Credit Card Error: Franchise ePNAccount is not setup.";
                    }
                    else
                    {
                        string error = CreditCard.Void(fran.ePNAccount, fran.restrictKey, giftCard.paymentID, out voidID);
                        if (error != null)
                        {
                            ErrorLabel.Text = "E-Credit Card Error: " + error;
                        }
                        else
                        {
                            DBRow row = new DBRow();
                            row.SetValue("isVoid", true);
                            error = Database.DynamicSetWithKeyInt("GiftCards", "giftCardID", ref giftCard.giftCardID, row);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Error Voiding Gift Card: " + error;
                            }
                            else
                            {
                                //SEND EMAIL
                                giftCard.isVoid = true;
                                giftCard.billingEmail = CustomerEmail.Text;
                                SendEmail.SendTransDoc(TransDoc.GetGiftCardDoc(customer.franchiseMask, giftCard));
                                Response.Redirect("GiftCards.aspx?giftCardID=" + giftCard.giftCardID);
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

                    decimal amount = Globals.FormatMoney(Amount.Text);

                    if (amount <= 0 || amount > 1000)
                    {
                        ErrorLabel.Text = "Invalid Gift Card Amount";
                        return;
                    }

                    if (!string.IsNullOrEmpty(CustomerEmail.Text) && !Globals.ValidEmail(CustomerEmail.Text))
                    {
                        ErrorLabel.Text = "Invalid Customer Email";
                        return;
                    }

                    if (!string.IsNullOrEmpty(RecipientEmail.Text) && !Globals.ValidEmail(RecipientEmail.Text))
                    {
                        ErrorLabel.Text = "Invalid Recipient Email";
                        return;
                    }

                    giftCard = new GiftCardStruct();
                    giftCard.customerID = customer.customerID;
                    giftCard.paymentType = PaymentType.Text;
                    giftCard.giverName = CustomerName.Text;
                    giftCard.billingEmail = CustomerEmail.Text;
                    giftCard.recipientName = RecipientName.Text;
                    giftCard.recipientEmail = RecipientEmail.Text;
                    giftCard.amount = amount;
                    giftCard.message = Message.Text;
                    giftCard.username = Globals.GetUsername();
                    giftCard.dateCreated = DateTime.UtcNow;

                    for (int i = 0; i < 10; i++)
                    {
                        giftCard.giftCardID = Globals.Get32BitGUID();
                        if (giftCard.giftCardID != 0 && Database.GetGiftCardByID(-1, giftCard.giftCardID).giftCardID == 0) break;
                    }

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
                            
                            error = CreditCard.Charge(fran.ePNAccount, fran.restrictKey, customer.creditCardNumber, customer.creditCardExpMonth, customer.creditCardExpYear, address, zip, customer.creditCardCCV, amount, out invoce, out paymentID);
                            if (error != null)
                            {
                                ErrorLabel.Text = "E-Credit Card Error: " + error;
                                return;
                            }

                            giftCard.lastFourCard = Globals.FormatCardLastFour(customer.creditCardNumber);
                            giftCard.paymentID = paymentID;
                        }
                    }

                    if (PaymentType.Text.ToLower() == "points")
                    {
                        //Use Points
                        if (customer.points < giftCard.amount)
                        {
                            ErrorLabel.Text = "Insufficient Points";
                            return;
                        }
                        error = Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, -giftCard.amount);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Using Points: " + error;
                            return;
                        }
                    }

                    error = Database.InsertGiftCard(giftCard);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Saving Transaction: " + error;
                    }
                    else
                    {
                        if (fran.rewardsPercentage > 0 && customer.rewardsEnabled && giftCard.paymentType != "Points")
                        {
                            //Earn Points
                            decimal pointsEarned = giftCard.amount * (fran.rewardsPercentage / 100);
                            Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, pointsEarned);
                        }

                        paymentID = null;
                        SendEmail.SendTransDoc(TransDoc.GetGiftCardDoc(customer.franchiseMask, giftCard));
                        SendEmail.SendGiftCard(customer.franchiseMask, giftCard.giftCardID);
                        Response.Redirect("GiftCards.aspx?giftCardID=" + giftCard.giftCardID);
                        return;
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

        private void LoadGiftCard()
        {
            try
            {
                if (customer.customerID > 0)
                {
                    if (giftCard.giftCardID == 0)
                    {
                        TitleLabel.InnerHtml = @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a> - Purchase Gift Card";
                        GiftCardSubTitle.Visible = false;
                        PurchaseButton.Enabled = true;
                        Globals.SetDropDownList(ref PaymentType, customer.paymentType);
                        CustomerName.Text = Globals.FormatFullName(customer.firstName, customer.lastName, "");
                        CustomerEmail.Text = customer.email;
                        RecipientName.Text = Globals.FormatFullName(customer.firstName, customer.lastName, "");
                        RecipientEmail.Text = customer.email;
                        Amount.Text = Globals.FormatMoney(50);
                    }
                    else
                    {
                        TitleLabel.InnerHtml = @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a> - Gift Card Sale Receipt";
                        GiftCardSubTitle.InnerHtml = "<b>Invoice: </b>" + giftCard.giftCardID + " <b>Code: </b>" + Globals.EncodeZBase32((uint)giftCard.giftCardID) + " <b>Date: </b>" + Globals.UtcToMst(giftCard.dateCreated).ToString("d") + @", <b>Payment Type: </b>" + giftCard.paymentType;
                        if (!string.IsNullOrEmpty(giftCard.lastFourCard)) GiftCardSubTitle.InnerHtml += " (xxxx " + giftCard.lastFourCard + ")";
                        if (giftCard.isVoid) GiftCardSubTitle.InnerHtml += " <b>(VOID)</b>";

                        GiftCardSubTitle.InnerHtml += @"<br>" + (giftCard.redeemed > 0 ? @"<b>Redeemed By: </b><a href=""Customers.aspx?custID=" + customer.customerID + @""">" + giftCard.redeemedTitle + @"</a>" : "<b>Not Redeemed</b>");
                        VoidButton.Enabled = !giftCard.isVoid && Globals.IsPaymentCreditCard(giftCard.paymentType);
                        EmailButton.Enabled = true;
                        PrintButton.Enabled = true;
                        Globals.SetDropDownList(ref PaymentType, giftCard.paymentType);

                        CustomerName.Text = giftCard.giverName;
                        CustomerEmail.Text = giftCard.billingEmail;
                        RecipientName.Text = giftCard.recipientName;
                        RecipientEmail.Text = giftCard.recipientEmail;

                        Amount.Text = Globals.FormatMoney(giftCard.amount);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error LoadGiftCard EX: " + ex.Message;
            }
        }
    }
}