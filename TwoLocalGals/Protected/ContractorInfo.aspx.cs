using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class ContractorInfo : System.Web.UI.Page
    {
        private int contractorID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                int userAccess = Globals.GetUserAccess(this);

                if (userAccess != 2)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG My Information";

                if (!IsPostBack)
                {
                    contractorID = Globals.GetUserContractorID(this);
                    ContractorStruct contractor = Database.GetContractorByID(Globals.GetFranchiseMask(), contractorID);

                    if (contractor.contractorID == 0)
                        Globals.LogoutUser(this);

                    FirstName.Text = contractor.firstName;
                    LastName.Text = contractor.lastName;
                    BusinessName.Text = contractor.businessName;
                    Address.Text = contractor.address;
                    City.Text = contractor.city;
                    State.Text = contractor.state;
                    Zip.Text = contractor.zip;
                    BestPhone.Text = Globals.FormatPhone(contractor.bestPhone);
                    AlternatePhone.Text = Globals.FormatPhone(contractor.alternatePhone);
                    Email.Text = contractor.email;
                    SSN.Text = contractor.ssn;
                    if (contractor.birthday > new DateTime(1900, 1, 1, 0, 0, 0)) Birthday.Text = contractor.birthday.ToString("d");
                    StartDay.Text = contractor.startDay.ToString("t");
                    EndDay.Text = contractor.endDay.ToString("t");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ContractorInfo.aspx");
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

        public bool SaveChanges()
        {
            try
            {
                ErrorLabel.Text = "";

                contractorID = Globals.GetUserContractorID(this);

                if (contractorID == 0)
                    Globals.LogoutUser(this);

                if ((string.IsNullOrEmpty(FirstName.Text) || string.IsNullOrEmpty(LastName.Text)) && string.IsNullOrEmpty(BusinessName.Text))
                {
                    ErrorLabel.Text = "First and Last Name cannot be blank";
                    return false;
                }

                if (string.IsNullOrEmpty(Address.Text) || string.IsNullOrEmpty(City.Text) || string.IsNullOrEmpty(State.Text) || string.IsNullOrEmpty(Zip.Text))
                {
                    ErrorLabel.Text = "Required fields Address, City, State, and Zip";
                    return false;
                }

                if (string.IsNullOrEmpty(BestPhone.Text))
                {
                    ErrorLabel.Text = "Required field Phone Number";
                    return false;
                }

                if (!string.IsNullOrEmpty(Email.Text) && !Globals.ValidEmail(Email.Text))
                {
                    ErrorLabel.Text = "Invalid Email Address";
                    return false;
                }

                if (string.IsNullOrEmpty(SSN.Text))
                {
                    ErrorLabel.Text = "Required field SSN (Social Security Number)";
                    return false;
                }

                if (Globals.DateTimeParse(Birthday.Text) == DateTime.MinValue)
                {
                    ErrorLabel.Text = "Invalid Birth Date";
                    return false;
                }

                if (Globals.TimeOnly(Globals.DateTimeParse(StartDay.Text)) >= Globals.TimeOnly(Globals.DateTimeParse(EndDay.Text)))
                {
                    ErrorLabel.Text = "Invalid Start and End Time Range";
                    return false;
                }

                DBRow row = new DBRow();
                row.SetValue("firstName", FirstName.Text);
                row.SetValue("lastName", LastName.Text);
                row.SetValue("businessName", BusinessName.Text);
                row.SetValue("address", Address.Text);
                row.SetValue("city", City.Text);
                row.SetValue("state", State.Text);
                row.SetValue("zip", Zip.Text);
                row.SetValue("bestPhone", Globals.FormatPhone(BestPhone.Text));
                row.SetValue("alternatePhone", Globals.FormatPhone(AlternatePhone.Text));
                row.SetValue("email", Email.Text);
                row.SetValue("ssn", SSN.Text);
                row.SetValue("birthday", Globals.DateTimeParseSql(Birthday.Text));
                row.SetValue("startDay", Globals.TimeOnly(Globals.DateTimeParse(StartDay.Text)));
                row.SetValue("endDay", Globals.TimeOnly(Globals.DateTimeParse(EndDay.Text)));


                if (UploadPic.HasFile)
                {
                    try
                    {
                        var getExtension = Path.GetExtension(UploadPic.FileName);
                        var fileName = Guid.NewGuid().ToString() + getExtension;
                        string folderPath = Server.MapPath("~/ContratorPics/");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        UploadPic.Width = 25;
                        UploadPic.SaveAs(Path.Combine(folderPath, fileName));
                        row.SetValue("ContractorPic", fileName);

                    }
                    catch (Exception ex)
                    {



                    }
                }

                string error = Database.DynamicSetWithKeyInt("Contractors", "contractorID", ref contractorID, row);
                if (error != null) ErrorLabel.Text = error;
                return error == null;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }



        }


        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (UploadPic.HasFile)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string fileExtension = Path.GetExtension(UploadPic.FileName).ToLower();

                if (Array.Exists(allowedExtensions, extension => extension == fileExtension))
                {

                }

            }
        }

    }
}