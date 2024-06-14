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
    public partial class Payments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Payments";

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetNoStore();
                Session["trans_custID"] = null;

                if (!IsPostBack)
                {
                    int selectedMask = Request.Cookies["PaymentsMask"] != null ? Globals.SafeIntParse(Request.Cookies["PaymentsMask"].Value) : -1;
                    if (selectedMask == 0) selectedMask = -1;
                    foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.ID = "FRANCHECK" + franchise.Value;
                        checkBox.Text = franchise.Text;
                        checkBox.Checked = franchise.Selected;
                        MenuPanel.Controls.AddAt(MenuPanel.Controls.Count - 2, checkBox);
                    }

                    DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                    DateTime startDate = Globals.SafeDateParse(Request["StartDate"]);
                    DateTime endDate = Globals.SafeDateParse(Request["EndDate"]);
                    if (startDate == DateTime.MinValue) startDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsStartDate"));
                    if (endDate == DateTime.MinValue) endDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsEndDate"));
                    if (startDate == DateTime.MinValue) startDate = mst;
                    if (endDate == DateTime.MinValue) endDate = mst;
                    StartDate.Text = startDate.ToString("d");
                    EndDate.Text = endDate.ToString("d");
                    Response.Cookies.Add(new HttpCookie("PaymentsStartDate", StartDate.Text));
                    Response.Cookies.Add(new HttpCookie("PaymentsEndDate", EndDate.Text));


                    if (string.IsNullOrEmpty(Request["Search"]))
                    {
                        if (Globals.GetCookieValue("PaymentsUnpaidOnly") == "yes")
                            UnpaidButton.ForeColor = Color.Green;
                        if (Globals.GetCookieValue("PaymentsCashCheckOnly") == "yes")
                            CashCheckButton.ForeColor = Color.Green;
                        if (Globals.GetCookieValue("PaymentsCreditCardOnly") == "yes")
                            CreditCardButton.ForeColor = Color.Green;
                    }
                    else
                    {
                        Globals.DeleteCookie("PaymentsUnpaidOnly");
                        Globals.DeleteCookie("PaymentsCashCheckOnly");
                        Globals.DeleteCookie("PaymentsCreditCardOnly");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void TodayClick(object sender, EventArgs e)
        {
            try
            {
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                StartDate.Text = mst.ToString("d");
                EndDate.Text = mst.ToString("d");
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TodayClick EX: " + ex.Message;
            }
        }

        public void TomorrowClick(object sender, EventArgs e)
        {
            try
            {
                DateTime tomorrow = Globals.UtcToMst(DateTime.UtcNow) + TimeSpan.FromDays(1);
                StartDate.Text = tomorrow.ToString("d");
                EndDate.Text = tomorrow.ToString("d");
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "TomorrowClick EX: " + ex.Message;
            }
        }

        public void YesterdayClick(object sender, EventArgs e)
        {
            try
            {
                DateTime yesterday = Globals.UtcToMst(DateTime.UtcNow) - TimeSpan.FromDays(1);
                StartDate.Text = yesterday.ToString("d");
                EndDate.Text = yesterday.ToString("d");
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "YesterdayClick EX: " + ex.Message;
            }
        }

        public void ThisWeekClick(object sender, EventArgs e)
        {
            try
            {
                DateTime weekStart = Globals.StartOfWeek(Globals.UtcToMst(DateTime.UtcNow));
                StartDate.Text = weekStart.ToString("d");
                EndDate.Text = (weekStart + TimeSpan.FromDays(6)).ToString("d"); 
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ThisWeekClick EX: " + ex.Message;
            }
        }

        public void LastWeekClick(object sender, EventArgs e)
        {
            try
            {
                DateTime weekStart = Globals.StartOfWeek(Globals.UtcToMst(DateTime.UtcNow));
                StartDate.Text = (weekStart - TimeSpan.FromDays(7)).ToString("d");
                EndDate.Text = (weekStart - TimeSpan.FromDays(1)).ToString("d");
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LastWeekClick EX: " + ex.Message;
            }
        }

        public void Last30DaysClick(object sender, EventArgs e)
        {
            try
            {
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                StartDate.Text = (mst - TimeSpan.FromDays(30)).ToString("d");
                EndDate.Text = mst.ToString("d");
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Last30DaysClick EX: " + ex.Message;
            }
        }

        public void UnpaidClick(object sender, EventArgs e)
        {
            try
            {
                string value = Globals.GetCookieValue("PaymentsUnpaidOnly");
                value = value == "yes" ? "no" : "yes";
                Globals.SetCookieValue("PaymentsUnpaidOnly", value);
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "UnpaidClick EX: " + ex.Message;
            }
        }

        public void CashCheckClick(object sender, EventArgs e)
        {
            try
            {
                Globals.DeleteCookie("PaymentsCreditCardOnly");
                string value = Globals.GetCookieValue("PaymentsCashCheckOnly");
                value = value == "yes" ? "no" : "yes";
                Globals.SetCookieValue("PaymentsCashCheckOnly", value);
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CashCheckClick EX: " + ex.Message;
            }
        }

        public void CreditCardClick(object sender, EventArgs e)
        {
            try
            {
                Globals.DeleteCookie("PaymentsCashCheckOnly");
                string value = Globals.GetCookieValue("PaymentsCreditCardOnly");
                value = value == "yes" ? "no" : "yes";
                Globals.SetCookieValue("PaymentsCreditCardOnly", value);
                ApplyButtonClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CreditCardClick EX: " + ex.Message;
            }
        }

        public void ApplyButtonClick(object sender, EventArgs e)
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

                Globals.SetCookieValue("ClearSearch", "Clear");
                Response.Cookies.Add(new HttpCookie("PaymentsMask", selectedMask.ToString()));

                string url = Globals.BuildQueryString("Payments.aspx", "StartDate", StartDate.Text);
                url = Globals.BuildQueryString(url, "EndDate", EndDate.Text);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ApplyButtonClick EX: " + ex.Message;
            }
        }

        public void GetPaymentsTableHTML()
        {
            DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
            DateTime startDate = Globals.SafeDateParse(Request["StartDate"]);
            DateTime endDate = Globals.SafeDateParse(Request["EndDate"]);
            if (startDate == DateTime.MinValue) startDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsStartDate"));
            if (endDate == DateTime.MinValue) endDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsEndDate"));
            if (startDate == DateTime.MinValue) startDate = mst;
            if (endDate == DateTime.MinValue) endDate = mst;

            bool unpaidOnly = Globals.GetCookieValue("PaymentsUnpaidOnly") == "yes";
            bool cashCheckOnly = Globals.GetCookieValue("PaymentsCashCheckOnly") == "yes";
            bool creditCardOnly = Globals.GetCookieValue("PaymentsCreditCardOnly") == "yes";

            if (!string.IsNullOrEmpty(Request["Search"]))
                unpaidOnly = cashCheckOnly = creditCardOnly = false;

            int selectedMask = Request.Cookies["PaymentsMask"] != null ? Globals.SafeIntParse(Request.Cookies["PaymentsMask"].Value) : Globals.GetFranchiseMask();

            List<DBRow> invoiceRangeList = Database.GetInvoiceRangeByDateRange(startDate, endDate);
            Lookup<int, DBRow> invoiceRangeLookup = (Lookup<int, DBRow>)invoiceRangeList.ToLookup(p => p.GetInt("customerID"), p => p);

            SortedList<DateTime, SortedList<int, List<AppStruct>>> dictApp = new SortedList<DateTime, SortedList<int, List<AppStruct>>>();
            List<AppStruct> appList = Database.GetAppsByDateRange(selectedMask, startDate, endDate, @"A.appointmentDate, A.startTime, A.endTime", false);
            foreach (AppStruct app in appList)
            {
                if (!dictApp.ContainsKey(app.appointmentDate)) dictApp.Add(app.appointmentDate, new SortedList<int, List<AppStruct>>());
                if (!dictApp[app.appointmentDate].ContainsKey(app.customerID)) dictApp[app.appointmentDate].Add(app.customerID, new List<AppStruct>());
                dictApp[app.appointmentDate][app.customerID].Add(app);
            }

            SortedList<DateTime, SortedList<int, List<TransactionStruct>>> dictTrans = new SortedList<DateTime, SortedList<int, List<TransactionStruct>>>();
            List<TransactionStruct> transList = Database.GetTransactions(selectedMask, 0, startDate, endDate, "T.dateCreated");
            foreach (TransactionStruct trans in transList)
            {
                if (trans.auth != 3)
                {
                    if (!dictTrans.ContainsKey(trans.dateApply)) dictTrans.Add(trans.dateApply, new SortedList<int, List<TransactionStruct>>());
                    if (!dictTrans[trans.dateApply].ContainsKey(trans.customerID)) dictTrans[trans.dateApply].Add(trans.customerID, new List<TransactionStruct>());
                    dictTrans[trans.dateApply][trans.customerID].Add(trans);
                }
            }

            foreach (DateTime appDate in dictApp.Keys)
            {
                foreach (int customerID in dictApp[appDate].Keys)
                {
                    bool oldPaid = false;
                    decimal dayAppTotal = 0;
                    decimal dayAppHours = 0;
                    decimal dayAppRate = decimal.MinValue;
                    decimal dayAppServiceFee = 0;
                    decimal dayAppSubContractorCC = 0;
                    decimal dayAppSubContractorWW = 0;
                    decimal dayAppSubContractorHW = 0;
                    decimal dayAppSubContractorCL = 0;
                    decimal dayAppTips = 0;
                    decimal dayAppDiscountAmount = 0;
                    decimal dayAppDiscountPercent = decimal.MinValue;
                    decimal dayAppDiscountReferral = decimal.MinValue;
                    decimal dayAppSalesTax = decimal.MinValue;

                    string itemString = "";
                    string serviceFeeString = "";
                    string subContractorString = "";
                    string hoursString = "";
                    string tipsString = "";
                    string discountAmountString = "";
                    string discountPercentString = "";
            
                    foreach (AppStruct appStruct in dictApp[appDate][customerID])
                    {
                        string status = "";
                        if (appStruct.appStatus == 1) status = " (Rescheduled)";
                        if (appStruct.appStatus == 2) status = " (Canceled)";

                        itemString += @"<div class=""PaymentContractorItem""><a href=""Appointments.aspx?appID=" + appStruct.appointmentID + @""">" + appStruct.contractorTitle + status + @"</a></div>";
                        serviceFeeString += @"<div class=""PaymentContractorItem"">" + Globals.FormatMoney(appStruct.customerServiceFee) + @"</div>";
                        subContractorString += @"<div class=""PaymentContractorItem"">" + Globals.FormatMoney(appStruct.customerSubContractor) + @"</div>";
                        hoursString += @"<div class=""PaymentContractorItem"">" + Globals.FormatHours(appStruct.appType == 1 ? appStruct.customerHours : 0) + @" (" + Globals.FormatMoney(appStruct.customerRate) + @")</div>";
                        tipsString += @"<div class=""PaymentContractorItem"">" + Globals.FormatMoney(appStruct.contractorTips) + @"</div>";
                        discountAmountString += @"<div class=""PaymentContractorItem"">" + Globals.FormatMoney(appStruct.customerDiscountAmount) + @"</div>";
                        discountPercentString += @"<div class=""PaymentContractorItem"">" + Globals.FormatPercent(appStruct.customerDiscountPercent + appStruct.customerDiscountReferral) + @"</div>";

                        if (appStruct.appStatus == 0)
                        {
                            if (appStruct.paymentFinished) oldPaid = true;
                            if (appStruct.appType == 1) dayAppHours += appStruct.customerHours;
                            dayAppServiceFee += appStruct.customerServiceFee;
                            if (appStruct.appType == 2) dayAppSubContractorCC += appStruct.customerSubContractor;
                            if (appStruct.appType == 3) dayAppSubContractorWW += appStruct.customerSubContractor;
                            if (appStruct.appType == 4) dayAppSubContractorHW += appStruct.customerSubContractor;
                            if (appStruct.appType == 5) dayAppSubContractorCL += appStruct.customerSubContractor;
                            dayAppTips += appStruct.contractorTips;
                            dayAppDiscountAmount += appStruct.customerDiscountAmount;
                            dayAppTotal += Globals.CalculateAppointmentTotal(appStruct);

                            if (dayAppRate != decimal.MaxValue)
                            {
                                if (dayAppRate == decimal.MinValue) dayAppRate = appStruct.customerRate;
                                else if (dayAppRate != appStruct.customerRate) dayAppRate = decimal.MaxValue;
                            }

                            if (dayAppDiscountPercent != decimal.MaxValue)
                            {
                                if (dayAppDiscountPercent == decimal.MinValue) dayAppDiscountPercent = appStruct.customerDiscountPercent;
                                else if (dayAppDiscountPercent != appStruct.customerDiscountPercent) dayAppDiscountPercent = decimal.MaxValue;
                            }

                            if (dayAppDiscountReferral != decimal.MaxValue)
                            {
                                if (dayAppDiscountReferral == decimal.MinValue) dayAppDiscountReferral = appStruct.customerDiscountReferral;
                                else if (dayAppDiscountReferral != appStruct.customerDiscountReferral) dayAppDiscountReferral = decimal.MaxValue;
                            }

                            if (dayAppSalesTax != decimal.MaxValue)
                            {
                                if (dayAppSalesTax == decimal.MinValue) dayAppSalesTax = appStruct.salesTax;
                                else if (dayAppSalesTax != appStruct.salesTax) dayAppSalesTax = decimal.MaxValue;
                            }
                        }
                    }

                    //Fix Service Fee Split Issue
                    dayAppTotal -= (dayAppServiceFee - Math.Round(dayAppServiceFee, 1));
                    dayAppServiceFee = Math.Round(dayAppServiceFee, 1);

                    int dayTransCount = 0;
                    decimal dayTransTotal = 0;
                    decimal dayTransHours = 0;
                    decimal dayTransRate = decimal.MinValue;
                    decimal dayTransServiceFee = 0;
                    decimal dayTransSubContractorCC = 0;
                    decimal dayTransSubContractorWW = 0;
                    decimal dayTransSubContractorHW = 0;
                    decimal dayTransSubContractorCL = 0;
                    decimal dayTransTips = 0;
                    decimal dayTransDiscountAmount = 0;
                    decimal dayTransDiscountPercent = decimal.MinValue;
                    decimal dayTransDiscountReferral = decimal.MinValue;

                    if (dictTrans.ContainsKey(appDate) && dictTrans[appDate].ContainsKey(customerID))
                    {
                        foreach (TransactionStruct transStruct in dictTrans[appDate][customerID])
                        {
                            string text = transStruct.paymentType;
                            string itemClass = "PaymentSaleItem";
                            if (transStruct.auth == 1)
                            {
                                itemClass = "PaymentOpenAuthItem";
                                text = "Open Auth";
                            }
                            if (transStruct.transType == "Return")
                            {
                                itemClass = "PaymentReturnItem";
                            }
                            if (transStruct.transType == "Invoice")
                            {
                                itemClass = "PaymentInvoiceItem";
                                text = "Invoice";
                            }
                            if (transStruct.isVoid)
                            {
                                itemClass = "PaymentVoidItem";
                                text = "Voided";
                            }
                            text += " " + transStruct.transID + " " + transStruct.dateCreated.ToString("d");

                            itemString += @"<div class=""" + itemClass + @"""><a href=""Transaction.aspx?transID=" + transStruct.transID + @""">" + text + @"</a></div>";
                            serviceFeeString += @"<div class=""" + itemClass + @""">" + Globals.FormatMoney(transStruct.serviceFee) + @"</div>";
                            subContractorString += @"<div class=""" + itemClass + @""">" + Globals.FormatMoney(transStruct.subContractorCC + transStruct.subContractorWW + transStruct.subContractorHW) + @"</div>";
                            hoursString += @"<div class=""" + itemClass + @""">" + Globals.FormatHours(transStruct.hoursBilled) + @" (" + Globals.FormatMoney(transStruct.hourlyRate) + @")</div>";
                            tipsString += @"<div class=""" + itemClass + @""">" + Globals.FormatMoney(transStruct.tips) + @"</div>";
                            discountPercentString += @"<div class=""" + itemClass + @""">" + Globals.FormatPercent(transStruct.discountPercent + transStruct.discountReferral) + @"</div>";
                            discountAmountString += @"<div class=""" + itemClass + @""">" + Globals.FormatMoney(transStruct.discountAmount) + @"</div>";

                            if (!transStruct.isVoid && !transStruct.IsAuth())
                            {
                                if (transStruct.transType == "Return")
                                {
                                    dayTransTotal -= transStruct.total;
                                    dayTransHours -= transStruct.hoursBilled;
                                    dayTransServiceFee -= transStruct.serviceFee;
                                    dayTransSubContractorCC -= transStruct.subContractorCC;
                                    dayTransSubContractorWW -= transStruct.subContractorWW;
                                    dayTransSubContractorHW -= transStruct.subContractorHW;
                                    dayTransSubContractorCL -= transStruct.subContractorCL;
                                    dayTransTips -= transStruct.tips;
                                    dayTransDiscountAmount -= transStruct.discountAmount;
                                }
                                if (transStruct.transType == "Sale")
                                {
                                    dayTransTotal += transStruct.total;
                                    dayTransHours += transStruct.hoursBilled;
                                    dayTransServiceFee += transStruct.serviceFee;
                                    dayTransSubContractorCC += transStruct.subContractorCC;
                                    dayTransSubContractorWW += transStruct.subContractorWW;
                                    dayTransSubContractorHW += transStruct.subContractorHW;
                                    dayTransSubContractorCL += transStruct.subContractorCL;
                                    dayTransTips += transStruct.tips;
                                    dayTransDiscountAmount += transStruct.discountAmount;
                                }
                            }

                            dayTransCount++;
                            if (dayTransRate != decimal.MaxValue)
                            {
                                if (dayTransRate == decimal.MinValue) dayTransRate = transStruct.hourlyRate;
                                else if (dayTransRate != transStruct.hourlyRate) dayTransRate = decimal.MaxValue;
                            }

                            if (dayTransDiscountPercent != decimal.MaxValue)
                            {
                                if (dayTransDiscountPercent == decimal.MinValue) dayTransDiscountPercent = transStruct.discountPercent;
                                else if (dayTransDiscountPercent != transStruct.discountPercent) dayTransDiscountPercent = decimal.MaxValue;
                            }

                            if (dayTransDiscountReferral != decimal.MaxValue)
                            {
                                if (dayTransDiscountReferral == decimal.MinValue) dayTransDiscountReferral = transStruct.discountReferral;
                                else if (dayTransDiscountReferral != transStruct.discountReferral) dayTransDiscountReferral = decimal.MaxValue;
                            }
                        }
                    }

                    foreach (var invoiceRange in invoiceRangeLookup[customerID])
                    {
                        if (appDate >= invoiceRange.GetDate("startDate") && appDate <= invoiceRange.GetDate("endDate"))
                        {
                            itemString += @"<div class=""PaymentInvoiceItem"">Bulk Invoice " + invoiceRange.GetInt("invoiceID") + @"</div>";
                            serviceFeeString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            subContractorString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            hoursString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            tipsString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            discountPercentString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            discountAmountString += @"<div class=""PaymentInvoiceItem"">-</div>";
                        }
                    }

                    decimal balance = decimal.Round(dayAppTotal - dayTransTotal, 2);
                    if (oldPaid) balance = 0;

                    if (unpaidOnly && balance == 0)
                        continue;

                    string invoiceHTML =
                    @"<tr class=""gradeX"">
                        <td>
                            {0}
                        </td>
                        <td>
                            <a href=""Customers.aspx?custID={1}"">{2}</a>
                        </td>
                        <td>
                            {3}
                        </td>
                        <td>
                            {4}
                        </td>
                        <td>
                            {5}
                        </td>
                        <td>
                            {6}
                        </td>
                        <td>
                            {7}
                        </td>
                        <td>
                            {8}
                        </td>
                        <td>
                            {9}
                        </td>
                        <td>
                            {10}
                        </td>
                        <td>
                            {11}
                        </td>
                        <td>
                            {12}
                        </td>
                        <td>
                            {13}
                        </td>
                    </tr>";

                    AppStruct firstApp = dictApp[appDate][customerID][0];

                    if (cashCheckOnly && !firstApp.customerPaymentType.ToLower().Contains("check") && !firstApp.customerPaymentType.ToLower().Contains("cash") && !firstApp.customerPaymentType.ToLower().Contains("points")) 
                        continue;

                    if (creditCardOnly && !Globals.IsPaymentCreditCard(firstApp.customerPaymentType))
                        continue;

                    string rate = (dayAppRate == decimal.MaxValue || dayAppRate == decimal.MinValue) ? "E" : dayAppRate.ToString();
                    if (dayTransRate != decimal.MinValue && dayAppRate != dayTransRate) rate = "E";

                    string discountPercent = (dayAppDiscountPercent == decimal.MaxValue || dayAppDiscountPercent == decimal.MinValue) ? "E" : dayAppDiscountPercent.ToString();
                    //if (dayTransDiscountPercent != decimal.MinValue && dayAppDiscountPercent != dayTransDiscountPercent) discountPercent = "E"; //THIS MAY HAVE BROKEN THINGS

                    string discountReferral = (dayAppDiscountReferral == decimal.MaxValue || dayAppDiscountReferral == decimal.MinValue) ? "E" : dayAppDiscountReferral.ToString();
                    if (dayTransDiscountReferral != decimal.MinValue && dayAppDiscountReferral != dayTransDiscountReferral) discountReferral = "E";

                    string salesTax = (dayAppSalesTax == decimal.MaxValue || dayAppSalesTax == decimal.MinValue) ? "E" : dayAppSalesTax.ToString();

                    string chargeQuery = Globals.BuildQueryString("Transaction.aspx", "custID", customerID);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "transDate", appDate.ToString("d"));
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "hoursBilled", dayAppHours - dayTransHours);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "hourlyRate", rate);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "serviceFee", dayAppServiceFee - dayTransServiceFee);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subCC", dayAppSubContractorCC - dayTransSubContractorCC);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subWW", dayAppSubContractorWW - dayTransSubContractorWW);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subHW", dayAppSubContractorHW - dayTransSubContractorHW);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subCL", dayAppSubContractorCL - dayTransSubContractorCL);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountAmount", dayAppDiscountAmount - dayTransDiscountAmount);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountPercent", discountPercent);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountReferral", discountReferral);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "salesTax", salesTax);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "tips", dayAppTips - dayTransTips);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "total", dayAppTotal - dayTransTotal);

                    string chargeLink = @"<a class=""paymentButton"" href=""" + Globals.BuildQueryString(chargeQuery, "transType", "Sale") + @""">Pay</a>";
                    string invoiceLink = @"<a class=""paymentButton"" href=""" + Globals.BuildQueryString(chargeQuery, "transType", "Invoice") + @""">Invoice</a>";

                    rate = (dayTransRate == decimal.MaxValue || dayTransRate == decimal.MinValue) ? "E" : dayTransRate.ToString();
                    discountPercent = (dayTransDiscountPercent == decimal.MaxValue || dayTransDiscountPercent == decimal.MinValue) ? "E" : dayTransDiscountPercent.ToString();
                    discountReferral = (dayTransDiscountReferral == decimal.MaxValue || dayTransDiscountReferral == decimal.MinValue) ? "E" : dayTransDiscountReferral.ToString();

                    string returnQuery = Globals.BuildQueryString("Transaction.aspx", "custID", customerID);
                    if (balance >= 0)
                    {
                        returnQuery = Globals.BuildQueryString(returnQuery, "transType", "Return");
                        returnQuery = Globals.BuildQueryString(returnQuery, "transDate", appDate.ToString("d"));
                        returnQuery = Globals.BuildQueryString(returnQuery, "hoursBilled", dayTransHours);
                        returnQuery = Globals.BuildQueryString(returnQuery, "hourlyRate", rate);
                        returnQuery = Globals.BuildQueryString(returnQuery, "serviceFee", dayTransServiceFee);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCC", dayTransSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subWW", dayTransSubContractorWW);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subHW", dayTransSubContractorHW);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCL", dayTransSubContractorCL);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountAmount", dayTransDiscountAmount);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountPercent", discountPercent);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountReferral", discountReferral);
                        returnQuery = Globals.BuildQueryString(returnQuery, "salesTax", salesTax);
                        returnQuery = Globals.BuildQueryString(returnQuery, "tips", dayTransTips);
                        returnQuery = Globals.BuildQueryString(returnQuery, "total", dayTransTotal);
                    }
                    else
                    {
                        returnQuery = Globals.BuildQueryString(returnQuery, "transType", "Return");
                        returnQuery = Globals.BuildQueryString(returnQuery, "transDate", appDate.ToString("d"));
                        returnQuery = Globals.BuildQueryString(returnQuery, "hoursBilled", dayTransHours - dayAppHours);
                        returnQuery = Globals.BuildQueryString(returnQuery, "hourlyRate", rate);
                        returnQuery = Globals.BuildQueryString(returnQuery, "serviceFee", dayTransServiceFee - dayAppServiceFee);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCC", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subWW", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subHW", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCL", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountAmount", dayTransDiscountAmount - dayAppDiscountAmount);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountPercent", discountPercent);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountReferral", discountReferral);
                        returnQuery = Globals.BuildQueryString(returnQuery, "salesTax", salesTax);
                        returnQuery = Globals.BuildQueryString(returnQuery, "tips", dayTransTips - dayAppTips);
                        returnQuery = Globals.BuildQueryString(returnQuery, "total", dayTransTotal - dayAppTotal);
                    }

                    string returnLink = @"<a class=""paymentButton"" href=""" + returnQuery + @""">Return</a>";

                    if (balance == 0) chargeLink = "";
                    if (dayTransCount == 0) returnLink = "";
                    if (firstApp.customerPaymentType.ToLower().Contains("invoice"))
                        returnLink = "";

                    string paymentType = firstApp.customerPaymentType;
                    if (Globals.IsPaymentCreditCard(paymentType))
                    {
                        int year = Globals.SafeIntParse(firstApp.customerCardExpYear);
                        int month = Globals.SafeIntParse(firstApp.customerCardExpMonth);

                        if (year == 0 || month == 0) paymentType = "Bad Card";
                        else if (year <= mst.Year && (year != mst.Year || month < mst.Month)) paymentType = "Expired Card";
                    }

                    invoiceHTML = string.Format(invoiceHTML,  appDate.ToString("d"), customerID, firstApp.customerTitleCustomNote, itemString, serviceFeeString, subContractorString, hoursString, tipsString, discountPercentString, discountAmountString, Globals.FormatMoney(dayAppTotal), balance == 0 ? "Paid" : Globals.FormatMoney(balance), paymentType, chargeLink + returnLink + invoiceLink);

                    Response.Write(invoiceHTML);
                }
            }

        }
    }
}