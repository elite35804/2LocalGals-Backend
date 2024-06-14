using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class TransactionPrintout : System.Web.UI.Page
    {
        int customerPortalID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Globals.ForceSSL(this);

            if (Globals.GetUserAccess(this) < 5)
            {
                customerPortalID = Globals.GetPortalCustomerID(this);
                if (customerPortalID <= 0) Globals.LogoutUser(this);
            }

            this.Page.Title = "2LG Transaction Printout";

            if (!IsPostBack)
            {
                LoadTransaction();
            }
        }

        private void LoadTransaction()
        {
            int franMask = Globals.GetFranchiseMask();
            TransactionStruct trans = new TransactionStruct();

            int transID = Globals.SafeIntParse(Request["transID"]);
            int giftCardID = Globals.SafeIntParse(Request["giftCardID"]);
            int cleaningPackID = Globals.SafeIntParse(Request["packID"]);
            if (customerPortalID > 0)
            {
                franMask = Globals.GetPortalFranchiseMask(this);
                trans = Database.GetTransactionByID(franMask, transID);
                if (trans.customerID != customerPortalID) Globals.LogoutUser(this);
                MainDiv.InnerHtml = TransDoc.GetTransactionDoc(franMask, trans).GetHTML();
            }
            else if (transID > 0)
            {
                trans = Database.GetTransactionByID(franMask, transID);
                MainDiv.InnerHtml = TransDoc.GetTransactionDoc(franMask, trans).GetHTML();
            }
            else if (giftCardID > 0)
            {
                GiftCardStruct giftCard = Database.GetGiftCardByID(franMask, giftCardID);
                MainDiv.InnerHtml = TransDoc.GetGiftCardDoc(franMask, giftCard).GetHTML();
            }
            else if (cleaningPackID > 0)
            {
                DBRow pack = Database.GetCleaningPackByID(cleaningPackID);
                MainDiv.InnerHtml = TransDoc.GetCleaningPackDoc(franMask, pack).GetHTML();
            }
            else
            {
                trans.customerID = Globals.SafeIntParse((string)Session["trans_custID"]);
                trans.transType = (string)Session["trans_type"];
                trans.dateCreated = DateTime.Now;
                trans.dateApply = Globals.DateTimeParse((string)Session["trans_date"]);
                trans.paymentType = (string)Session["trans_payment"];
                trans.hoursBilled = Globals.FormatHours((string)Session["trans_hours"]);
                trans.hourlyRate = Globals.FormatMoney((string)Session["trans_rate"]);
                trans.serviceFee = Globals.FormatMoney((string)Session["trans_fee"]);
                trans.subContractorCC = Globals.FormatMoney((string)Session["trans_subConCC"]);
                trans.subContractorWW = Globals.FormatMoney((string)Session["trans_subConWW"]);
                trans.subContractorHW = Globals.FormatMoney((string)Session["trans_subConHW"]);
                trans.subContractorCL = Globals.FormatMoney((string)Session["trans_subConCL"]);
                trans.tips = Globals.FormatMoney((string)Session["trans_tips"]);
                trans.salesTax = Globals.FormatPercent((string)Session["trans_salesTax"], false);
                trans.discountAmount = Globals.FormatMoney((string)Session["trans_discountA"]);
                trans.discountPercent = Globals.FormatPercent((string)Session["trans_discountP"]);
                trans.total = Globals.FormatMoney((string)Session["trans_total"]);
                trans.notes = (string)Session["trans_notes"];
                MainDiv.InnerHtml = TransDoc.GetTransactionDoc(franMask, trans).GetHTML();
            }
        }
    }
}