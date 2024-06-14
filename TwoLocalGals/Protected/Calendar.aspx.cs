using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nexus.Protected
{
    public partial class WebFormCalendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Calendar";

                if (!IsPostBack)
                {
                    ContractorType.Items.Clear();
                    ContractorType.Items.AddRange(Globals.GetServicesList(Database.GetFranchiseServiceMask(Globals.GetFranchiseMask())));

                    if (!string.IsNullOrEmpty(Globals.GetCookieValue("CalendarContractorType")))
                        ContractorType.SelectedValue = Globals.GetCookieValue("CalendarContractorType");

                    int selectedMask = Request.Cookies["CalendarMask"] != null ? Globals.SafeIntParse(Request.Cookies["CalendarMask"].Value) : -1;
                    if (selectedMask == 0) selectedMask = -1;

                    int contractorSubType = Globals.SafeIntParse(ContractorType.SelectedValue);
                    int contractorMaskType = 1 << (contractorSubType - 1);
                    

                    foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.ID = "FRANCHECK" + franchise.Value;
                        checkBox.Text = franchise.Text;
                        checkBox.Checked = franchise.Selected;
                        MenuTable.Rows[0].Cells[3].Controls.Add(checkBox);
                    }

                    decimal weekHours = 0;
                    DateTime weekDate = Globals.DateTimeParse(Request["date"]);
                    if (weekDate == DateTime.MinValue) weekDate = DateTime.Now;
                    weekDate = Globals.StartOfWeek(weekDate);
                    WeekDate.Text = weekDate.ToString("d");

                    Dictionary<DateTime, List<AppStruct>> dict = new Dictionary<DateTime, List<AppStruct>>();
                    foreach (AppStruct app in Database.GetAppsByDateRange(Globals.GetFranchiseMask() & selectedMask, weekDate, weekDate + TimeSpan.FromDays(13), "A.startTime, A.endTime, CU.firstName, CU.lastName", true))
                    {
                        if (app.appStatus != 0 || app.appType != contractorSubType) continue;
                        if (!dict.ContainsKey(app.appointmentDate)) dict.Add(app.appointmentDate, new List<AppStruct>());
                        dict[app.appointmentDate].Add(app);
                    }

                    weekHours = PopulateTableDay(ref MondayTableOne, ref dict, weekDate);
                    weekHours += PopulateTableDay(ref TuesdayTableOne, ref dict, weekDate + TimeSpan.FromDays(1));
                    weekHours += PopulateTableDay(ref WednesdayTableOne, ref dict, weekDate + TimeSpan.FromDays(2));
                    weekHours += PopulateTableDay(ref ThursdayTableOne, ref dict, weekDate + TimeSpan.FromDays(3));
                    weekHours += PopulateTableDay(ref FridayTableOne, ref dict, weekDate + TimeSpan.FromDays(4));
                    weekHours += PopulateTableDay(ref SaturdayTableOne, ref dict, weekDate + TimeSpan.FromDays(5));
                    weekHours += PopulateTableDay(ref SundayTableOne, ref dict, weekDate + TimeSpan.FromDays(6));

                    OutsideTableOne.Caption = "Week " + ((weekDate.DayOfYear / 7) + 0) + " - Total Hours: " + weekHours.ToString("N2");

                    weekHours = PopulateTableDay(ref MondayTableTwo, ref dict, weekDate + TimeSpan.FromDays(7));
                    weekHours += PopulateTableDay(ref TuesdayTableTwo, ref dict, weekDate + TimeSpan.FromDays(8));
                    weekHours += PopulateTableDay(ref WednesdayTableTwo, ref dict, weekDate + TimeSpan.FromDays(9));
                    weekHours += PopulateTableDay(ref ThursdayTableTwo, ref dict, weekDate + TimeSpan.FromDays(10));
                    weekHours += PopulateTableDay(ref FridayTableTwo, ref dict, weekDate + TimeSpan.FromDays(11));
                    weekHours += PopulateTableDay(ref SaturdayTableTwo, ref dict, weekDate + TimeSpan.FromDays(12));
                    weekHours += PopulateTableDay(ref SundayTableTwo, ref dict, weekDate + TimeSpan.FromDays(13));

                    OutsideTableTwo.Caption = "Week " + ((weekDate.DayOfYear / 7) + 1) + " - Total Hours: " + weekHours.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        private void TransferToDate(int dayOffset)
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
                Globals.SetCookieValue("CalendarContractorType", ContractorType.SelectedValue.ToString());
                Globals.SetCookieValue("CalendarMask", selectedMask.ToString());
                DateTime weekDate = Globals.DateTimeParse(WeekDate.Text);
                if (weekDate == DateTime.MinValue) weekDate = DateTime.Now;
                weekDate += TimeSpan.FromDays(dayOffset);

                Response.Redirect("Calendar.aspx?date=" + weekDate.ToString("d"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TransferToDate EX: " + ex.Message;
            }
        }

        public void PrevButtonClick(object sender, EventArgs e)
        {
            TransferToDate(-14);
        }

        public void NextButtonClick(object sender, EventArgs e)
        {
            TransferToDate(+14);
        }

        public void ApplyButtonClick(object sender, EventArgs e)
        {
            TransferToDate(0);
        }

        private decimal PopulateTableDay(ref Table table, ref Dictionary<DateTime,List<AppStruct>> dict,  DateTime dateTime)
        {
            try
            {
                decimal totalHours = 0;

                table.Caption = dateTime.ToString("dddd - MMM dd");

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Start", 0));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 0));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Hrs", 0));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 200));
                table.Rows.Add(headerRow);

                if (dict.ContainsKey(dateTime))
                {
                    foreach (AppStruct app in dict[dateTime])
                    {
                        if (app.customerAccountStatus != "Ignored")
                        {
                            TimeSpan span = app.endTime - app.startTime;
                            decimal hours = (decimal)span.TotalMinutes / 60;

                            totalHours += hours;

                            TableRow row = new TableRow();
                            row.Cells.Add(Globals.FormatedTableCell(app.startTime.ToString("HH:mm")));
                            row.Cells.Add(Globals.FormatedTableCell(app.endTime.ToString("HH:mm")));
                            row.Cells.Add(Globals.FormatedTableCell(hours.ToString()));
                            row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.contractorTitle + @"</a>"));
                            row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>"));
                            table.Rows.Add(row);
                        }
                    }
                }

                TableFooterRow totalHoursRow = new TableFooterRow();
                TableCell totalHoursCell = new TableCell();
                totalHoursCell.Text = "Total Hours: " + totalHours.ToString("N2");
                totalHoursCell.Style["font-size"] = "1.4em";
                totalHoursCell.ColumnSpan = 5;
                totalHoursRow.Cells.Add(totalHoursCell);
                table.Rows.AddAt(0, totalHoursRow);

                return totalHours;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "PopulateTableDay EX: " + ex.Message;
                return 0;
            }
        }
    }
}