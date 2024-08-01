using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nexus.Protected
{
    public partial class Contractors : System.Web.UI.Page
    {
        private int contractorID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                int userAccess = Globals.GetUserAccess(this);

                if (userAccess < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Contractors";

                if (!IsPostBack)
                {
                    DeleteButton.Visible = (userAccess >= 7);

                    ContractorStruct contractor = new ContractorStruct();
                    OnlyActive.Checked = Request["ShowAll"] != "Y";
                    ContractorList.Items.Clear();
                    ContractorList.Items.Add(new ListItem("(New Contractor)", "0"));
                    ContractorList.Items.AddRange(Globals.GetContractorList(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["conID"]), 0, out contractor, OnlyActive.Checked, false));
                    FranchiseList.Items.Clear();
                    FranchiseList.Items.AddRange(Globals.GetFranchiseListNoMask(Globals.GetFranchiseMask() | contractor.franchiseMask, contractor.franchiseMask));
                    if (FranchiseList.Items.Count == 1) FranchiseList.SelectedIndex = 0;

                    if (contractor.contractorID == 0)
                    {
                        contractor.contractorType = 1;
                        contractor.startDay = new DateTime(1900, 1, 1, 9, 0, 0);
                        contractor.endDay = new DateTime(1900, 1, 1, 17, 0, 0);
                        contractor.paymentDay = "Tuesday";
                    }

                    ContractorType.Items.Clear();
                    ContractorType.Items.AddRange(Globals.GetContractorTypeList(contractor.contractorType));

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
                    SSN.Text = Globals.Decrypt(contractor.ssn);
                    StartDay.Text = contractor.startDay.ToString("t");
                    EndDay.Text = contractor.endDay.ToString("t");

                    TeamName.Text = contractor.team;
                    Notes.Text = contractor.notes;
                    PaymentType.Text = contractor.paymentType;
                    PaymentDay.Text = contractor.paymentDay;
                    HourlyRate.Text = Globals.FormatMoney(contractor.hourlyRate);
                    ServiceSplit.Text = Globals.FormatPercent(contractor.serviceSplit);
                    if (contractor.hireDate > new DateTime(1900, 1, 1, 0, 0, 0)) HireDate.Text = contractor.hireDate.ToString("d");
                    if (contractor.birthday > new DateTime(1900, 1, 1, 0, 0, 0)) Birthday.Text = contractor.birthday.ToString("d");
                    if (contractor.waiverDate > new DateTime(1900, 1, 1, 0, 0, 0)) WaiverDate.Text = contractor.waiverDate.ToString("d");
                    if (contractor.waiverUpdateDate > new DateTime(1900, 1, 1, 0, 0, 0)) WaiverUpdateDate.Text = contractor.waiverUpdateDate.ToString("d");
                    if (contractor.insuranceDate > new DateTime(1900, 1, 1, 0, 0, 0)) InsuranceDate.Text = contractor.insuranceDate.ToString("d");
                    if (contractor.insuranceUpdateDate > new DateTime(1900, 1, 1, 0, 0, 0)) InsuranceUpdateDate.Text = contractor.insuranceUpdateDate.ToString("d");
                    if (contractor.backgroundCheck > new DateTime(1900, 1, 1, 0, 0, 0)) BackgroundCheck.Text = contractor.backgroundCheck.ToString("d");
                    AccountRep.Checked = contractor.contractorID == 0 ? true : contractor.accountRep;
                    Applicant.Checked = contractor.applicant;
                    Active.Checked = contractor.contractorID == 0 ? true : contractor.active;          
                    Scheduled.Checked = contractor.contractorID == 0 ? true : contractor.scheduled;
                    SendSchedules.Checked = contractor.contractorID == 0 ? true : contractor.sendSchedules;
                    SendPayroll.Checked = contractor.contractorID == 0 ? true : contractor.sendPayroll;
                    SendScheduleByEmail.Checked = contractor.SendSchedulesByEmail;
                    if (!string.IsNullOrEmpty(contractor.ContractorPic))
                    {
                        DefaultPic.Visible = true;
                        DefaultPic.ImageUrl = "~/ContratorPics/" + contractor.ContractorPic;
                    } else
                    {
                        DefaultPic.Visible = true;
                        DefaultPic.ImageUrl = "~/ContratorPics/2LG_Logo.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void ContractorChanged(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Contractors.aspx?conID=" + Globals.SafeIntParse(ContractorList.SelectedValue) + "&ShowAll=" + Request["ShowAll"]);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ContractorChanged EX: " + ex.Message;
            }
        }

        public void OnlyActiveChanged(object sender, EventArgs e)
        {
            try
            {
                if (Request["ShowAll"] == "Y") Response.Redirect("Contractors.aspx");
                else Response.Redirect("Contractors.aspx?ShowAll=Y");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "OnlyActiveChanged EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Contractors.aspx");
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
                string error = Database.DeleteContractor(Globals.SafeIntParse(Request["conID"]));
                if (error == null)
                    Response.Redirect("Contractors.aspx");
                else
                    ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }

        public void ApplicationClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ApplicationPrint.aspx?conID=" + Globals.SafeIntParse(Request["conID"]));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ApplicationClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Contractors.aspx?conID=" + Globals.SafeIntParse(Request["conID"]));
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
                    Response.Redirect("Contractors.aspx?conID=" + contractorID);
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

                if (string.IsNullOrEmpty(FirstName.Text) && string.IsNullOrEmpty(LastName.Text) && string.IsNullOrEmpty(BusinessName.Text))
                {
                    return true;
                }

                int franchiseMask = 0;
                foreach (ListItem item in FranchiseList.Items)
                    if (item.Selected) franchiseMask |= Globals.IDToMask(Globals.SafeIntParse(item.Value));

                int contractorType = 0;
                foreach (ListItem item in ContractorType.Items)
                    if (item.Selected) contractorType |= Globals.SafeIntParse(item.Value);


                if (franchiseMask == 0)
                {
                    ErrorLabel.Text = "You must select at least one Franchise";
                    return false;
                }

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

                if (string.IsNullOrEmpty(SSN.Text) && (contractorType & 1) != 0)
                {
                    ErrorLabel.Text = "Required field SSN (Social Security Number)";
                    return false;
                }

                if (Globals.DateTimeParse(Birthday.Text) == DateTime.MinValue && (contractorType & 1) != 0)
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
                row.SetValue("contractorType", contractorType);
                row.SetValue("franchiseMask", franchiseMask);
                row.SetValue("firstName", FirstName.Text);
                row.SetValue("lastName", LastName.Text);
                row.SetValue("businessName", BusinessName.Text);
                row.SetValue("team", TeamName.Text);
                row.SetValue("address", Address.Text);
                row.SetValue("city", City.Text);
                row.SetValue("state", State.Text);
                row.SetValue("zip", Zip.Text);
                row.SetValue("bestPhone", Globals.FormatPhone(BestPhone.Text));
                row.SetValue("alternatePhone", Globals.FormatPhone(AlternatePhone.Text));
                row.SetValue("email", Email.Text);
                row.SetValue("ssn", SSN.Text);
                row.SetValue("startDay", Globals.TimeOnly(Globals.DateTimeParse(StartDay.Text)));
                row.SetValue("endDay", Globals.TimeOnly(Globals.DateTimeParse(EndDay.Text)));
                row.SetValue("notes", Notes.Text);
                row.SetValue("paymentType", PaymentType.Text);
                row.SetValue("paymentDay", PaymentDay.Text);
                row.SetValue("hourlyRate", Globals.FormatMoney(HourlyRate.Text));
                row.SetValue("serviceSplit", Globals.FormatPercent(ServiceSplit.Text));
                row.SetValue("hireDate", Globals.DateTimeParseSql(HireDate.Text));
                row.SetValue("birthday", Globals.DateTimeParseSql(Birthday.Text));
                row.SetValue("waiverDate", Globals.DateTimeParseSql(WaiverDate.Text));
                row.SetValue("waiverUpdateDate", Globals.DateTimeParseSql(WaiverUpdateDate.Text));
                row.SetValue("insuranceDate", Globals.DateTimeParseSql(InsuranceDate.Text));
                row.SetValue("insuranceUpdateDate", Globals.DateTimeParseSql(InsuranceUpdateDate.Text));
                row.SetValue("backgroundCheck", Globals.DateTimeParseSql(BackgroundCheck.Text));
                row.SetValue("accountRep", AccountRep.Checked);
                row.SetValue("applicant", Applicant.Checked);
                row.SetValue("active", Active.Checked);
                row.SetValue("scheduled",  Scheduled.Checked);
                row.SetValue("sendSchedules", SendSchedules.Checked);
                row.SetValue("sendPayroll", SendPayroll.Checked);
                row.SetValue("SendSchedulesByEmail", SendScheduleByEmail.Checked);

                if (ContractorPic.HasFile)
                {
                    try
                    {
                        string extension = Path.GetExtension(ContractorPic.FileName);
                        string fileName = Guid.NewGuid().ToString().Split('-').Last() + extension;
                        string folderPath = Server.MapPath("~/ContratorPics/");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        ContractorPic.Width = 25;
                        ContractorPic.SaveAs(Path.Combine(folderPath, fileName));
                        row.SetValue("ContractorPic", fileName);

                    }
                    catch (Exception ex)
                    {
                        // TODO: Throw exception for image upload error
                    }
                }

                contractorID = Globals.SafeIntParse(Request["conID"]);

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
    }
}