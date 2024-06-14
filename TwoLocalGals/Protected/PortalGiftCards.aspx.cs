using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalGiftCards : System.Web.UI.Page
    {
        private int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Globals.ForceSSL(this);
            customerID = Globals.GetPortalCustomerID(this);
            if (customerID <= 0) Globals.LogoutUser(this);
            ((CustomerPortal)this.Page.Master).SetActiveMenuItem(4);

            PurchaseButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(PurchaseButton, null) + ";");

            if (UsePoints.Checked)
            {
                CreditCardRowZero.Style["display"] = "none";
                CreditCardRowOne.Style["display"] = "none";
                CreditCardRowTwo.Style["display"] = "none";
                CreditCardRowThree.Style["display"] = "none";
                CreditCardRowFour.Style["display"] = "none";
            }

            if (!IsPostBack)
            {
                Globals.SetPreviousPage(this, null);

                CustomerStruct customer;
                string error = Database.GetCustomerByID(-1, customerID, out customer);
                if (error != null)
                {
                    ErrorLabel.Text = "Error Get Customer: " + error;
                }
                else
                {
                    GiverName.Text = Globals.FormatFullName(customer.firstName, customer.lastName, "");
                    BillingEmail.Text = customer.email;
                }
            }
        }

        protected void RedeemClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.Text = "";
                SuccessLabel.Text = "";

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
                                giftCard.redeemed = customerID;

                                DBRow row = new DBRow();
                                row.SetValue("redeemed", customerID);
                                string error = Database.DynamicSetWithKeyInt("GiftCards", "giftCardID", ref giftCard.giftCardID, row);
                                if (error != null)
                                {
                                    ErrorLabel.Text = "Error Redeeming E-Gift Card: " + error;
                                }
                                else
                                {
                                    error = Database.AddCustomerPoints(-1, customerID, giftCard.amount);
                                    if (error != null)
                                    {
                                        ErrorLabel.Text = "Error converting E-Gift Card Point: " + error;
                                    }
                                    else
                                    {
                                        SuccessLabel.Text = "E-Gift Card successfully redeemed. We have added " + Globals.FormatMoney(giftCard.amount) + " worth of points to your account.";
                                        ((CustomerPortal)this.Page.Master).RefreshPoints();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }

        protected void PurchaseClick(object sender, EventArgs e)
        {
            FranchiseStruct franchise = new FranchiseStruct();
            GiftCardStruct giftCard = new GiftCardStruct();
            try
            {
                ErrorLabel.Text = "";
                SuccessLabel.Text = "";

                if (string.IsNullOrEmpty(GiverName.Text))
                {
                    ErrorLabel.Text = "'Your Name' cannot be empty";
                    return;
                }

                if (string.IsNullOrEmpty(GiverName.Text))
                {
                    ErrorLabel.Text = "'Recipient Name' cannot be empty";
                    return;
                }

                if (!Globals.ValidEmail(RecipientEmail.Text))
                {
                    ErrorLabel.Text = "Valid Recipient Email Required";
                    return;
                }

                decimal amount = Globals.FormatMoney(Amount.Text);
                if (amount <= 0 || amount > 1000)
                {
                    ErrorLabel.Text = "Invalid Gift Card Amount";
                    return;
                }

                CustomerStruct customer;
                string error = Database.GetCustomerByID(-1, Globals.GetPortalCustomerID(this), out customer);
                if (error != null)
                {
                    ErrorLabel.Text = "Error Get Customer: " + error;
                }
                else
                {
                    franchise = Globals.GetFranchiseByMask(customer.franchiseMask);

                    
                    giftCard.customerID = customer.customerID;
                    giftCard.amount = amount;
                    giftCard.giverName = GiverName.Text;
                    giftCard.recipientName = RecipientName.Text;
                    giftCard.recipientEmail = RecipientEmail.Text;
                    giftCard.billingEmail = BillingEmail.Text;
                    giftCard.message = Message.Text;
                    giftCard.dateCreated = DateTime.UtcNow;

                    for (int i = 0; i < 10; i++)
                    {
                        giftCard.giftCardID = Globals.Get32BitGUID();
                        if (giftCard.giftCardID != 0 && Database.GetGiftCardByID(-1, giftCard.giftCardID).giftCardID == 0) break;
                    }

                    if (UsePoints.Checked)
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

                        giftCard.paymentType = "Points";
                    }
                    else
                    {
                        string creditCardNumber = Globals.OnlyNumbers(CardNumber.Text);
                        if (creditCardNumber.Length != 15 && creditCardNumber.Length != 16)
                        {
                            ErrorLabel.Text = "Invalid Credit Card Number";
                            return;
                        }

                        if (!Globals.ValidEmail(BillingEmail.Text))
                        {
                            ErrorLabel.Text = "Valid Billing Email Required";
                            return;
                        }

                        if (string.IsNullOrEmpty(Address.Text))
                        {
                            ErrorLabel.Text = "'Street Address' cannot be empty";
                            return;
                        }

                        if (string.IsNullOrEmpty(ZipCode.Text))
                        {
                            ErrorLabel.Text = "'Zip Code' cannot be empty";
                            return;
                        }

                        string invoice;
                        error = CreditCard.Charge(franchise.ePNAccount, franchise.restrictKey, creditCardNumber, ExpirationMonth.Text, ExpirationYear.Text, Address.Text, ZipCode.Text, CCVCode.Text, amount, out invoice, out giftCard.paymentID);
                        if (error != null)
                        {
                            ErrorLabel.Text = "E-Credit Card Error: " + error;
                            return;
                        }

                        giftCard.paymentType = "Credit Card";
                        giftCard.lastFourCard = Globals.FormatCardLastFour(creditCardNumber);
                    }

                    error = Database.InsertGiftCard(giftCard);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Processing Gift Card: " + error;
                    }
                    else
                    {
                        giftCard.paymentID = null;

                        if (franchise.rewardsPercentage > 0 && customer.rewardsEnabled && giftCard.paymentType != "Points")
                        {
                            //Earn Points
                            decimal pointsEarned = giftCard.amount * (franchise.rewardsPercentage / 100);
                            Database.AddCustomerPoints(customer.franchiseMask, customer.customerID, pointsEarned);
                        }

                        error = SendEmail.SendTransDoc(TransDoc.GetGiftCardDoc(customer.franchiseMask, giftCard));
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Sending Gift Card Receipt Email: " + error;
                        }

                        error = SendEmail.SendGiftCard(customer.franchiseMask, giftCard.giftCardID);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Sending Gift Card Email: " + error;
                        }

                        if (string.IsNullOrEmpty(ErrorLabel.Text))
                        {
                            SuccessLabel.Text = "Purchase successful! A receipt has been emailed to you and a E-Gift Card Email has been sent to the recipient.";
                            PurchaseButton.Enabled = false;
                            CancelButton.Enabled = false;
                            ((CustomerPortal)this.Page.Master).RefreshPoints();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Purchase EX: " + ex.Message;
            }
            finally
            {
                if (giftCard.paymentID != null)
                {
                    string voidID;
                    CreditCard.Void(franchise.ePNAccount, franchise.restrictKey, giftCard.paymentID, out voidID);
                }
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "PortalGiftCards.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }
    }
}