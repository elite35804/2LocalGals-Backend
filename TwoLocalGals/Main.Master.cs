using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Nexus
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["URL"].Contains("Calendar.aspx") || Request.ServerVariables["URL"].Contains("Schedule.aspx"))
            {
                int clientWidth = Globals.SafeIntParse(Globals.GetCookieValue("ClientWidth"));
                PageDiv.Style["width"] = clientWidth > 1800 ? clientWidth - 60 + "px" : "1800px";
            }

            if (Request.ServerVariables["URL"].Contains("Payments.aspx"))
            {
                PageDiv.Style["width"] = "1600px";
            }

            int userAcess = Globals.GetUserAccess(this.Page);

            ContractorInfoMenuButton.Visible = userAcess == 2;
            UnavailabilityMenuButton.Visible = userAcess == 2;
            ReportsMainMenuButton.Visible = userAcess >= 2;
            ScheduleMainMenuButton.Visible = userAcess >= 2;
            AnalyticsMenuButton.Visible = userAcess >= 7;
            NotesMenuButton.Visible = userAcess >= 5;
            CalendarMainMenuButton.Visible = userAcess >= 5;
            CustomersMainMenuButton.Visible = userAcess >= 5;
            PaymentsMainMenuButton.Visible = userAcess >= 5;
            ContractorsMainMenuButton.Visible = userAcess >= 5;
            UsersMainMenuButton.Visible = userAcess >= 5;
            FranchisesMainMenuButton.Visible = userAcess >= 7;

            if (userAcess == 2)
            {
                ContractorStruct contractor = Database.GetContractorByID(Globals.GetFranchiseMask(), Globals.GetUserContractorID(this.Page));
                LoginExtraText.InnerHtml = "Contractor Score: <b>" + contractor.score.ToString("N2") + "</b>";
            }
        }

        public void LinkSaveCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                bool ret = true;
                MethodInfo saveMethod = this.Page.GetType().GetMethod("SaveChanges");
                if (saveMethod != null) ret = (bool)saveMethod.Invoke(this.Page, null);
                if (ret) Response.Redirect(e.CommandArgument.ToString());
            }
        }
    }
}