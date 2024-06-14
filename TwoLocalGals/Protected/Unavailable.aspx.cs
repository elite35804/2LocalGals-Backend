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
    public partial class Unavailable : System.Web.UI.Page
    {
        private int userAccess = 0;
        private DateTime dateRequest = DateTime.MinValue;
        private int contractorID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                userAccess = Globals.GetUserAccess(this);
                if (userAccess < 2) Globals.LogoutUser(this);

                dateRequest = Globals.DateTimeParse(Request["date"]);
                if (dateRequest == DateTime.MinValue) dateRequest = Globals.UtcToMst(DateTime.UtcNow);
                contractorID = (userAccess == 2 ? Globals.GetUserContractorID(this) : Globals.SafeIntParse(Request["conID"]));

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                    DateRequested.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                    LoadUnavaliable(Globals.SafeIntParse(Request["unID"]));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    ReloadPage();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public void DoneClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Globals.RedirectToPeviousPage(this, "Schedule.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = Database.DeleteUnavailable(Globals.SafeIntParse(Request["unID"]));
                if (error != null)
                {
                    ErrorLabel.Text = error;
                }
                else
                {
                    ReloadPage();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }

        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    ReloadPage();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public void PrevButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    dateRequest = dateRequest.AddMonths(-1);
                    dateRequest = new DateTime(dateRequest.Year, dateRequest.Month, 1);
                    ReloadPage();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PrevButtonClick EX: " + ex.Message;
            }
        }

        public void NextButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    dateRequest = dateRequest.AddMonths(1);
                    dateRequest = new DateTime(dateRequest.Year, dateRequest.Month, 1);
                    ReloadPage();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NextButtonClick EX: " + ex.Message;
            }
        }

        public void ReloadPage()
        {
            try
            {
                string url = Globals.BuildQueryString("Unavailable.aspx", "date", dateRequest.ToString("d"));
                if (userAccess > 2) url = Globals.BuildQueryString(url, "conID", contractorID);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ReloadPage EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                if (StartTime.Text == EndTime.Text) return true;
                UnavailableStruct unavailable = GetUnavaliableFromForms();
                if (unavailable.startTime >= unavailable.endTime)
                {
                    ErrorLabel.Text = "Invalid Start and End Times";
                }
                else
                {
                    if (unavailable.dateRequested.Date < Globals.UtcToMst(DateTime.UtcNow).Date) return true;
                    string error = Database.SetUnavailable(unavailable);
                    if (error != null) ErrorLabel.Text = error;
                    return error == null;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
            }
            return false;
        }

        private UnavailableStruct GetUnavaliableFromForms()
        {
            UnavailableStruct ret = new UnavailableStruct();
            try
            {
                ret.unavailableID = Globals.SafeIntParse(Request["unID"]);
                ret.contractorID = contractorID;
                ret.dateRequested = Globals.DateTimeParse(DateRequested.Text);
                ret.startTime = Convert.ToDateTime(StartTime.Text);
                ret.endTime = Convert.ToDateTime(EndTime.Text);
                ret.recurrenceType = RecurrenceType.SelectedIndex;
            }
            catch { }
            return ret;
        }

        private void LoadUnavaliable(int unavailableID)
        {
            try
            {
                ContractorStruct con = Database.GetContractorByID(Globals.GetFranchiseMask(), contractorID);

                UnavailableStruct unavailable;
                string error = Database.GetUnavailableByID(Globals.GetFranchiseMask(), unavailableID, out unavailable);

                ErrorLabel.Text = error;
                DeleteButton.Enabled = error == null && unavailable.unavailableID != 0;
                SaveButton.Enabled = error == null;

                if (unavailable.unavailableID == 0)
                {
                    unavailable.contractorID = contractorID;
                    unavailable.contractorTitle = con.title;
                    unavailable.dateRequested = dateRequest;
                    unavailable.startTime = Globals.TimeOnly(9, 0, 0);
                    if (!string.IsNullOrEmpty(Request["start"])) unavailable.startTime = Globals.TimeOnly(DateTime.Parse(Request["start"]));
                    unavailable.endTime = Globals.TimeOnly(9, 0, 0);
                    if (!string.IsNullOrEmpty(Request["end"])) unavailable.endTime = Globals.TimeOnly(DateTime.Parse(Request["end"]));
                    TitleLabel.Text = unavailable.contractorTitle + " - New Unavailability";
                }
                else
                {
                    TitleLabel.Text = unavailable.contractorTitle + " - Edit Unavailability";
                }

                DateRequested.Text = unavailable.dateRequested.ToString("d");
                StartTime.Text = unavailable.startTime.ToString("t");
                EndTime.Text = unavailable.endTime.ToString("t");
                RecurrenceType.SelectedIndex = unavailable.recurrenceType;

                UnCalendarCaption.Text = dateRequest.ToString("MMMM yyyy");
  
                int index = 0;
                DateTime monthStart = new DateTime(dateRequest.Year, dateRequest.Month, 1);
                int offset = (int)monthStart.DayOfWeek;
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                List<UnavailableStruct> historyList = Database.GetUnavailableByDateRange(Globals.GetFranchiseMask(), -1, unavailable.contractorID, monthStart, monthStart.AddMonths(1), "U.dateRequested, U.startTime, U.endTime");
                for (int i = 0; monthStart.Month == monthStart.AddDays(i).Month; i++)
                {
                    TableCell cell = UnCalendar.Rows[((i + offset) / 7) + 1].Cells[(i + offset) % 7];
                    cell.Style["background-color"] = "#FFF";

                    if (monthStart.AddDays(i).Date == mst.Date) cell.Text = @"<span style=""font-size:150%;"">" + (i + 1).ToString() + @"</span>";
                    else cell.Text = (i + 1).ToString();
                    if (monthStart.AddDays(i).Date >= mst.Date)
                        cell.Text = @"<a href=""Unavailable.aspx?date=" + monthStart.AddDays(i).Date.ToString("d") + @"&conID=" + contractorID + @""">" + cell.Text + @"</a>";

                    while (index < historyList.Count && historyList[index].dateRequested.Date == monthStart.AddDays(i).Date)
                    {
                        if (historyList[index].dateRequested.Date < mst.Date) cell.Text += @"<br/><span class=""UnavailableOld"">" + historyList[index].startTime.ToString("t") + " - " + historyList[index].endTime.ToString("t") + @"</span>";
                        else cell.Text += @"<br/><a class=""" + (historyList[index].recurrenceID == 0 ? "UnavailableLink" : "UnavailableRecurringLink") + @""" href=""Unavailable.aspx?unID=" + historyList[index].unavailableID + @"&conID=" + historyList[index].contractorID + @"&date=" +  HttpUtility.UrlEncode(historyList[index].dateRequested.ToString("d")) +  @""">" + historyList[index].startTime.ToString("t") + " - " + historyList[index].endTime.ToString("t") + @"</a>";
                        if (historyList[index].startTime.TimeOfDay <= con.startDay.TimeOfDay && historyList[index].endTime.TimeOfDay >= con.endDay.TimeOfDay)
                            cell.Style["background-color"] = "#FAA";
                        if (cell.Style["background-color"] != "#FAA")
                            cell.Style["background-color"] = "#FFFF80";
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadUnavaliable EX: " + ex.Message;
            }
        }
    }
}