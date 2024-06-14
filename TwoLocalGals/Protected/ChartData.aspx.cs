using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Diagnostics;

namespace TwoLocalGals.Protected
{
    public partial class ChartData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int franchiseMask = Globals.SafeIntParse(Globals.GetCookieValue("AnalyticsMask"));
            if (franchiseMask == 0) franchiseMask = -1;
            franchiseMask &= Globals.GetFranchiseMask();

            DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
            DateTime startDate = Globals.SafeDateParse(Globals.GetCookieValue("AnalyticsStartDate"));
            DateTime endDate = Globals.SafeDateParse(Globals.GetCookieValue("AnalyticsEndDate"));
            if (startDate == DateTime.MinValue) startDate = mst - TimeSpan.FromDays(98);
            if (endDate == DateTime.MinValue) endDate = mst;
            if (startDate.Date == endDate.Date) startDate = startDate - TimeSpan.FromDays(1);
            Globals.FormatDateRange(ref startDate, ref endDate);

            int serviceType = Globals.SafeIntParse(Globals.GetCookieValue("AnalyticsContractorType"));
            if (serviceType == 0) serviceType = 1;

            Response.ContentType = "text/xml";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            Response.Output.WriteLine(@"<?xml version=""1.0""?>");
            Response.Output.WriteLine(@"<JSChart>");
            Response.Output.WriteLine(@"<dataset type=""bar"">");

            if (Request["C"] == "1")
            {
                SortedList<DateTime, decimal> data = Database.GetDailyTotalsByDateRange(franchiseMask, "Active Customers", startDate, endDate, 7);
                if (data.Count == 0) data.Add(startDate, 0);
                if (data.Count == 1) data.Add(endDate, 0);

                foreach (DateTime date in data.Keys)
                {
                    Response.Output.WriteLine(@"<data unit=""" + date.ToString("MMM dd") + @""" value=""" + data[date].ToString("G") + @"""/>");
                }
            }
            if (Request["C"] == "2")
            {
                SortedList<DateTime, decimal> data = Database.GetChartDataAppHours(franchiseMask, startDate, endDate);
                if (data.Count == 0) data.Add(startDate, 0);
                if (data.Count == 1) data.Add(endDate, 0);

                foreach (DateTime date in data.Keys)
                {
                    Response.Output.WriteLine(@"<data unit=""" + date.ToString("MMM dd") + @""" value=""" + data[date].ToString("G") + @"""/>");
                }
            }

            if (Request["C"] == "3")
            {
                SortedList<DateTime, decimal> data = Database.GetDailyTotalsByDateRange(franchiseMask, "Scheduling Satisfaction", startDate, endDate, 7, "average");
                if (data.Count == 0) data.Add(startDate, 0);
                if (data.Count == 1) data.Add(endDate, 0);

                foreach (DateTime date in data.Keys)
                {
                    Response.Output.WriteLine(@"<data unit=""" + date.ToString("MMM dd") + @""" value=""" + data[date].ToString("G") + @"""/>");
                }
            }

            if (Request["C"] == "4")
            {
                Dictionary<int, FranchiseStruct> franDict = new Dictionary<int, FranchiseStruct>();
                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                    franDict.Add(fran.franchiseMask, fran);

                Dictionary<int, ContractorStruct> conDict = new Dictionary<int, ContractorStruct>();
                foreach (ContractorStruct con in Database.GetContractorList(franchiseMask, -1, false, false, false, false))
                    conDict.Add(con.contractorID, con);

                decimal grossRevenue = 0;
                decimal payrollTotal = 0;

                SortedList<DateTime, decimal> appList = new SortedList<DateTime, decimal>();
                foreach(AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", false))
                {
                    if (app.appType != serviceType) continue;
                    if (app.customerAccountStatus == "Ignored") continue;

                    ContractorStruct con = conDict.ContainsKey(app.contractorID) ? conDict[app.contractorID] : new ContractorStruct();
                    FranchiseStruct fran = franDict[app.franchiseMask];

                    decimal conTotal = Globals.CalculateAppointmentContractorTotal(app, con, fran);
                    decimal total = Globals.CalculateAppointmentTotal(app);

                    grossRevenue += total;
                    payrollTotal += conTotal;

                    if (!appList.ContainsKey(app.appointmentDate)) appList.Add(app.appointmentDate, 0);
                    appList[app.appointmentDate] += (total - conTotal);
                }


                Debug.WriteLine("Gross Revenue: " + grossRevenue + ", Payroll: " + payrollTotal);

                if (appList.Count == 0) appList.Add(startDate, 0);
                if (appList.Count == 1) appList.Add(endDate, 0);

                int dataRate = appList.Count / 10;
                if (dataRate <= 1) dataRate = 1;

                int index = 0;
                decimal indexTotal = 0;
                foreach (DateTime date in appList.Keys)
                {
                    index++;
                    indexTotal += appList[date];
                    if (index >= dataRate)
                    {
                        Response.Output.WriteLine(@"<data unit=""" + date.ToString("MMM dd") + @""" value=""" + indexTotal.ToString("G") + @"""/>");
                        index = 0;
                        indexTotal = 0;
                    }
                }
            }


            Response.Output.WriteLine(@"</dataset>");
            Response.Output.WriteLine(@"</JSChart>");
        }
    }
}