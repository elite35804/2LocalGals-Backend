using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalAppointments : System.Web.UI.Page
    {
        private int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Globals.ForceSSL(this);
            customerID = Globals.GetPortalCustomerID(this);
            if (customerID <= 0) Globals.LogoutUser(this);
            ((CustomerPortal)this.Page.Master).SetActiveMenuItem(2);

            LoadAppointments();
            LoadTransactions();

            if (!IsPostBack)
            {
                Globals.SetPreviousPage(this, null);
            }
        }

        private void LoadAppointments()
        {
            CustomerStruct customer;
            Database.GetCustomerByID(-1, customerID, out customer);

            List<string> customerTimeList = new List<string>();
            if (!string.IsNullOrEmpty(customer.NC_Frequency)) customerTimeList.Add(customer.NC_Frequency);
            if (!string.IsNullOrEmpty(customer.NC_DayOfWeek)) customerTimeList.Add(customer.NC_DayOfWeek);
            if (!string.IsNullOrEmpty(customer.NC_TimeOfDayPrefix) && !string.IsNullOrEmpty(customer.NC_TimeOfDay)) customerTimeList.Add(customer.NC_TimeOfDayPrefix + " " + customer.NC_TimeOfDay);
            Frequency.InnerHtml = "(" + string.Join(", ", customerTimeList.ToArray()) + ")";

            List<AppStruct> apps = Database.GetAppsByCustomerID(customerID);
            SortedList<DateTime, List<AppStruct>> appDict = new SortedList<DateTime, List<AppStruct>>();
            foreach (AppStruct app in apps)
            {
                if (app.appStatus == 0 && app.contractorID != 0)
                {
                    if (!appDict.ContainsKey(app.appointmentDate)) appDict.Add(app.appointmentDate, new List<AppStruct>());
                    appDict[app.appointmentDate].Add(app);
                }
            }

            int currentDateIndex = 1;
            DateTime currDate = Globals.UtcToMst(DateTime.UtcNow);

            bool colorChange = true;
            int futureCount = 0;
            foreach (DateTime key in appDict.Keys)
            {
                TableRow row = new TableRow();
                if (key > currDate.Date)
                {
                    TableCell futureCell = new TableCell();
                    futureCell.ColumnSpan = 4;
                    futureCell.Style["text-align"] = "left";
                    futureCell.Text = key.ToString("ddd MM/dd/yy") + " - Scheduled Appointment";
                    row.Cells.Add(futureCell);
                    futureCount++;
                    if (futureCount == 5) break;
                }
                else
                {
                    row.Cells.Add(Globals.FormatedTableCell(key.ToString("ddd MM/dd/yy") + @" - <a href=""PortalReview.aspx?appID=" + appDict[key][0].appointmentID + @""">Review</a>"));

                    TableCell contractorCell = new TableCell();
                    contractorCell.Style["text-align"] = "left";

                    TableCell tipsCell = new TableCell();
                    tipsCell.Style["text-align"] = "right";
                    tipsCell.Style["width"] = "150px";

                    decimal hoursTotal = 0;
                    foreach (AppStruct app in appDict[key])
                    {
                        if (app.appType == 1) hoursTotal += app.customerHours;
                        contractorCell.Text += @"<div class=""Contractor"">" + app.contractorTitle.Split(' ')[0] + @"</div>";
                        tipsCell.Text += @"<div class=""Tip"">" + Globals.FormatMoney(app.contractorTips) + @" - <a href=""PortalAddTip.aspx?appID=" + app.appointmentID + @""">Add Tip</a></div>";
                    }

                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(hoursTotal)));
                    row.Cells.Add(contractorCell);
                    row.Cells.Add(tipsCell);
                }

                if (colorChange) row.Style["background-color"] = "#D9D9D9";
                colorChange = !colorChange;
                AppointmentTable.Rows.AddAt(1, row);

                if (key >= currDate.Date)
                    currentDateIndex++;
            }

            TableRow currDateRow = new TableRow();
            currDateRow.Style["color"] = "Black";
            currDateRow.Style["background"] = "Orange";
            currDateRow.Style["text-align"] = "center";
            currDateRow.Style["font-weight"] = "Bold";
            TableCell currDateCell = new TableCell();
            currDateCell.ColumnSpan = 4;
            currDateCell.Text = "Today: " + currDate.ToString("dddd MM/dd/yy");
            currDateRow.Cells.Add(currDateCell);
            AppointmentTable.Rows.AddAt(currentDateIndex, currDateRow);
        }

        private void LoadTransactions()
        {
            DateTime currDate = Globals.UtcToMst(DateTime.UtcNow);
            List<TransactionStruct> transList = Database.GetTransactions(-1, customerID, currDate - TimeSpan.FromDays(365), currDate, "T.dateApply DESC");

            bool colorChange = false;
            foreach (TransactionStruct trans in transList)
            {
                if (trans.auth != 3)
                {
                    string transType = trans.transType;
                    if (trans.auth == 1) transType = "Authorize";
                    if (trans.isVoid) transType = "VOID";

                    TableRow row = new TableRow();
                    row.Cells.Add(Globals.FormatedTableCell(trans.dateApply.ToString("ddd MM/dd/yy")));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""TransactionPrintout.aspx?transID=" + trans.transID + @""">" + transType + " - " + trans.paymentType + " (" + trans.dateCreated.ToString("MM/dd/yy") + ")</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(trans.total)));
                    if (colorChange) row.Style["background-color"] = "#D9D9D9";
                    colorChange = !colorChange;
                    TransactionTable.Rows.Add(row);
                }
            }
        }
    }
}