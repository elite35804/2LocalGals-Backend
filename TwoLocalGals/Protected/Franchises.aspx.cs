using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace Nexus.Protected
{
    public partial class Franchises : System.Web.UI.Page
    {
        private FranchiseStruct franchise;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int userAccess = Globals.GetUserAccess(this);
                if (userAccess < 7)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Franchises";

                if (!IsPostBack)
                {
                    FranchiseStruct franchise;
                    FranchiseList.Items.Clear();
                    FranchiseList.Items.Add(new ListItem(userAccess == 9 ? "(New Franchise)" : "(Select Franchise)", "0"));
                    FranchiseList.Items.AddRange(Globals.GetFranchiseList((userAccess == 9 ? -1 : Globals.GetFranchiseMask()), Globals.SafeIntParse(Request["franID"]), out franchise));
                    FranchiseName.Text = franchise.franchiseName;
                    EM.Text = franchise.email;
                    EP.Text = franchise.emailPassword;
                    EmailSmtp.Text = franchise.emailSmtp;
                    EmailPort.Text = franchise.emailPort.ToString();
                    EmailSecure.Checked = franchise.emailSecure;
                    Phone.Text = Globals.FormatPhone(franchise.phone);
                    Address.Text = franchise.address;
                    City.Text = franchise.city;
                    State.Text = franchise.state;
                    Zip.Text = franchise.zip;
                    WebLink.Text = franchise.webLink;
                    DefaultRatePerHour.Text = Globals.FormatMoney(franchise.defaultRatePerHour);
                    DefaultServiceFee.Text = Globals.FormatMoney(franchise.defaultServiceFee);
                    DefaultScheduleFee.Text = franchise.scheduleFeeString;
                    RewardsPercentage.Text = Globals.FormatPercent(franchise.rewardsPercentage);
                    SuppliesPercentage.Text = Globals.FormatPercent(franchise.suppliesPercentage);
                    SalesTax.Text = Globals.FormatPercent(franchise.salesTax, false);
                    CarPercentage.Text = Globals.FormatPercent(franchise.carPercentage);
                    NotesEnabled.Checked = franchise.notesEnabled;
                    SendSchedules.Text = franchise.sendSchedules.ToString("t");
                    NewButton.Visible = (userAccess == 9);
                    EPNAccount.Text = franchise.ePNAccount;
                    RestrictKey.Text = franchise.restrictKey;
                    BatchTime.Text = franchise.batchTime.ToString("t");
                    SMSUsername.Text = franchise.smsUsername;
                    SMSPassword.Text = franchise.smsPassword;
                    ReviewUsLink.Text = franchise.reviewUsLink;
                    if (!string.IsNullOrEmpty(franchise.FranchiseImg))
                    {
                        DefaultPic.Visible = true;
                        DefaultPic.ImageUrl = "~/ContratorPics/" + franchise.FranchiseImg;
                    }
                    AdvertisementList.Text = franchise.advertisementList;
                    if (string.IsNullOrEmpty(AdvertisementList.Text))
                        AdvertisementList.Text = @"Google Search|Google Maps|Google Top/Side|Angies List|Word Of Mouth|Yellow Pages|Unknown|Other";
                    AdjustmentList.Text = franchise.adjustmentList;
                    if (string.IsNullOrEmpty(AdjustmentList.Text))
                        AdjustmentList.Text = @"None|Aflac|Supplies|Advance|Breakage|Redo|Lawyer|Child Support|Theft|No Show|Other";
                    PaymentList.Text = franchise.paymentList;
                    if (string.IsNullOrEmpty(PaymentList.Text))
                        PaymentList.Text = @"Credit Card|Visa|Master Card|Discover|Check|Check (In Mail)|Cash|Gift Certificate|Trade|Invoice (print)|Need CC Info";
                    PartnerCategoryList.Text = franchise.partnerCategoryList;
                    if (string.IsNullOrEmpty(PartnerCategoryList.Text))
                        PartnerCategoryList.Text = @"Carpet Cleaning|Window Cleaning|Accountants|Electricians|Moving Companies|Pet Care|Emergency Restoration|Chiropractic|Realtors|Plumbing|Printing|Other";
                    

                    /*string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/ReferralList/ReferralList_" + franchise.franchiseID + ".pdf";
                    if (File.Exists(uploadPath))
                    {
                        FileInfo fi = new FileInfo(uploadPath);
                        UploadReferralLabel.Text = "Last Update (" + fi.CreationTime.ToString("d") + ") ";
                    }*/
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void FranchiseChanged(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Franchises.aspx?franID=" + Globals.SafeIntParse(FranchiseList.SelectedValue));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "FranchiseChanged EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Franchises.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NewClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = Database.DeleteFranchise(Globals.SafeIntParse(Request["franID"]));
                if (error == null)
                    Response.Redirect("Franchises.aspx");
                else
                    ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Franchises.aspx?franID=" + Globals.SafeIntParse(Request["franID"]));
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
                if (string.IsNullOrEmpty(FranchiseName.Text))
                    ErrorLabel.Text = "Franchise Name Cannot be Blank";
                else if (SaveChanges())
                {
                    Response.Redirect("Franchises.aspx?franID=" + franchise.franchiseID);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                int franchiseID = Globals.SafeIntParse(Request["franID"]);
                franchise = new FranchiseStruct();
                franchise.franchiseName = FranchiseName.Text;
                franchise.email = EM.Text;
                if (!string.IsNullOrEmpty(EP.Text))
                    franchise.emailPassword = EP.Text;
                franchise.emailSmtp = EmailSmtp.Text;
                franchise.emailPort = Globals.SafeIntParse(EmailPort.Text);
                franchise.emailSecure = EmailSecure.Checked;
                franchise.phone = Globals.FormatPhone(Phone.Text);
                franchise.address = Address.Text;
                franchise.city = City.Text;
                franchise.state = State.Text;
                franchise.zip = Zip.Text;
                franchise.webLink = WebLink.Text;
                franchise.defaultRatePerHour = Globals.FormatMoney(DefaultRatePerHour.Text);
                franchise.defaultServiceFee = Globals.FormatMoney(DefaultServiceFee.Text);
                franchise.scheduleFeeString = DefaultScheduleFee.Text;
                franchise.rewardsPercentage = Globals.FormatPercent(RewardsPercentage.Text);
                franchise.suppliesPercentage = Globals.FormatPercent(SuppliesPercentage.Text);
                franchise.salesTax = Globals.FormatPercent(SalesTax.Text, false);
                franchise.carPercentage = Globals.FormatPercent(CarPercentage.Text);
                franchise.notesEnabled = NotesEnabled.Checked;
                franchise.sendSchedules = Globals.TimeOnly(Globals.DateTimeParse(SendSchedules.Text));
                franchise.ePNAccount = EPNAccount.Text;
                franchise.restrictKey = RestrictKey.Text;
                franchise.batchTime = Globals.TimeOnly(Globals.DateTimeParse(BatchTime.Text));
                franchise.advertisementList = AdvertisementList.Text;
                franchise.adjustmentList = AdjustmentList.Text;
                franchise.paymentList = PaymentList.Text;
                franchise.partnerCategoryList = PartnerCategoryList.Text;
                franchise.smsUsername = SMSUsername.Text;
                franchise.smsPassword = SMSPassword.Text;
                franchise.reviewUsLink = ReviewUsLink.Text;

                if (FranchiseImg.HasFile)
                {
                    try
                    {
                        string extension = Path.GetExtension(FranchiseImg.FileName);
                        string fileName = Guid.NewGuid().ToString().Split('-').Last() + extension;
                        string folderPath = Server.MapPath("~/ContratorPics/");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        FranchiseImg.Width = 25;
                        FranchiseImg.SaveAs(Path.Combine(folderPath,fileName));
                        franchise.FranchiseImg = fileName;

                    }
                    catch (Exception ex)
                    {



                    }
                }

                if (string.IsNullOrEmpty(franchise.franchiseName)) return true;
                if (Globals.GetUserAccess(this) < 9 && (Globals.IDToMask(franchiseID) & Globals.GetFranchiseMask()) == 0) return true;

                string error = Database.SetFranchise(franchiseID, ref franchise);
                if (error != null) ErrorLabel.Text = error;

                if (franchiseID == 0) Database.CopyQuoteTemplates(7, franchise.franchiseID);

                /*if (UploadReferral.HasFile && UploadReferral.PostedFile.ContentType == "application/pdf" && UploadReferral.PostedFile.ContentLength < 10485760)
                {
                    string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~") + "/ReferralList/ReferralList_" + franchise.franchiseID + ".pdf";
                    UploadReferral.SaveAs(uploadPath);
                }*/

                return error == null;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
    }
}