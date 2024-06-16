using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Drawing;
using System.Diagnostics;
using OfficeOpenXml.Style;

namespace TwoLocalGals.Protected
{
    public partial class Schedule : System.Web.UI.Page
    {
        private int franchiseMask = 0;
        private DateTime weekStartDate = DateTime.MinValue;
        private AppStruct replaceApp = new AppStruct();
        private CustomerStruct customer = new CustomerStruct();
        private int lookupCount = 0;
        private decimal lookupHoursTotal = 0;
        private decimal lookupHours = 0.5m;
        DateTime lookupStart = DateTime.MinValue;
        DateTime lookupEnd = DateTime.MinValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            Globals.ForceSSL(this);

            int userAcess = Globals.GetUserAccess(this);
            if (userAcess < 2) Globals.LogoutUser(this);
            if (userAcess <= 2 && !string.IsNullOrEmpty(Request["custID"]) && !string.IsNullOrEmpty(Request["appID"]) && !string.IsNullOrEmpty(Request["replaceID"])) Globals.LogoutUser(this);

            franchiseMask = Globals.GetFranchiseMask();

            customer.customerID = Globals.SafeIntParse(Request["custID"]);

            int appointmentID = Globals.SafeIntParse(Request["appID"]);
            if (appointmentID > 0)
            {
                AppStruct srcApp;
                Database.GetApointmentpByID(franchiseMask, appointmentID, out srcApp);
                weekStartDate = srcApp.appointmentDate;
                if (customer.customerID == 0) customer.customerID = srcApp.customerID;
                Globals.SetCookieValue("ScheduleContractorType", srcApp.appType.ToString());
            }

            replaceApp.appointmentID = Globals.SafeIntParse(Request["replaceID"]);
            if (replaceApp.appointmentID > 0)
            {
                Database.GetApointmentpByID(franchiseMask, replaceApp.appointmentID, out replaceApp);
                weekStartDate = replaceApp.appointmentDate;
                customer.customerID = replaceApp.customerID;
                Globals.SetCookieValue("ScheduleContractorType", replaceApp.appType.ToString());
            }

            if (customer.customerID > 0)
            {
                Database.GetCustomerByID(franchiseMask, customer.customerID, out customer);
            }

            int selectedMask = 0;
            if (userAcess > 2)
            {
                selectedMask = Request.Cookies["ScheduleMask"] != null ? Globals.SafeIntParse(Request.Cookies["ScheduleMask"].Value) : -1;
                if (selectedMask == 0) selectedMask = -1;
                if (customer.customerID > 0) selectedMask = customer.franchiseMask;
                foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.ID = "FRANCHECK" + franchise.Value;
                    checkBox.Text = franchise.Text;
                    checkBox.Checked = franchise.Selected;
                    MenuPanel.Controls.AddAt(MenuPanel.Controls.Count - 2, checkBox);
                }
                CompactModeButton.Text = (Globals.GetCookieValue("ScheduleCompact") == "Y") ? "Disable Compact Mode" : "Compact Mode";
            }
            else
            {
                int userContractorID = Globals.GetUserContractorID(this);
                foreach (ContractorStruct contractor in Database.GetContractorList(-1, -1, false, true, true, false, "team, firstName, lastName"))
                    if (contractor.contractorID == userContractorID) selectedMask = contractor.franchiseMask;
                SearchPanel.Visible = false;
                CompactModeButton.Visible = false;
                SortBy.Visible = false;
                ContractorType.Visible = false;
            }
            franchiseMask &= selectedMask;

            if (weekStartDate == DateTime.MinValue) weekStartDate = Globals.DateTimeParse(Request["date"]);
            if (weekStartDate == DateTime.MinValue) weekStartDate = DateTime.Now;
            weekStartDate = Globals.StartOfWeek(weekStartDate).Date;
            
            if (!IsPostBack)
            {
                ContractorType.Items.Clear();
                ContractorType.Items.AddRange(Globals.GetServicesList(Database.GetFranchiseServiceMask(Globals.GetFranchiseMask())));

                WeekDate.Text = weekStartDate.ToString("d");

                if (!string.IsNullOrEmpty(Globals.GetCookieValue("ScheduleSortBy")))
                    SortBy.SelectedValue = Globals.GetCookieValue("ScheduleSortBy");

                if (!string.IsNullOrEmpty(Globals.GetCookieValue("ScheduleContractorType")))
                    ContractorType.SelectedValue = Globals.GetCookieValue("ScheduleContractorType");

                lookupCount = Globals.SafeIntParse(Request["lookupCount"]);
                lookupHoursTotal = Globals.SafeDecimalParse(Request["lookupHours"]);
                if (customer.customerID > 0)
                {
                    if (replaceApp.appointmentID > 0)
                    {
                        lookupCount = 1;
                        lookupHoursTotal = (decimal)(replaceApp.endTime - replaceApp.startTime).TotalHours;
                    }
                    else
                    {
                        if (lookupCount == 0) lookupCount = Database.GetCustomerAverageCount(customer.customerID);
                        if (lookupCount == 0) lookupCount = 2;
                        if (lookupHoursTotal == 0) lookupHoursTotal = (decimal)Database.GetCustomerAverageHoursPerContractor(customer.customerID).TotalHours * lookupCount;
                        if (lookupHoursTotal == 0) lookupHoursTotal = Globals.SafeDecimalParse(customer.estimatedHours);
                        if (lookupHoursTotal == 0) lookupHoursTotal = 4;
                    }
                    lookupHours = lookupHoursTotal / lookupCount;
                    LookupPanel.Visible = true;
                    LookupContractorCount.SelectedValue = lookupCount.ToString();
                    LookupContractorHours.SelectedValue = lookupHoursTotal.ToString("N2");
                    LookupLabel.Text = " = <b>" + Globals.FormatHours(lookupHours) + "</b> hours per contractor (" + customer.NC_Special + ")";
                }
                LoadSchedule();
            }
        }

        public void LookupChanged(object sender, EventArgs e)
        {
            TransferToDate(0, false);
        }

        public void PrevButtonClick(object sender, EventArgs e)
        {
            TransferToDate(-7, false);
        }

        public void NextButtonClick(object sender, EventArgs e)
        {
            TransferToDate(+7, false);
        }

        public void ApplyButtonClick(object sender, EventArgs e)
        {
            TransferToDate(0, false);
        }

        public void UnavailableButtonClick(object sender, EventArgs e)
        {
            Response.Redirect("Unavailable.aspx" + ((Button)sender).CommandArgument);
        }

        public void SearchClick(object sender, EventArgs e)
        {
            try
            {
                if (SearchBox.Text.Contains("ID="))
                {
                    customer.customerID = Globals.SafeIntParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3));
                    TransferToDate(0, true);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SearchClick EX: " + ex.Message;
            }
        }

        public void CompactModeClick(object sender, EventArgs e)
        {
            try
            {
                string value = Globals.GetCookieValue("ScheduleCompact");
                Response.Cookies.Add(new HttpCookie("ScheduleCompact", value == "Y" ? "N" : "Y"));
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CompactModeClick EX: " + ex.Message;
            }
        }

        private string BuildTransferUrl(int dayOffset, bool newCustomer)
        {
            try
            {
                DateTime weekDate = Globals.DateTimeParse(WeekDate.Text);
                if (weekDate == DateTime.MinValue) weekDate = DateTime.Now;
                weekDate += TimeSpan.FromDays(dayOffset);

                string url = Globals.BuildQueryString("Schedule.aspx", "date", weekDate.ToString("d"));
                if (customer.customerID > 0)
                {
                    url = Globals.BuildQueryString(url, "custID", customer.customerID);
                    if (!newCustomer)
                    {
                        if (dayOffset == 0 && replaceApp.appointmentID > 0) url = Globals.BuildQueryString(url, "replaceID", replaceApp.appointmentID);
                        url = Globals.BuildQueryString(url, "lookupCount", Globals.SafeDecimalParse(LookupContractorCount.Text));
                        url = Globals.BuildQueryString(url, "lookupHours", Globals.SafeDecimalParse(LookupContractorHours.Text));
                    }
                }
                url = Globals.BuildQueryString(url, "DoScroll", "Y");
                return url;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TransferToDate EX: " + ex.Message;
                return null;
            }
        }

        private void TransferToDate(int dayOffset, bool newCustomer)
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
                Globals.SetCookieValue("ScheduleMask", selectedMask.ToString());
                Globals.SetCookieValue("ScheduleSortBy", SortBy.Text);
                Globals.SetCookieValue("ScheduleContractorType", ContractorType.SelectedValue.ToString());

                string url = BuildTransferUrl(dayOffset, newCustomer);
                if (url != null)
                    Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TransferToDate EX: " + ex.Message;
            }
        }

        public void LoadSchedule()
        {
            if (replaceApp.appointmentID == 0)
            {
                TitleDiv.InnerHtml = customer.customerID > 0 ? @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">Schedule Appointment - " + customer.customerTitle + @" - (" + customer.NC_Frequency + ", " + customer.NC_DayOfWeek + " ," + customer.NC_TimeOfDayPrefix + " " + customer.NC_TimeOfDay + ")</a>" : "";
            }
            else
            {
                TitleDiv.InnerHtml = customer.customerID > 0 ? @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">Replace Contractor - " + replaceApp.contractorTitle + " for " + customer.customerTitle + @" - (Must Be " + replaceApp.startTime.ToString("t") + @") preference (" + customer.NC_Frequency + ", " + customer.NC_DayOfWeek + " ," + customer.NC_TimeOfDayPrefix + " " + customer.NC_TimeOfDay + ")</a>" : "";
                customer.NC_TimeOfDayPrefix = "Must Be";
                customer.NC_TimeOfDay = replaceApp.startTime.ToString("t");
            }

            Dictionary<DateTime, Dictionary<int, LinkedList<AppStruct>>> appDict = new Dictionary<DateTime, Dictionary<int, LinkedList<AppStruct>>>();
            Dictionary<DateTime, Dictionary<int, List<AppStruct>>> partnerDict = new Dictionary<DateTime, Dictionary<int, List<AppStruct>>>();
            foreach (AppStruct app in Database.GetAppsByDateRange(-1, weekStartDate, weekStartDate + TimeSpan.FromDays(7), "A.appointmentDate, A.startTime, A.endTime", true))
            {
                if (app.appStatus != 0) continue;
                if (app.appointmentID == replaceApp.appointmentID) continue;

                if (!appDict.ContainsKey(app.appointmentDate.Date)) appDict.Add(app.appointmentDate.Date, new Dictionary<int, LinkedList<AppStruct>>());
                if (!appDict[app.appointmentDate.Date].ContainsKey(app.contractorID)) appDict[app.appointmentDate.Date].Add(app.contractorID, new LinkedList<AppStruct>());
                appDict[app.appointmentDate.Date][app.contractorID].AddLast(app);

                if (!partnerDict.ContainsKey(app.appointmentDate)) partnerDict.Add(app.appointmentDate, new Dictionary<int, List<AppStruct>>());
                if (!partnerDict[app.appointmentDate].ContainsKey(app.customerID)) partnerDict[app.appointmentDate].Add(app.customerID, new List<AppStruct>());
                partnerDict[app.appointmentDate][app.customerID].Add(app);
            }

            Dictionary<DateTime, Dictionary<int, LinkedList<UnavailableStruct>>> unDict = new Dictionary<DateTime, Dictionary<int, LinkedList<UnavailableStruct>>>();
            foreach (UnavailableStruct un in Database.GetUnavailableByDateRange(-1, -1, -1, weekStartDate, weekStartDate + TimeSpan.FromDays(7), "U.dateRequested, U.startTime, U.endTime DESC"))
            {
                if (!unDict.ContainsKey(un.dateRequested.Date)) unDict.Add(un.dateRequested.Date, new Dictionary<int, LinkedList<UnavailableStruct>>());
                if (!unDict[un.dateRequested.Date].ContainsKey(un.contractorID)) unDict[un.dateRequested.Date].Add(un.contractorID, new LinkedList<UnavailableStruct>());

                bool useless = false;
                foreach (UnavailableStruct compUn in unDict[un.dateRequested.Date][un.contractorID])
                    if (un.startTime >= compUn.startTime && un.endTime <= compUn.endTime) useless = true;
                if (!useless) unDict[un.dateRequested.Date][un.contractorID].AddLast(un);
            }

            int userAccess = Globals.GetUserAccess(this);
            int userContractorID = Globals.GetUserContractorID(this);
            int contractorSubType = Globals.SafeIntParse(ContractorType.SelectedValue);
            int contractorMaskType = userAccess == 2 ? -1 : 1 << (contractorSubType - 1);
            bool compactMode = (Globals.GetCookieValue("ScheduleCompact") == "Y");
            decimal totalHoursBooked = 0;

            decimal[] dayBookedArray = new decimal[7];
            decimal[] dayAvailableArray = new decimal[7];

            DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

            string nextUrl = BuildTransferUrl(7, false);
            string prevUrl = BuildTransferUrl(-7, false);

            List<ContractorStruct> contractors = new List<ContractorStruct>();
            if (SortBy.Text == "Sort by Score")
            {
                contractors = Database.GetContractorList(franchiseMask, contractorMaskType, false, false, false, false, "score DESC, firstName, lastName");
            }
            else
            {
                List<ContractorStruct> teams = new List<ContractorStruct>();
                List<ContractorStruct> nonTeams = new List<ContractorStruct>();
                foreach (ContractorStruct contractor in Database.GetContractorList(franchiseMask, contractorMaskType, false, false, false, false, "team, firstName, lastName"))
                {

                    if (string.IsNullOrEmpty(contractor.team)) nonTeams.Add(contractor);
                    else teams.Add(contractor);
                }
                contractors.AddRange(teams);
                contractors.AddRange(nonTeams);
            }

            if (customer.customerID > 0)
            {
                Dictionary<int, int> customerFrequencyDict = Database.GetCustomerFrequency(customer.customerID);
                for (int i = 0; i < contractors.Count; i++)
                {
                    if (customerFrequencyDict.ContainsKey(contractors[i].contractorID))
                    {
                        ContractorStruct contractor = contractors[i];
                        contractor.customerFreuqency = customerFrequencyDict[contractor.contractorID];
                        contractors[i] = contractor;
                    }
                }
            }

            if (SortBy.Text == "Sort by Frequency" && customer.customerID > 0)
            {
                contractors.Sort(delegate(ContractorStruct a, ContractorStruct b)
                {
                    if (a.customerFreuqency == b.customerFreuqency)
                        return b.score.CompareTo(a.score);
                    return b.customerFreuqency.CompareTo(a.customerFreuqency);
                });
            }

            foreach (ContractorStruct contractor in contractors)
            {
                if (userAccess == 2 && contractor.contractorID != userContractorID) continue;

                bool moveToFront = false;
                decimal weekBookedHours = 0;
                decimal weekAvailableHours = 0;
                TableRow conRow = new TableRow();
                for (int i = 0; i < 7; i++)
                {
                    List<string> routeData = new List<string>();
                    string lastRouteAddr = Globals.CleanAddr(contractor.address) + "," + Globals.CleanAddr(contractor.city) + "," + Globals.CleanAddr(contractor.state) + "," + Globals.CleanAddr(contractor.zip);
                    routeData.Add(lastRouteAddr);

                    decimal dayBookedHours = 0;
                    decimal dayAvailableHours = 0;
                    DateTime dayDate = weekStartDate + TimeSpan.FromDays(i);
                    DateTime startTime = Globals.TimeOnly(contractor.startDay);
                    DateTime endTime = startTime;
                    DateTime midTime = startTime;
                    LinkedList<AppStruct> appList = appDict.ContainsKey(dayDate) && appDict[dayDate].ContainsKey(contractor.contractorID) ? appDict[dayDate][contractor.contractorID] : new LinkedList<AppStruct>();
                    LinkedList<UnavailableStruct> unList = unDict.ContainsKey(dayDate) && unDict[dayDate].ContainsKey(contractor.contractorID) ? unDict[dayDate][contractor.contractorID] : new LinkedList<UnavailableStruct>();
                    List<string> partners = new List<string>();

                    TableCell conCell = new TableCell();
                    Table dayTable = new Table();
                    dayTable.CssClass = "ScheduleDay";

                    dayTable.Caption = dayDate.ToString("dddd - MMM dd");
                    if (customer.customerID > 0 && (string.IsNullOrEmpty(customer.NC_DayOfWeek) || customer.NC_DayOfWeek == "Flexible" || dayTable.Caption.Contains(customer.NC_DayOfWeek)))
                        dayTable.Caption = @"<span style=""color: blue;"">" + dayTable.Caption + @"</span>";

                    TableRow dayRowHeader = new TableRow();
                    dayRowHeader.Cells.Add(Globals.FormatedTableHeaderCell("Start", 0));
                    dayRowHeader.Cells.Add(Globals.FormatedTableHeaderCell("End", 0));
                    dayRowHeader.Cells.Add(Globals.FormatedTableHeaderCell("Hrs", 0));
                    dayRowHeader.Cells.Add(Globals.FormatedTableHeaderCell("Event", 200));
                    dayTable.Rows.Add(dayRowHeader);

                    while (appList.Count > 0 || unList.Count > 0)
                    {
                        DrivingRoute route = new DrivingRoute();
                        route.travelTime = 1800;

                        TableRow dayRow = new TableRow();
                        if ((appList.Count > 0 ? appList.First.Value.startTime : DateTime.MaxValue) <= (unList.Count > 0 ? unList.First.Value.startTime : DateTime.MaxValue))
                        {
                            //Add Appointment
                            AppStruct app = appList.First.Value;

                            if (!string.IsNullOrEmpty(app.customerDayOfWeek) && app.customerDayOfWeek != "Flexible" && !dayTable.Caption.Contains(app.customerDayOfWeek))
                            {
                                dayRow.Style["background-color"] = "#CCC";
                            }
                            if (!Globals.CheckPreferedCustomerTime(app.customerTimeOfDayPrefix, app.customerTimeOfDay, app.startTime, app.endTime))
                            {
                                dayRow.Style["background-color"] = "#CCC";
                            }
                            if (replaceApp.appointmentID > 0 && app.customerID == replaceApp.customerID)
                            {
                                dayRow.Style["background-color"] = "#F1B8F1";
                                moveToFront = true;
                            }
                            if (app.appointmentDate.Date < contractor.hireDate.Date)
                            {
                                dayRow.Style["background-color"] = "#FFA0A0";
                            }
                            if (app.startTime < contractor.startDay || app.endTime > contractor.endDay)
                            {
                                dayRow.Style["background-color"] = "#FFA0A0";
                            }
                            decimal hours = (decimal)(app.endTime - app.startTime).TotalMinutes / 60;
                            if (hours < 0) hours = 0;

                            string routeAddr = Globals.CleanAddr(app.customerAddress) + "," + Globals.CleanAddr(app.customerCity) + "," + Globals.CleanAddr(app.customerState) + "," + Globals.CleanAddr(app.customerZip);
                            route = GoogleMaps.GetDrivingRoute(lastRouteAddr, routeAddr);
                            if (route.travelTime <= 0 && lastRouteAddr != routeAddr) route.travelTime = 1800;
                            string routeLink = GoogleMaps.GetDrivingLink(new string[] { lastRouteAddr, routeAddr });
                            lastRouteAddr = routeAddr;
                            routeData.Add(routeAddr);
                            Debug.WriteLine("Distance: " + route.distance + ", Time: " + TimeSpan.FromSeconds((double)route.travelTime));

                            if (app.startTime - startTime < TimeSpan.FromSeconds((double)route.travelTime) && dayTable.Rows.Count > 1)
                            {
                                dayRow.Style["background-color"] = "#FFA0A0";
                            }
                            if (unList.Count > 0 && unList.First.Value.startTime - app.endTime < TimeSpan.FromMinutes(30))
                            {
                                dayRow.Style["background-color"] = "#FFA0A0";
                            }
                            if (Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes) == 10 || Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes) == -10)
                            {
                                dayRow.Style["background-color"] = "#72D9FA";

                            }

                            dayRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.startTime.ToString("HH:mm") + @"</a>"));
                            dayRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.endTime.ToString("HH:mm") + @"</a>"));
                            dayRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + hours.ToString() + @"</a>"));
                            if (userAccess <= 2) dayRow.Cells.Add(Globals.FormatedTableCell(app.customerTitle + @" <b><a href=""" + routeLink + @""">" + route.distance.ToString("N1") + @" mi (" + Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes) + @" min)</a></b>"));
                            else dayRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @" (" + app.customerCity + @")</a> <b><a href=""" + routeLink + @""">" + route.distance.ToString("N1") + @" mi (" + Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes) + @" min)</a> <a href=""Schedule.aspx?replaceID=" + app.appointmentID + @""">[R]</a></b>"));
                            midTime = Globals.TimeOnly(app.startTime);
                            endTime = Globals.TimeOnly(app.endTime);
                            if (app.customerAccountStatus != "Ignored" && app.appType == contractorSubType)
                            {
                                dayBookedHours += hours;
                                totalHoursBooked += hours;
                            }
                            appList.RemoveFirst();

                            //Get Partners
                            if (userAccess == 2)
                            {
                                foreach (var partner in partnerDict[app.appointmentDate][app.customerID])
                                {
                                    if (partner.contractorID != app.contractorID)
                                    {
                                        string partnerString = partner.contractorTitle + (string.IsNullOrWhiteSpace(partner.contractorPhone) ? "" : " " + partner.contractorPhone);
                                        if (!partners.Contains(partnerString))
                                            partners.Add(partnerString);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Add Unavailable
                            UnavailableStruct un = unList.First.Value;
                            unList.RemoveFirst();

                            route = new DrivingRoute();

                            if (appList.Count > 0 && un.startTime < appList.First.Value.startTime && un.endTime > appList.First.Value.endTime)
                            {
                                UnavailableStruct un2 = un;
                                un2.startTime = appList.First.Value.endTime;
                                unList.AddFirst(un2);
                                un.endTime = appList.First.Value.endTime;
                            }

                            decimal hours = (decimal)(un.endTime - un.startTime).TotalMinutes / 60;
                            if (hours < 0) hours = 0;
                            dayRow.Style["background-color"] = "#FFFF00";
                            dayRow.Cells.Add(Globals.FormatedTableCell(un.startTime.ToString("HH:mm")));
                            dayRow.Cells.Add(Globals.FormatedTableCell(un.endTime.ToString("HH:mm")));
                            dayRow.Cells.Add(Globals.FormatedTableCell(hours.ToString()));
                            if (userAccess <= 2 && userContractorID != contractor.contractorID) dayRow.Cells.Add(Globals.FormatedTableCell(un.recurrenceType != 0 ? "Recurring Unavailable" : "Unavailable"));
                            else dayRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Unavailable.aspx?unID=" + un.unavailableID + @"&conID=" + un.contractorID + @""">" + (un.recurrenceType != 0 ? "Recurring Unavailable" : "Unavailable") + @"</a>"));
                            midTime = Globals.TimeOnly(un.startTime);
                            endTime = Globals.TimeOnly(un.endTime);
                        }

                        //if (midTime - startTime < TimeSpan.FromSeconds((double)route.travelTime) && dayTable.Rows.Count > 1)
                        //{
                        //    dayRow.Style["background-color"] = "#FFA0A0";
                        //}

                        if (contractor.active && contractor.scheduled && contractor.hireDate.Date <= dayDate.Date)
                        {
                            if (replaceApp.appointmentID == 0 || i == Globals.GetDayOfWeek(replaceApp.appointmentDate))
                            {
                                if (midTime - startTime >= TimeSpan.FromHours((double)lookupHours + (dayTable.Rows.Count > 1 ? 1.0 : 0.5)))
                                {
                                    if (dayTable.Rows.Count > 1)
                                        startTime += TimeSpan.FromMinutes(30);

                                    DateTime midEndTime = midTime - TimeSpan.FromMinutes(30);

                                    //Add Available
                                    TableRow avRow = new TableRow();
                                    decimal avHours = (decimal)(midEndTime - startTime).TotalMinutes / 60;
                                    DateTime avEndTime = avHours > 2.0m ? startTime + TimeSpan.FromHours(2) : midEndTime;
                                    avRow.Cells.Add(Globals.FormatedTableCell(startTime.ToString("HH:mm")));
                                    avRow.Cells.Add(Globals.FormatedTableCell(midEndTime.ToString("HH:mm")));
                                    avRow.Cells.Add(Globals.FormatedTableCell(avHours.ToString()));

                                    avRow.Style["background-color"] = "#99FFCC";
                                    if (Globals.IsPreferedCustomerTime(customer.NC_TimeOfDayPrefix, customer.NC_TimeOfDay, ref startTime, midEndTime, TimeSpan.FromHours((float)lookupHours)))
                                        avRow.Style["background-color"] = "#94DBFF";

                                    if (replaceApp.appointmentID > 0) avRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + replaceApp.appointmentID + @"&replaceConID=" + contractor.contractorID + @"&appType=" + contractorSubType + @"&start=" + startTime.ToString("HH:mm") + @"&end=" + startTime.AddHours((double)lookupHours).ToString("HH:mm") + @""">Available</a>"));
                                    else if (customer.customerID > 0) avRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?custID=" + customer.customerID + @"&conID=" + contractor.contractorID + @"&appType=" + contractorSubType + @"&date=" + dayDate.ToString("d") + @"&start=" + startTime.ToString("HH:mm") + @"&end=" + startTime.AddHours((double)lookupHours).ToString("HH:mm") + @""">Available</a>"));
                                    else avRow.Cells.Add(Globals.FormatedTableCell(@"Available"));

                                    dayTable.Rows.Add(avRow);
                                    dayAvailableHours += avHours;
                                }
                            }
                        }

                        startTime = endTime;

                        if (!compactMode || dayRow.Style["background-color"] != "#FFFF00")
                            dayTable.Rows.Add(dayRow);
                    }

                    if (dayTable.Rows.Count > 1)
                        startTime += TimeSpan.FromMinutes(30);

                    //Add Ending Available
                    TableRow avEndRow = new TableRow();
                    endTime = Globals.TimeOnly(contractor.endDay);
                    if (startTime > endTime) endTime = startTime;
                    decimal avEndHours = (decimal)(endTime - startTime).TotalMinutes / 60;
                    DateTime avEndTimeEnd = avEndHours > 2.0m ? startTime + TimeSpan.FromHours(2) : endTime;
                    avEndRow.Cells.Add(Globals.FormatedTableCell(startTime.ToString("HH:mm")));
                    avEndRow.Cells.Add(Globals.FormatedTableCell(endTime.ToString("HH:mm")));
                    avEndRow.Cells.Add(Globals.FormatedTableCell(avEndHours.ToString()));

                    avEndRow.Style["background-color"] = "#99FFCC";
                    if (Globals.IsPreferedCustomerTime(customer.NC_TimeOfDayPrefix, customer.NC_TimeOfDay, ref startTime, endTime, TimeSpan.FromHours((float)lookupHours)))
                        avEndRow.Style["background-color"] = "#94DBFF";

                    if (replaceApp.appointmentID > 0) avEndRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?appID=" + replaceApp.appointmentID + @"&replaceConID=" + contractor.contractorID + @"&appType=" + contractorSubType + @"&start=" + startTime.ToString("HH:mm") + @"&end=" + startTime.AddHours((double)lookupHours).ToString("HH:mm") + @""">Available</a>"));
                    else if (customer.customerID > 0) avEndRow.Cells.Add(Globals.FormatedTableCell(@"<a href=""Appointments.aspx?custID=" + customer.customerID + @"&conID=" + contractor.contractorID + @"&appType=" + contractorSubType + @"&date=" + dayDate.ToString("d") + @"&start=" + startTime.ToString("HH:mm") + @"&end=" + startTime.AddHours((double)lookupHours).ToString("HH:mm") + @""">Available</a>"));
                    else avEndRow.Cells.Add(Globals.FormatedTableCell(@"Available"));

                    if (userAccess > 2 && (!compactMode || avEndHours >= lookupHours) && contractor.active && contractor.scheduled && contractor.hireDate.Date <= dayDate.Date)
                    {
                        if (replaceApp.appointmentID == 0 || i == Globals.GetDayOfWeek(replaceApp.appointmentDate))
                        {
                            dayTable.Rows.Add(avEndRow);
                            dayAvailableHours += avEndHours;
                        }
                    }
                    if (userAccess <= 2 && avEndHours > 0 && contractor.hireDate.Date <= dayDate.Date)
                    {
                        dayTable.Rows.Add(avEndRow);
                        dayAvailableHours += avEndHours;
                    }

                    if (partners.Count > 0)
                    {
                        //Add Partners
                        TableRow dayPartnersRow = new TableRow();
                        TableCell dayPartnersCell = new TableCell();
                        dayPartnersCell.Text = string.Join(", ", partners.ToArray());
                        dayPartnersCell.ColumnSpan = 5;
                        dayPartnersRow.Cells.Add(dayPartnersCell);
                        dayTable.Rows.AddAt(0, dayPartnersRow);
                    }

                    //Add hours
                    TableRow dayHoursRow = new TableRow();
                    TableCell dayHoursCell = new TableCell();
                    dayHoursCell.Text = "Available: " + dayAvailableHours.ToString("N2") + ", Booked: " + dayBookedHours.ToString("N2") + @", <b><a href=""" + GoogleMaps.GetDrivingLink(routeData.ToArray()) + @""">Map</a></b>";
                    dayHoursCell.Style["font-size"] = "1.2em";
                    dayHoursCell.ColumnSpan = 5;
                    dayHoursRow.Cells.Add(dayHoursCell);
                    dayTable.Rows.AddAt(0, dayHoursRow);

                    conCell.Controls.Add(dayTable);
                    conRow.Cells.Add(conCell);

                    weekBookedHours += dayBookedHours;
                    dayBookedArray[i] += dayBookedHours;
                    if (contractor.active && contractor.scheduled)
                    {
                        weekAvailableHours += dayAvailableHours;
                        dayAvailableArray[i] += dayAvailableHours;  
                    }
                }


                TableRow titleRow = new TableRow();

                TableCell titleCell = new TableCell();
                titleCell.ColumnSpan = 5;
                titleCell.Style["text-align"] = "left";

                HyperLink prevLink = new HyperLink();
                prevLink.ID = "PrevButton_" + contractor.contractorID;
                prevLink.NavigateUrl = prevUrl;
                prevLink.Text = "<<<";
                prevLink.Style["text-decoration"] = "none";
                prevLink.Style["border"] = "1px solid #000";
                prevLink.Style["padding"] = "3px";
                titleCell.Controls.Add(prevLink);

                HyperLink nextLink = new HyperLink();
                nextLink.ID = "NextButton_" + contractor.contractorID;
                nextLink.NavigateUrl = nextUrl;
                nextLink.Text = ">>>";
                nextLink.Style["text-decoration"] = "none";
                nextLink.Style["border"] = "1px solid #000";
                nextLink.Style["margin-left"] = "5px";
                nextLink.Style["padding"] = "3px";
                titleCell.Controls.Add(nextLink);

                System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
                image.ImageUrl = "~/ContratorPics/" + contractor.ContractorPic;
                image.Width = 25; image.Height = 25;
                image.Visible = true;
                image.Style["border-radius"] = "45%";
                image.Style["border"] = "solid 0.1px";
                titleCell.Controls.Add(image);

                Label conLabel = new Label();
                conLabel.CssClass = "ScheduleConTitle";
                conLabel.Text = @"<a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + (string.IsNullOrEmpty(contractor.team) ? "" : "(" + contractor.team + ") ") + contractor.title + " - Available: " + weekAvailableHours + ", Booked: " + weekBookedHours.ToString("N2") + ", Score: " + contractor.score.ToString("N2") + (contractor.customerFreuqency > 0 ? (", Frequency: " + contractor.customerFreuqency) : "") + @"</a>";
                if (userAccess > 2 && (!contractor.active || !contractor.scheduled))
                {
                    conLabel.Text += " (UNASSIGNED APPOINTMENTS)";
                    conLabel.ForeColor = Color.Red;
                    moveToFront = true;
                }
                titleCell.Controls.Add(conLabel);

                Label unavailableLabel = new Label();
                unavailableLabel.Text = @"<a href=""Unavailable.aspx?conID=" + contractor.contractorID.ToString() + @""" class=""unavailableButton"">Add Unavailability</a>";
                if (userAccess > 2 || userContractorID == contractor.contractorID) titleCell.Controls.Add(unavailableLabel);

                titleRow.Cells.Add(titleCell);

                TableCell rightConLabelCell = new TableCell();
                rightConLabelCell.ColumnSpan = 2;
                rightConLabelCell.CssClass = "ScheduleConTitle";
                rightConLabelCell.Text = (string.IsNullOrEmpty(contractor.team) ? "" : "(" + contractor.team + ") ") + contractor.title;
                rightConLabelCell.Style["text-align"] = "right";
                titleRow.Cells.Add(rightConLabelCell);

                TableRow blankRow = new TableRow();
                TableCell blankCell = new TableCell();
                blankCell.ColumnSpan = 7;
                blankCell.CssClass = "ScheduleConBlank";
                blankRow.Cells.Add(blankCell);

                if (userAccess <= 2 || weekBookedHours > 0  || (contractor.active && contractor.scheduled))
                {
                    if (moveToFront)
                    {
                        ScheduleTable.Rows.AddAt(0, blankRow);
                        ScheduleTable.Rows.AddAt(0, conRow);
                        ScheduleTable.Rows.AddAt(0, titleRow);
                    }
                    else
                    {
                        ScheduleTable.Rows.Add(titleRow);
                        ScheduleTable.Rows.Add(conRow);
                        ScheduleTable.Rows.Add(blankRow);
                    }
                }
            }

            //Totals
            if (Globals.GetUserAccess(this) > 2)
            {
                TableRow totalRow = new TableRow();
                for (int i = 0; i < 7; i++)
                {
                    TableHeaderCell totalCell = new TableHeaderCell();
                    totalCell.Text = "Total Available: " + dayAvailableArray[i].ToString("N2");
                    totalRow.Cells.Add(totalCell);
                }
                ScheduleTable.Rows.AddAt(0, totalRow);
                WeekTotal.InnerText = "Total Hours Booked: " + Globals.FormatHours(totalHoursBooked);
            }
        }
    }
}