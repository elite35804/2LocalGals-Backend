using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TwoLocalGals;

namespace Nexus.Protected
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                int userAccess = Globals.GetUserAccess(this);
                if (userAccess < 2)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Reports";

                int selectedMask = -1;
                if (Globals.GetUserAccess(this) > 2)
                {
                    if (Request.Cookies["ReportsMask"] != null) selectedMask = Globals.SafeIntParse(Globals.GetCookieValue("ReportsMask"));
                    if (selectedMask == 0) selectedMask = -1;
                    foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.ID = "FRANCHECK" + franchise.Value;
                        checkBox.Text = franchise.Text;
                        checkBox.Checked = franchise.Selected;
                        checkBox.CssClass = "FranchiseCheckbox";
                        FranchiseCell.Controls.Add(checkBox);
                    }

                    ContractorTypeRow.Style["display"] = "table-row";
                }
                else
                {
                    FranchiseTable.Visible = false;
                    ContractorsTable.Visible = false;
                }

                if (!IsPostBack)
                {
                    DateTime now = Globals.UtcToMst(DateTime.UtcNow);

                    int serviceMask = Database.GetFranchiseServiceMask(Globals.GetFranchiseMask());
                    if ((serviceMask & 2) == 0) CarpetCleaningLabel.Visible = false;
                    if ((serviceMask & 4) == 0) WindowWashingLabel.Visible = false;
                    if ((serviceMask & 8) == 0) HomewatchLabel.Visible = false;

                    int contractorMask = Globals.SafeIntParse(Globals.GetCookieValue("ReportsContractorMask"));
                    if (contractorMask == 0) contractorMask = 1;
                    if ((contractorMask & 1) != 0) HousekeepingCheckbox.Checked = true;
                    if ((contractorMask & 2) != 0) CarpetCleaningCheckbox.Checked = true;
                    if ((contractorMask & 4) != 0) WindowWashingCheckbox.Checked = true;
                    if ((contractorMask & 8) != 0) HomewatchCheckbox.Checked = true;

                    ReportsList.Items.Add(new ListItem("Appointments by Contractor Report", "0"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Scheduling Appointments Report", "1"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Confirmation Appointments Report", "2"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Contractor Directory Report", "3"));
                    ReportsList.Items.Add(new ListItem("Payroll Report", "4"));
                    //if (userAccess >= 7) ReportsList.Items.Add(new ListItem("Accounting Report", "5"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Unavailability Report", "6"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("New/Follow-up Customers Report", "7"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Keys Report", "8"));
                    //if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Checks/Cash Needed Report", "9"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Web Quotes Report", "10"));
                    if (userAccess >= 7) ReportsList.Items.Add(new ListItem("Sales Report", "11"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Gift Cards Report", "12"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Appointment Request Report", "13"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Applicants Report", "14"));
                    ReportsList.Items.Add(new ListItem("Review Scores Report", "15"));
                    if (userAccess >= 7) ReportsList.Items.Add(new ListItem("Hours Booked Report", "16"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Hours Earned Report", "17"));
                    if (userAccess >= 5) ReportsList.Items.Add(new ListItem("Customer Emails", "18"));
                    if (userAccess >= 7) ReportsList.Items.Add(new ListItem("WCF Audit", "19"));

                    foreach (ContractorStruct contractor in Database.GetContractorList(Globals.GetFranchiseMask(), -1, false, true, false, false, "firstName, lastName"))
                        ContractorList.Items.Add(new ListItem(contractor.title, contractor.contractorID.ToString()));

                    StartDate.Text = Globals.GetCookieValue("ReportsStartDate");
                    EndDate.Text = Globals.GetCookieValue("ReportsEndDate");
                    QuickDates.SelectedValue = Globals.GetCookieValue("ReportsQuickDates");
                    ReportsList.SelectedValue = Globals.GetCookieValue("ReportsReportType");
                    if (string.IsNullOrEmpty(StartDate.Text)) StartDate.Text = now.ToString("d");
                    if (string.IsNullOrEmpty(EndDate.Text)) EndDate.Text = now.ToString("d");

                    ExportCustomersButton.Visible = userAccess >= 7;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void QuickDatesChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime now = Globals.UtcToMst(DateTime.UtcNow);
                DateTime monday = now - TimeSpan.FromDays(Globals.GetDayOfWeek(now));

                switch (QuickDates.Text)
                {
                    case "Today":
                        StartDate.Text = now.ToString("d");
                        EndDate.Text = now.ToString("d");
                        break;
                    case "Tomorrow":
                        now += TimeSpan.FromDays(1);
                        StartDate.Text = now.ToString("d");
                        EndDate.Text = now.ToString("d");
                        break;
                    case "Yesterday":
                        now -= TimeSpan.FromDays(1);
                        StartDate.Text = now.ToString("d");
                        EndDate.Text = now.ToString("d");
                        break;
                    case "Last Week":
                        monday -= TimeSpan.FromDays(1);
                        EndDate.Text = monday.ToString("d");
                        monday -= TimeSpan.FromDays(6);
                        StartDate.Text = monday.ToString("d");
                        break;
                    case "2 Weeks Ago":
                        monday -= TimeSpan.FromDays(8);
                        EndDate.Text = monday.ToString("d");
                        monday -= TimeSpan.FromDays(6);
                        StartDate.Text = monday.ToString("d");
                        break;
                    case "This Week":
                        StartDate.Text = monday.ToString("d");
                        monday += TimeSpan.FromDays(6);
                        EndDate.Text = monday.ToString("d");
                        break;
                    case "Next Week":
                        monday += TimeSpan.FromDays(7);
                        StartDate.Text = monday.ToString("d");
                        monday += TimeSpan.FromDays(6);
                        EndDate.Text = monday.ToString("d");
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "QuickDatesChanged EX: " + ex.Message;
            }
        }

        public void ViewReportClick(object sender, EventArgs e)
        {
            try
            {
                int selectedMask = 0;
                foreach (string value in Request.Form)
                {
                    if (value.Contains("FRANCHECK"))
                    {
                        string franID = value.Substring(value.IndexOf("FRANCHECK") + 9, 2);
                        selectedMask |= Globals.IDToMask(Globals.SafeIntParse(franID));
                    }
                }

                int contractorMask = 0;
                if (HousekeepingCheckbox.Checked) contractorMask |= 1;
                if (CarpetCleaningCheckbox.Checked) contractorMask |= 2;
                if (WindowWashingCheckbox.Checked) contractorMask |= 4;
                if (HomewatchCheckbox.Checked) contractorMask |= 8;

                Globals.SetCookieValue("ReportsMask", selectedMask.ToString());
                Globals.SetCookieValue("ReportsContractorMask", contractorMask.ToString());
                Globals.SetCookieValue("ReportsStartDate", StartDate.Text);
                Globals.SetCookieValue("ReportsEndDate", EndDate.Text);
                Globals.SetCookieValue("ReportsQuickDates", QuickDates.SelectedValue);
                Globals.SetCookieValue("ReportsReportType", ReportsList.SelectedValue);

                Globals.DeleteCookie("ReportScrollPos");

                if (ReportsList.SelectedValue == "19")
                {
                    string url = Globals.BuildQueryString("ExportExcel.aspx", "Report", ReportsList.SelectedValue);
                    url = Globals.BuildQueryString(url, "startDate", StartDate.Text);
                    url = Globals.BuildQueryString(url, "endDate", EndDate.Text);
                    url = Globals.BuildQueryString(url, "mask", selectedMask);
                    url = Globals.BuildQueryString(url, "contractorMask", contractorMask);
                    Response.Redirect(url);
                }
                else
                {
                    string url = Globals.BuildQueryString("ViewReport.aspx", "report", ReportsList.SelectedValue);
                    url = Globals.BuildQueryString(url, "startDate", StartDate.Text);
                    url = Globals.BuildQueryString(url, "endDate", EndDate.Text);
                    url = Globals.BuildQueryString(url, "mask", selectedMask);
                    url = Globals.BuildQueryString(url, "contractorMask", contractorMask);
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ViewReportClick EX: " + ex.Message;
            }
        }

        public void SendSchedulesClick(object sender, EventArgs e)
        {
            try
            {
                List<int> selectedList = new List<int>();
                foreach (ListItem item in ContractorList.Items)
                    if (item.Selected) selectedList.Add(Globals.SafeIntParse(item.Value));

                List<ContractorStruct> contractorList = new List<ContractorStruct>();
                foreach (ContractorStruct contractor in Database.GetContractorList(Globals.GetFranchiseMask(), -1, false, true, false, false, "firstName, lastName"))
                    if (selectedList.Contains(contractor.contractorID)) contractorList.Add(contractor);


                string error = SendEmail.SendContractorSchedules(contractorList, DateTime.Parse(StartDate.Text), DateTime.Parse(EndDate.Text), false);
                if (error != null) ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendSchedulesClick EX: " + ex.Message;
            }
        }

        public void SendPayrollClick(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = Globals.DateTimeParse(StartDate.Text);
                DateTime endDate = Globals.DateTimeParse(EndDate.Text);

                if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                {
                    ErrorLabel.Text = "Invalid Dates Selected";
                }
                else
                {
                    List<int> selectedList = new List<int>();
                    foreach (ListItem item in ContractorList.Items)
                        if (item.Selected) selectedList.Add(Globals.SafeIntParse(item.Value));

                    List<ContractorStruct> contractorList = new List<ContractorStruct>();
                    foreach (ContractorStruct contractor in Database.GetContractorList(Globals.GetFranchiseMask(), -1, false, false, false, false, "firstName, lastName"))
                        if (selectedList.Contains(contractor.contractorID)) contractorList.Add(contractor);

                    string error = SendEmail.SendContractorPayroll(contractorList, startDate, endDate);
                    if (error != null) ErrorLabel.Text = error;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendPayrollClick EX: " + ex.Message;
            }
        }

        public void PartnersClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PartnersModify.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PartnersClick EX: " + ex.Message;
            }
        }

        public void SendPromotionsClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SendPromotions.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendPromotionsClick EX: " + ex.Message;
            }
        }

        public void ExportCustomersClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ExportExcel.aspx?Report=1");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ExportCustomersClick EX: " + ex.Message;
            }
        }

        public void TestSMSClick(object sender, EventArgs e)
        {
            try
            {
                int selectedMask = 0;
                foreach (string value in Request.Form)
                {
                    if (value.Contains("FRANCHECK"))
                    {
                        string franID = value.Substring(value.IndexOf("FRANCHECK") + 9, 2);
                        selectedMask |= Globals.IDToMask(Globals.SafeIntParse(franID));
                    }
                }

                foreach(FranchiseStruct franchise in Database.GetFranchiseList())
                {
                    if ((franchise.franchiseMask & selectedMask) != 0)
                    {
                        string error = Texting.SendText(TestSMSTextBox.Text, "Test Message From Franchise: " + franchise.franchiseName, franchise);
                        if (error != null)
                        {
                            ErrorLabel.Text = "Error Sending SMS from (" + franchise.franchiseName + "): " + error;
                        }
                    }
                }

                ErrorLabel.Text = "SMS Sent Successfully";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TestSMSClick EX: " + ex.Message;
            }
        }
    }
}