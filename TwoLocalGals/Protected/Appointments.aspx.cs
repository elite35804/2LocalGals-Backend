using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Threading;
using System.IO;
using Image = System.Web.UI.WebControls.Image;
using System.Web.UI.HtmlControls;

namespace Nexus.Protected
{
    public partial class WebFormAppointments : System.Web.UI.Page
    {
        private int appID = 0;
        private bool splitServiceFee = false;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                appID = Globals.SafeIntParse(Request["appID"]);

                PopulateTimeDropDown(ContractorStartTime_0);
                PopulateTimeDropDown(ContractorEndTime_0);
                PopulateTimeDropDown(ContractorStartTime_1);
                PopulateTimeDropDown(ContractorEndTime_1);
                PopulateTimeDropDown(ContractorStartTime_2);
                PopulateTimeDropDown(ContractorEndTime_2);
                PopulateTimeDropDown(ContractorStartTime_3);
                PopulateTimeDropDown(ContractorEndTime_3);
                PopulateTimeDropDown(ContractorStartTime_4);
                PopulateTimeDropDown(ContractorEndTime_4);
                PopulateTimeDropDown(ContractorStartTime_5);
                PopulateTimeDropDown(ContractorEndTime_5);
                PopulateTimeDropDown(ContractorStartTime_6);
                PopulateTimeDropDown(ContractorEndTime_6);
                PopulateTimeDropDown(ContractorStartTime_7);
                PopulateTimeDropDown(ContractorEndTime_7);
                PopulateTimeDropDown(ContractorStartTime_8);
                PopulateTimeDropDown(ContractorEndTime_8);
                PopulateTimeDropDown(ContractorStartTime_9);
                PopulateTimeDropDown(ContractorEndTime_9);

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, new string[] { "Users.aspx", "FollowUp.aspx", "PrintOut.aspx" });
                    AppDate.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                    LoadAppointment();
                    Session["IsAdd"] = null;
                }
                else
                {
                    // DownloadImage(this, e);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }
        #endregion

        #region PopulateTimeDropDown
        private void PopulateTimeDropDown(DropDownList list)
        {
            list.Items.Add("6:00 AM");
            list.Items.Add("6:15 AM");
            list.Items.Add("6:30 AM");
            list.Items.Add("6:45 AM");
            list.Items.Add("7:00 AM");
            list.Items.Add("7:15 AM");
            list.Items.Add("7:30 AM");
            list.Items.Add("7:45 AM");
            list.Items.Add("8:00 AM");
            list.Items.Add("8:15 AM");
            list.Items.Add("8:30 AM");
            list.Items.Add("8:45 AM");
            list.Items.Add("9:00 AM");
            list.Items.Add("9:15 AM");
            list.Items.Add("9:30 AM");
            list.Items.Add("9:45 AM");
            list.Items.Add("10:00 AM");
            list.Items.Add("10:15 AM");
            list.Items.Add("10:30 AM");
            list.Items.Add("10:45 AM");
            list.Items.Add("11:00 AM");
            list.Items.Add("11:15 AM");
            list.Items.Add("11:30 AM");
            list.Items.Add("11:45 AM");
            list.Items.Add("12:00 PM");
            list.Items.Add("12:15 PM");
            list.Items.Add("12:30 PM");
            list.Items.Add("12:45 PM");
            list.Items.Add("1:00 PM");
            list.Items.Add("1:15 PM");
            list.Items.Add("1:30 PM");
            list.Items.Add("1:45 PM");
            list.Items.Add("2:00 PM");
            list.Items.Add("2:15 PM");
            list.Items.Add("2:30 PM");
            list.Items.Add("2:45 PM");
            list.Items.Add("3:00 PM");
            list.Items.Add("3:15 PM");
            list.Items.Add("3:30 PM");
            list.Items.Add("3:45 PM");
            list.Items.Add("4:00 PM");
            list.Items.Add("4:15 PM");
            list.Items.Add("4:30 PM");
            list.Items.Add("4:45 PM");
            list.Items.Add("5:00 PM");
            list.Items.Add("5:15 PM");
            list.Items.Add("5:30 PM");
            list.Items.Add("5:45 PM");
            list.Items.Add("6:00 PM");
            list.Items.Add("6:15 PM");
            list.Items.Add("6:30 PM");
            list.Items.Add("6:45 PM");
            list.Items.Add("7:00 PM");
            list.Items.Add("7:15 PM");
            list.Items.Add("7:30 PM");
            list.Items.Add("7:45 PM");
            list.Items.Add("8:00 PM");
            list.Items.Add("8:15 PM");
            list.Items.Add("8:30 PM");
            list.Items.Add("8:45 PM");
            list.Items.Add("9:00 PM");
            list.Items.Add("9:15 PM");
            list.Items.Add("9:30 PM");
            list.Items.Add("9:45 PM");
            list.Items.Add("10:00 PM");
            list.Items.Add("10:15 PM");
            list.Items.Add("10:30 PM");
            list.Items.Add("10:45 PM");
            list.Items.Add("11:00 PM");
            list.Items.Add("11:15 PM");
            list.Items.Add("11:30 PM");
            list.Items.Add("11:45 PM");
        }
        #endregion

        #region LinkSaveCommand
        public void LinkSaveCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                if (SaveChanges()) Response.Redirect(e.CommandArgument.ToString());
            }
        }
        #endregion

        #region FollowUpClick
        public void FollowUpClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect(@"FollowUp.aspx?appID=" + appID);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }
        #endregion

        #region ViewPaymentClick
        public void ViewPaymentClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    AppStruct app;
                    string error = Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app);
                    if (error == null)
                    {
                        string url = "Payments.aspx";
                        url = Globals.BuildQueryString(url, "StartDate", app.appointmentDate.ToString("d"));
                        url = Globals.BuildQueryString(url, "EndDate", app.appointmentDate.ToString("d"));
                        url = Globals.BuildQueryString(url, "Search", app.customerTitleCustomNote);
                        Response.Redirect(url);
                    }
                    else ErrorLabel.Text = "ViewPaymentClick: " + error;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }
        #endregion

        #region SaveClick
        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges()) CancelClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }
        #endregion

        #region SaveChanges
        public bool SaveChanges()
        {
            try
            {
                string error = null;
                ErrorLabel.Text = "";

                appID = Globals.SafeIntParse(Request["appID"]);
                List<AppStruct> apps = GetAppsFromForms();
                for (int i = 0; i < apps.Count; i++)
                {
                    if (apps[i].contractorID <= 0)
                    {
                        ErrorLabel.Text = "Invalid Contractor Selected";
                        return false;
                    }
                }

                for (int i = 0; i < apps.Count; i++)
                {
                    AppStruct app = apps[i];
                    bool applyFuture = ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorRecurrenceType_" + i)).Text == "None" ? false : ((CheckBox)Globals.FindControlRecursive(ContractorTable, "ContractorApplyToFuture_" + i)).Checked;
                    app.username = Globals.GetUsername();
                    if (null != (error = Database.SetAppointment(ref app, applyFuture)))
                    {
                        ErrorLabel.Text = "Error SetAppointment(" + app.appointmentID + "): " + error;
                        break;
                    }
                    if (i == 0) appID = app.appointmentID;
                    apps[i] = app;
                }

                // Update Related Appointments
                List<int> relatedAppointmentIds = apps.Select(x => x.appointmentID).ToList();
                relatedAppointmentIds = relatedAppointmentIds.Distinct().ToList();
                relatedAppointmentIds.Remove(0);
                relatedAppointmentIds.Sort();
                if (relatedAppointmentIds.Count > 1)
                {
                    Database.UpdateRelatedApointmentIDs(relatedAppointmentIds);
                }


                (new Thread(() =>
                {
                    //Recalculate Referrals
                    Database.UpdateInactiveCustomers();
                })).Start();



                return error == null;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region CancelClick
        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                string url = Globals.GetPeviousPage(this, "Schedule.aspx");
                if (url.Contains("Customers.aspx") && !url.Contains("custID="))
                {
                    //New Customer Check
                    url = Globals.BuildQueryString("Customers.aspx", "custID", Request["custID"]);
                    url = Globals.BuildQueryString(url, "DoScroll", "Y");
                }
                if (url.Contains("Schedule.aspx") && url.Contains("appID=") && appID > 0)
                {
                    //Scheduled Appointment
                    url = Globals.BuildQueryString("Appointments.aspx", "appID", appID);
                }
                if (url.Contains("Schedule.aspx") && url.Contains("replaceID=") && appID > 0)
                {
                    //Replace Appointment
                    url = Globals.BuildQueryString("Schedule.aspx", "appID", appID);
                }
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }
        #endregion

        #region DeleteClick
        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = null;
                appID = Globals.SafeIntParse(Request["appID"]);
                if (appID > 0)
                {
                    AppStruct app;
                    if (null == (error = Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app)))
                    {
                        decimal balance = 0;
                        foreach (TransactionStruct trans in Database.GetTransactions(Globals.GetFranchiseMask(), app.customerID, app.appointmentDate, app.appointmentDate, "T.dateCreated"))
                        {
                            if (trans.auth == 1 && !trans.isVoid)
                            {
                                ErrorLabel.Text = "Cannot delete appointment with open authorization.";
                                return;
                            }
                            if (!trans.isVoid && !trans.IsAuth())
                            {
                                balance += (trans.transType == "Return" ? -trans.total : trans.total);
                            }
                        }
                        if (balance == 0)
                        {
                            for (int i = 0; i < ((ContractorTable.Rows.Count / 2) - 1); i++)
                            {
                                HiddenField hidden = (HiddenField)Globals.FindControlRecursive(ContractorTable, "ContractorAppID_" + i);
                                int appointmentID = Globals.SafeIntParse(hidden.Value);
                                if (appointmentID > 0)
                                {
                                    if (null != (error = Database.DeleteAppointment(appointmentID)))
                                    {
                                        ErrorLabel.Text = "Delete Canceled AppID(" + appointmentID + "), " + error;
                                        return;
                                    }
                                }
                            }
                            CancelClick(sender, e);
                        }
                        else ErrorLabel.Text = "Cannot Delete Appointment with Nonzero Transaction Balance (" + Globals.FormatMoney(balance) + ")";
                    }
                    else ErrorLabel.Text = "Delete Canceled, " + error;
                }
                else ErrorLabel.Text = "Delete Canceled, Inavlid Appointment";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }
        #endregion

        #region RemoveContractorClick
        public void RemoveContractorClick(object sender, EventArgs e)
        {
            try
            {
                appID = Globals.SafeIntParse(Request["appID"]);
                List<AppStruct> apps = GetAppsFromForms();
                for (int i = 0; i < apps.Count; i++)
                {
                    if (apps[i].contractorID <= 0)
                    {
                        ErrorLabel.Text = "Invalid Contractor Selected";
                        return;
                    }
                }

                string controlID = ((Button)sender).ID;

                int index = int.Parse(controlID.Split('_')[1]);
                HiddenField hidden = (HiddenField)Globals.FindControlRecursive(ContractorTable, "ContractorAppID_" + index);
                int delID = Globals.SafeIntParse(hidden.Value);
                hidden.Value = "-1";

                string error = Database.DeleteAppointment(delID);
                if (error != null)
                {
                    ErrorLabel.Text = "RemoveContractorClick DeleteAppointment: " + error;
                }
                else
                {
                    decimal hoursBilled = Globals.FormatMoney(HoursBilled.Text);
                    hoursBilled -= Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorHours_" + index)).Text);
                    HoursBilled.Text = Globals.FormatMoney(hoursBilled);
                    splitServiceFee = true;

                    if (SaveChanges())
                    {
                        Response.Redirect(Globals.BuildQueryString("Appointments.aspx", "appID", appID));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "RemoveContractorClick EX: " + ex.Message;
            }
        }
        #endregion

        #region AddContractorNewClick
        public void AddContractorNewClick(object sender, EventArgs e)
        {
            try
            {
                Session["IsAdd"] = true;
                if (SaveChanges())
                {
                    string url = Globals.BuildQueryString("Appointments.aspx", "appID", appID);
                    //url = Globals.BuildQueryString(url, "conID", "0");
                    Response.Redirect(url);

                    //LoadAppointment();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "AddContractorNewClick EX: " + ex.Message;
            }
        }
        #endregion

        #region AddContractorScheduleClick
        public void AddContractorScheduleClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    if (appID > 0)
                    {
                        Response.Redirect(Globals.BuildQueryString("Schedule.aspx", "appID", appID));
                    }
                    else
                    {
                        Response.Redirect(Globals.BuildQueryString("Schedule.aspx", "custID", Request["custID"]));
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "AddContractorScheduleClick EX: " + ex.Message;
            }
        }
        #endregion

        #region ReplaceContractorClick
        public void ReplaceContractorClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    Response.Redirect(Globals.BuildQueryString("Schedule.aspx", "replaceID", appID));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ReplaceContractorScheduleClick EX: " + ex.Message;
            }
        }
        #endregion

        #region GetAppsFromForms
        private List<AppStruct> GetAppsFromForms()
        {
            List<AppStruct> ret = new List<AppStruct>();
            try
            {
                int contractorCount = ((ContractorTable.Rows.Count / 2) - 1);
                int housekeepingCount = 0;

                AppStruct app;
                Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app);
                if (appID == 0)
                {
                    app.customerID = Globals.SafeIntParse(Request["custID"]);
                    CustomerStruct customer;
                    Database.GetCustomerByID(Globals.GetFranchiseMask(), app.customerID, out customer);
                    app.combinedFee = customer.serviceFee;
                }

                app.franchiseMask = Globals.GetFranchiseMask();
                app.appointmentDate = Globals.DateTimeParse(AppDate.Text);
                app.appStatus = AppStatus.SelectedIndex;
                app.customerHours = Globals.FormatMoney(HoursBilled.Text);
                app.customerRate = Globals.FormatMoney(HourlyRate.Text);
                app.customerDiscountPercent = Globals.FormatPercent(DiscountPercent.Text);
                app.customerDiscountAmount = Globals.FormatMoney(DiscountAmount.Text);
                app.salesTax = Globals.FormatPercent(SalesTax.Text, false);

                for (int i = 0; i < contractorCount; i++)
                {
                    AppStruct conApp = app;
                    HiddenField hidden = (HiddenField)Globals.FindControlRecursive(ContractorTable, "ContractorAppID_" + i);
                    conApp.appointmentID = Globals.SafeIntParse(hidden.Value);
                    if (conApp.appointmentID >= 0)
                    {
                        conApp.contractorID = Globals.SafeIntParse(((DropDownList)Globals.FindControlRecursive(ContractorTable, "Contractor_" + i)).SelectedValue.Split('|')[0]);
                        var temp = (DropDownList)Globals.FindControlRecursive(ContractorTable, "Contractor_" + i);
                        conApp.appType = temp.SelectedValue.Split('|').Length > 1 ? Globals.SafeIntParse(temp.SelectedValue.Split('|')[1]) : 1;
                        conApp.startTime = Convert.ToDateTime(((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorStartTime_" + i)).Text);
                        conApp.endTime = Convert.ToDateTime(((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorEndTime_" + i)).Text);
                        if (conApp.appType > 1) conApp.customerHours = (decimal)(conApp.endTime - conApp.startTime).TotalHours;
                        conApp.contractorHours = conApp.appType == 1 ? Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorHours_" + i)).Text) : 0;
                        conApp.customerServiceFee = conApp.appType == 1 ? Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorServiceFee_" + i)).Text) : 0;
                        conApp.customerSubContractor = conApp.appType != 1 ? Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorServiceFee_" + i)).Text) : 0;
                        conApp.contractorTips = Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorTips_" + i)).Text);
                        conApp.contractorAdjustAmount = Globals.FormatMoney(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorAdjustment_" + i)).Text);
                        conApp.contractorAdjustType = ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorAdjustmentType_" + i)).SelectedValue;

                        conApp.recurrenceType = ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorRecurrenceType_" + i)).SelectedIndex;
                        conApp.weeklyFrequency = Globals.SafeIntParse(((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorWeekFreqeuncy_" + i)).Text);
                        if (conApp.weeklyFrequency < 1) app.weeklyFrequency = 1;
                        conApp.monthlyWeek = ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorWeekOfMonth_" + i)).SelectedIndex;
                        conApp.monthlyDay = ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorDayOfWeek_" + i)).SelectedIndex;

                        ContractorStruct contractor = Database.GetContractorByID(conApp.franchiseMask, conApp.contractorID);
                        conApp.contractorRate = contractor.hourlyRate;

                        ret.Add(conApp);

                        if (conApp.appType == 1) housekeepingCount++;
                    }
                }

                for (int i = 0; i < ret.Count; i++)
                {
                    AppStruct conApp = ret[i];
                    if (conApp.appType == 1 && housekeepingCount > 0) conApp.customerHours /= housekeepingCount;
                    conApp.customerDiscountAmount /= ret.Count;
                    if (splitServiceFee && conApp.appType == 1 && housekeepingCount > 0) conApp.customerServiceFee = app.combinedFee / housekeepingCount;

                    ret[i] = conApp;
                }
            }
            catch (Exception ex) { }
            return ret;
        }
        #endregion

        #region LoadAppointment
        private void LoadAppointment()
        {
            try
            {
                AppStruct app;
                string error = Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app);
                ErrorLabel.Text = error;
                DeleteButton.Enabled = error == null;
                SaveButton.Enabled = error == null;

                if (app.appointmentID == 0)
                {
                    CustomerStruct customer;
                    Database.GetCustomerByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), out customer);
                    //ContractorStruct contractor = Database.GetContractorByID(customer.franchiseMask, Globals.SafeIntParse(Request["conID"]));
                    ContractorStruct contractor = Database.GetContractorByID(customer.franchiseMask, 0);

                    app.franchiseMask = customer.franchiseMask;
                    app.customerID = customer.customerID;
                    app.customerDiscountReferral = Globals.GetReferralDiscount(Database.GetCustomerReferrals(customer.customerID));
                    app.customerTitle = customer.customerTitle;
                    app.customerRate = customer.ratePerHour;

                    FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);
                    app.salesTax = fran.salesTax;

                    app.combinedFee = customer.serviceFee;
                    app.appointmentDate = Globals.UtcToMst(DateTime.UtcNow).Date;
                    if (!string.IsNullOrEmpty(Request["date"])) app.appointmentDate = DateTime.Parse(Request["date"]).Date;
                    app.startTime = Globals.TimeOnly(9, 0, 0);
                    if (!string.IsNullOrEmpty(Request["start"])) app.startTime = Globals.TimeOnly(DateTime.Parse(Request["start"]));
                    if (!string.IsNullOrEmpty(Request["end"])) app.endTime = Globals.TimeOnly(DateTime.Parse(Request["end"]));
                    else app.endTime = app.startTime + Database.GetCustomerAverageHoursPerContractor(customer.customerID);
                    if (app.startTime == app.endTime) app.endTime = app.startTime + TimeSpan.FromHours(2);
                    app.contractorID = contractor.contractorID;
                    app.appType = Globals.SafeIntParse(Request["appType"]);
                    if (app.appType <= 0) app.appType = 1;

                    if (contractor.contractorType == 0 || (contractor.contractorType & 1) != 0)
                    {
                        decimal hours = (decimal)(app.endTime - app.startTime).TotalHours;
                        app.contractorHours = hours;
                        app.customerHours = hours;
                        app.customerServiceFee = customer.serviceFee;
                    }

                    PaymentButton.Visible = false;
                }
                else
                {
                    int replaceID = Globals.SafeIntParse(Request["ReplaceConID"]);
                    if (replaceID > 0)
                    {
                        app.contractorID = replaceID;
                        if (!string.IsNullOrEmpty(Request["start"])) app.startTime = Globals.TimeOnly(DateTime.Parse(Request["start"]));
                        if (!string.IsNullOrEmpty(Request["end"])) app.endTime = Globals.TimeOnly(DateTime.Parse(Request["end"]));
                    }
                }

                TitleLink.Text = app.customerTitle;
                TitleLink.CommandArgument = @"Customers.aspx?custID=" + app.customerID;
                TitleLabel.Text = @" - Scheduled Appointment";
                CustomerServiceFee.InnerHtml = "<b>Customer Service Fee: </b>" + Globals.FormatMoney(app.combinedFee) + " <b>Referral Discount: </b>" + Globals.FormatPercent(app.customerDiscountReferral);

                AppDate.Text = app.appointmentDate.ToString("d");
                AppStatus.SelectedIndex = app.appStatus;

                HourlyRate.Text = Globals.FormatMoney(app.customerRate);
                DiscountPercent.Text = Globals.FormatPercent(app.customerDiscountPercent);
                DiscountReferral.Value = Globals.FormatPercent(app.customerDiscountReferral);
                SalesTax.Text = Globals.FormatPercent(app.salesTax, false);
                if (app.salesTax == 0) SalesTax.Enabled = false;

                List<AppStruct> sameApps = new List<AppStruct>();
                sameApps.Add(app);

                //if (app.appointmentID != 0 || !string.IsNullOrEmpty(Request["conID"]))
                if (app.appointmentID != 0 || (Session["IsAdd"] != null && (bool)Session["IsAdd"]))
                {
                    List<AppStruct> relatedApps = new List<AppStruct>();
                    if (!string.IsNullOrEmpty(app.RelatedAppointments))
                    {
                        List<int> appIds = app.RelatedAppointments.Split(',')?.Select(Int32.Parse).ToList();
                        appIds.Sort();
                        List<int> newAppIds = appIds;
                        var areSameSet = true;
                        do
                        {
                            newAppIds = Database.GetRelatedApointmentIDs(appIds);
                            areSameSet = AreSameSets(newAppIds, appIds);
                            appIds = newAppIds;
                        } while (!areSameSet);

                        newAppIds = newAppIds.Distinct().ToList();
                        newAppIds.Remove(appID);

                        foreach (var item in newAppIds)
                        {
                            AppStruct fetchedApp = new AppStruct();
                            Database.GetApointmentpByID(Globals.GetFranchiseMask(), item, out fetchedApp);
                            if (relatedApps.Find(x => x.appointmentID == fetchedApp.appointmentID).appointmentID <= 0 && fetchedApp.appointmentID > 0)
                            {
                                relatedApps.Add(fetchedApp);
                            }
                        }

                    }
                    sameApps.AddRange(relatedApps);
                    // sameApps.AddRange(Database.GetSameApointments(app));

                    //if (!string.IsNullOrEmpty(Request["conID"]))
                    if (Session["IsAdd"] != null && (bool)Session["IsAdd"])
                    {
                        AppStruct addApp = new AppStruct();
                        //addApp.contractorID = Globals.SafeIntParse(Request["conID"]);
                        addApp.contractorID = 0;
                        addApp.appType = Globals.SafeIntParse(Request["appType"]);
                        if (addApp.appType <= 0) addApp.appType = 1;
                        addApp.startTime = app.startTime;
                        addApp.endTime = app.endTime;
                        decimal hours = (decimal)(app.endTime - app.startTime).TotalHours;
                        addApp.contractorHours = hours;
                        addApp.customerHours = hours;

                        for (int i = 0; i < sameApps.Count; i++)
                        {
                            if (sameApps[i].contractorID == addApp.contractorID)
                                addApp.remove = true;
                        }

                        if (!addApp.remove)
                            sameApps.Add(addApp);
                    }

                    //if (sameApps.Count > 1 && !string.IsNullOrEmpty(Request["conID"]))
                    if (sameApps.Count > 1 && (Session["IsAdd"] != null && (bool)Session["IsAdd"]))
                    {
                        int houseKeepingCount = 0;
                        for (int i = 0; i < sameApps.Count; i++)
                        {
                            if (sameApps[i].appType == 1) houseKeepingCount++;
                        }

                        for (int i = 0; i < sameApps.Count; i++)
                        {
                            if (sameApps[i].appType == 1)
                            {
                                AppStruct same = sameApps[i];
                                same.customerServiceFee = app.combinedFee / houseKeepingCount;
                                sameApps[i] = same;
                            }
                        }
                    }
                }


                decimal serviceFee = 0;
                decimal hoursBilled = 0;
                decimal tips = 0;
                decimal discountAmount = 0;

                List<string> adjustmentTypes = Database.GetFranchiseDropDown(Globals.GetFranchiseMask(), "adjustmentList");

                for (int i = 0; i < sameApps.Count; i++)
                {
                    serviceFee += sameApps[i].customerServiceFee;
                    if (sameApps[i].appType == 1) hoursBilled += sameApps[i].customerHours;
                    tips += sameApps[i].contractorTips;
                    discountAmount += sameApps[i].customerDiscountAmount;

                    List<JobLogsStruct> jobLogs = Database.GetJobLogs(sameApps[i].appointmentID, sameApps[i].contractorID);
                    jobLogs = jobLogs.FindAll(x => x.CustomerId == sameApps[i].customerID);


                    ((TableRow)Globals.FindControlRecursive(ContractorTable, "ContractorRow_" + i)).Style["display"] = "table-row";
                    ((HiddenField)Globals.FindControlRecursive(ContractorTable, "ContractorAppID_" + i)).Value = sameApps[i].appointmentID.ToString();
                    ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorStartTime_" + i)).Text = sameApps[i].startTime.ToString("t");
                    ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorEndTime_" + i)).Text = sameApps[i].endTime.ToString("t");
                    ((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorHours_" + i)).Text = Globals.FormatHours(sameApps[i].contractorHours);
                    ((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorServiceFee_" + i)).Text = Globals.FormatMoney(sameApps[i].appType == 1 ? sameApps[i].customerServiceFee : sameApps[i].customerSubContractor);
                    ((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorTips_" + i)).Text = Globals.FormatHours(sameApps[i].contractorTips);
                    ((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorAdjustment_" + i)).Text = Globals.FormatMoney(sameApps[i].contractorAdjustAmount);

                    ((TableRow)Globals.FindControlRecursive(ContractorTable, "ContractorRowExtra_" + i)).Style["display"] = "table-row";
                    ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorRecurrenceType_" + i)).SelectedIndex = sameApps[i].recurrenceType;
                    ((TextBox)Globals.FindControlRecursive(ContractorTable, "ContractorWeekFreqeuncy_" + i)).Text = sameApps[i].weeklyFrequency.ToString();
                    ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorWeekOfMonth_" + i)).SelectedIndex = sameApps[i].monthlyWeek;
                    ((DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorDayOfWeek_" + i)).SelectedIndex = sameApps[i].monthlyDay;

                    DropDownList contractorDropDown = (DropDownList)Globals.FindControlRecursive(ContractorTable, "Contractor_" + i);
                    ContractorStruct contractor;
                    contractorDropDown.Items.Clear();
                    contractorDropDown.Items.Add(new ListItem("", "0"));
                    contractorDropDown.Items.AddRange(Globals.GetContractorList(app.franchiseMask, sameApps[i].contractorID, sameApps[i].appType, out contractor, true, false));

                    DropDownList adjustmentTypeDropDown = (DropDownList)Globals.FindControlRecursive(ContractorTable, "ContractorAdjustmentType_" + i);
                    adjustmentTypeDropDown.Items.Clear();
                    foreach (string type in adjustmentTypes)
                        adjustmentTypeDropDown.Items.Add(type);
                    adjustmentTypeDropDown.SelectedValue = sameApps[i].contractorAdjustType;



                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label lbl = new Label();
                    lbl.ID = "ContNamelbl" + i;
                    lbl.Text = "Name: ";
                    PlaceHolder1.Controls.Add(lbl);

                    // Create a Label
                    Label ContractorName = new Label();
                    ContractorName.ID = "ContName" + i;
                    ContractorName.Text = sameApps[i].contractorTitle;
                    PlaceHolder1.Controls.Add(ContractorName);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));


                    // Create a Label
                    Label locationSharelbl = new Label();
                    locationSharelbl.ID = "locationSharelbl" + i;
                    locationSharelbl.Text = "Location Sharing: ";
                    PlaceHolder1.Controls.Add(locationSharelbl);

                    // Create a Label
                    Label locationShare = new Label();
                    locationShare.ID = "locationShare" + i;
                    locationShare.Text = contractor.ShareLocation ? "Yes" : "No";
                    locationShare.Style["Color"] = contractor.ShareLocation ? "Green" : "Red";
                    PlaceHolder1.Controls.Add(locationShare);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label startTimelbl = new Label();
                    startTimelbl.ID = "StartJobTimelbl" + i;
                    startTimelbl.Text = "Start Job time: ";
                    PlaceHolder1.Controls.Add(startTimelbl);

                    // Create a Label
                    Label StartJobTime = new Label();
                    StartJobTime.ID = "StartJobTime" + i;
                    TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    if (sameApps[i].jobStartTime != null)
                        StartJobTime.Text = sameApps[i].jobStartTime.Value.ToShortTimeString();
                    StartJobTime.Style["Color"] = "Green";
                    PlaceHolder1.Controls.Add(StartJobTime);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label endTimelbl = new Label();
                    endTimelbl.ID = "endJobTimelbl" + i;
                    endTimelbl.Text = "Finish Job time: ";
                    PlaceHolder1.Controls.Add(endTimelbl);

                    // Create a Label
                    Label endJobTime = new Label();
                    endJobTime.ID = "endJobTime" + i;
                    if (sameApps[i].jobEndTime != null)
                        endJobTime.Text = sameApps[i].jobEndTime.Value.ToShortTimeString();
                    endJobTime.Style["Color"] = "Green";
                    PlaceHolder1.Controls.Add(endJobTime);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label jobCompletedlbl = new Label();
                    jobCompletedlbl.ID = "jobCompletedlbl" + i;
                    jobCompletedlbl.Text = "Job Completed: ";
                    PlaceHolder1.Controls.Add(jobCompletedlbl);

                    // Create a Label
                    Label jobCompleted = new Label();
                    jobCompleted.ID = "jobCompleted" + i;
                    jobCompleted.Text = sameApps[i].jobEndTime.HasValue ? "Yes" : "No";
                    jobCompleted.Style["Color"] = sameApps[i].jobEndTime.HasValue ? "Green" : "Red";
                    PlaceHolder1.Controls.Add(jobCompleted);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label AreasCompletedlbl = new Label();
                    AreasCompletedlbl.ID = "AreasCompletedlbl" + i;
                    AreasCompletedlbl.Text = "Areas Completed: ";
                    PlaceHolder1.Controls.Add(AreasCompletedlbl);
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    foreach (JobLogsStruct jl in jobLogs)
                    {
                        if (jl.ContractorId != sameApps[i].contractorID)
                        {
                            continue;
                        }
                        string acText = "   •  " + jl.Content.Trim();
                        if (!string.IsNullOrEmpty(jl.SubContent))
                        {
                            acText += (" - " + jl.SubContent);
                        }
                        Label AreasCompleted = new Label();
                        AreasCompleted.ID = "AreasCompleted" + i + jl.id;
                        AreasCompleted.Text = acText;
                        PlaceHolder1.Controls.Add(AreasCompleted);
                        if (jl.CreatedAt != null || !string.IsNullOrEmpty(jl.CreatedAtStr))
                        {
                            Label Timestamp = new Label();
                            Timestamp.Style["Color"] = "Blue";
                            Timestamp.Text = " (" + (string.IsNullOrEmpty(jl.CreatedAtStr) ? jl.CreatedAt.ToShortTimeString() : jl.CreatedAtStr) + ")";
                            PlaceHolder1.Controls.Add(Timestamp);
                        }
                        PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    }
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    // Create a Label
                    Label Noteslbl = new Label();
                    Noteslbl.ID = "Noteslbl" + i;
                    Noteslbl.Text = "Notes: ";
                    PlaceHolder1.Controls.Add(Noteslbl);

                    // Create a Label
                    Label Notes = new Label();
                    Notes.ID = "Notes" + i;
                    Notes.Text = sameApps[i].Notes;
                    PlaceHolder1.Controls.Add(Notes);

                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));


                    // Create a Label
                    Label Pictureslbl = new Label();
                    Pictureslbl.ID = "Pictures" + i;
                    Pictureslbl.Text = "Pictures: ";
                    PlaceHolder1.Controls.Add(Pictureslbl);


                    foreach (var img in Database.GetAppointmentAttachments(sameApps[i].appointmentID))
                    {

                        var fileName = Path.GetFileName(img.ImageURL);
                        var imageUrl = "../../ContratorPics/" + fileName;
                        var imgNew = new ImageButton
                        {
                            ImageUrl = imageUrl,
                            CssClass = "image",
                            OnClientClick = $"previewImage('{imageUrl}'); return false;"
                        };

                        // Create a Download Button
                        Button btnDownload = new Button();
                        btnDownload.ID = "btnDownload" + img.id;
                        btnDownload.Text = "⇩";
                        btnDownload.CssClass = "image-button download-button";
                        btnDownload.OnClientClick = $"downloadImage('{imageUrl}'); return false;";

                        PlaceHolder1.Controls.Add(imgNew);
                        PlaceHolder1.Controls.Add(btnDownload);

                    }


                    // Optionally add a line break
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<hr>"));


                    // Optionally add a line break
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<br/>"));

                }




                HoursBilled.Text = Globals.FormatHours(hoursBilled);
                ServiceFee.Text = Globals.FormatMoney(Math.Round(serviceFee, 1));
                Tips.Text = Globals.FormatMoney(tips);
                DiscountAmount.Text = Globals.FormatMoney(discountAmount);

            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadAppointment EX: " + ex.Message;
            }
        }

        #endregion

        #region Helper

        public bool AreSameSets(List<int> set1, List<int> set2)
        {
            set1 = set1.Distinct().ToList();
            set2 = set2.Distinct().ToList();
            if (set1.Count != set2.Count) { return false; }
            else
            {
                foreach (int item1 in set1)
                {
                    if (!set2.Contains(item1)) { return false; }
                }
            }
            return true;
        }

        #endregion
    }
}