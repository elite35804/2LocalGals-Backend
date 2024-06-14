using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace Nexus.Protected
{
    public partial class ViewReport : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 2)
                    Globals.LogoutUser(this);

                DateTime startDate = Globals.SafeDateParse(Request["startDate"]);
                DateTime endDate = Globals.SafeDateParse(Request["endDate"]);

                int franchiseMask = Globals.SafeIntParse(Request["mask"]);
                if (Globals.GetUserAccess(this) == 2) franchiseMask = -1;

                int contractorMask = Globals.SafeIntParse(Request["contractorMask"]);
                if (Globals.GetUserAccess(this) == 2 || contractorMask == 0) contractorMask = -1;

                switch (Globals.SafeIntParse(Request["report"]))
                {
                    case 0: AppointmentsByContractorReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 1: SchedulingAppointmentsReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 2: ConfirmationAppointmentsReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 3: ContractorDirectoryReport(franchiseMask, contractorMask); break;
                    case 4: PayrollReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 6: UnavailabilityReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 7: NewFollowUpCustomersReport(franchiseMask, contractorMask); break;
                    case 8: KeysReport(franchiseMask, startDate, endDate); break;
                    case 10: WebQuotesReport(franchiseMask); break;
                    case 11: SalesReport(franchiseMask, startDate, endDate); break;
                    case 12: GiftCardReport(franchiseMask, startDate, endDate); break;
                    case 13: AppointmentRequestReport(franchiseMask); break;
                    case 14: ApplicantReport(franchiseMask); break;
                    case 15: ReviewScoresReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 16: HoursBookedReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 17: HoursEarnedReport(franchiseMask, contractorMask, startDate, endDate); break;
                    case 18: CustomerEmailsReport(franchiseMask, contractorMask, startDate, endDate); break;
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "Page_Load EX: " + ex.Message;
            }
        }
        #endregion

        #region ConfirmButtonClick
        public void ConfirmButtonClick(object sender, EventArgs e)
        {
            try
            {
                ErrorDiv.InnerHtml = "";

                Button button = (Button)sender;
                int appID = Globals.SafeIntParse(button.CommandArgument);
                if (appID != 0)
                {
                    if (button.Text.StartsWith("Text/Email"))
                    {
                        string error = SendEmail.SendConfimation(appID, button.Text == "Text/Email Week");
                        if (error != null)
                        {
                            ErrorDiv.InnerHtml = error;
                            return;
                        }

                        error = Texting.SendCustomerConfirmationText(appID, button.Text == "Text/Email Week");
                        if (error != null)
                        {
                            ErrorDiv.InnerHtml = error;
                            return;
                        }
                    }

                    if (null == Database.SetAppointmentBits(appID, button.Text == "Verified", button.Text == "Left Message", button.Text == "Text/Email Day", button.Text == "Text/Email Week", false, false))
                    {
                        Response.Redirect(Globals.BuildQueryString(Request.RawUrl, "DoScroll", "Y"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "ConfirmButtonClick EX: " + ex.Message;
            }
        }
        #endregion

        #region ReturnedkeysClicked
        public void ReturnedkeysClicked(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int appID = Globals.SafeIntParse(button.CommandArgument);
                if (appID != 0)
                {
                    if (null == Database.SetAppointmentBits(appID, false, false, false, false, false, true))
                        button.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "ReturnedkeysClicked EX: " + ex.Message;
            }
        }
        #endregion

        #region DeleteAppointmentRequestClick
        public void DeleteAppointmentRequestClick(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int serviceRequestID = Globals.SafeIntParse(button.CommandArgument);
                if (serviceRequestID != 0)
                {
                    Database.DynamicDeleteWithKey("ServiceRequest", "serviceRequestID", serviceRequestID);
                    Response.Redirect(Globals.BuildQueryString(Request.RawUrl, "DoScroll", "Y"));
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "DeleteAppointmentRequestClick EX: " + ex.Message;
            }
        }
        #endregion

        #region AppointmentsByContractorReport
        private void AppointmentsByContractorReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                int userAcess = Globals.GetUserAccess(this);
                int contractorID = Globals.GetUserContractorID(this);
                SortedList<DateTime, SortedList<string, List<ConAppStruct>>> list = new SortedList<DateTime, SortedList<string, List<ConAppStruct>>>();
                Dictionary<DateTime, Dictionary<int, List<ConAppStruct>>> partnerDict = new Dictionary<DateTime, Dictionary<int, List<ConAppStruct>>>();

                foreach (ConAppStruct conApp in Database.GetCleaningProjects(franchiseMask, startDate, endDate))
                {
                    if (userAcess < 5 && conApp.contractorID != contractorID) continue;
                    if ((Globals.IDToMask(conApp.appType) & contractorMask) == 0) continue;

                    if (!list.ContainsKey(conApp.appointmentDate)) list.Add(conApp.appointmentDate, new SortedList<string, List<ConAppStruct>>());
                    if (!list[conApp.appointmentDate].ContainsKey(conApp.contractorTitle)) list[conApp.appointmentDate].Add(conApp.contractorTitle, new List<ConAppStruct>());
                    list[conApp.appointmentDate][conApp.contractorTitle].Add(conApp);

                    if (!partnerDict.ContainsKey(conApp.appointmentDate)) partnerDict.Add(conApp.appointmentDate, new Dictionary<int, List<ConAppStruct>>());
                    if (!partnerDict[conApp.appointmentDate].ContainsKey(conApp.customerID)) partnerDict[conApp.appointmentDate].Add(conApp.customerID, new List<ConAppStruct>());
                    partnerDict[conApp.appointmentDate][conApp.customerID].Add(conApp);
                }

                Table table = null;

                foreach (DateTime appDate in list.Keys)
                {
                    foreach (string conTitle in list[appDate].Keys)
                    {
                        decimal totalHours = 0;
                        table = new Table();
                        table.CssClass = @"Project";
                        table.Caption = @"<b>Scheduled Projects For: </b>" + conTitle + @"<b style=""margin-left: 30px;"">Date: </b>" + appDate.ToString("d");

                        string lastAddr = null;
                        foreach (ConAppStruct conApp in list[appDate][conTitle])
                        {
                            if (lastAddr == null) lastAddr = Globals.CleanAddr(conApp.contractorAddress) + "," + Globals.CleanAddr(conApp.contractorCity) + "," + Globals.CleanAddr(conApp.contractorState) + "," + Globals.CleanAddr(conApp.contractorZip);
                            TimeSpan span = conApp.endTime - conApp.startTime;
                            decimal hours = (decimal)span.TotalMinutes / 60;
                            totalHours += hours;

                            TableRow row = new TableRow();
                            TableCell cell = new TableCell();
                            Table infoTable = new Table();
                            infoTable.CssClass = "ProjectInfo";

                            TableHeaderRow infoHeaderRow = new TableHeaderRow();
                            infoHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Appointment Time", 120));
                            infoHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours", 50));
                            infoHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 150));
                            infoHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Address", 0));
                            infoHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("City", 130));
                            infoTable.Rows.Add(infoHeaderRow);

                            TableRow infoRow = new TableRow();
                            if (userAcess < 5) infoRow.Cells.Add(Globals.FormatedTableCell(conApp.startTime.ToString("t") + " to " + conApp.endTime.ToString("t")));
                            else infoRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + conApp.appointmentID + @""">" + conApp.startTime.ToString("t") + " to " + conApp.endTime.ToString("t") + @"</a>"));
                            infoRow.Cells.Add(Globals.FormatedTableCell(hours.ToString("N2")));
                            if (userAcess < 5) infoRow.Cells.Add(Globals.FormatedTableCell(conApp.customerTitle));
                            else infoRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + conApp.customerID + @""">" + conApp.customerTitle + @"</a>"));
                            string newAddr = Globals.CleanAddr(conApp.address) + "," + Globals.CleanAddr(conApp.city) + "," + Globals.CleanAddr(conApp.state) + "," + Globals.CleanAddr(conApp.zip);
                            infoRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""" + GoogleMaps.GetDrivingLink(new string[] { lastAddr, newAddr }) + @""">" + conApp.address + @"</a>"));
                            lastAddr = newAddr;
                            infoRow.Cells.Add(Globals.FormatedTableCell(conApp.city.Trim() + ", " +  conApp.zip));

                            infoTable.Rows.Add(infoRow);

                            cell.Controls.Add(infoTable);
                            row.Cells.Add(cell);
                            table.Rows.Add(row);

                            row = new TableRow();
                            row.Cells.Add(Globals.FormatedTableCell("<b>Appointment Type: </b>" + Globals.IndexToServiceType(conApp.appType)));
                            table.Rows.Add(row);

                            List<string> partners = new List<string>();
                            foreach(var partner in partnerDict[conApp.appointmentDate][conApp.customerID])
                            {
                                if (partner.contractorID != conApp.contractorID)
                                {
                                    partners.Add(partner.contractorTitle + (string.IsNullOrWhiteSpace(partner.bestPhone) ? "" : " " + partner.bestPhone));
                                }
                            }
                            if (partners.Count > 0)
                            {
                                row = new TableRow();
                                row.Cells.Add(Globals.FormatedTableCell("<b>Partners: </b>" + string.Join(", ", partners.ToArray())));
                                table.Rows.Add(row);
                            }

                            row = new TableRow();
                            row.Cells.Add(Globals.FormatedTableCell("<b>Building Info: </b>" + conApp.general));
                            table.Rows.Add(row);

                            row = new TableRow();
                            row.Cells.Add(Globals.FormatedTableCell("<b>Instructions: </b>" + conApp.instructions));
                            table.Rows.Add(row);

                            row = new TableRow();
                            row.Cells.Add(Globals.FormatedTableCell("<b>Details: </b>" + conApp.details));
                            table.Rows.Add(row);

                            row = new TableRow();
                            string rowText = "";
                            rowText += @"<b>Account Status: </b>" + conApp.customerAccountStatus;
                            if (conApp.appType == 1)
                                rowText += @"<b style=""margin-left: 20px;"">Service Fee: </b>" + Globals.FormatMoney(conApp.customerServiceFee);
                            if (conApp.appType != 4)
                            {
                                if ((conApp.paymentType.ToLower().Contains("ch") || conApp.paymentType.ToLower().Contains("cash")) && !conApp.paymentType.ToLower().Contains("mail"))
                                    rowText += @"<b style=""font-size: 1.0em; margin-left: 20px;"">Payment Type: " + conApp.paymentType + @" (COLLECT " + Globals.FormatMoney(conApp.customerCollect) + @")</b>";
                                else
                                    rowText += @"<b style=""font-size: 1.0em; margin-left: 20px;"">Payment Type: " + conApp.paymentType + @"</b>";
                            }
                            if (conApp.keys && conApp.appType == 1) rowText += @"<b style=""font-size: 1.3em; border: 2px solid black; margin-left: 20px;"">TAKE KEYS</b>";

                            row.Cells.Add(Globals.FormatedTableCell(rowText));
                            table.Rows.Add(row);

                            row = new TableRow();
                            rowText = "";
                            if (conApp.bestPhone != null && conApp.bestPhone != "") rowText += @"<b>Phone:</b> " + Globals.FormatPhone(conApp.bestPhone);
                            if (conApp.alternatePhoneOne != null && conApp.alternatePhoneOne != "") rowText += @"<b style=""margin-left: 20px;"">Alternate:</b> " + Globals.FormatPhone(conApp.alternatePhoneOne);
                            if (conApp.alternatePhoneTwo != null && conApp.alternatePhoneTwo != "") rowText += @"<b style=""margin-left: 20px;"">Alternate:</b> " + Globals.FormatPhone(conApp.alternatePhoneTwo);
                            row.Cells.Add(Globals.FormatedTableCell(rowText));
                            table.Rows.Add(row);
                        }

                        TableFooterRow footerRow = new TableFooterRow();
                        footerRow.CssClass = "ProjectFooter";
                        footerRow.Cells.Add(Globals.FormatedTableCell(totalHours.ToString("N2") + " Total Hours"));
                        table.Rows.Add(footerRow);

                        PageDiv.Controls.Add(table);
                    }
                }

                if (table != null) table.Style["page-break-after"] = "Avoid";

            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "AppointmentsByContractorReport EX: " + ex.Message;
            }
        }
        #endregion

        #region SchedulingAppointmentsReport
        private void SchedulingAppointmentsReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                string orderBy = "A.startTime, A.endTime, CU.firstName, CU.lastName";
                if (Request["sort"] == "1") orderBy = "CO.firstName, CO.lastName, " + orderBy;
                SortedList<DateTime, List<AppStruct>> appList = new SortedList<DateTime, List<AppStruct>>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, orderBy, false))
                {
                    if (app.appStatus != 0) continue;
                    if ((Globals.IDToMask(app.appType) & contractorMask) == 0) continue;
                    if (!appList.ContainsKey(app.appointmentDate)) appList.Add(app.appointmentDate, new List<AppStruct>());
                    appList[app.appointmentDate].Add(app);
                }

                Table table = null;
                foreach (DateTime key in appList.Keys)
                {
                    decimal totalHours = 0;

                    table = new Table();
                    table.CssClass = "AppSchedule";

                    table.Caption = "Appointments For " + key.ToString("d");

                    TableHeaderRow headerRow = new TableHeaderRow();
                    string sortStartLink = "ViewReport.aspx?startDate=" + Request["startDate"] + "&endDate=" + Request["endDate"] + "&mask=" + Request["mask"] + "&report=" + Request["report"];
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + sortStartLink + @""">Start</a>", 150));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 150));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours", 100));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Status", 100));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 200));
                    string sortContractorLink = "ViewReport.aspx?startDate=" + Request["startDate"] + "&endDate=" + Request["endDate"] + "&mask=" + Request["mask"] + "&report=" + Request["report"] + "&sort=1";
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + sortContractorLink + @""">Contractor</a>", 170));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Time", 150));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("City", 150));
                    table.Rows.Add(headerRow);

                    foreach (AppStruct app in appList[key])
                    {
                        TimeSpan span = app.endTime - app.startTime;
                        decimal hours = (decimal)span.TotalMinutes / 60;
                        totalHours += hours;

                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.startTime.ToString("t") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.endTime.ToString("t") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(hours.ToString("N2")));
                        row.Cells.Add(Globals.FormatedTableCell(app.customerAccountStatus));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitleCustomNote + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.contractorTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(app.customerTimeOfDayPrefix + " " + app.customerTimeOfDay));
                        row.Cells.Add(Globals.FormatedTableCell(app.customerCity));
                        table.Rows.Add(row);

                        row = new TableRow();
                        row.Style["border-bottom"] = "2px solid #2B2B2B";
                        TableCell cell = new TableCell();
                        cell.ColumnSpan = 8;
                        cell.Text = app.customerSpecial;
                        cell.Style["text-align"] = "right";
                        row.Cells.Add(cell);
                        table.Rows.Add(row);
                    }

                    TableRow footerRow = new TableRow();
                    footerRow.Style["border-top"] = "3px solid #2B2B2B";
                    footerRow.Style["font-weight"] = "bold";
                    TableCell footerCell = new TableCell();
                    footerCell.ColumnSpan = 7;
                    footerCell.Text = "Total Hours: " + totalHours;
                    footerRow.Cells.Add(footerCell);
                    table.Rows.Add(footerRow);

                    PageDiv.Controls.Add(table);
                }

                if (table != null) table.Style["page-break-after"] = "Avoid";
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "SchedulingAppointmentsReport EX: " + ex.Message;
            }
        }
        #endregion

        #region ConfirmationAppointmentsReport
        private void ConfirmationAppointmentsReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Confirmation Appointments Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Start", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Type", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 350));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 170));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Payment Type", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("City", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Amount", 80));
                table.Rows.Add(headerRow);

                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", false))
                {
                    if (app.appStatus == 0 && (Globals.IDToMask(app.appType) & contractorMask) != 0)
                    {
                        string appTypeRowText = Globals.IndexToServiceType(app.appType);
                        if (app.appType == 4)
                        {
                            DBRow contract = Database.GetHomeGuardContract(app.customerID);
                            if (contract != null && !string.IsNullOrEmpty(contract.GetString("dateSigned")))
                                appTypeRowText = @"<b style=""color:green;"">" + appTypeRowText + @"</b>";
                            else
                                appTypeRowText = @"<b style=""color:red;"">" + appTypeRowText + @"</b>";
                        }

                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(app.startTime.ToString("t")));
                        row.Cells.Add(Globals.FormatedTableCell(app.endTime.ToString("t")));
                        row.Cells.Add(Globals.FormatedTableCell(appTypeRowText));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitleCustomNote + @"</a>" + (string.IsNullOrEmpty(app.customerPreferredContact) ? "" : "<b> (" + app.customerPreferredContact + ")</b>")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + app.contractorID + @""">" + app.contractorTitle + @"</a>"));
                        if (app.customerPaymentType.Contains("Credit Card") && Globals.CreditCardExpired(app.customerCardExpMonth, app.customerCardExpYear))
                            row.Cells.Add(Globals.FormatedTableCell(app.customerPaymentType + " <b>(EXP)</b>"));
                        else
                            row.Cells.Add(Globals.FormatedTableCell(app.customerPaymentType));
                        row.Cells.Add(Globals.FormatedTableCell(app.customerCity));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(Globals.CalculateAppointmentTotal(app))));
                        table.Rows.Add(row);

                        row = new TableRow();
                        row.Style["border-bottom"] = "1px solid #2B2B2B";

                        TableCell confirmCell = new TableCell();
                        confirmCell.ColumnSpan = 4;

                        Button verifyButton = new Button();
                        verifyButton.Enabled = !app.confirmed;
                        verifyButton.Text = "Verified";
                        verifyButton.Click += ConfirmButtonClick;
                        verifyButton.CommandArgument = app.appointmentID.ToString();
                        verifyButton.Attributes.Add("onclick", "JsSetScrollPos(this)");
                        confirmCell.Controls.Add(verifyButton);

                        Button leftMessageButton = new Button();
                        leftMessageButton.Enabled = !app.leftMessage;
                        leftMessageButton.Text = "Left Message";
                        leftMessageButton.Click += ConfirmButtonClick;
                        leftMessageButton.CommandArgument = app.appointmentID.ToString();
                        leftMessageButton.Attributes.Add("onclick", "JsSetScrollPos(this)");
                        confirmCell.Controls.Add(leftMessageButton);

                        Button confirmDayButton = new Button();
                        if (app.sentSMS) confirmDayButton.ForeColor = Color.Green;
                        if (app.sentSMS) confirmDayButton.Style["font-weight"] = "bold";
                        confirmDayButton.Enabled = app.customerPhoneOneCell || app.customerPhoneTwoCell || app.customerPhoneThreeCell || !string.IsNullOrWhiteSpace(app.customerEmail);
                        confirmDayButton.Text = "Text/Email Day";
                        confirmDayButton.Click += ConfirmButtonClick;
                        confirmDayButton.CommandArgument = app.appointmentID.ToString();
                        confirmDayButton.Attributes.Add("onclick", "JsSetScrollPos(this)");
                        confirmCell.Controls.Add(confirmDayButton);

                        Button confirmWeekButton = new Button();
                        if (app.sentWeekSMS) confirmWeekButton.ForeColor = Color.Green;
                        if (app.sentWeekSMS) confirmWeekButton.Style["font-weight"] = "bold";
                        confirmWeekButton.Enabled = app.customerPhoneOneCell || app.customerPhoneTwoCell || app.customerPhoneThreeCell || !string.IsNullOrWhiteSpace(app.customerEmail);
                        confirmWeekButton.Text = "Text/Email Week";
                        confirmWeekButton.Click += ConfirmButtonClick;
                        confirmWeekButton.CommandArgument = app.appointmentID.ToString();
                        confirmWeekButton.Attributes.Add("onclick", "JsSetScrollPos(this)");
                        confirmCell.Controls.Add(confirmWeekButton);

                        row.Cells.Add(confirmCell);

                        TableCell phoneCell = new TableCell();
                        phoneCell.ColumnSpan = 5;
                        phoneCell.Text = "<b>Best: </b>" + Globals.FormatPhone(app.customerPhoneOne);
                        if (app.customerPhoneTwo != null && app.customerPhoneTwo != "") phoneCell.Text += @"<b style=""margin-left: 20px;"">Alternate: </b>" + Globals.FormatPhone(app.customerPhoneTwo);
                        if (app.customerPhoneThree != null && app.customerPhoneThree != "") phoneCell.Text += @"<b style=""margin-left: 20px;"">Alternate: </b>" + Globals.FormatPhone(app.customerPhoneThree);
                        row.Cells.Add(phoneCell);

                        table.Rows.Add(row);
                    }
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "LoadAppointmentsReportPayroll EX: " + ex.Message;
            }
        }
        #endregion

        #region ContractorDirectoryReport
        private void ContractorDirectoryReport(int franchiseMask, int contractorMask)
        {
            try
            {
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Contractor Directory";

                Table table = new Table();
                table.CssClass = "Main";

                string url = Globals.BuildQueryString("ViewReport.aspx", "mask", Globals.SafeIntParse(Request["mask"]));
                url = Globals.BuildQueryString(url, "report", Globals.SafeIntParse(Request["report"]));

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + url + @""">Type</a>", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + url + @""">Name</a>", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Best Phone", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Alternate Phone", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Email", 250));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Background Check", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + Globals.BuildQueryString(url, "sortBy", Request["sortBy"] == "b" ? "br" : "b") + @""">Birthday</a>", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + Globals.BuildQueryString(url, "sortBy", Request["sortBy"] == "h" ? "hr" : "h") + @""">Days Hired</a>", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell(@"<a href=""" + Globals.BuildQueryString(url, "sortBy", Request["sortBy"] == "s" ? "sr" : "s") + @""">Score</a>", 100));
                table.Rows.Add(headerRow);

                int totalCount = 0;
                decimal totalScore = 0;

                string orderBy = "contractorType, businessName, firstName, lastName";
                if (Request["sortBy"] == "b") orderBy = "DATEPART(dayofyear, birthday) DESC";
                if (Request["sortBy"] == "h") orderBy = "hireDate";
                if (Request["sortBy"] == "s") orderBy = "score DESC";
                if (Request["sortBy"] == "br") orderBy = "DATEPART(dayofyear, birthday)";
                if (Request["sortBy"] == "hr") orderBy = "hireDate DESC";
                if (Request["sortBy"] == "sr") orderBy = "score";

                foreach (ContractorStruct contractor in Database.GetContractorList(franchiseMask, contractorMask, false, true, false, false, orderBy))
                {
                    List<string> typeList = new List<string>();
                    foreach (int index in Globals.BitMaskToIndexList(contractor.contractorType))
                        typeList.Add(Globals.IndexToServiceType(index));

                    TableRow row = new TableRow();
                    row.Style["border-top"] = "1px solid #2B2B2B";
                    row.Style["border-bottom"] = "1px solid #2B2B2B";
                    row.Cells.Add(Globals.FormatedTableCell(string.Join(", ", typeList)));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + contractor.title + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPhone(contractor.bestPhone)));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPhone(contractor.alternatePhone)));
                    row.Cells.Add(Globals.FormatedTableCell(contractor.email));
                    row.Cells.Add(Globals.FormatedTableCell(contractor.backgroundCheck.Date > new DateTime(1900, 1, 1) ? contractor.backgroundCheck.ToString("d") : ""));
                    row.Cells.Add(Globals.FormatedTableCell(contractor.birthday > new DateTime(1900, 1, 1) ? contractor.birthday.ToString("MMM d") : ""));
                    row.Cells.Add(Globals.FormatedTableCell((mst - contractor.hireDate).TotalDays.ToString("N0")));
                    row.Cells.Add(Globals.FormatedTableCell(contractor.score.ToString("N2")));
                    table.Rows.Add(row);
                    if (contractor.score > 0)
                    {
                        totalCount++;
                        totalScore += contractor.score;
                    }
                }

                if (totalCount > 0)
                {
                    TableRow totalRow = new TableRow();
                    TableCell totalCell = new TableCell();
                    totalCell.ColumnSpan = 7;
                    totalCell.Style["text-align"] = "center";
                    totalCell.Text = "<b>Average Score:</b> " + (totalScore / totalCount).ToString("N2");
                    totalRow.Cells.Add(totalCell);
                    table.Rows.Add(totalRow);
                }

                PageDiv.Controls.Add(table);

            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "ContractorDirectoryReport EX: " + ex.Message;
            }
        }
        #endregion

        #region OldPayrollReport
        /*private void OldPayrollReport(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                int userAccess = Globals.GetUserAccess(this);

                List<ContractorStruct> contractorList = Database.GetContractorList(franchiseMask, 0, false, false, false, false, "paymentType, lastName, firstName");

                Dictionary<int, List<AppStruct>> appDict = new Dictionary<int, List<AppStruct>>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", true))
                {
                    if (app.appStatus != 0 || app.appType != 0) continue;
                    if (!appDict.ContainsKey(app.contractorID)) appDict.Add(app.contractorID, new List<AppStruct>());
                    appDict[app.contractorID].Add(app);
                }

                Dictionary<int, FranchiseStruct> franDict = new Dictionary<int, FranchiseStruct>();
                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                    franDict.Add(fran.franchiseMask, fran);

                Table contractorTotalTable = new Table();
                contractorTotalTable.CssClass = "PayrollConTotal";
                contractorTotalTable.Caption = "Payroll Summary: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                TableHeaderRow conHeaderRow = new TableHeaderRow();
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 170));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Amount", 100));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Method", 170));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Pay Day", 170));
                contractorTotalTable.Rows.Add(conHeaderRow);

                Dictionary<KeyValuePair<int, DateTime>, bool> totalAppCount = new Dictionary<KeyValuePair<int,DateTime>,bool>();
                decimal totalMiles = 0;
                decimal totalContractorHours = 0;
                decimal totalCustomerHours = 0;
                decimal totalTips = 0;
                decimal totalServiceFee = 0;
                decimal totalScheduleFee = 0;
                decimal totalAdjustments = 0;
                decimal totalPayroll = 0;
                decimal totalLabor = 0;
                decimal totalRevenue = 0;
                decimal totalDiscounts = 0;

                foreach (ContractorStruct contractor in contractorList)
                {
                    if (userAccess < 7 && Globals.GetUserContractorID(this) != contractor.contractorID) continue;
                    if (!appDict.ContainsKey(contractor.contractorID)) continue;

                    bool didWork = false;
                    string lastAddr = "";
                    DateTime lastDate = DateTime.MinValue;
                    decimal subTotalHours = 0;
                    decimal subTotalMiles = 0;
                    decimal subTotalTips = 0;
                    decimal subTotalServiceFee = 0;
                    decimal subTotalScheduleFee = 0;
                    decimal subTotalAdjustments = 0;
                    decimal subTotalCommission = 0;
                    decimal scheduleFeeMax = 0;
                    List<string> adjustmentList = new List<string>();

                    Table appTable = new Table();
                    appTable.CssClass = "PayrollApp";
                    if (userAccess < 7) appTable.Caption = @"Payroll Report for: " + contractor.title + @"&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Period: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");
                    else appTable.Caption = @"Payroll Report for: <a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + contractor.title + @"</a>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Period: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                    TableHeaderRow appHeaderRow = new TableHeaderRow();
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 100));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Start", 100));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 100));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 250));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Miles", 100));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Tips", 100));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Service Fees", 120));
                    appHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours", 100));
                    appTable.Rows.Add(appHeaderRow);

                    foreach (AppStruct app in appDict[contractor.contractorID])
                    {
                        if (app.contractorID == contractor.contractorID)
                        {
                            didWork = true;

                            if (lastDate.Date != app.appointmentDate)
                            {
                                lastAddr = Globals.CleanAddr(contractor.address) + "," + Globals.CleanAddr(contractor.city) + "," + Globals.CleanAddr(contractor.state) + "," + Globals.CleanAddr(contractor.zip);
                                lastDate = app.appointmentDate;
                            }
                            string routeAddr = Globals.CleanAddr(app.customerAddress) + "," + Globals.CleanAddr(app.customerCity) + "," + Globals.CleanAddr(app.customerState) + "," + Globals.CleanAddr(app.customerZip);
                            decimal miles = GoogleMaps.GetDrivingRoute(lastAddr, routeAddr).distance;
                            lastAddr = routeAddr;

                            TableRow appRow = new TableRow();
                            if (userAccess < 7) appRow.Cells.Add(Globals.FormatedTableCell(app.appointmentDate.ToString("d")));
                            else appRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>"));
                            appRow.Cells.Add(Globals.FormatedTableCell(app.startTime.ToString("t")));
                            appRow.Cells.Add(Globals.FormatedTableCell(app.endTime.ToString("t")));
                            if (userAccess < 7) appRow.Cells.Add(Globals.FormatedTableCell(app.customerTitle));
                            else appRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>"));
                            appRow.Cells.Add(Globals.FormatedTableCell(miles.ToString("N2")));
                            appRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(app.contractorTips)));
                            appRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(app.customerServiceFee)));
                            string hours = app.contractorHours.ToString("N2");
                            if (app.contractorRate != contractor.hourlyRate) hours += " @ " + Globals.FormatMoney(app.contractorRate);
                            appRow.Cells.Add(Globals.FormatedTableCell(hours));
                            appTable.Rows.Add(appRow);

                            decimal feePercent = Globals.ParseScheduleFee(franDict[app.franchiseMask].scheduleFeeString, app.appointmentDate);
                            if (feePercent > scheduleFeeMax) scheduleFeeMax = feePercent;

                            subTotalHours += app.contractorHours;
                            subTotalMiles += miles;
                            subTotalTips += app.contractorTips;
                            subTotalServiceFee += app.customerServiceFee;
                            subTotalScheduleFee += (app.contractorRate * app.contractorHours) * (feePercent / 100.0m);
                            subTotalAdjustments += app.contractorAdjustAmount;
                            if (app.contractorAdjustAmount != 0)
                                adjustmentList.Add((app.contractorAdjustType ?? "Unknown") + " " + Globals.FormatMoney(app.contractorAdjustAmount));
                            subTotalCommission += app.contractorRate * app.contractorHours;

                            if (!totalAppCount.ContainsKey(new KeyValuePair<int, DateTime>(app.customerID, app.appointmentDate)))
                                totalAppCount.Add(new KeyValuePair<int, DateTime>(app.customerID, app.appointmentDate), true);

                            if (app.customerAccountStatus != "Ignored")
                            {
                                decimal revenue = app.customerRate * app.customerHours;
                                totalRevenue += revenue;
                                totalDiscounts += (revenue - (revenue * ((100 - (app.customerDiscountPercent + app.customerDiscountReferral)) / 100)));
                                totalDiscounts += app.customerDiscountAmount;
                                totalContractorHours += app.contractorHours;
                                totalCustomerHours += app.customerHours;
                                totalMiles += miles;
                            }
                        }
                    }

                    if (didWork)
                    {
                        Table appSummaryTable = new Table();
                        appSummaryTable.CssClass = "PayrollAppSummary";

                        TableRow appMilesRow = new TableRow();
                        appMilesRow.Cells.Add(Globals.FormatedTableCell("<b>Total Miles: </b>" + subTotalMiles.ToString("N2")));
                        appMilesRow.Cells[0].ColumnSpan = 2;
                        appMilesRow.Cells[0].Style["text-align"] = "left";
                        appSummaryTable.Rows.Add(appMilesRow);

                        TableRow appHoursRow = new TableRow();
                        appHoursRow.Cells.Add(Globals.FormatedTableCell("Total Hours:"));
                        appHoursRow.Cells.Add(Globals.FormatedTableCell(subTotalHours.ToString("N2")));
                        appHoursRow.Cells[0].Style["font-weight"] = "bold";
                        appHoursRow.Cells[1].Style["width"] = "150px";
                        appSummaryTable.Rows.Add(appHoursRow);

                        decimal adjustedHourly = subTotalHours == 0 ? 0 : (subTotalCommission + subTotalTips + subTotalServiceFee - subTotalScheduleFee) / subTotalHours;
                        TableRow appRateRow = new TableRow();
                        appRateRow.Cells.Add(Globals.FormatedTableCell("Hourly Rate:"));
                        appRateRow.Cells.Add(Globals.FormatedTableCell("(" + Globals.FormatMoney(adjustedHourly) + ")&nbsp&nbsp&nbsp" + Globals.FormatMoney(contractor.hourlyRate)));
                        appRateRow.Cells[0].Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appRateRow);

                        TableRow appCommissionRow = new TableRow();
                        appCommissionRow.Cells.Add(Globals.FormatedTableCell("Amount of Commission:"));
                        appCommissionRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(subTotalCommission)));
                        appCommissionRow.Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appCommissionRow);

                        TableRow appSchedulingFeeRow = new TableRow();
                        appSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell("Scheduling Fee (" + scheduleFeeMax.ToString("N0") + "%):"));
                        appSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell(" " + Globals.FormatMoney(subTotalScheduleFee)));
                        appSchedulingFeeRow.Cells[0].Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appSchedulingFeeRow);

                        TableRow appTipsRow = new TableRow();
                        appTipsRow.Cells.Add(Globals.FormatedTableCell("Tips:"));
                        appTipsRow.Cells.Add(Globals.FormatedTableCell(" " + Globals.FormatMoney(subTotalTips)));
                        appTipsRow.Cells[0].Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appTipsRow);

                        TableRow appServiceFeeRow = new TableRow();
                        appServiceFeeRow.Cells.Add(Globals.FormatedTableCell("Service Fees:"));
                        appServiceFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(subTotalServiceFee)));
                        appServiceFeeRow.Cells[0].Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appServiceFeeRow);

                        TableRow appOtherRow = new TableRow();
                        appOtherRow.Cells.Add(Globals.FormatedTableCell("Other Adjustments" + (adjustmentList.Count > 0 ? " (" + string.Join(", ", adjustmentList.ToArray()) + ")" : "") + ":"));
                        appOtherRow.Cells.Add(Globals.FormatedTableCell(" " + Globals.FormatMoney(subTotalAdjustments)));
                        appOtherRow.Cells[0].Style["font-weight"] = "bold";
                        appSummaryTable.Rows.Add(appOtherRow);

                        decimal appTotal = subTotalCommission + subTotalTips + subTotalServiceFee + subTotalAdjustments - subTotalScheduleFee;
                        TableRow appTotalRow = new TableRow();
                        appTotalRow.Cells.Add(Globals.FormatedTableCell("Total:"));
                        appTotalRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(appTotal)));
                        appTotalRow.Style["font-weight"] = "bold";
                        appTotalRow.Cells[1].Style["font-size"] = "1.2em";
                        appSummaryTable.Rows.Add(appTotalRow);

                        PageDiv.Controls.Add(appTable);
                        PageDiv.Controls.Add(appSummaryTable);

                        TableRow conRow = new TableRow();
                        conRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + contractor.title + @"</a>"));
                        conRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(appTotal)));
                        conRow.Cells.Add(Globals.FormatedTableCell(contractor.paymentType));
                        conRow.Cells.Add(Globals.FormatedTableCell(contractor.paymentDay));
                        contractorTotalTable.Rows.Add(conRow);

                        totalTips += subTotalTips;
                        totalServiceFee += subTotalServiceFee;
                        totalScheduleFee += subTotalScheduleFee;
                        totalAdjustments += subTotalAdjustments;
                        totalLabor += subTotalCommission;
                        totalPayroll += appTotal;
                    }
                }

                TableRow conFooterRow = new TableRow();
                TableCell conFooterCell = new TableCell();
                conFooterCell.ColumnSpan = 4;
                conFooterCell.Text = "Total: " + Globals.FormatMoney(totalPayroll);
                conFooterCell.Style["font-weight"] = "bold";
                conFooterRow.Cells.Add(conFooterCell);
                contractorTotalTable.Rows.Add(conFooterRow);

                if (userAccess >= 7) PageDiv.Controls.Add(contractorTotalTable);

                Table payrollTotalTable = new Table();
                payrollTotalTable.CssClass = "PayrollTotals";
                payrollTotalTable.Caption = "Payroll Totals: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                TableRow totalAppCountRow = new TableRow();
                totalAppCountRow.Cells.Add(Globals.FormatedTableCell("<b># of Appointments:</b>"));
                totalAppCountRow.Cells.Add(Globals.FormatedTableCell(totalAppCount.Keys.Count.ToString()));
                payrollTotalTable.Rows.Add(totalAppCountRow);

                TableRow totalMilesRow = new TableRow();
                totalMilesRow.Cells.Add(Globals.FormatedTableCell("<b>Miles:</b>"));
                totalMilesRow.Cells.Add(Globals.FormatedTableCell(totalMiles.ToString("N2"))); 
                payrollTotalTable.Rows.Add(totalMilesRow);

                TableRow totalBilledHoursRow = new TableRow();
                totalBilledHoursRow.Cells.Add(Globals.FormatedTableCell("<b>Billed Hours:</b>"));
                totalBilledHoursRow.Cells.Add(Globals.FormatedTableCell(totalCustomerHours.ToString("N2")));
                payrollTotalTable.Rows.Add(totalBilledHoursRow);

                TableRow totalContractorHoursRow = new TableRow();
                totalContractorHoursRow.Cells.Add(Globals.FormatedTableCell("<b>Contractor Hours:</b>"));
                totalContractorHoursRow.Cells.Add(Globals.FormatedTableCell(totalContractorHours.ToString("N2")));
                payrollTotalTable.Rows.Add(totalContractorHoursRow);

                TableRow totalRevenueRow = new TableRow();
                totalRevenueRow.Cells.Add(Globals.FormatedTableCell("<b>Revenue:</b>"));
                totalRevenueRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalRevenue)));
                payrollTotalTable.Rows.Add(totalRevenueRow);

                TableRow totalLaborRow = new TableRow();
                totalLaborRow.Cells.Add(Globals.FormatedTableCell("<b>Labor:</b>"));
                totalLaborRow.Cells.Add(Globals.FormatedTableCell((Globals.FormatMoney(totalLabor))));
                payrollTotalTable.Rows.Add(totalLaborRow);

                TableRow totalSchedulingFeeRow = new TableRow();
                totalSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Scheduling Fee:</b>"));
                totalSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalScheduleFee)));
                payrollTotalTable.Rows.Add(totalSchedulingFeeRow);

                TableRow totalServiceFeeRow = new TableRow();
                totalServiceFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Service Fees:</b>"));
                totalServiceFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalServiceFee)));
                payrollTotalTable.Rows.Add(totalServiceFeeRow);

                TableRow totalTipsRow = new TableRow();
                totalTipsRow.Cells.Add(Globals.FormatedTableCell("<b>Tips:</b>"));
                totalTipsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalTips)));
                payrollTotalTable.Rows.Add(totalTipsRow);

                TableRow totalDiscountsRow = new TableRow();
                totalDiscountsRow.Cells.Add(Globals.FormatedTableCell("<b>Discounts:</b>"));
                totalDiscountsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalDiscounts)));
                payrollTotalTable.Rows.Add(totalDiscountsRow);

                TableRow totalAdjustmentsRow = new TableRow();
                totalAdjustmentsRow.Cells.Add(Globals.FormatedTableCell("<b>Adjustments:</b>"));
                totalAdjustmentsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalAdjustments)));
                payrollTotalTable.Rows.Add(totalAdjustmentsRow);

                TableRow totalPayrollRow = new TableRow();
                totalPayrollRow.Cells.Add(Globals.FormatedTableCell("<b>Payroll:</b>"));
                totalPayrollRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalPayroll)));
                payrollTotalTable.Rows.Add(totalPayrollRow);

                decimal totalProfit = totalRevenue - (totalPayroll + totalDiscounts);

                TableRow totalProfitRow = new TableRow();
                totalProfitRow.Cells.Add(Globals.FormatedTableCell("<b>Profit:</b>"));
                totalProfitRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalProfit)));
                payrollTotalTable.Rows.Add(totalProfitRow);

                if (userAccess >= 7) PageDiv.Controls.Add(payrollTotalTable);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "AppointmentsReportPayroll EX: " + ex.Message;
            }
        }*/
        #endregion

        #region AccountingReport
        private void AccountingReport(int franchiseMask, string title, bool unpaid, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < (unpaid ? 5 : 7))
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = title;
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Start", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 350));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 170));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Payment Type", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Total", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Amount", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Status", 80));
                table.Rows.Add(headerRow);

                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", false))
                {
                    if (unpaid && app.paymentFinished) continue;
                    if (unpaid && app.customerPaymentType != "Check" && app.customerPaymentType != "Cash") continue;

                    TableRow row = new TableRow();
                    row.Style["border-bottom"] = "1px solid #2B2B2B";
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(app.startTime.ToString("t")));
                    row.Cells.Add(Globals.FormatedTableCell(app.endTime.ToString("t")));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + app.contractorID + @""">" + app.contractorTitle + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(app.customerPaymentType));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(Globals.CalculateAppointmentTotal(app) - app.customerServiceFee)));
                    row.Cells.Add(Globals.FormatedTableCell(app.amountPaid > 0 ? Globals.FormatMoney(app.amountPaid) : ""));
                    row.Cells.Add(Globals.FormatedTableCell(app.paymentFinished ? "Paid" : ""));
                    table.Rows.Add(row);
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "AccountingReport EX: " + ex.Message;
            }
        }
        #endregion

        #region UnavailabilityReport
        private void UnavailabilityReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Unavailability Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 300));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Created", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Requested", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Start", 120));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("End", 120));
                table.Rows.Add(headerRow);


                foreach (UnavailableStruct unavailable in Database.GetUnavailableByDateRange(franchiseMask, contractorMask, -1, startDate, endDate, "U.dateRequested, U.startTime, U.endTime"))
                {
                    if (unavailable.contractorActive && unavailable.contractorScheduled)
                    {
                        TableRow row = new TableRow();
                        row.Style["border-top"] = "1px solid #2B2B2B";
                        row.Style["border-bottom"] = "1px solid #2B2B2B";
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + unavailable.contractorID + @""">" + unavailable.contractorTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(unavailable.dateCreated.ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Unavailable.aspx?unID=" + unavailable.unavailableID + @"&conID=" + unavailable.contractorID + @""">" + unavailable.dateRequested.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(unavailable.startTime.ToString("t")));
                        row.Cells.Add(Globals.FormatedTableCell(unavailable.endTime.ToString("t")));
                        table.Rows.Add(row);
                    }
                }

                PageDiv.Controls.Add(table);

            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "UnavailabilityReport EX: " + ex.Message;
            }
        }
        #endregion

        #region KeysReport
        private void KeysReport(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Keys Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 350));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 170));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Returned", 100));
                table.Rows.Add(headerRow);

                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", false))
                {
                    if (app.customerKeys && app.appStatus == 0)
                    {
                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + app.contractorID + @""">" + app.contractorTitle + @"</a>"));

                        Button returnedButton = new Button();
                        returnedButton.Enabled = !app.keysReturned;
                        returnedButton.Text = "Returned";
                        returnedButton.Click += ReturnedkeysClicked;
                        returnedButton.CommandArgument = app.appointmentID.ToString();
                        returnedButton.Attributes.Add("onclick", "JsSetScrollPos(this)");

                        TableCell returnedCell = new TableCell();
                        returnedCell.Controls.Add(returnedButton);
                        row.Cells.Add(returnedCell);

                        table.Rows.Add(row);
                    }
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "KeysReport EX: " + ex.Message;
            }
        }
        #endregion

        #region NewFollowUpCustomersReport
        private void NewFollowUpCustomersReport(int franchiseMask, int contractorMask)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

                List<CustomerStruct> customerList = new List<CustomerStruct>();

                Dictionary<int, AppStruct> lastAppDict = new Dictionary<int, AppStruct>();
                Dictionary<int, bool> lastAppFollowUpDict = new Dictionary<int, bool>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, mst.AddDays(-60), mst, "A.appointmentDate DESC, A.startTime, A.endTime", false))
                {
                    if (app.appStatus != 0) continue;
                    if ((Globals.IDToMask(app.appType) & contractorMask) == 0) continue;
                    if (!lastAppDict.ContainsKey(app.customerID)) lastAppDict.Add(app.customerID, app);
                    if (app.followUpSent && !lastAppFollowUpDict.ContainsKey(app.customerID)) lastAppFollowUpDict.Add(app.customerID, true);
                }

                foreach (CustomerStruct customer in Database.GetCustomers(franchiseMask, "(accountStatus = 'New' OR accountStatus = 'Follow Up' OR accountStatus = 'As Needed' OR accountStatus = 'Active')", "accountStatus, firstName, lastName, businessName"))
                {
                    CustomerStruct c = customer;
                    if (lastAppDict.ContainsKey(c.customerID))
                    {
                        c.bookedBy = lastAppDict[c.customerID].appointmentID;
                        c.bookedDate = lastAppDict[c.customerID].appointmentDate;
                    }
                    else c.bookedDate = DateTime.MinValue;

                    if (c.accountStatus == "New" || c.accountStatus == "Follow Up") customerList.Add(c);
                    else if (!lastAppFollowUpDict.ContainsKey(c.customerID)) customerList.Add(c);
                }
                customerList.Sort(delegate(CustomerStruct c1, CustomerStruct c2) { return c1.bookedDate.CompareTo(c2.bookedDate); });

                ReportTitleDiv.InnerHtml = "New/Follow-up Customers Report";

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Status", 50));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Last App", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("City", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Best Phone", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Alt Phone", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Alt Phone", 80));
                table.Rows.Add(headerRow);

                foreach (CustomerStruct customer in customerList)
                {
                    if (customer.bookedDate != DateTime.MinValue && customer.bookedDate >= mst.AddDays(-14))
                    {
                        TableRow row = new TableRow();
                        row.Style["border-top"] = "1px solid #2B2B2B";
                        row.Style["border-bottom"] = "1px solid #2B2B2B";

                        row.Cells.Add(Globals.FormatedTableCell(customer.accountStatus));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""FollowUp.aspx?appID=" + customer.bookedBy + @""">" + customer.bookedDate.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(customer.locationCity));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPhone(customer.bestPhone)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPhone(customer.alternatePhoneOne)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPhone(customer.alternatePhoneTwo)));
                        table.Rows.Add(row);
                    }
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "NewCustomersReport EX: " + ex.Message;
            }
        }
        #endregion

        #region WebQuotesReport
        private void WebQuotesReport(int franchiseMask)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Web Quotes Report";

                Table table = new Table();
                table.CssClass = "Main";
                table.Style["width"] = "100%";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Created", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("City", 50));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Replied", 50));
                table.Rows.Add(headerRow);

                foreach (CustomerStruct customer in Database.GetCustomers(franchiseMask, "accountStatus = 'Web Quote' AND bookedDate > DATEADD(day, -7, GETUTCDATE())", "quoteReply, bookedDate DESC"))
                {
                    TableRow row = new TableRow();
                    row.Style["border-top"] = "1px solid #2B2B2B";
                    row.Style["border-bottom"] = "1px solid #2B2B2B";

                    row.Cells.Add(Globals.FormatedTableCell(customer.bookedDate.ToString("d")));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""WebQuoteReply.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(customer.locationCity));
                    row.Cells.Add(Globals.FormatedTableCell(customer.quoteReply ? "Yes" : "No"));
                    table.Rows.Add(row);
                }

                PageDiv.Controls.Add(table);

            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "WebQuotesReport EX: " + ex.Message;
            }
        }
        #endregion

        #region SalesReport
        private void SalesReport(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Sales Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Appointment", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Created", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Type", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Service Fee", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Disc %", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Disc $", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Tax $", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Tips", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Total", 80));
                table.Rows.Add(headerRow);

                decimal totalServiceFee = 0;
                decimal totalHours = 0;
                decimal totalDiscount = 0;
                decimal totalTips = 0;
                decimal total = 0;
                decimal totalSalesTax = 0;

                TableRow row;
                foreach (TransactionStruct i in Database.GetTransactions(franchiseMask, 0, startDate, endDate, "T.dateApply, C.firstName, C.lastName, C.businessName, T.dateCreated"))
                {

                    if (i.customerAccountStatus == "Ignored") continue;

                    TransactionStruct trans = i;
                    if (!trans.isVoid && !trans.IsAuth())
                    {
                        switch (trans.transType)
                        {
                            case "Invoice":
                                trans.total = 0;
                                break;
                            case "Sale":
                                trans.transType += " (" + trans.paymentType + ")";
                                break;
                            case "Return":
                                trans.transType += " (" + trans.paymentType + ")";
                                trans.serviceFee *= -1;
                                trans.hoursBilled *= -1;
                                trans.discountAmount *= -1;
                                trans.tips *= -1;
                                trans.total *= -1;
                                break;
                        }

                        decimal salesTax = (trans.salesTax / 100M) * (trans.total - trans.tips);

                        row = new TableRow();
                        row.Style["border-top"] = "1px solid #2B2B2B";
                        row.Style["border-bottom"] = "1px solid #2B2B2B";
                        row.Cells.Add(Globals.FormatedTableCell(trans.dateApply.ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + trans.customerID + @""">" + trans.customerTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(trans.dateCreated.ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(trans.transType));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(trans.serviceFee)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(trans.hoursBilled) + " (" + Globals.FormatMoney(trans.hourlyRate) + ")"));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatPercent(trans.discountPercent)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(trans.discountAmount)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(salesTax)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(trans.tips)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(trans.total)));
                        table.Rows.Add(row);

                        totalServiceFee += trans.serviceFee;
                        totalHours += trans.hoursBilled;
                        totalDiscount += trans.discountAmount;
                        totalSalesTax += salesTax;
                        totalTips += trans.tips;
                        total += trans.total;
                    }
                }

                row = new TableRow();
                row.Style["border-top"] = "2px solid #2B2B2B";
                row.Style["font-weight"] = "Bold";
                row.Cells.Add(Globals.FormatedTableCell(""));
                row.Cells.Add(Globals.FormatedTableCell(""));
                row.Cells.Add(Globals.FormatedTableCell(""));
                row.Cells.Add(Globals.FormatedTableCell("Total"));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalServiceFee)));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(totalHours)));
                row.Cells.Add(Globals.FormatedTableCell(""));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalDiscount)));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalSalesTax)));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalTips)));
                row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(total)));
                table.Rows.Add(row);

                PageDiv.Controls.Add(table);

            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "SalesReport EX: " + ex.Message;
            }
        }
        #endregion

        #region GiftCardReport
        private void GiftCardReport(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Gift Cards Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Recipient", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Redeemed", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Code", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Billing Email", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Recipient Email", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Payment", 80));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Amount", 80));

                table.Rows.Add(headerRow);

                foreach (GiftCardStruct giftCard in Database.GetGiftCardsByDateRange(franchiseMask, startDate, endDate))
                {
                    TableRow row = new TableRow();
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""GiftCards.aspx?giftCardID=" + giftCard.giftCardID + @""">" + Globals.UtcToMst(giftCard.dateCreated).ToString("d") + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + giftCard.customerID + @""">" + giftCard.customerTitle + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(giftCard.recipientName));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + giftCard.redeemed + @""">" + giftCard.redeemedTitle + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.EncodeZBase32((uint)giftCard.giftCardID)));
                    row.Cells.Add(Globals.FormatedTableCell(giftCard.billingEmail));
                    row.Cells.Add(Globals.FormatedTableCell(giftCard.recipientEmail));
                    row.Cells.Add(Globals.FormatedTableCell(giftCard.paymentType + (Globals.IsPaymentCreditCard(giftCard.paymentType) ? " **** " + giftCard.lastFourCard : "")));
                    row.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(giftCard.amount)));
                    table.Rows.Add(row);
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "GiftCardReport EX: " + ex.Message;
            }
        }
        #endregion

        #region AppointmentRequestReport
        private void AppointmentRequestReport(int franchiseMask)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Appointment Request Report";

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date Created", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date Requested", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Preferred Time", 150));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Notes", 300));
                table.Rows.Add(headerRow);

                foreach (DBRow serviceRequest in Database.GetServiceRequestsByDateRange(franchiseMask, new DateTime(1900, 1, 1), new DateTime(9999, 1, 1)))
                {
                    TableCell actionCell = new TableCell();

                    Button deleteButton = new Button();
                    deleteButton.Text = "Remove";
                    deleteButton.Click += DeleteAppointmentRequestClick;
                    deleteButton.CommandArgument = serviceRequest.GetInt("serviceRequestID").ToString();
                    deleteButton.Attributes.Add("onclick", "JsSetScrollPos(this)");
                    actionCell.Controls.Add(deleteButton);

                    TableRow row = new TableRow();
                    row.Cells.Add(actionCell);
                    row.Cells.Add(Globals.FormatedTableCell(Globals.UtcToMst(serviceRequest.GetDate("dateCreated")).ToString("d")));
                    row.Cells.Add(Globals.FormatedTableCell(serviceRequest.GetDate("requestDate").ToString("ddd M/d/yyyy")));
                    row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + serviceRequest.GetInt("customerID") + @""">" + Globals.FormatCustomerTitle(serviceRequest) + @"</a>"));
                    row.Cells.Add(Globals.FormatedTableCell(serviceRequest.GetString("timePrefix") + " " + serviceRequest.GetString("timeSuffix")));
                    row.Cells.Add(Globals.FormatedTableCell(serviceRequest.GetString("notes")));
                    table.Rows.Add(row);
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "AppointmentRequestReport EX: " + ex.Message;
            }
        }
        #endregion

        #region ApplicantReport
        private void ApplicantReport(int franchiseMask)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Applicant Report";

                Table table = new Table();
                table.CssClass = "Main";

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Applied", 100));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Name", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Application", 200));
                headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Resume", 400));

                table.Rows.Add(headerRow);

                foreach (ContractorStruct contractor in Database.GetContractorList(franchiseMask, -1, false, false, false, true, "hireDate"))
                {
                    if (contractor.applicant)
                    {
                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(Globals.UtcToMst(contractor.dateCreated).ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + contractor.firstName + " " + contractor.lastName + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""ApplicationPrint.aspx?conID=" + contractor.contractorID + @""">View Application</a>"));

                        List<string> files = new List<string>();
                        string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), "Resume");
                        foreach (string filePath in Directory.GetFiles(path))
                        {
                            string fileName = new FileInfo(filePath).Name;
                            if (filePath.Contains(contractor.contractorID.ToString()))
                                files.Add(@"<a href=""" + Globals.baseUrl + @"Resume/" + fileName + @""" target=""_blank"" download>" + fileName + "</a>");
                        }
                        row.Cells.Add(Globals.FormatedTableCell(string.Join(", ", files)));
                        table.Rows.Add(row);
                    }
                }

                PageDiv.Controls.Add(table);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "ApplicantReport EX: " + ex.Message;
            }
        }
        #endregion

        #region ReviewScoresReport
        private void ReviewScoresReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                int userAcess = Globals.GetUserAccess(this);
                int userContractorID = Globals.GetUserContractorID(this);

                if (userAcess < 2) Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Review Scores Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                int totalSchedulingSatisfaction = 0;
                int totalTimeManagement = 0;
                int totalProfessionalism = 0;
                int totalCleaningQuality = 0;
                int totalCount = 0;

                List<DBRow> rows = Database.GetFollowUpScoresByDateRange(franchiseMask, contractorMask, startDate, endDate);

                int customerID = Globals.SafeIntParse(Request["custID"]);
                if (customerID > 0)
                {
                    for (int i = rows.Count - 1; i >= 0; i--)
                    {
                        if (rows[i].GetInt("customerID") != customerID)
                        {
                            rows.RemoveAt(i);
                        }
                    }
                }

                for (int i = 0; i < rows.Count; )
                {
                    int contractorID = rows[i].GetInt("contractorID");

                    if (userAcess == 2 && contractorID != userContractorID)
                    {
                        i++;
                        continue;
                    }

                    int conSchedulingSatisfaction = 0;
                    int conTimeManagement = 0;
                    int conProfessionalism = 0;
                    int conCleaningQuality = 0;
                    int conCount = 0;

                    Table conTable = new Table();
                    conTable.CssClass = "ContractorScore";
                    conTable.Caption = Globals.FormatFullName(rows[i].GetString("coFirstName"), rows[i].GetString("coLastName"), "Unknown Contractor");

                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Date", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Scheduling", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Time Management", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Professionalism", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Quality Of Work", 0));
                    conTable.Rows.Add(headerRow);

                    for (; i < rows.Count && rows[i].GetInt("contractorID") == contractorID; i++)
                    {
                        int schedulingSatisfaction = rows[i].GetInt("schedulingSatisfaction");
                        int timeManagement = rows[i].GetInt("timeManagement");
                        int professionalism = rows[i].GetInt("professionalism");
                        int cleaningQuality = rows[i].GetInt("cleaningQuality");

                        totalSchedulingSatisfaction += schedulingSatisfaction;
                        totalTimeManagement += timeManagement;
                        totalProfessionalism += professionalism;
                        totalCleaningQuality += cleaningQuality;
                        totalCount++;

                        conSchedulingSatisfaction += schedulingSatisfaction;
                        conTimeManagement += timeManagement;
                        conProfessionalism += professionalism;
                        conCleaningQuality += cleaningQuality;
                        conCount++;

                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(rows[i].GetDate("appointmentDate").ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + rows[i].GetInt("customerID") + @""">" + Globals.FormatFullName(rows[i].GetString("cuFirstName"), rows[i].GetString("cuLastName"), "Unknown") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(schedulingSatisfaction + " Stars"));
                        row.Cells.Add(Globals.FormatedTableCell(timeManagement + " Stars"));
                        row.Cells.Add(Globals.FormatedTableCell(professionalism + " Stars"));
                        row.Cells.Add(Globals.FormatedTableCell(cleaningQuality + " Stars"));
                        conTable.Rows.Add(row);

                        TableRow notesRow = new TableRow();
                        TableCell notesCell = new TableCell();
                        notesCell.ColumnSpan = 6;
                        notesCell.Text = "<b>Comments: </b>" + rows[i].GetString("notes");
                        notesCell.Style["text-align"] = "left";
                        notesRow.Cells.Add(notesCell);
                        conTable.Rows.Add(notesRow);
                    }

                    TableRow avgRow = new TableRow();
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>Average</b>"));
                    avgRow.Cells[avgRow.Cells.Count - 1].ColumnSpan = 2;
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + ((decimal)conSchedulingSatisfaction / (decimal)conCount).ToString("N2") + " Stars</b>"));
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + ((decimal)conTimeManagement / (decimal)conCount).ToString("N2") + " Stars</b>"));
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + ((decimal)conProfessionalism / (decimal)conCount).ToString("N2") + " Stars</b>"));
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + ((decimal)conCleaningQuality / (decimal)conCount).ToString("N2") + " Stars</b>"));
                    conTable.Rows.Add(avgRow);

                    decimal contractorScore = ((decimal)(conTimeManagement + conProfessionalism + conCleaningQuality)) / (conCount * 3);
                    TableRow footerRow = new TableRow();
                    TableCell footerCell = new TableCell();
                    footerCell.ColumnSpan = 6;
                    footerCell.Style["border"] = "none";
                    footerCell.Style["text-align"] = "center";
                    footerCell.Text = "<b>Contractor Score: </b>" + contractorScore.ToString("N2");
                    footerRow.Cells.Add(footerCell);
                    conTable.Rows.Add(footerRow);

                    PageDiv.Controls.Add(conTable);
                }

                Table totalTable = new Table();
                totalTable.CssClass = "ReviewScoresTotal";
                totalTable.Caption = "Follow Up Totals: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                if (totalCount > 0)
                {
                    decimal avgSchedulingSatisfaction = ((decimal)totalSchedulingSatisfaction) / totalCount;
                    TableRow totalRow = new TableRow();
                    totalRow.Cells.Add(Globals.FormatedTableCell("<b>Scheduling Satisfaction:</b>"));
                    totalRow.Cells.Add(Globals.FormatedTableCell(avgSchedulingSatisfaction.ToString("N2")));
                    totalTable.Rows.Add(totalRow);

                    decimal avgTimeManagement = ((decimal)totalTimeManagement) / totalCount;
                    totalRow = new TableRow();
                    totalRow.Cells.Add(Globals.FormatedTableCell("<b>Time Management:</b>"));
                    totalRow.Cells.Add(Globals.FormatedTableCell(avgTimeManagement.ToString("N2")));
                    totalTable.Rows.Add(totalRow);

                    decimal avgProfessionalism = ((decimal)totalProfessionalism) / totalCount;
                    totalRow = new TableRow();
                    totalRow.Cells.Add(Globals.FormatedTableCell("<b>Professionalism:</b>"));
                    totalRow.Cells.Add(Globals.FormatedTableCell(avgProfessionalism.ToString("N2")));
                    totalTable.Rows.Add(totalRow);

                    decimal avgCleaningQuality = ((decimal)totalCleaningQuality) / totalCount;
                    totalRow = new TableRow();
                    totalRow.Cells.Add(Globals.FormatedTableCell("<b>Quality of Work:</b>"));
                    totalRow.Cells.Add(Globals.FormatedTableCell(avgCleaningQuality.ToString("N2")));
                    totalTable.Rows.Add(totalRow);

                    decimal avgContractorScore = ((decimal)(totalTimeManagement + totalProfessionalism + totalCleaningQuality)) / (totalCount * 3);
                    totalRow = new TableRow();
                    totalRow.Cells.Add(Globals.FormatedTableCell("<b>Average Contractor Score:</b>"));
                    totalRow.Cells.Add(Globals.FormatedTableCell(avgContractorScore.ToString("N2")));
                    totalTable.Rows.Add(totalRow);
                }

                if (userAcess > 2) PageDiv.Controls.Add(totalTable);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "ReviewScoresReport EX: " + ex.Message;
            }
        }
        #endregion

        #region HoursBookedReport
        private void HoursBookedReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 7) Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Hours Booked Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                List<AppStruct> appList = Database.GetAppsByBooking(franchiseMask, startDate, endDate);
                for (int i = 0; i < appList.Count; )
                { 
                    if ((Globals.IDToMask(appList[i].appType) & contractorMask) == 0) continue;

                    decimal hoursBooked = 0;
                    DateTime bookedDate = Globals.UtcToMst(appList[i].dateCreated).Date;

                    Table bookedTable = new Table();
                    bookedTable.CssClass = "ContractorScore";
                    bookedTable.Caption = "Booking Date: " + bookedDate.ToString("D");

                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("User", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Timestamp", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Appointment", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Status", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours Booked", 0));
                    bookedTable.Rows.Add(headerRow);

                    string currentUser = null;
                    decimal currentUserTotal = 0;

                    for (; i < appList.Count && Globals.UtcToMst(appList[i].dateCreated).Date == bookedDate.Date; i++)
                    {
                        if (currentUser != appList[i].username && currentUser != null)
                        {
                            TableRow subTotalRow = new TableRow();
                            subTotalRow.Cells.Add(Globals.FormatedTableCell("<b>" + currentUser + "</b>"));
                            subTotalRow.Cells[subTotalRow.Cells.Count - 1].ColumnSpan = 5;
                            subTotalRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(currentUserTotal)));
                            bookedTable.Rows.Add(subTotalRow);
                            currentUserTotal = 0;
                        }

                        currentUser = appList[i].username;
                        currentUserTotal += appList[i].customerHours;

                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(appList[i].username));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.UtcToMst(appList[i].dateCreated).ToString("t")));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + appList[i].appointmentID + @""">" + appList[i].appointmentDate.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + appList[i].customerID + @""">" + appList[i].customerTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatAppStatus(appList[i].appStatus)));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(appList[i].customerHours)));
                        bookedTable.Rows.Add(row);

                        hoursBooked += appList[i].customerHours;
                    }

                    if (currentUser != null)
                    {
                        TableRow subTotalLastRow = new TableRow();
                        subTotalLastRow.Cells.Add(Globals.FormatedTableCell("<b>" + currentUser + "</b>"));
                        subTotalLastRow.Cells[subTotalLastRow.Cells.Count - 1].ColumnSpan = 5;
                        subTotalLastRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(currentUserTotal)));
                        bookedTable.Rows.Add(subTotalLastRow);
                    }

                    TableRow avgRow = new TableRow();
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>Total Hours Booked:</b>"));
                    avgRow.Cells[avgRow.Cells.Count - 1].ColumnSpan = 5;
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + Globals.FormatHours(hoursBooked) + "</b>"));
                    bookedTable.Rows.Add(avgRow);

                    PageDiv.Controls.Add(bookedTable);
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "HoursBookedReport EX: " + ex.Message;
            }
        }
        #endregion
        
        #region HoursEarnedReport
        private void HoursEarnedReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);

                ReportTitleDiv.InnerHtml = "Hours Earned Report";
                DateRangeDiv.InnerHtml = "<b>Date Range: </b>" + (Request["startDate"] ?? "") + "<b> To: </b>" + (Request["endDate"] ?? "");

                AppStruct indexApp = new AppStruct();
                SortedList<string, List<AppStruct>> userAppList = new SortedList<string, List<AppStruct>>();

                foreach (AppStruct app in Database.GetAppsByEarnedHours(franchiseMask, startDate, endDate))
                {
                    if ((Globals.IDToMask(app.appType) & contractorMask) == 0) continue;

                    if (app.appointmentDate != indexApp.appointmentDate || app.customerID != indexApp.customerID || !indexApp.usernameBooked)
                    {
                        if (indexApp.usernameBooked)
                        {
                            if (!userAppList.ContainsKey(indexApp.username)) userAppList.Add(indexApp.username, new List<AppStruct>());
                            userAppList[indexApp.username].Add(indexApp);
                        }

                        indexApp = app;
                    }
                    else
                    {
                        indexApp.customerHours += app.customerHours;
                    }
                }

                if (indexApp.usernameBooked)
                {
                    if (!userAppList.ContainsKey(indexApp.username)) userAppList.Add(indexApp.username, new List<AppStruct>());
                    userAppList[indexApp.username].Add(indexApp);
                }

                foreach (string username in userAppList.Keys)
                {
                    Table bookedTable = new Table();
                    bookedTable.CssClass = "ContractorScore";
                    bookedTable.Caption = "User: " + username;

                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Booked", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Appointment", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Customer", 0));
                    headerRow.Cells.Add(Globals.FormatedTableHeaderCell("Hours Booked", 0));
                    bookedTable.Rows.Add(headerRow);

                    decimal hoursBooked = 0;

                    foreach (AppStruct app in userAppList[username])
                    {
                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(Globals.UtcToMst(app.dateCreated).ToString()));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>"));
                        row.Cells.Add(Globals.FormatedTableCell(Globals.FormatHours(app.customerHours)));
                        bookedTable.Rows.Add(row);

                        hoursBooked += app.customerHours;
                    }

                    TableRow avgRow = new TableRow();
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>Total Hours Booked:</b>"));
                    avgRow.Cells[avgRow.Cells.Count - 1].ColumnSpan = 3;
                    avgRow.Cells.Add(Globals.FormatedTableCell("<b>" + Globals.FormatHours(hoursBooked) + "</b>"));
                    bookedTable.Rows.Add(avgRow);

                    PageDiv.Controls.Add(bookedTable);
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "HoursBookedReport EX: " + ex.Message;
            }
        }
        #endregion

        #region PayrollReport
        private void PayrollReport(int franchiseMask, int contractorType, DateTime startDate, DateTime endDate)
        {
            try
            {
                int userAccess = Globals.GetUserAccess(this);
                int contractorID = Globals.GetUserContractorID(this);

                List<ContractorStruct> contractorList = new List<ContractorStruct>();
                if (userAccess >= 7) contractorList = Database.GetContractorList(franchiseMask, contractorType, false, false, false, false, "paymentType, lastName, firstName");
                else contractorList.Add(Database.GetContractorByID(franchiseMask, contractorID));

                List<PayrollDoc> payrollList = new List<PayrollDoc>();
                string error = PayrollDoc.GetPayroll(userAccess, franchiseMask, contractorType, contractorList, startDate, endDate, false, out payrollList);

                Table contractorTotalTable = new Table();
                contractorTotalTable.CssClass = "PayrollConTotal";
                contractorTotalTable.Caption = "Payroll Summary: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                TableHeaderRow conHeaderRow = new TableHeaderRow();
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Contractor", 170));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Service Fee", 100));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Amount", 100));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Method", 170));
                conHeaderRow.Cells.Add(Globals.FormatedTableHeaderCell("Pay Day", 170));
                contractorTotalTable.Rows.Add(conHeaderRow);

                foreach (PayrollDoc payrollDoc in payrollList)
                {
                    HtmlGenericControl contractorDiv = new HtmlGenericControl("DIV");
                    contractorDiv.InnerHtml = payrollDoc.html;

                    PageDiv.Controls.Add(contractorDiv);

                    TableRow conRow = new TableRow();
                    conRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Contractors.aspx?conID=" + payrollDoc.contractor.contractorID + @""">" + payrollDoc.contractor.title + @"</a>"));
                    conRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(payrollDoc.serviceFeeTotal)));
                    conRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(payrollDoc.appTotal)));
                    conRow.Cells.Add(Globals.FormatedTableCell(payrollDoc.contractor.paymentType));
                    conRow.Cells.Add(Globals.FormatedTableCell(payrollDoc.contractor.paymentDay));
                    contractorTotalTable.Rows.Add(conRow);
                }

                TableRow conFooterRow = new TableRow();
                TableCell conFooterCell = new TableCell();
                conFooterCell.ColumnSpan = 5;
                conFooterCell.Text = "Total: " + Globals.FormatMoney(PayrollDoc.totalPayroll);
                conFooterCell.Style["font-weight"] = "bold";
                conFooterRow.Cells.Add(conFooterCell);
                contractorTotalTable.Rows.Add(conFooterRow);

                if (userAccess >= 7) PageDiv.Controls.Add(contractorTotalTable);

                Table payrollTotalTable = new Table();
                payrollTotalTable.CssClass = "PayrollTotals";
                payrollTotalTable.Caption = "Payroll Totals: " + (Request["startDate"] ?? "") + " - " + (Request["endDate"] ?? "");

                TableRow totalAppCountRow = new TableRow();
                totalAppCountRow.Cells.Add(Globals.FormatedTableCell("<b># of Appointments:</b>"));
                totalAppCountRow.Cells.Add(Globals.FormatedTableCell(PayrollDoc.totalAppCount.Keys.Count.ToString()));
                payrollTotalTable.Rows.Add(totalAppCountRow);

                TableRow totalMilesRow = new TableRow();
                totalMilesRow.Cells.Add(Globals.FormatedTableCell("<b>Miles:</b>"));
                totalMilesRow.Cells.Add(Globals.FormatedTableCell(PayrollDoc.totalMiles.ToString("N2")));
                payrollTotalTable.Rows.Add(totalMilesRow);

                TableRow totalBilledHoursRow = new TableRow();
                totalBilledHoursRow.Cells.Add(Globals.FormatedTableCell("<b>Billed Hours:</b>"));
                totalBilledHoursRow.Cells.Add(Globals.FormatedTableCell(PayrollDoc.totalCustomerHours.ToString("N2")));
                payrollTotalTable.Rows.Add(totalBilledHoursRow);

                TableRow totalContractorHoursRow = new TableRow();
                totalContractorHoursRow.Cells.Add(Globals.FormatedTableCell("<b>Contractor Hours:</b>"));
                totalContractorHoursRow.Cells.Add(Globals.FormatedTableCell(PayrollDoc.totalContractorHours.ToString("N2")));
                payrollTotalTable.Rows.Add(totalContractorHoursRow);

                TableRow totalRevenueRow = new TableRow();
                totalRevenueRow.Cells.Add(Globals.FormatedTableCell("<b>Revenue:</b>"));
                totalRevenueRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalRevenue)));
                payrollTotalTable.Rows.Add(totalRevenueRow);

                TableRow totalLaborRow = new TableRow();
                totalLaborRow.Cells.Add(Globals.FormatedTableCell("<b>Labor:</b>"));
                totalLaborRow.Cells.Add(Globals.FormatedTableCell((Globals.FormatMoney(PayrollDoc.totalLabor))));
                payrollTotalTable.Rows.Add(totalLaborRow);

                TableRow totalSchedulingFeeRow = new TableRow();
                totalSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Scheduling Fee:</b>"));
                totalSchedulingFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalScheduleFee)));
                payrollTotalTable.Rows.Add(totalSchedulingFeeRow);

                if (PayrollDoc.totalSuppliesFee > 0 || PayrollDoc.totalCarFee > 0)
                {
                    TableRow totalSuppliesFeeNWRow = new TableRow();
                    totalSuppliesFeeNWRow.Cells.Add(Globals.FormatedTableCell("<b>Equipment/Supplies Reimbursed Exp (NW):</b>"));
                    totalSuppliesFeeNWRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalSuppliesFeeNW)));
                    payrollTotalTable.Rows.Add(totalSuppliesFeeNWRow);

                    TableRow totalSuppliesFeeRow = new TableRow();
                    totalSuppliesFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Equipment/Supplies Reimbursed Exp (W):</b>"));
                    totalSuppliesFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalSuppliesFee - PayrollDoc.totalSuppliesFeeNW)));
                    payrollTotalTable.Rows.Add(totalSuppliesFeeRow);

                    TableRow totalCarFeeNWRow = new TableRow();
                    totalCarFeeNWRow.Cells.Add(Globals.FormatedTableCell("<b>Fuel/Car Maintenance Reimbursed Exp (NW):</ b>"));
                    totalCarFeeNWRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalCarFeeNW)));
                    payrollTotalTable.Rows.Add(totalCarFeeNWRow);

                    TableRow totalCarFeeRow = new TableRow();
                    totalCarFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Fuel/Car Maintenance Reimbursed Exp (W):</ b>"));
                    totalCarFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalCarFee - PayrollDoc.totalCarFeeNW)));
                    payrollTotalTable.Rows.Add(totalCarFeeRow);
                }

                TableRow totalServiceFeeRow = new TableRow();
                totalServiceFeeRow.Cells.Add(Globals.FormatedTableCell("<b>Total Service Fee:</b>"));
                totalServiceFeeRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalServiceFee)));
                payrollTotalTable.Rows.Add(totalServiceFeeRow);

                TableRow totalTipsRow = new TableRow();
                totalTipsRow.Cells.Add(Globals.FormatedTableCell("<b>Tips:</b>"));
                totalTipsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalTips)));
                payrollTotalTable.Rows.Add(totalTipsRow);

                TableRow totalDiscountsRow = new TableRow();
                totalDiscountsRow.Cells.Add(Globals.FormatedTableCell("<b>Discounts:</b>"));
                totalDiscountsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalDiscounts)));
                payrollTotalTable.Rows.Add(totalDiscountsRow);

                TableRow totalAdjustmentsRow = new TableRow();
                totalAdjustmentsRow.Cells.Add(Globals.FormatedTableCell("<b>Adjustments:</b>"));
                totalAdjustmentsRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalAdjustments)));
                payrollTotalTable.Rows.Add(totalAdjustmentsRow);

                TableRow totalSalesTaxRow = new TableRow();
                totalSalesTaxRow.Cells.Add(Globals.FormatedTableCell("<b>Sales Tax:</b>"));
                totalSalesTaxRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalSalesTax)));
                payrollTotalTable.Rows.Add(totalSalesTaxRow);

                TableRow totalGrossRevenueRow = new TableRow();
                totalGrossRevenueRow.Cells.Add(Globals.FormatedTableCell("<b>Gross Revenue:</b>"));
                totalGrossRevenueRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalGrossRevenue)));
                payrollTotalTable.Rows.Add(totalGrossRevenueRow);

                TableRow totalWaiverRow = new TableRow();
                totalWaiverRow.Cells.Add(Globals.FormatedTableCell("<b>Waiver Payroll Total:</b>"));
                totalWaiverRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalPayroll - PayrollDoc.totalNonWaiver)));
                payrollTotalTable.Rows.Add(totalWaiverRow);

                TableRow totalNonWaiverRow = new TableRow();
                totalNonWaiverRow.Cells.Add(Globals.FormatedTableCell("<b>Non-Waiver Payroll Total:</b>"));
                totalNonWaiverRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalNonWaiver)));
                payrollTotalTable.Rows.Add(totalNonWaiverRow);

                TableRow totalInsuranceRow = new TableRow();
                totalInsuranceRow.Cells.Add(Globals.FormatedTableCell("<b>Insurance Payroll Total:</b>"));
                totalInsuranceRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalPayroll - PayrollDoc.totalNonInsurance)));
                payrollTotalTable.Rows.Add(totalInsuranceRow);

                TableRow totalNonInsuranceRow = new TableRow();
                totalNonInsuranceRow.Cells.Add(Globals.FormatedTableCell("<b>Non-Insurance Payroll Total:</b>"));
                totalNonInsuranceRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalNonInsurance)));
                payrollTotalTable.Rows.Add(totalNonInsuranceRow);

                TableRow totalPayrollRow = new TableRow();
                totalPayrollRow.Cells.Add(Globals.FormatedTableCell("<b>Payroll:</b>"));
                totalPayrollRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(PayrollDoc.totalPayroll)));
                payrollTotalTable.Rows.Add(totalPayrollRow);

                decimal totalProfit = PayrollDoc.totalGrossRevenue - PayrollDoc.totalPayroll;

                TableRow totalProfitRow = new TableRow();
                totalProfitRow.Cells.Add(Globals.FormatedTableCell("<b>Profit:</b>"));
                totalProfitRow.Cells.Add(Globals.FormatedTableCell(Globals.FormatMoney(totalProfit)));
                payrollTotalTable.Rows.Add(totalProfitRow);

                if (userAccess >= 7) PageDiv.Controls.Add(payrollTotalTable);
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "AppointmentsReportPayroll EX: " + ex.Message;
            }
        }
        #endregion

        #region CustomerEmailsReport
        private void CustomerEmailsReport(int franchiseMask, int contractorMask, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);

                Dictionary<int, bool> appDict = new Dictionary<int, bool>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentID", false))
                {
                    if (app.appStatus == 0 && !appDict.ContainsKey(app.customerID))
                    {
                        appDict.Add(app.customerID, true);
                    }
                }

                string html = "First Name,Last Name,Phone Number,Email<br/>";
                foreach (CustomerStruct customer in Database.GetCustomers(franchiseMask, "(accountStatus = 'Active' OR accountStatus = 'Inactive' OR accountStatus = 'As Needed' OR accountStatus = 'Quotes' OR accountStatus = 'Web Quotes' OR accountStatus = 'One-Time')", "firstName, lastName"))
                {
                    if (appDict.ContainsKey(customer.customerID) && Globals.ValidEmail(customer.email))
                    {
                        html += customer.firstName + "," + customer.lastName + "," + Globals.FormatPhone(customer.bestPhone) + "," + Globals.SplitEmail(customer.email)[0] + "<br/>";
                    }
                }
                PageDiv.InnerHtml = html;
            }
            catch (Exception ex)
            {
                ErrorDiv.InnerHtml = "CustomerEmailsReport EX: " + ex.Message;
            }
        }
        #endregion
    }
}