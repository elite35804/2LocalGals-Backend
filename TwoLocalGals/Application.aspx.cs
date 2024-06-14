using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.IO;

namespace TwoLocalGals
{
    public partial class Application : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (!IsPostBack)
                {
                    if (Request["success"] == "t")
                    {
                        NormalDiv.Visible = false;
                    }
                    else
                    {
                        SuccessDiv.Visible = false;
                        if (Globals.SafeIntParse(Request["franID"]) <= 0) 
                            Response.Redirect("https://www.2localgals.com");
                    }
                    Globals.SetPreviousPage(this, null);
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
                Response.Redirect(Globals.GetPeviousPage(this, "https://www.2localgals.com"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }

        protected void SubmitClick(object sender, EventArgs e)
        {
            try
            {
                int franchiseID = Globals.SafeIntParse(Request["franID"]);
                FranchiseStruct franchise = new FranchiseStruct();
                foreach(FranchiseStruct fran in Database.GetFranchiseList())
                {
                    if (fran.franchiseID == franchiseID)
                        franchise = fran;
                }
                if (franchise.franchiseID <= 0)
                {
                    CancelClick(sender, e);
                }
                else
                {
                    ErrorLabel.Text = "";

                    if (UploadResume.HasFile)
                    {
                        if (!GoodFileExtension(new FileInfo(UploadResume.PostedFile.FileName).Extension.ToLower()))
                        {
                            ErrorLabel.Text = "Invalid Resume Extension";
                            return;
                        }
                        if (UploadResume.FileContent.Length > 26214400)
                        {
                            ErrorLabel.Text = "Resume must be smaller than 25 MB";
                            return;
                        }
                    }

                    if (UploadCoverLetter.HasFile)
                    {
                        if (!GoodFileExtension(new FileInfo(UploadCoverLetter.PostedFile.FileName).Extension.ToLower()))
                        {
                            ErrorLabel.Text = "Invalid Cover Letter Extension";
                            return;
                        }
                        if (UploadCoverLetter.FileContent.Length > 26214400)
                        {
                            ErrorLabel.Text = "Cover Letter must be smaller than 25 MB";
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(FirstName.Text) || string.IsNullOrEmpty(LastName.Text))
                    {
                        ErrorLabel.Text = "First and Last Name cannot be blank";
                        return;
                    }

                    if (string.IsNullOrEmpty(Address.Text) || string.IsNullOrEmpty(City.Text) || string.IsNullOrEmpty(State.Text) || string.IsNullOrEmpty(Zip.Text))
                    {
                        ErrorLabel.Text = "Required fields Address, City, State, and Zip";
                        return;
                    }

                    if (string.IsNullOrEmpty(BestPhone.Text))
                    {
                        ErrorLabel.Text = "Required field Phone Number";
                        return;
                    }

                    if (!Globals.ValidEmail(Email.Text))
                    {
                        ErrorLabel.Text = "Invalid Email Address";
                        return;
                    }

                    if (string.IsNullOrEmpty(SSN.Text))
                    {
                        ErrorLabel.Text = "Required field SSN (Social Security Number)";
                        return;
                    }

                    if (Globals.DateTimeParse(Birthday.Text) == DateTime.MinValue)
                    {
                        ErrorLabel.Text = "Invalid Birth Date";
                        return;
                    }

                    DBRow row = new DBRow();

                    row.SetValue("applicant", true);
                    row.SetValue("active", false);
                    row.SetValue("franchiseMask", franchise.franchiseMask);
                    row.SetValue("firstName", FirstName.Text);
                    row.SetValue("lastName", LastName.Text);
                    row.SetValue("address", Address.Text);
                    row.SetValue("city", City.Text);
                    row.SetValue("state", State.Text);
                    row.SetValue("zip", Zip.Text);
                    row.SetValue("bestPhone", Globals.FormatPhone(BestPhone.Text));
                    row.SetValue("alternatePhone", Globals.FormatPhone(AlternatePhone.Text));
                    row.SetValue("email", Email.Text);
                    row.SetValue("ssn", SSN.Text);
                    row.SetValue("birthday", Globals.DateTimeParseSql(Birthday.Text));

                    row.SetValue("apFindUs", FindUs.Text);
                    row.SetValue("apWorkedBefore", WorkedBefore.Text);
                    row.SetValue("apWorkedBeforeWhen", WorkedBeforeWhen.Text);
                    row.SetValue("apHowLongAddress", HowLongAddress.Text);
                    row.SetValue("apDriversLicense", DriversLicense.Text);
                    row.SetValue("apDriversLicenseExpire", DriversLicenseExpire.Text);
                    row.SetValue("apHaveCar", HaveCar.Text);
                    row.SetValue("apDaysAvailable", DaysAvailable.Text);
                    row.SetValue("apHighSchool", HighSchool.Text);
                    row.SetValue("apCollege", College.Text);
                    row.SetValue("apHighSchoolDiploma", HighSchoolDiploma.Text);
                    row.SetValue("apFelony", Felony.Text);
                    row.SetValue("apFelonyDescription", FelonyDescription.Text);
                    row.SetValue("apRefOneName", RefOneName.Text);
                    row.SetValue("apRefOnePosition", RefOnePosition.Text);
                    row.SetValue("apRefOneCompany", RefOneCompany.Text);
                    row.SetValue("apRefOneAddress", RefOneAddress.Text);
                    row.SetValue("apRefOnePhoneNumber", RefOnePhoneNumber.Text);
                    row.SetValue("apRefTwoName", RefTwoName.Text);
                    row.SetValue("apRefTwoPosition", RefTwoPosition.Text);
                    row.SetValue("apRefTwoCompany", RefTwoCompany.Text);
                    row.SetValue("apRefTwoAddress", RefTwoAddress.Text);
                    row.SetValue("apRefTwoPhoneNumber", RefTwoPhoneNumber.Text);
                    row.SetValue("apEmpOneName", EmpOneName.Text);
                    row.SetValue("apEmpOneAddress", EmpOneAddress.Text);
                    row.SetValue("apEmpOnePhoneNumber", EmpOnePhoneNumber.Text);
                    row.SetValue("apEmpOneSupervisor", EmpOneSupervisor.Text);
                    row.SetValue("apEmpOneStartDate", EmpOneStartDate.Text);
                    row.SetValue("apEmpOneEndDate", EmpOneEndDate.Text);
                    row.SetValue("apEmpOneJobTitle", EmpOneJobTitle.Text);
                    row.SetValue("apEmpOneReasonLeave", EmpOneReasonLeave.Text);
                    row.SetValue("apEmpTwoName", EmpTwoName.Text);
                    row.SetValue("apEmpTwoAddress", EmpTwoAddress.Text);
                    row.SetValue("apEmpTwoPhoneNumber", EmpTwoPhoneNumber.Text);
                    row.SetValue("apEmpTwoSupervisor", EmpTwoSupervisor.Text);
                    row.SetValue("apEmpTwoStartDate", EmpTwoStartDate.Text);
                    row.SetValue("apEmpTwoEndDate", EmpTwoEndDate.Text);
                    row.SetValue("apEmpTwoJobTitle", EmpTwoJobTitle.Text);
                    row.SetValue("apEmpTwoReasonLeave", EmpTwoReasonLeave.Text);
                    row.SetValue("apEmpThreeName", EmpThreeName.Text);
                    row.SetValue("apEmpThreeAddress", EmpThreeAddress.Text);
                    row.SetValue("apEmpThreePhoneNumber", EmpThreePhoneNumber.Text);
                    row.SetValue("apEmpThreeSupervisor", EmpThreeSupervisor.Text);
                    row.SetValue("apEmpThreeStartDate", EmpThreeStartDate.Text);
                    row.SetValue("apEmpThreeEndDate", EmpThreeEndDate.Text);
                    row.SetValue("apEmpThreeJobTitle", EmpThreeJobTitle.Text);
                    row.SetValue("apEmpThreeReasonLeave", EmpThreeReasonLeave.Text);
                    row.SetValue("apContactEmployer", ContactEmployer.Text);

                    int contractorID = 0;
                    string error = Database.DynamicSetWithKeyInt("Contractors", "contractorID", ref contractorID, row);
                    if (error != null)
                    {
                        ErrorLabel.Text = error;
                        return;
                    }

                    if (UploadResume.HasFile)
                    {
                        string extension = new FileInfo(UploadResume.PostedFile.FileName).Extension.ToLower();
                        string uploadPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), "Resume/Resume_" + contractorID + extension);
                        UploadResume.SaveAs(uploadPath);
                    }

                    if (UploadCoverLetter.HasFile)
                    {
                        string extension = new FileInfo(UploadCoverLetter.PostedFile.FileName).Extension.ToLower();
                        string uploadPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), "Resume/CoverLetter_" + contractorID + extension);
                        UploadCoverLetter.SaveAs(uploadPath);
                    }

                    Response.Redirect("Application.aspx?success=t");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Submit EX: " + ex.Message;
            }
        }

        private bool GoodFileExtension(string fileExtension)
        {
            try
            {
                switch (fileExtension)
                {
                    case ".zip": return true;
                    case ".pdf": return true;
                    case ".doc": return true;
                    case ".dot": return true;
                    case ".docx": return true;
                    case ".docm": return true;
                    case ".txt": return true;
                    case ".rar": return true;
                    case ".jpg": return true;
                    case ".xls": return true;
                    case ".wpd": return true;
                    case ".rtf": return true;
                    case ".odt": return true;
                    case ".ods": return true;
                    case ".odp": return true;
                }
            }
            catch { }
            return false;
        }
    }
}